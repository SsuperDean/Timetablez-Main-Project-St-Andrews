namespace Timetablez
{
    partial class MainForm
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
            menuStrip1 = new MenuStrip();
            appsToolStripMenuItem = new ToolStripMenuItem();
            dataManagerToolStripMenuItem = new ToolStripMenuItem();
            timetableGeneratorToolStripMenuItem = new ToolStripMenuItem();
            timetableBrowserToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { appsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // appsToolStripMenuItem
            // 
            appsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { dataManagerToolStripMenuItem, timetableGeneratorToolStripMenuItem, timetableBrowserToolStripMenuItem });
            appsToolStripMenuItem.Name = "appsToolStripMenuItem";
            appsToolStripMenuItem.Size = new Size(46, 20);
            appsToolStripMenuItem.Text = "Apps";
            // 
            // dataManagerToolStripMenuItem
            // 
            dataManagerToolStripMenuItem.Name = "dataManagerToolStripMenuItem";
            dataManagerToolStripMenuItem.Size = new Size(181, 22);
            dataManagerToolStripMenuItem.Text = "Data Manager";
            dataManagerToolStripMenuItem.Click += dataManagerToolStripMenuItem_Click;
            // 
            // timetableGeneratorToolStripMenuItem
            // 
            timetableGeneratorToolStripMenuItem.Name = "timetableGeneratorToolStripMenuItem";
            timetableGeneratorToolStripMenuItem.Size = new Size(181, 22);
            timetableGeneratorToolStripMenuItem.Text = "Timetable Generator";
            timetableGeneratorToolStripMenuItem.Click += timetableGeneratorToolStripMenuItem_Click;
            // 
            // timetableBrowserToolStripMenuItem
            // 
            timetableBrowserToolStripMenuItem.Name = "timetableBrowserToolStripMenuItem";
            timetableBrowserToolStripMenuItem.Size = new Size(181, 22);
            timetableBrowserToolStripMenuItem.Text = "Timetable Browser";
            timetableBrowserToolStripMenuItem.Click += timetableBrowserToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(menuStrip1);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "Main";
            WindowState = FormWindowState.Maximized;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem appsToolStripMenuItem;
        private ToolStripMenuItem dataManagerToolStripMenuItem;
        private ToolStripMenuItem timetableGeneratorToolStripMenuItem;
        private ToolStripMenuItem timetableBrowserToolStripMenuItem;
    }
}