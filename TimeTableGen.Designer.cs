namespace Timetablez
{
    partial class TimeTableGen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimeTableGen));
            txtLogBox = new TextBox();
            label1 = new Label();
            btnGenerate = new Button();
            tableLayoutPanel2 = new TableLayoutPanel();
            editorBox = new TextBox();
            richTextBox1 = new RichTextBox();
            label2 = new Label();
            label5 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            btnFixRooms = new Button();
            btnFixTeachers = new Button();
            btnFixStudents = new Button();
            label4 = new Label();
            tableLayoutPanel3 = new TableLayoutPanel();
            btnFixAll = new Button();
            btnPrintConflicts = new Button();
            btnWriteToSQL = new Button();
            splitContainer4 = new SplitContainer();
            btnStop = new Button();
            pictureBox2 = new PictureBox();
            label3 = new Label();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer4).BeginInit();
            splitContainer4.Panel1.SuspendLayout();
            splitContainer4.Panel2.SuspendLayout();
            splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // txtLogBox
            // 
            txtLogBox.Dock = DockStyle.Fill;
            txtLogBox.Location = new Point(3, 141);
            txtLogBox.Multiline = true;
            txtLogBox.Name = "txtLogBox";
            txtLogBox.ReadOnly = true;
            txtLogBox.ScrollBars = ScrollBars.Both;
            txtLogBox.Size = new Size(392, 467);
            txtLogBox.TabIndex = 0;
            txtLogBox.WordWrap = false;
            // 
            // label1
            // 
            label1.BackColor = Color.FromArgb(166, 143, 161);
            tableLayoutPanel2.SetColumnSpan(label1, 2);
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(791, 30);
            label1.TabIndex = 1;
            label1.Text = "Time Table Configuration";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnGenerate
            // 
            btnGenerate.BackColor = Color.FromArgb(163, 155, 161);
            btnGenerate.FlatStyle = FlatStyle.Flat;
            btnGenerate.Location = new Point(3, 155);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(194, 25);
            btnGenerate.TabIndex = 2;
            btnGenerate.Text = "Generate";
            btnGenerate.UseVisualStyleBackColor = false;
            btnGenerate.Click += GenerateBtn_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(editorBox, 1, 4);
            tableLayoutPanel2.Controls.Add(txtLogBox, 0, 4);
            tableLayoutPanel2.Controls.Add(richTextBox1, 0, 2);
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Controls.Add(label2, 0, 3);
            tableLayoutPanel2.Controls.Add(label5, 1, 3);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel1, 1, 1);
            tableLayoutPanel2.Controls.Add(label4, 0, 1);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 1, 2);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 5;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(797, 611);
            tableLayoutPanel2.TabIndex = 4;
            // 
            // editorBox
            // 
            editorBox.Dock = DockStyle.Fill;
            editorBox.Location = new Point(401, 141);
            editorBox.Multiline = true;
            editorBox.Name = "editorBox";
            editorBox.ReadOnly = true;
            editorBox.ScrollBars = ScrollBars.Both;
            editorBox.Size = new Size(393, 467);
            editorBox.TabIndex = 5;
            editorBox.WordWrap = false;
            // 
            // richTextBox1
            // 
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.Location = new Point(3, 71);
            richTextBox1.Multiline = false;
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(392, 34);
            richTextBox1.TabIndex = 3;
            richTextBox1.Text = "0";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // label2
            // 
            label2.BackColor = Color.WhiteSmoke;
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(3, 108);
            label2.Name = "label2";
            label2.Size = new Size(392, 30);
            label2.TabIndex = 2;
            label2.Text = "Generator Log";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            label5.BackColor = Color.WhiteSmoke;
            label5.BorderStyle = BorderStyle.FixedSingle;
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(401, 108);
            label5.Name = "label5";
            label5.Size = new Size(393, 30);
            label5.TabIndex = 3;
            label5.Text = "Editor Log";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Controls.Add(btnFixRooms, 0, 0);
            tableLayoutPanel1.Controls.Add(btnFixTeachers, 1, 0);
            tableLayoutPanel1.Controls.Add(btnFixStudents, 2, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(401, 33);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(393, 32);
            tableLayoutPanel1.TabIndex = 6;
            // 
            // btnFixRooms
            // 
            btnFixRooms.Dock = DockStyle.Fill;
            btnFixRooms.Location = new Point(3, 3);
            btnFixRooms.Name = "btnFixRooms";
            btnFixRooms.Size = new Size(124, 26);
            btnFixRooms.TabIndex = 0;
            btnFixRooms.Text = "Fix Rooms";
            btnFixRooms.UseVisualStyleBackColor = true;
            btnFixRooms.Click += btnFixRooms_Click;
            // 
            // btnFixTeachers
            // 
            btnFixTeachers.Dock = DockStyle.Fill;
            btnFixTeachers.Location = new Point(133, 3);
            btnFixTeachers.Name = "btnFixTeachers";
            btnFixTeachers.Size = new Size(124, 26);
            btnFixTeachers.TabIndex = 1;
            btnFixTeachers.Text = "Fix Teachers";
            btnFixTeachers.UseVisualStyleBackColor = true;
            btnFixTeachers.Click += btnFixTeachers_Click;
            // 
            // btnFixStudents
            // 
            btnFixStudents.Dock = DockStyle.Fill;
            btnFixStudents.Location = new Point(263, 3);
            btnFixStudents.Name = "btnFixStudents";
            btnFixStudents.Size = new Size(127, 26);
            btnFixStudents.TabIndex = 2;
            btnFixStudents.Text = "Fix Students";
            btnFixStudents.UseVisualStyleBackColor = true;
            btnFixStudents.Click += btnFixStudents_Click;
            // 
            // label4
            // 
            label4.Dock = DockStyle.Fill;
            label4.Location = new Point(3, 30);
            label4.Name = "label4";
            label4.Size = new Size(392, 38);
            label4.TabIndex = 4;
            label4.Text = "Max Conflicts Allowed";
            label4.TextAlign = ContentAlignment.BottomLeft;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 3;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel3.Controls.Add(btnFixAll, 0, 0);
            tableLayoutPanel3.Controls.Add(btnPrintConflicts, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(401, 71);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(393, 34);
            tableLayoutPanel3.TabIndex = 7;
            // 
            // btnFixAll
            // 
            btnFixAll.Dock = DockStyle.Fill;
            btnFixAll.Location = new Point(3, 3);
            btnFixAll.Name = "btnFixAll";
            btnFixAll.Size = new Size(124, 28);
            btnFixAll.TabIndex = 0;
            btnFixAll.Text = "Fix All Conflicts";
            btnFixAll.UseVisualStyleBackColor = true;
            btnFixAll.Click += btnFixAll_Click;
            // 
            // btnPrintConflicts
            // 
            btnPrintConflicts.Dock = DockStyle.Fill;
            btnPrintConflicts.Location = new Point(133, 3);
            btnPrintConflicts.Name = "btnPrintConflicts";
            btnPrintConflicts.Size = new Size(124, 28);
            btnPrintConflicts.TabIndex = 2;
            btnPrintConflicts.Text = "Print Conflicts";
            btnPrintConflicts.UseVisualStyleBackColor = true;
            btnPrintConflicts.Click += btnPrintConflicts_Click;
            // 
            // btnWriteToSQL
            // 
            btnWriteToSQL.BackColor = Color.FromArgb(163, 155, 161);
            btnWriteToSQL.Enabled = false;
            btnWriteToSQL.FlatStyle = FlatStyle.Flat;
            btnWriteToSQL.Location = new Point(3, 186);
            btnWriteToSQL.Name = "btnWriteToSQL";
            btnWriteToSQL.Size = new Size(194, 25);
            btnWriteToSQL.TabIndex = 4;
            btnWriteToSQL.Text = "Write To SQL";
            btnWriteToSQL.UseVisualStyleBackColor = false;
            btnWriteToSQL.Click += btnWriteToSQL_Click;
            // 
            // splitContainer4
            // 
            splitContainer4.Dock = DockStyle.Fill;
            splitContainer4.FixedPanel = FixedPanel.Panel1;
            splitContainer4.Location = new Point(0, 0);
            splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            splitContainer4.Panel1.BackColor = Color.FromArgb(166, 143, 161);
            splitContainer4.Panel1.Controls.Add(btnStop);
            splitContainer4.Panel1.Controls.Add(btnWriteToSQL);
            splitContainer4.Panel1.Controls.Add(pictureBox2);
            splitContainer4.Panel1.Controls.Add(label3);
            splitContainer4.Panel1.Controls.Add(btnGenerate);
            // 
            // splitContainer4.Panel2
            // 
            splitContainer4.Panel2.Controls.Add(tableLayoutPanel2);
            splitContainer4.Size = new Size(1001, 611);
            splitContainer4.SplitterDistance = 200;
            splitContainer4.TabIndex = 4;
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.FromArgb(163, 155, 161);
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.Location = new Point(3, 217);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(195, 28);
            btnStop.TabIndex = 5;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStop_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Dock = DockStyle.Top;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(0, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(200, 119);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 2;
            pictureBox2.TabStop = false;
            // 
            // label3
            // 
            label3.BackColor = Color.Gray;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(0, 122);
            label3.Name = "label3";
            label3.Size = new Size(200, 24);
            label3.TabIndex = 3;
            label3.Text = "Generator";
            label3.TextAlign = ContentAlignment.TopCenter;
            // 
            // TimeTableGen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1001, 611);
            Controls.Add(splitContainer4);
            Name = "TimeTableGen";
            Text = "Time Table Generator";
            WindowState = FormWindowState.Maximized;
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            splitContainer4.Panel1.ResumeLayout(false);
            splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer4).EndInit();
            splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Label label1;
        private Button btnGenerate;
        private TextBox txtLogBox;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label2;
        private RichTextBox richTextBox1;
        private Button btnWriteToSQL;
        private SplitContainer splitContainer4;
        private PictureBox pictureBox2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox editorBox;
        private TableLayoutPanel tableLayoutPanel1;
        private Button btnFixRooms;
        private TableLayoutPanel tableLayoutPanel3;
        private Button btnFixTeachers;
        private Button btnFixStudents;
        private Button btnFixAll;
        private Button btnPrintConflicts;
        private Button btnStop;
    }
}