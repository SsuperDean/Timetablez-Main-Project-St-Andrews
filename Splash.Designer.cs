namespace Timetablez
{
    partial class Splash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Splash));
            btnSplashGo = new Button();
            splitContainer1 = new SplitContainer();
            btnCancel = new Button();
            txtPassword = new TextBox();
            txtUserName = new TextBox();
            lblPassword = new Label();
            lblUserName = new Label();
            lblSplashMessage = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // btnSplashGo
            // 
            btnSplashGo.Location = new Point(402, 6);
            btnSplashGo.Name = "btnSplashGo";
            btnSplashGo.Size = new Size(70, 53);
            btnSplashGo.TabIndex = 0;
            btnSplashGo.Text = "Connect";
            btnSplashGo.UseVisualStyleBackColor = true;
            btnSplashGo.Click += cmdSplashGo_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel2;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackgroundImage = (Image)resources.GetObject("splitContainer1.Panel1.BackgroundImage");
            splitContainer1.Panel1.BackgroundImageLayout = ImageLayout.Zoom;
            splitContainer1.Panel1.Controls.Add(btnCancel);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = Color.WhiteSmoke;
            splitContainer1.Panel2.Controls.Add(txtPassword);
            splitContainer1.Panel2.Controls.Add(txtUserName);
            splitContainer1.Panel2.Controls.Add(lblPassword);
            splitContainer1.Panel2.Controls.Add(lblUserName);
            splitContainer1.Panel2.Controls.Add(lblSplashMessage);
            splitContainer1.Panel2.Controls.Add(btnSplashGo);
            splitContainer1.Size = new Size(480, 501);
            splitContainer1.SplitterDistance = 400;
            splitContainer1.TabIndex = 3;
            // 
            // btnCancel
            // 
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Location = new Point(452, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(23, 23);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "X";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += cmdCancel_Click;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.White;
            txtPassword.Location = new Point(76, 35);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(320, 23);
            txtPassword.TabIndex = 5;
            // 
            // txtUserName
            // 
            txtUserName.BackColor = Color.White;
            txtUserName.Location = new Point(76, 6);
            txtUserName.Name = "txtUserName";
            txtUserName.Size = new Size(320, 23);
            txtUserName.TabIndex = 4;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(7, 37);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(57, 15);
            lblPassword.TabIndex = 3;
            lblPassword.Text = "Password";
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Location = new Point(5, 9);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(65, 15);
            lblUserName.TabIndex = 2;
            lblUserName.Text = "User Name";
            // 
            // lblSplashMessage
            // 
            lblSplashMessage.BackColor = Color.Gainsboro;
            lblSplashMessage.Location = new Point(7, 66);
            lblSplashMessage.Name = "lblSplashMessage";
            lblSplashMessage.Size = new Size(465, 22);
            lblSplashMessage.TabIndex = 1;
            lblSplashMessage.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Splash
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(480, 501);
            ControlBox = false;
            Controls.Add(splitContainer1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Splash";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Load += Splash_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button btnSplashGo;
        private SplitContainer splitContainer1;
        private Label lblSplashMessage;
        private TextBox txtUserName;
        private Label lblPassword;
        private Label lblUserName;
        private TextBox txtPassword;
        private Button btnCancel;
    }
}