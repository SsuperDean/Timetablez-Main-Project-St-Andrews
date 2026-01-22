namespace Timetablez
{
    partial class TimetableBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimetableBrowser));
            tcBrowser = new TabControl();
            tabSchedule = new TabPage();
            splitScheduleMain = new SplitContainer();
            trackBar1 = new TrackBar();
            cboDataSource = new ComboBox();
            lblPage = new Label();
            pbLogo = new PictureBox();
            splitSchedule = new SplitContainer();
            tlpInfo = new TableLayoutPanel();
            dgvDetails = new DataGridView();
            dgvErrors = new DataGridView();
            tcBrowser.SuspendLayout();
            tabSchedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitScheduleMain).BeginInit();
            splitScheduleMain.Panel1.SuspendLayout();
            splitScheduleMain.Panel2.SuspendLayout();
            splitScheduleMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbLogo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitSchedule).BeginInit();
            splitSchedule.Panel2.SuspendLayout();
            splitSchedule.SuspendLayout();
            tlpInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetails).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvErrors).BeginInit();
            SuspendLayout();
            // 
            // tcBrowser
            // 
            tcBrowser.Controls.Add(tabSchedule);
            tcBrowser.Dock = DockStyle.Fill;
            tcBrowser.Location = new Point(0, 0);
            tcBrowser.Name = "tcBrowser";
            tcBrowser.SelectedIndex = 0;
            tcBrowser.Size = new Size(1036, 723);
            tcBrowser.SizeMode = TabSizeMode.FillToRight;
            tcBrowser.TabIndex = 4;
            // 
            // tabSchedule
            // 
            tabSchedule.Controls.Add(splitScheduleMain);
            tabSchedule.Location = new Point(4, 24);
            tabSchedule.Name = "tabSchedule";
            tabSchedule.Size = new Size(1028, 695);
            tabSchedule.TabIndex = 1;
            tabSchedule.Text = "Schedule";
            tabSchedule.UseVisualStyleBackColor = true;
            // 
            // splitScheduleMain
            // 
            splitScheduleMain.Dock = DockStyle.Fill;
            splitScheduleMain.FixedPanel = FixedPanel.Panel1;
            splitScheduleMain.IsSplitterFixed = true;
            splitScheduleMain.Location = new Point(0, 0);
            splitScheduleMain.Name = "splitScheduleMain";
            // 
            // splitScheduleMain.Panel1
            // 
            splitScheduleMain.Panel1.BackColor = Color.FromArgb(121, 145, 126);
            splitScheduleMain.Panel1.Controls.Add(trackBar1);
            splitScheduleMain.Panel1.Controls.Add(cboDataSource);
            splitScheduleMain.Panel1.Controls.Add(lblPage);
            splitScheduleMain.Panel1.Controls.Add(pbLogo);
            // 
            // splitScheduleMain.Panel2
            // 
            splitScheduleMain.Panel2.Controls.Add(splitSchedule);
            splitScheduleMain.Size = new Size(1028, 695);
            splitScheduleMain.SplitterDistance = 200;
            splitScheduleMain.TabIndex = 6;
            // 
            // trackBar1
            // 
            trackBar1.LargeChange = 20;
            trackBar1.Location = new Point(3, 178);
            trackBar1.Maximum = 200;
            trackBar1.Minimum = 120;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(194, 45);
            trackBar1.SmallChange = 20;
            trackBar1.TabIndex = 4;
            trackBar1.TickFrequency = 20;
            trackBar1.Value = 120;

            trackBar1.MouseUp += trackBar1_MouseUp;
            // 
            // cboDataSource
            // 
            cboDataSource.BackColor = Color.FromArgb(121, 145, 126);
            cboDataSource.FlatStyle = FlatStyle.Flat;
            cboDataSource.FormattingEnabled = true;
            cboDataSource.Location = new Point(0, 149);
            cboDataSource.Name = "cboDataSource";
            cboDataSource.Size = new Size(200, 23);
            cboDataSource.TabIndex = 3;
            cboDataSource.SelectedIndexChanged += cboDataSource_SelectedIndexChanged;
            // 
            // lblPage
            // 
            lblPage.BackColor = Color.Gray;
            lblPage.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPage.ForeColor = Color.Black;
            lblPage.Location = new Point(3, 122);
            lblPage.Name = "lblPage";
            lblPage.Size = new Size(194, 24);
            lblPage.TabIndex = 2;
            lblPage.Text = "Browser";
            lblPage.TextAlign = ContentAlignment.TopCenter;
            // 
            // pbLogo
            // 
            pbLogo.Dock = DockStyle.Top;
            pbLogo.Image = (Image)resources.GetObject("pbLogo.Image");
            pbLogo.Location = new Point(0, 0);
            pbLogo.Name = "pbLogo";
            pbLogo.Size = new Size(200, 119);
            pbLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pbLogo.TabIndex = 1;
            pbLogo.TabStop = false;
            // 
            // splitSchedule
            // 
            splitSchedule.Dock = DockStyle.Fill;
            splitSchedule.FixedPanel = FixedPanel.Panel2;
            splitSchedule.Location = new Point(0, 0);
            splitSchedule.Name = "splitSchedule";
            // 
            // splitSchedule.Panel2
            // 
            splitSchedule.Panel2.Controls.Add(tlpInfo);
            splitSchedule.Size = new Size(824, 695);
            splitSchedule.SplitterDistance = 428;
            splitSchedule.TabIndex = 4;
            // 
            // tlpInfo
            // 
            tlpInfo.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tlpInfo.ColumnCount = 1;
            tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpInfo.Controls.Add(dgvDetails, 0, 0);
            tlpInfo.Controls.Add(dgvErrors, 0, 1);
            tlpInfo.Dock = DockStyle.Fill;
            tlpInfo.Location = new Point(0, 0);
            tlpInfo.Name = "tlpInfo";
            tlpInfo.Padding = new Padding(5);
            tlpInfo.RowCount = 2;
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 57.461647F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 42.538353F));
            tlpInfo.Size = new Size(392, 695);
            tlpInfo.TabIndex = 1;
            // 
            // dgvDetails
            // 
            dgvDetails.AllowUserToAddRows = false;
            dgvDetails.AllowUserToDeleteRows = false;
            dgvDetails.BackgroundColor = Color.WhiteSmoke;
            dgvDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetails.Dock = DockStyle.Fill;
            dgvDetails.Location = new Point(9, 9);
            dgvDetails.Name = "dgvDetails";
            dgvDetails.ReadOnly = true;
            dgvDetails.RowHeadersWidth = 82;
            dgvDetails.Size = new Size(374, 385);
            dgvDetails.TabIndex = 1;
            // 
            // dgvErrors
            // 
            dgvErrors.AllowUserToAddRows = false;
            dgvErrors.AllowUserToDeleteRows = false;
            dgvErrors.AllowUserToOrderColumns = true;
            dgvErrors.BackgroundColor = Color.WhiteSmoke;
            dgvErrors.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvErrors.Dock = DockStyle.Fill;
            dgvErrors.Location = new Point(9, 401);
            dgvErrors.Name = "dgvErrors";
            dgvErrors.ReadOnly = true;
            dgvErrors.RowHeadersWidth = 82;
            dgvErrors.Size = new Size(374, 285);
            dgvErrors.TabIndex = 0;
            // 
            // TimetableBrowser
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1036, 723);
            Controls.Add(tcBrowser);
            Name = "TimetableBrowser";
            Text = "TimetableBrowser";
            WindowState = FormWindowState.Maximized;
            Load += TimetableBrowser_Load;
            tcBrowser.ResumeLayout(false);
            tabSchedule.ResumeLayout(false);
            splitScheduleMain.Panel1.ResumeLayout(false);
            splitScheduleMain.Panel1.PerformLayout();
            splitScheduleMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitScheduleMain).EndInit();
            splitScheduleMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbLogo).EndInit();
            splitSchedule.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitSchedule).EndInit();
            splitSchedule.ResumeLayout(false);
            tlpInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDetails).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvErrors).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tcBrowser;
        private TabPage tabSchedule;
        internal SplitContainer splitScheduleMain;
        private SplitContainer splitSchedule;
        private DataGridView dgvErrors;
        private TableLayoutPanel tlpInfo;
        private DataGridView dgvDetails;
        private PictureBox pbLogo;
        private Label lblPage;
        private ComboBox cboDataSource;
        private TrackBar trackBar1;
    }
}