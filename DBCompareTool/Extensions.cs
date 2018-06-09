using DBCompareTool.FileReader;
using DBDefsLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DBCompareTool
{
	public static class Extensions
	{
		public static string Reverse(this string s)
		{
			return new string(s.ToCharArray().Reverse().ToArray());
		}

		public static string ReadStringNull(this BinaryReader reader, out int pos)
		{
			byte num;
			List<byte> temp = new List<byte>();

			while ((num = reader.ReadByte()) != 0)
				temp.Add(num);

			pos = temp.Count + 1;

			return Encoding.UTF8.GetString(temp.ToArray());
		}

		public static string ReadString(this BinaryReader br, int count)
		{
			byte[] stringArray = br.ReadBytes(count);
			return Encoding.UTF8.GetString(stringArray);
		}

		public static void Scrub(this BinaryReader br, long pos)
		{
			br.BaseStream.Seek(pos, SeekOrigin.Begin);
		}

		public static bool IsDefault<T>(this T obj) where T : struct
		{
			return obj.Equals(default(T));
		}

		public static void DoubleBuffer(this System.Windows.Forms.DataGridView dg)
		{
			BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty;
			typeof(System.Windows.Forms.DataGridView).InvokeMember("DoubleBuffered", flags, null, dg, new object[] { true });
		}

		public static bool ContainsV2(this BuildRange range, Build build)
		{
			Build minBuild = range.minBuild;
			Build maxBuild = range.maxBuild;

			// flat expansion range check
			if (build.expansion < minBuild.expansion || build.expansion > maxBuild.expansion)
				return false;

			// sits between two expansions i.e. 2...4, build = 3.x.x
			if (build.expansion > minBuild.expansion && build.expansion < maxBuild.expansion)
				return true;

			// end exp build vs alpha/beta build 3.0.1.8885-4.0.0.[12319], 3.3.5.[12340]
			if (build.expansion >= minBuild.expansion && build.build > minBuild.build && build.expansion < maxBuild.expansion)
				return true;

			// alpha/beta builds are lower than final exp builds i.e. 2.4.3.[8606] vs 3.0.1.[8391]
			if (build.expansion > minBuild.expansion && build.build < maxBuild.build)
				return true;

			return (build.build >= minBuild.build && build.build <= maxBuild.build);
		}

		public static bool Contains(this Structs.VersionDefinitions version, Build build)
		{
			if (version.builds.Any(x => x.build == build.build))
				return true;
			if (version.buildRanges.Any(x => x.ContainsV2(build)))
				return true;

			return false;
		}
	}
}
