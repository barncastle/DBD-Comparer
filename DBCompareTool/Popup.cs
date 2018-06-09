using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBCompareTool
{
	public partial class Popup : Form
	{
		public IEnumerable<IModel> Data;

		public Popup()
		{
			InitializeComponent();
		}

		private void Popup_Load(object sender, EventArgs e)
		{
			string data = string.Join("\r\n", Data.Select(x => x.ToString()));
			richTextBox1.Text = data;
		}
	}
}
