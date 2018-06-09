using DBDefsLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static DBDefsLib.Structs;

namespace DBCompareTool.FileReader
{
	using readFunc = Func<BinaryReader, dynamic>;

	public static class DBReader
	{
		const float FLOAT_PERC = 0.9f;
		const StringComparison WO_CASE = StringComparison.InvariantCultureIgnoreCase;

		#region Lookups

		private readonly static Dictionary<TypeCode, int> _typeSize = new Dictionary<TypeCode, int>()
		{
			{ TypeCode.Boolean, 1 },
			{ TypeCode.Byte, 1 },
			{ TypeCode.SByte, 1 },
			{ TypeCode.Int16, 2 },
			{ TypeCode.UInt16, 2 },
			{ TypeCode.Int32, 4 },
			{ TypeCode.UInt32, 4 },
			{ TypeCode.String, 4 },
			{ TypeCode.Single, 4 },
			{ TypeCode.Int64, 8 },
			{ TypeCode.UInt64, 8 },
		};

		private static Dictionary<TypeCode, readFunc> _readerFuncs = new Dictionary<TypeCode, readFunc>()
		{
			{ TypeCode.Boolean, (b) => b.ReadBoolean() },
			{ TypeCode.Byte,    (b) => b.ReadByte() },
			{ TypeCode.SByte,   (b) => b.ReadSByte() },
			{ TypeCode.Int16,   (b) => b.ReadInt16() },
			{ TypeCode.UInt16,  (b) => b.ReadUInt16() },
			{ TypeCode.Int32,   (b) => b.ReadInt32() },
			{ TypeCode.UInt32,  (b) => b.ReadUInt32() },
			{ TypeCode.UInt64,  (b) => b.ReadUInt64() },
			{ TypeCode.Int64,   (b) => b.ReadInt64() },
			{ TypeCode.Single,  (b) => b.ReadSingle() },
			{ TypeCode.String,  (b) => null }
		};

		#endregion


		private static DBHeader ReadHeader(BinaryReader br, string dbFile)
		{
			DBHeader header = null;
			string signature = br.ReadString(4);

			if (string.IsNullOrWhiteSpace(signature))
				return null;

			if (signature[0] != 'W')
				signature = signature.Reverse();

			switch (signature)
			{
				case "WDBC":
					header = new WDBC();
					break;
				case "WDB2":
					header = new WDB2();
					break;
			}

			try
			{
				header?.ReadHeader(ref br, signature);
			}
			catch
			{
				header = null;
			}

			return header;
		}


		public static DataTable Read(string dbFile, string dbdFile, Build build, out DBHeader header)
		{
			DataTable table = new DataTable();

			// create table structure
			var typeLookup = BuildStructure(dbdFile, build, out bool defissue);
			foreach (var t in typeLookup)
				table.Columns.Add(t.Key, Type.GetType("System." + t.Value));

			using (var fs = new FileStream(dbFile, FileMode.Open, FileAccess.Read))
			using (var buffer = new BufferedStream(fs))
			using (var br = new BinaryReader(buffer))
			{
				header = ReadHeader(br, dbFile);
				if (defissue)
					header.Issues |= DBIssues.DEFINITION_ISSUE; // multiple or missing def

				if (!ValidationChecks(header, dbFile)) // validate header info
					return table;

				int fields = typeLookup.Count(x => !x.Key.StartsWith("padding", WO_CASE));
				if (fields != header.FieldCount)
					header.Issues |= DBIssues.FIELD_COUNT; // validate field count vs def

				// stringtable stuff
				long pos = br.BaseStream.Position;
				br.BaseStream.Position = br.BaseStream.Position + (header.RecordCount * header.RecordSize);
				var stringTable = ReadStringTable(br, br.BaseStream.Position); // get stringtable
				br.BaseStream.Seek(pos, SeekOrigin.Begin);

				// add stringtable reader func
				LoadStringFunc(stringTable);

				// read data
				var rowLoader = typeLookup.Values.Select(x => _readerFuncs[x]).ToArray(); // generate a reader collection

				table.BeginLoadData();
				try
				{
					for (int i = 0; i < header.RecordCount; i++)
					{
						DataRow row = table.NewRow();
						row.ItemArray = rowLoader.Select(x => x(br)).ToArray();
						table.Rows.Add(row);
					}
				}
				catch (EndOfStreamException)
				{
					header.Issues |= DBIssues.FIELD_COUNT; // overflow
				}

				// check for unused strings
				if (!typeLookup.Values.Any(x => x == TypeCode.String) && !stringTable.All(x => string.IsNullOrWhiteSpace(x.Value)))
					header.Issues |= DBIssues.UNUSED_STRINGS;

				table.EndLoadData();

				return table;
			}
		}

