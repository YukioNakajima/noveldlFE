namespace noveldlFE
{
	partial class frmMain
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.tbNovelPath = new System.Windows.Forms.TextBox();
			this.cboxListSelect = new System.Windows.Forms.ComboBox();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.lblTimeCount = new System.Windows.Forms.Label();
			this.lblStatus = new System.Windows.Forms.Label();
			this.lblProgress = new System.Windows.Forms.Label();
			this.tbURL = new System.Windows.Forms.TextBox();
			this.btnDL = new System.Windows.Forms.Button();
			this.lblNovelTitle = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.btnNovelOpen = new System.Windows.Forms.Button();
			this.btnIniSelect = new System.Windows.Forms.Button();
			this.lblIniPath = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// tbNovelPath
			// 
			this.tbNovelPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbNovelPath.Location = new System.Drawing.Point(144, 76);
			this.tbNovelPath.Name = "tbNovelPath";
			this.tbNovelPath.Size = new System.Drawing.Size(349, 19);
			this.tbNovelPath.TabIndex = 30;
			// 
			// cboxListSelect
			// 
			this.cboxListSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboxListSelect.FormattingEnabled = true;
			this.cboxListSelect.Location = new System.Drawing.Point(7, 75);
			this.cboxListSelect.Name = "cboxListSelect";
			this.cboxListSelect.Size = new System.Drawing.Size(134, 20);
			this.cboxListSelect.TabIndex = 29;
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDown1.Enabled = false;
			this.numericUpDown1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numericUpDown1.Location = new System.Drawing.Point(262, 24);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
			this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(59, 23);
			this.numericUpDown1.TabIndex = 28;
			this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// lblTimeCount
			// 
			this.lblTimeCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblTimeCount.Location = new System.Drawing.Point(433, 30);
			this.lblTimeCount.Name = "lblTimeCount";
			this.lblTimeCount.Size = new System.Drawing.Size(60, 15);
			this.lblTimeCount.TabIndex = 25;
			this.lblTimeCount.Text = "00:00:00";
			this.lblTimeCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblStatus
			// 
			this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblStatus.Location = new System.Drawing.Point(7, 30);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(249, 15);
			this.lblStatus.TabIndex = 26;
			this.lblStatus.Text = "Status";
			// 
			// lblProgress
			// 
			this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblProgress.Location = new System.Drawing.Point(327, 30);
			this.lblProgress.Name = "lblProgress";
			this.lblProgress.Size = new System.Drawing.Size(100, 15);
			this.lblProgress.TabIndex = 27;
			this.lblProgress.Text = "0% (0/0)";
			this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tbURL
			// 
			this.tbURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbURL.Location = new System.Drawing.Point(7, 50);
			this.tbURL.Name = "tbURL";
			this.tbURL.Size = new System.Drawing.Size(486, 19);
			this.tbURL.TabIndex = 24;
			// 
			// btnDL
			// 
			this.btnDL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDL.Location = new System.Drawing.Point(499, 35);
			this.btnDL.Name = "btnDL";
			this.btnDL.Size = new System.Drawing.Size(88, 34);
			this.btnDL.TabIndex = 23;
			this.btnDL.Text = "ダウンロード";
			this.btnDL.UseVisualStyleBackColor = true;
			this.btnDL.Click += new System.EventHandler(this.btnDL_Click);
			// 
			// lblNovelTitle
			// 
			this.lblNovelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblNovelTitle.Location = new System.Drawing.Point(7, 100);
			this.lblNovelTitle.Name = "lblNovelTitle";
			this.lblNovelTitle.Size = new System.Drawing.Size(580, 17);
			this.lblNovelTitle.TabIndex = 22;
			this.lblNovelTitle.Text = "小説タイトル";
			// 
			// btnNovelOpen
			// 
			this.btnNovelOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNovelOpen.Location = new System.Drawing.Point(499, 73);
			this.btnNovelOpen.Name = "btnNovelOpen";
			this.btnNovelOpen.Size = new System.Drawing.Size(88, 23);
			this.btnNovelOpen.TabIndex = 33;
			this.btnNovelOpen.Text = "小説File選択...";
			this.btnNovelOpen.UseVisualStyleBackColor = true;
			this.btnNovelOpen.Click += new System.EventHandler(this.btnNovelOpen_Click);
			// 
			// btnIniSelect
			// 
			this.btnIniSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnIniSelect.Location = new System.Drawing.Point(499, 4);
			this.btnIniSelect.Name = "btnIniSelect";
			this.btnIniSelect.Size = new System.Drawing.Size(88, 23);
			this.btnIniSelect.TabIndex = 34;
			this.btnIniSelect.Text = "ini選択...";
			this.btnIniSelect.UseVisualStyleBackColor = true;
			this.btnIniSelect.Click += new System.EventHandler(this.btnIniSelect_Click);
			// 
			// lblIniPath
			// 
			this.lblIniPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblIniPath.AutoSize = true;
			this.lblIniPath.Location = new System.Drawing.Point(54, 9);
			this.lblIniPath.Name = "lblIniPath";
			this.lblIniPath.Size = new System.Drawing.Size(35, 12);
			this.lblIniPath.TabIndex = 31;
			this.lblIniPath.Text = "label1";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 12);
			this.label1.TabIndex = 32;
			this.label1.Text = "対象ini";
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(594, 121);
			this.Controls.Add(this.tbNovelPath);
			this.Controls.Add(this.cboxListSelect);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.lblTimeCount);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.lblProgress);
			this.Controls.Add(this.tbURL);
			this.Controls.Add(this.btnDL);
			this.Controls.Add(this.lblNovelTitle);
			this.Controls.Add(this.btnNovelOpen);
			this.Controls.Add(this.btnIniSelect);
			this.Controls.Add(this.lblIniPath);
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(1920, 160);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(610, 160);
			this.Name = "frmMain";
			this.Text = "小説ダウンローダーUI";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
			this.Load += new System.EventHandler(this.frmMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbNovelPath;
		private System.Windows.Forms.ComboBox cboxListSelect;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Label lblTimeCount;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Label lblProgress;
		private System.Windows.Forms.TextBox tbURL;
		private System.Windows.Forms.Button btnDL;
		private System.Windows.Forms.Label lblNovelTitle;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Button btnNovelOpen;
		private System.Windows.Forms.Button btnIniSelect;
		private System.Windows.Forms.Label lblIniPath;
		private System.Windows.Forms.Label label1;
	}
}

