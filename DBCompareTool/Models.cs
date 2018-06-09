using DBCompareTool.FileReader;
using DBDefsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCompareTool
{
	public interface IModel
	{

	}

	public class MatchPerc : IModel
	{
		public string Col1;
		public string Col2;
		public double Percent;

		public override string ToString() => $"{Col1} | {Col2} | {Percent.ToString("0.00%")}";
	}

	public class ValidateResult : IModel
	{
		public Build Build;
		public DBIssues Issues;

		public override string ToString() => $"{Build} | {Issues}";
	}
}
