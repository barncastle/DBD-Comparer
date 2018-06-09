namespace DBCompareTool
{
	partial class Main
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.pnlGrids = new System.Windows.Forms.Panel();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.dgOriginal = new System.Windows.Forms.DataGridView();
			this.dgNew = new System.Windows.Forms.DataGridView();
			this.pnlHeader = new System.Windows.Forms.Panel();
			this.btnValidate = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.radCols = new System.Windows.Forms.RadioButton();
			this.radRow = new System.Windows.Forms.RadioButton();
			this.lblMatch = new System.Windows.Forms.Label();
			this.btnNextFile = new System.Windows.Forms.Button();
			this.btnMatch = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.btnNext1 = new System.Windows.Forms.Button();
			this.btnPrev1 = new System.Windows.Forms.Button();
			this.chkMatchingID = new System.Windows.Forms.CheckBox();
			this.lblInfo1 = new System.Windows.Forms.Label();
			this.cbBuild2 = new System.Windows.Forms.ComboBox();
			this.cbBuild1 = new System.Windows.Forms.ComboBox();
			this.txtFile1 = new System.Windows.Forms.ComboBox();
			this.chkLockScrollX = new System.Windows.Forms.CheckBox();
			this.chkLockScrollY = new System.Windows.Forms.CheckBox();
			this.btnFile1 = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.radNamed = new System.Windows.Forms.RadioButton();
			this.pnlGrids.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgOriginal)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgNew)).BeginInit();
			this.pnlHeader.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlGrids
			// 
			this.pnlGrids.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlGrids.Controls.Add(this.splitContainer);
			this.pnlGrids.Location = new System.Drawing.Point(12, 67);
			this.pnlGrids.Name = "pnlGrids";
			this.pnlGrids.Size = new System.Drawing.Size(1130, 511);
			this.pnlGrids.TabIndex = 0;
			// 
			// splitContainer
			// 
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.Location = new System.Drawing.Point(0, 0);
			this.splitContainer.Name = "splitContainer";
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.dgOriginal);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.dgNew);
			this.splitContainer.Size = new System.Drawing.Size(1130, 511);
			this.splitContainer.SplitterDistance = 488;
			this.splitContainer.TabIndex = 2;
			// 
			// dgOriginal
			// 
			this.dgOriginal.AllowUserToAddRows = false;
			this.dgOriginal.AllowUserToDeleteRows = false;
			this.dgOriginal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgOriginal.Location = new System.Drawing.Point(0, 0);
			this.dgOriginal.Name = "dgOriginal";
			this.dgOriginal.ReadOnly = true;
			this.dgOriginal.RowHeadersVisible = false;
			this.dgOriginal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgOriginal.Size = new System.Drawing.Size(488, 511);
			this.dgOriginal.TabIndex = 0;
			this.dgOriginal.Scroll += new System.Windows.Forms.ScrollEventHandler(this.DgView_Scroll);
			// 
			// dgNew
			// 
			this.dgNew.AllowUserToAddRows = false;
			this.dgNew.AllowUserToDeleteRows = false;
			this.dgNew.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgNew.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgNew.Location = new System.Drawing.Point(0, 0);
			this.dgNew.Name = "dgNew";
			this.dgNew.ReadOnly = true;
			this.dgNew.RowHeadersVisible = false;
			this.dgNew.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgNew.Size = new System.Drawing.Size(638, 511);
			this.dgNew.TabIndex = 1;
			this.dgNew.Scroll += new System.Windows.Forms.ScrollEventHandler(this.DgView_Scroll);
			// 
			// pnlHeader
			// 
			this.pnlHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlHeader.Controls.Add(this.radNamed);
			this.pnlHeader.Controls.Add(this.btnValidate);
			this.pnlHeader.Controls.Add(this.label2);
			this.pnlHeader.Controls.Add(this.radCols);
			this.pnlHeader.Controls.Add(this.radRow);
			this.pnlHeader.Controls.Add(this.lblMatch);
			this.pnlHeader.Controls.Add(this.btnNextFile);
			this.pnlHeader.Controls.Add(this.btnMatch);
			this.pnlHeader.Controls.Add(this.label1);
			this.pnlHeader.Controls.Add(this.button1);
			this.pnlHeader.Controls.Add(this.button2);
			this.pnlHeader.Controls.Add(this.btnNext1);
			this.pnlHeader.Controls.Add(this.btnPrev1);
			this.pnlHeader.Controls.Add(this.chkMatchingID);
			this.pnlHeader.Controls.Add(this.lblInfo1);
			this.pnlHeader.Controls.Add(this.cbBuild2);
			this.pnlHeader.Controls.Add(this.cbBuild1);
			this.pnlHeader.Controls.Add(this.txtFile1);
			this.pnlHeader.Controls.Add(this.chkLockScrollX);
			this.pnlHeader.Controls.Add(this.chkLockScrollY);
			this.pnlHeader.Controls.Add(this.btnFile1);
			this.pnlHeader.Location = new System.Drawing.Point(12, 12);
			this.pnlHeader.Name = "pnlHeader";
			this.pnlHeader.Size = new System.Drawing.Size(1130, 49);
			this.pnlHeader.TabIndex = 1;
			// 
			// btnValidate
			// 
			this.btnValidate.Location = new System.Drawing.Point(714, 4);
			this.btnValidate.Name = "btnValidate";
			this.btnValidate.Size = new System.Drawing.Size(75, 23);
			this.btnValidate.TabIndex = 23;
			this.btnValidate.Text = "Validate Def";
			this.toolTip1.SetToolTip(this.btnValidate, "Validate the whole definition");
			this.btnValidate.UseVisualStyleBackColor = true;
			this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(260, 30);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 13);
			this.label2.TabIndex = 22;
			this.label2.Text = "Match:";
			// 
			// radCols
			// 
			this.radCols.AutoSize = true;
			this.radCols.Location = new System.Drawing.Point(364, 28);
			this.radCols.Name = "radCols";
			this.radCols.Size = new System.Drawing.Size(45, 17);
			this.radCols.TabIndex = 21;
			this.radCols.Text = "Cols";
			this.radCols.UseVisualStyleBackColor = true;
			// 
			// radRow
			// 
			this.radRow.AutoSize = true;
			this.radRow.Checked = true;
			this.radRow.Location = new System.Drawing.Point(306, 28);
			this.radRow.Name = "radRow";
			this.radRow.Size = new System.Drawing.Size(52, 17);
			this.radRow.TabIndex = 20;
			this.radRow.TabStop = true;
			this.radRow.Text = "Rows";
			this.radRow.UseVisualStyleBackColor = true;
			// 
			// lblMatch
			// 
			this.lblMatch.AutoSize = true;
			this.lblMatch.Location = new System.Drawing.Point(624, 30);
			this.lblMatch.Name = "lblMatch";
			this.lblMatch.Size = new System.Drawing.Size(63, 13);
			this.lblMatch.TabIndex = 19;
			this.lblMatch.Text = "Matching: 0";
			// 
			// btnNextFile
			// 
			this.btnNextFile.Location = new System.Drawing.Point(227, 3);
			this.btnNextFile.Name = "btnNextFile";
			this.btnNextFile.Size = new System.Drawing.Size(23, 23);
			this.btnNextFile.TabIndex = 18;
			this.btnNextFile.Text = ">";
			this.toolTip1.SetToolTip(this.btnNextFile, "Next Definition");
			this.btnNextFile.UseVisualStyleBackColor = true;
			this.btnNextFile.Click += new System.EventHandler(this.btnNextFile_Click);
			// 
			// btnMatch
			// 
			this.btnMatch.Location = new System.Drawing.Point(619, 4);
			this.btnMatch.Name = "btnMatch";
			this.btnMatch.Size = new System.Drawing.Size(89, 23);
			this.btnMatch.TabIndex = 17;
			this.btnMatch.Text = "Show Matching";
			this.toolTip1.SetToolTip(this.btnMatch, "Highlight matching records");
			this.btnMatch.UseVisualStyleBackColor = true;
			this.btnMatch.Click += new System.EventHandler(this.btnMatch_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(63, 13);
			this.label1.TabIndex = 16;
			this.label1.Text = "Lock Scroll:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(529, 3);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(23, 23);
			this.button1.TabIndex = 15;
			this.button1.Text = ">";
			this.toolTip1.SetToolTip(this.button1, "Next Build");
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(407, 3);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(23, 23);
			this.button2.TabIndex = 14;
			this.button2.Text = "<";
			this.toolTip1.SetToolTip(this.button2, "Previous Build");
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.btnPrev_Click);
			// 
			// btnNext1
			// 
			this.btnNext1.Location = new System.Drawing.Point(378, 3);
			this.btnNext1.Name = "btnNext1";
			this.btnNext1.Size = new System.Drawing.Size(23, 23);
			this.btnNext1.TabIndex = 13;
			this.btnNext1.Text = ">";
			this.toolTip1.SetToolTip(this.btnNext1, "Next Build");
			this.btnNext1.UseVisualStyleBackColor = true;
			this.btnNext1.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnPrev1
			// 
			this.btnPrev1.Location = new System.Drawing.Point(256, 3);
			this.btnPrev1.Name = "btnPrev1";
			this.btnPrev1.Size = new System.Drawing.Size(23, 23);
			this.btnPrev1.TabIndex = 12;
			this.btnPrev1.Text = "<";
			this.toolTip1.SetToolTip(this.btnPrev1, "Previous Build");
			this.btnPrev1.UseVisualStyleBackColor = true;
			this.btnPrev1.Click += new System.EventHandler(this.btnPrev_Click);
			// 
			// chkMatchingID
			// 
			this.chkMatchingID.AutoSize = true;
			this.chkMatchingID.Checked = true;
			this.chkMatchingID.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMatchingID.Location = new System.Drawing.Point(140, 29);
			this.chkMatchingID.Name = "chkMatchingID";
			this.chkMatchingID.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkMatchingID.Size = new System.Drawing.Size(114, 17);
			this.chkMatchingID.TabIndex = 10;
			this.chkMatchingID.Text = ":Matching Ids Only";
			this.chkMatchingID.UseVisualStyleBackColor = true;
			this.chkMatchingID.CheckedChanged += new System.EventHandler(this.chkMatchingID_CheckedChanged);
			// 
			// lblInfo1
			// 
			this.lblInfo1.AutoSize = true;
			this.lblInfo1.Location = new System.Drawing.Point(795, 4);
			this.lblInfo1.Name = "lblInfo1";
			this.lblInfo1.Size = new System.Drawing.Size(25, 13);
			this.lblInfo1.TabIndex = 9;
			this.lblInfo1.Text = "Info";
			// 
			// cbBuild2
			// 
			this.cbBuild2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbBuild2.FormattingEnabled = true;
			this.cbBuild2.Location = new System.Drawing.Point(430, 4);
			this.cbBuild2.Name = "cbBuild2";
			this.cbBuild2.Size = new System.Drawing.Size(99, 21);
			this.cbBuild2.TabIndex = 8;
			// 
			// cbBuild1
			// 
			this.cbBuild1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbBuild1.FormattingEnabled = true;
			this.cbBuild1.Location = new System.Drawing.Point(279, 4);
			this.cbBuild1.Name = "cbBuild1";
			this.cbBuild1.Size = new System.Drawing.Size(99, 21);
			this.cbBuild1.TabIndex = 7;
			// 
			// txtFile1
			// 
			this.txtFile1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.txtFile1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.txtFile1.FormattingEnabled = true;
			this.txtFile1.Location = new System.Drawing.Point(3, 4);
			this.txtFile1.Name = "txtFile1";
			this.txtFile1.Size = new System.Drawing.Size(222, 21);
			this.txtFile1.TabIndex = 1;
			this.txtFile1.SelectedIndexChanged += new System.EventHandler(this.txtFile1_SelectedIndexChanged);
			// 
			// chkLockScrollX
			// 
			this.chkLockScrollX.AutoSize = true;
			this.chkLockScrollX.Checked = true;
			this.chkLockScrollX.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkLockScrollX.Location = new System.Drawing.Point(69, 29);
			this.chkLockScrollX.Name = "chkLockScrollX";
			this.chkLockScrollX.Size = new System.Drawing.Size(33, 17);
			this.chkLockScrollX.TabIndex = 6;
			this.chkLockScrollX.Text = "X";
			this.chkLockScrollX.UseVisualStyleBackColor = true;
			// 
			// chkLockScrollY
			// 
			this.chkLockScrollY.AutoSize = true;
			this.chkLockScrollY.Checked = true;
			this.chkLockScrollY.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkLockScrollY.Location = new System.Drawing.Point(108, 29);
			this.chkLockScrollY.Name = "chkLockScrollY";
			this.chkLockScrollY.Size = new System.Drawing.Size(33, 17);
			this.chkLockScrollY.TabIndex = 5;
			this.chkLockScrollY.Text = "Y";
			this.chkLockScrollY.UseVisualStyleBackColor = true;
			// 
			// btnFile1
			// 
			this.btnFile1.Location = new System.Drawing.Point(558, 3);
			this.btnFile1.Name = "btnFile1";
			this.btnFile1.Size = new System.Drawing.Size(55, 23);
			this.btnFile1.TabIndex = 2;
			this.btnFile1.Text = "...";
			this.btnFile1.UseVisualStyleBackColor = true;
			this.btnFile1.Click += new System.EventHandler(this.ButtonFind_Click);
			// 
			// radNamed
			// 
			this.radNamed.AutoSize = true;
			this.radNamed.Location = new System.Drawing.Point(415, 28);
			this.radNamed.Name = "radNamed";
			this.radNamed.Size = new System.Drawing.Size(59, 17);
			this.radNamed.TabIndex = 24;
			this.radNamed.Text = "Named";
			this.radNamed.UseVisualStyleBackColor = true;
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1154, 590);
			this.Controls.Add(this.pnlHeader);
			this.Controls.Add(this.pnlGrids);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(781, 200);
			this.Name = "Main";
			this.Text = "Main";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.pnlGrids.ResumeLayout(false);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgOriginal)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgNew)).EndInit();
			this.pnlHeader.ResumeLayout(false);
			this.pnlHeader.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlGrids;
		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.DataGridView dgOriginal;
		private System.Windows.Forms.DataGridView dgNew;
		private System.Windows.Forms.Panel pnlHeader;
		private System.Windows.Forms.Button btnFile1;
		private System.Windows.Forms.CheckBox chkLockScrollY;
		private System.Windows.Forms.CheckBox chkLockScrollX;
		private System.Windows.Forms.ComboBox txtFile1;
		private System.Windows.Forms.ComboBox cbBuild2;
		private System.Windows.Forms.ComboBox cbBuild1;
		private System.Windows.Forms.Label lblInfo1;
		private System.Windows.Forms.CheckBox chkMatchingID;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button btnNext1;
		private System.Windows.Forms.Button btnPrev1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnMatch;
		private System.Windows.Forms.Button btnNextFile;
		private System.Windows.Forms.Label lblMatch;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.RadioButton radCols;
		private System.Windows.Forms.RadioButton radRow;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnValidate;
		private System.Windows.Forms.RadioButton radNamed;
	}
}