		public static ValidateResult Validate(string dbFile, string dbdFile, Build build)
		{
			DBHeader header;
			ValidateResult validateResult = new ValidateResult() { Build = build };

			using (var fs = new FileStream(dbFile, FileMode.Open, FileAccess.Read))
			using (var buffer = new BufferedStream(fs))
			using (var br = new BinaryReader(buffer))
			{
				header = ReadHeader(br, dbFile);
				if (header == null)
				{
					validateResult.Issues |= DBIssues.MALFORMED;
					return validateResult;
				}

				var typeLookup = BuildStructure(dbdFile, build, out bool defissue);
				if (defissue)
					validateResult.Issues |= DBIssues.DEFINITION_ISSUE; // multiple or missing def

				if (!ValidationChecks(header, dbFile))
					validateResult.Issues |= header?.Issues ?? 0;

				int fields = typeLookup.Count(x => !x.Key.StartsWith("padding", WO_CASE));
				if (fields != header.FieldCount)
					validateResult.Issues |= DBIssues.FIELD_COUNT; // validate field count vs def

				if (validateResult.Issues != DBIssues.NONE)
					return validateResult;

				// stringtable stuff
				long pos = br.BaseStream.Position;
				long stringTableStart = br.BaseStream.Position + (header.RecordCount * header.RecordSize);
				br.BaseStream.Position = stringTableStart;
				var stringTable = ReadStringTable(br, stringTableStart); // get stringtable
				br.BaseStream.Seek(pos, SeekOrigin.Begin);

				// read data
				TypeCode[] columnTypes = typeLookup.Values.ToArray();

				// check for unused strings
				if (!columnTypes.Any(x => x == TypeCode.String) && !stringTable.All(x => string.IsNullOrWhiteSpace(x.Value)))
					validateResult.Issues |= DBIssues.UNUSED_STRINGS;

				// read the data
				byte[][] records = null;
				try
				{
					records = Enumerable.Range(0, (int)header.RecordCount)
								.Select(x => br.ReadBytes((int)header.RecordSize))
								.ToArray();
				}
				catch (EndOfStreamException)
				{
					validateResult.Issues |= DBIssues.FIELD_COUNT; // overflow
					return validateResult;
				}

				// validate id column for positive unique-ness
				bool hasID = typeLookup.Keys.First().StartsWith("$id$");
				if (hasID)
				{
					HashSet<int> ids = new HashSet<int>(Enumerable.Range(0, records.Length).Select(x => BitConverter.ToInt32(records[x], 0)));
					if (ids.Count < records.Length || ids.Any(x => x < 0))
						validateResult.Issues |= DBIssues.INVALID_IDS;
				}

				int offset = 0;
				for (int x = 0; x < header.FieldCount; x++)
				{
					if (columnTypes[x] < TypeCode.Single) // ignore int types
					{
						offset += _typeSize[columnTypes[x]];
						continue;
					}

					int likelyfloatcount = records.Length;
					bool isstring = columnTypes[x] == TypeCode.String;

					Parallel.For(0, records.Length, i =>
					{
						int val = BitConverter.ToInt32(records[i], offset);
						if (isstring)
						{
							if (!stringTable.ContainsKey(val)) // ensure that all string indices are correct
								validateResult.Issues |= DBIssues.INVALID_STRINGS;
						}
						else
						{
							if (val != 0 && !FloatUtil.IsLikelyFloat(val)) // capture the amount of iffy floats in this column
								Interlocked.Decrement(ref likelyfloatcount);
						}
					});

					if (!isstring)
					{
						bool islikelyfloat = (likelyfloatcount / (float)records.Length) >= FLOAT_PERC;
						if (!islikelyfloat)
							validateResult.Issues |= DBIssues.INTASFLOAT; // < % of floats are valid floats - could be an int
					}

					offset += _typeSize[columnTypes[x]];
				}

				Array.Resize(ref records, 0);
			}

			return validateResult;
		}


