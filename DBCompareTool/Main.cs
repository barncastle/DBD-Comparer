using DBCompareTool.FileReader;
using DBDefsLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBCompareTool
{
	public partial class Main : Form
	{
		public const string DBC_LOCATION = @"D:\DBCDump";
		public const string DBD_LOCATION = @"D:\DBCDump\!!todo!!";

		private static Dictionary<string, string[]> AllDBs;
		private static IReadOnlyCollection<string> AllDBDs;
		private DataTable[] tables = new DataTable[4];
		private int curindex = int.MaxValue;


		public Main()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			splitContainer.SplitterDistance = splitContainer.Width / 2;

			// performance
			dgOriginal.DoubleBuffer();
			dgNew.DoubleBuffer();

			AllDBs = Directory.EnumerateFiles(DBC_LOCATION, "*.db*", SearchOption.AllDirectories).Select(x => x.ToUpper())
							  .Where(x => !x.EndsWith("DBD"))
							  .GroupBy(x => Path.GetFileNameWithoutExtension(x))
							  .OrderBy(x => x.Key)
							  .ToDictionary(x => x.Key, x => x.ToArray());

			AllDBDs = Directory.EnumerateFiles(DBD_LOCATION, "*.dbd", SearchOption.AllDirectories).Select(x => x.ToUpper()).OrderBy(x => x).ToList();

			txtFile1.DataSource = AllDBs.Keys.Intersect(AllDBDs.Select(x => Path.GetFileNameWithoutExtension(x))).ToList();
		}

		private void ButtonFind_Click(object sender, EventArgs e)
		{
			LoadGridViews();
		}

		private void DgView_Scroll(object sender, ScrollEventArgs e)
		{
			if (dgOriginal.Columns.Count == 0 || dgNew.Columns.Count == 0)
				return;
			if (dgOriginal.Rows.Count == 0 || dgNew.Rows.Count == 0)
				return;

			if (chkLockScrollY.Checked)
			{
				switch ((sender as Control).Name)
				{
					case "dgNew":
						dgOriginal.FirstDisplayedScrollingRowIndex = Math.Min(dgOriginal.RowCount - 1, dgNew.FirstDisplayedScrollingRowIndex);
						break;
					case "dgOriginal":
						dgNew.FirstDisplayedScrollingRowIndex = Math.Min(dgNew.RowCount - 1, dgOriginal.FirstDisplayedScrollingRowIndex);
						break;
				}
			}

			if (chkLockScrollX.Checked)
			{
				switch ((sender as Control).Name)
				{
					case "dgNew":
						dgOriginal.FirstDisplayedScrollingColumnIndex = Math.Min(dgOriginal.ColumnCount - 1, dgNew.FirstDisplayedScrollingColumnIndex);
						break;
					case "dgOriginal":
						dgNew.FirstDisplayedScrollingColumnIndex = Math.Min(dgNew.ColumnCount - 1, dgOriginal.FirstDisplayedScrollingColumnIndex);
						break;
				}
			}
		}

		private void txtFile1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (txtFile1.SelectedIndex != curindex)
			{
				PopulateBuilds();
				curindex = txtFile1.SelectedIndex;
			}
		}

		private void chkMatchingID_CheckedChanged(object sender, EventArgs e)
		{
			if (chkMatchingID.Checked)
			{
				SetGridSource(tables[2], tables[3]);
			}
			else
			{
				SetGridSource(tables[0], tables[1]);
			}

			btnMatch.Enabled = chkMatchingID.Checked;
		}

		private void btnPrev_Click(object sender, EventArgs e)
		{
			ComboBox combo = (sender as Button).Name == "btnPrev1" ? cbBuild1 : cbBuild2;
			if (combo.SelectedIndex != 0)
			{
				combo.SelectedIndex--;
				LoadGridViews();
			}
		}

		private void btnNext_Click(object sender, EventArgs e)
		{
			ComboBox combo = (sender as Button).Name == "btnNext1" ? cbBuild1 : cbBuild2;
			if (combo.SelectedIndex != combo.Items.Count - 1)
			{
				combo.SelectedIndex++;
				LoadGridViews();
			}
		}

		private void btnMatch_Click(object sender, EventArgs e)
		{
			if (!chkMatchingID.Checked)
				return;

			dgOriginal.SuspendLayout();
			dgNew.SuspendLayout();

			if (radRow.Checked)
				CompareRows();
			else if (radCols.Checked)
				CompareCols();
			else if (radNamed.Checked)
				CompareNamed();

			dgOriginal.ResumeLayout();
			dgNew.ResumeLayout();
		}

		private void btnNextFile_Click(object sender, EventArgs e)
		{
			if (txtFile1.SelectedIndex != txtFile1.Items.Count - 1)
			{
				txtFile1.SelectedIndex++;
				LoadGridViews();
			}
		}

		private void btnValidate_Click(object sender, EventArgs e)
		{
			ValidateDefinition();
		}


		private void LoadGridViews()
		{
			lblMatch.Text = "Matching: 0";

			var org = AllDBs[txtFile1.Text.ToUpper()].First(x => x.Replace("a", "A").Replace("A", "").Contains(cbBuild1.Text));
			var next = AllDBs[txtFile1.Text.ToUpper()].First(x => x.Replace("a", "A").Replace("A", "").Contains(cbBuild2.Text));

			var dbd = AllDBDs.First(x => Path.GetFileNameWithoutExtension(x).ToUpper() == txtFile1.Text.ToUpper());

			tables[0] = DBReader.Read(org, dbd, new Build(cbBuild1.Text), out DBHeader header1);
			tables[1] = DBReader.Read(next, dbd, new Build(cbBuild2.Text), out DBHeader header2);

			BuildDataSets();
			SetGridSource(tables[0], tables[1]);

			lblInfo1.Text = $"Fields: {header1?.FieldCount}, Size: {header1?.RecordSize}, Issues: {header1?.Issues}";
			lblInfo1.Text += "\r\n";
			lblInfo1.Text += $"Fields: {header2?.FieldCount}, Size: {header2?.RecordSize}, Issues: {header2?.Issues}";

			// reset scroll and matching id filter
			DgView_Scroll(dgNew, null);
			chkMatchingID_CheckedChanged(null, null);
		}

		private void PopulateBuilds()
		{
			Regex buildmatch = new Regex(@"\d\.\d{1,2}.\d.\d{4,}", RegexOptions.Compiled);
			string file = Path.GetFileNameWithoutExtension(txtFile1.Text).ToUpper();

			var dbbuilds = AllDBs[file].Select(x => Directory.GetParent(x).Name.ToUpper().Replace("A", "")).Where(x => buildmatch.IsMatch(x)).ToArray();

			// flatten
			cbBuild1.DataSource = null;
			cbBuild2.DataSource = null;

			if (dbbuilds.Length == 0 || !AllDBDs.Any(x => Path.GetFileNameWithoutExtension(x).ToUpper() == file))
				return;

			DBDReader reader = new DBDReader();
			var dbd = reader.Read(AllDBDs.First(x => Path.GetFileNameWithoutExtension(x).ToUpper() == file));

			var builds = new HashSet<string>(dbd.versionDefinitions.SelectMany(x => x.builds.Select(y => y.ToString())).Intersect(dbbuilds));
			var ranges = dbd.versionDefinitions.SelectMany(x => x.buildRanges).ToArray();

			foreach (var b in dbbuilds)
			{
				Build build = new Build(b);
				foreach (var range in ranges)
				{
					if (range.ContainsV2(build))
					{
						builds.Add(b);
						break;
					}
				}
			}

			// load
			cbBuild1.DataSource = builds.Select(x => new Build(x)).OrderBy(x => x.expansion).ThenBy(x => x.build).Select(x => x.ToString()).ToList();
			cbBuild2.DataSource = (cbBuild1.DataSource as List<string>).ToList(); // binding issues
			cbBuild2.SelectedIndex = cbBuild2.Items.Count - 1;
		}

		private void BuildDataSets()
		{
			// Hacky (at the expense of RAM) because M$ datatable row filter is shit slow
			// tables 0,1 = original data
			// tables 2,3 = matching id rows only

			tables[2] = tables[0].Clone();
			tables[3] = tables[1].Clone();

			var matchingIds = new HashSet<object>(tables[0].Rows.Cast<DataRow>().Select(x => x[0]).Intersect(tables[1].Rows.Cast<DataRow>().Select(x => x[0])));

			// copy rows with wanted ids
			tables[2].BeginLoadData();
			tables[3].BeginLoadData();

			for (int i = 0; i < tables[0].Rows.Count; i++)
			{
				if (matchingIds.Contains(tables[0].Rows[i][0]))
					tables[2].ImportRow(tables[0].Rows[i]);
			}

			for (int i = 0; i < tables[1].Rows.Count; i++)
			{
				if (matchingIds.Contains(tables[1].Rows[i][0]))
					tables[3].ImportRow(tables[1].Rows[i]);
			}

			tables[2].EndLoadData();
			tables[3].EndLoadData();
		}

		private void SetGridSource(DataTable dgOriginalSource, DataTable dgNewSource)
		{
			dgOriginal.SuspendLayout();
			dgNew.SuspendLayout();

			dgOriginal.DataSource = null;
			dgOriginal.DataSource = dgOriginalSource;
			dgNew.DataSource = null;
			dgNew.DataSource = dgNewSource;

			dgOriginal.Sort(dgOriginal.Columns[0], ListSortDirection.Ascending);
			dgNew.Sort(dgNew.Columns[0], ListSortDirection.Ascending);

			dgOriginal.ResumeLayout();
			dgNew.ResumeLayout();
		}

		private void CompareRows()
		{
			var orgstringidx = LocStringIndicies(dgOriginal);
			var newstringidx = LocStringIndicies(dgNew);

			var orgcols = Enumerable.Range(0, dgOriginal.ColumnCount).Except(orgstringidx.Values.SelectMany(x => x)).ToArray();
			var newcols = Enumerable.Range(0, dgNew.ColumnCount).Except(newstringidx.Values.SelectMany(x => x)).ToArray();

			int length = Math.Min(orgcols.Length, newcols.Length);
			int preffered = Math.Max(orgcols.Length, newcols.Length);

			int matches = 0;
			Parallel.For(0, dgOriginal.RowCount, i =>
			{
				var vals1 = (dgOriginal.Rows[i].DataBoundItem as DataRowView).Row.ItemArray;
				var vals2 = (dgNew.Rows[i].DataBoundItem as DataRowView).Row.ItemArray;

				int matching = Enumerable.Range(0, length).TakeWhile(x =>
					vals1[orgcols[x]].ToString().ToLower() == vals2[newcols[x]].ToString().ToLower()
				).Count();

				if (matching == preffered)
				{
					dgOriginal.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
					dgNew.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
					Interlocked.Increment(ref matches);
				}
				else if (matching > 0)
				{
					for (int x = 0; x < matching; x++)
					{
						dgOriginal.Rows[i].Cells[orgcols[x]].Style.BackColor = Color.LightBlue;
						if (orgstringidx.ContainsKey(orgcols[x]))
							foreach (var idx in orgstringidx[orgcols[x]])
								dgOriginal.Rows[i].Cells[idx].Style.BackColor = Color.LightBlue;

						dgNew.Rows[i].Cells[newcols[x]].Style.BackColor = Color.LightBlue;
						if (newstringidx.ContainsKey(newcols[x]))
							foreach (var idx in newstringidx[newcols[x]])
								dgNew.Rows[i].Cells[idx].Style.BackColor = Color.LightBlue;
					}
				}
			});

			lblMatch.Text = $"Matches: {matches} ({((double)matches / dgOriginal.Rows.Count).ToString("0.00%")})";
		}

		private void CompareCols()
		{
			int length = Math.Min(dgOriginal.ColumnCount, dgNew.ColumnCount);
			MatchPerc[] matchPercentage = new MatchPerc[length];

			for (int i = 0; i < length; i++)
			{
				int rows = Math.Min(dgOriginal.RowCount, dgNew.RowCount);
				int matches = 0;

				Parallel.For(0, rows, x =>
				{
					string val1 = dgOriginal.Rows[x].Cells[i].Value.ToString();
					string val2 = dgNew.Rows[x].Cells[i].Value.ToString();

					if (val1 == val2 && (val1 == "0" || val1 == ""))
					{
						Interlocked.Decrement(ref rows);
					}
					else if (val1 == val2)
					{
						dgOriginal.Rows[x].Cells[i].Style.BackColor = Color.LightGreen;
						dgNew.Rows[x].Cells[i].Style.BackColor = Color.LightGreen;
						Interlocked.Increment(ref matches);
					}
				});

				float perc = rows == 0 ? 1 : (matches / (float)rows);
				matchPercentage[i] = new MatchPerc()
				{
					Col1 = dgOriginal.Columns[i].Name,
					Col2 = dgNew.Columns[i].Name,
					Percent = Math.Round(perc, 2)
				};
			}

			new Popup { Data = matchPercentage }.ShowDialog();
		}

		private void CompareNamed()
		{
			var matchingCols = (from o in dgOriginal.Columns.Cast<DataGridViewColumn>()
								join x in dgNew.Columns.Cast<DataGridViewColumn>() on o.Name equals x.Name
								select new { OrgIndex = o.Index, NewIndex = x.Index })
								.ToArray();

			List<MatchPerc> matchPercentage = new List<MatchPerc>();

			foreach (var c in matchingCols)
			{
				int rows = Math.Min(dgOriginal.RowCount, dgNew.RowCount);
				int matches = 0;

				Parallel.For(0, rows, x =>
				{
					string val1 = dgOriginal.Rows[x].Cells[c.OrgIndex].Value.ToString();
					string val2 = dgNew.Rows[x].Cells[c.NewIndex].Value.ToString();

					if (val1 == val2 && (val1 == "0" || val1 == ""))
					{
						Interlocked.Decrement(ref rows);
					}
					else if (val1 == val2)
					{
						dgOriginal.Rows[x].Cells[c.OrgIndex].Style.BackColor = Color.LightGreen;
						dgNew.Rows[x].Cells[c.NewIndex].Style.BackColor = Color.LightGreen;
						Interlocked.Increment(ref matches);
					}
				});

				float perc = rows == 0 ? 1 : (matches / (float)rows);
				matchPercentage.Add(new MatchPerc()
				{
					Col1 = dgOriginal.Columns[c.OrgIndex].Name,
					Col2 = dgNew.Columns[c.NewIndex].Name,
					Percent = Math.Round(perc, 2)
				});
			}

			new Popup { Data = matchPercentage }.ShowDialog();
		}

		private void ValidateDefinition()
		{
			Regex buildmatch = new Regex(@"\d\.\d{1,2}.\d.\d{4,}", RegexOptions.Compiled);
			string file = Path.GetFileNameWithoutExtension(txtFile1.Text).ToUpper();

			Dictionary<Build, string> dbbuilds = new Dictionary<Build, string>();
			foreach (var db in AllDBs[file])
				dbbuilds.Add(new Build(Directory.GetParent(db).Name.ToUpper().Replace("A", "")), db);

			if (dbbuilds.Count == 0 || !AllDBDs.Any(x => Path.GetFileNameWithoutExtension(x).ToUpper() == file))
				return;

			DBDReader reader = new DBDReader();
			string dbdfile = AllDBDs.First(x => Path.GetFileNameWithoutExtension(x).ToUpper() == file);
			var dbd = reader.Read(dbdfile);

			var builds = dbd.versionDefinitions.SelectMany(x => x.builds).ToArray();
			var ranges = dbd.versionDefinitions.SelectMany(x => x.buildRanges).ToArray();

			List<ValidateResult> validationResults = new List<ValidateResult>();
			foreach (var b in dbbuilds)
			{
				Application.DoEvents();

				if (builds.Any(x => x.build == b.Key.build) || ranges.Any(x => x.ContainsV2(b.Key)))
				{
					var result = DBReader.Validate(b.Value, dbdfile, b.Key);
					if (result.Issues != DBIssues.NONE)
						validationResults.Add(result);
				}
			}

			if (validationResults.Count > 0)
			{
				new Popup() { Data = validationResults }.ShowDialog();
			}
			else
			{
				MessageBox.Show("Smashing job!");
			}
		}

		private Dictionary<int, int[]> LocStringIndicies(DataGridView dg)
		{
			return dg.Columns.Cast<DataGridViewColumn>()
					 .Where(x => x.Name.EndsWith("_lang") || x.Name.Contains("_lang_"))
					 .GroupBy(x => x.Name.Split(new[] { "_lang" }, StringSplitOptions.None).First())
					 .Where(x => x.Count() > 0)
					 .ToDictionary(x => x.First().Index, x => x.Skip(1).Select(y => y.Index).ToArray());
		}
	}
}
