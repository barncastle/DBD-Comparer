using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DBCompareTool.FileReader
{
	public class WDBC : DBHeader
	{
		public override void ReadHeader(ref BinaryReader dbReader, string signature)
		{
			base.ReadHeader(ref dbReader, signature);
		}
	}
}