		private static Dictionary<string, TypeCode> BuildStructure(string filename, Build build, out bool defissue)
		{
			DBDReader reader = new DBDReader();
			var dbd = reader.Read(filename);

			Dictionary<string, TypeCode> typeLookup = new Dictionary<string, TypeCode>();

			void AddColumn(TypeCode type, string name, int arrlength)
			{
				for (int i = 0; i < arrlength || i < 1; i++)
					typeLookup.Add(name + (arrlength > 1 ? "_" + (i + 1) : ""), type);
			}

			// size, signed type, unsigned type
			var intTypes = new Dictionary<int, Tuple<TypeCode, TypeCode>>()
			{
				{08, new Tuple<TypeCode, TypeCode>(TypeCode.Byte, TypeCode.Byte) },
				{16, new Tuple<TypeCode, TypeCode>(TypeCode.Int16, TypeCode.UInt16) },
				{32, new Tuple<TypeCode, TypeCode>(TypeCode.Int32, TypeCode.UInt32) },
				{64, new Tuple<TypeCode, TypeCode>(TypeCode.Int64, TypeCode.UInt64) },
			};

			var vers = dbd.versionDefinitions.Where(y => y.Contains(build));
			defissue = vers.Count() != 1;

			var ver = vers.FirstOrDefault();
			if (!ver.IsDefault())
			{
				foreach (var def in ver.definitions)
				{
					string name = def.name;
					if (def.isID && !def.isNonInline)
						name = "$id$" + name;

					switch (dbd.columnDefinitions[def.name].type)
					{
						case "int":
							AddColumn(intTypes[def.size].Item1, name, def.arrLength);
							break;
						case "uint":
							AddColumn(intTypes[def.size].Item2, name, def.arrLength);
							break;
						case "float":
							AddColumn(TypeCode.Single, name, def.arrLength);
							break;
						case "string":
							AddColumn(TypeCode.String, name, def.arrLength);
							break;
						case "locstring":
							{
								int len = (build.expansion > 3 ? def.arrLength : (build.build < 6692 ? 8 : 16)); // string or locstring
								AddColumn(TypeCode.String, name, len);
								if (build.expansion <= 3)
									AddColumn(TypeCode.Int32, name + "_mask", 1); // mask
							}
							break;
					}
				}
			}

			return typeLookup;
		}


		private static bool ValidationChecks(DBHeader header, string FileName)
		{
			string name = Path.GetFileName(FileName) + " " + Directory.GetParent(FileName).Name;

			if (header == null)
				return false;

			if (header.RecordCount == 0)
				header.Issues |= DBIssues.NO_RECORDS;
			if (header.FieldCount == 0)
				header.Issues |= DBIssues.NO_FIELDS;

			if (header.RecordCount == 0 || header.RecordSize == 0)
				return false;

			return true;
		}

		private static Dictionary<int, string> ReadStringTable(BinaryReader br, long stringTableStart)
		{
			Dictionary<int, string> table = new Dictionary<int, string>();

			long len = br.BaseStream.Length;
			long pos = br.BaseStream.Position;

			if (pos > len)
				return table;

			while (pos < len)
			{
				int index = (int)(pos - stringTableStart);
				table[index] = br.ReadStringNull(out int count); //Extract all the strings to the string table
				pos += count;
			}

			return table;
		}

		private static void LoadStringFunc(Dictionary<int, string> stringTable)
		{
			_readerFuncs[TypeCode.String] = (b) =>
			{
				if (stringTable.TryGetValue(b.ReadInt32(), out string s))
					return s.TrimEnd('\n', '\r', '\t');
				else
					return "STRING MISSING";
			};
		}
	}
}
