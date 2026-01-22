using System.Data;
using Timetablez.Models;
using Timetablez.Classes;

namespace Timetablez
{
    public partial class Splash : Form
    {
        private static string conString = "Data Source=XXXXX; Initial Catalog=Timetablez; User ID=sa; Password=XXXXX; Encrypt=False; TrustServerCertificate=True;";


        private bool connectionOk = false;
        private bool userVerified = false;
        private string key = "T1m3Tabl3z_Key!";

        public Splash()
        {
            InitializeComponent();
            txtPassword.PasswordChar = '*';
        }

        private void Splash_Load(object sender, EventArgs e)
        {

            // ====================================
            // Bypass splash for testing
            //userVerified = true;
            //AppGlobals.currentUser = "Admin";
            //AppGlobals.dbLocal = conString;
            //this.Close();
            // ====================================

            this.StartPosition = FormStartPosition.CenterScreen;

            // === Check database connection first ===
            if (Connections.CheckConnection(conString))
            {
                connectionOk = true;
                AppGlobals.dbLocal = conString;
                lblSplashMessage.Text = "Connected successfully. Please log in.";
                btnSplashGo.Text = "Login";
                txtUserName.Text = "Admin";
                txtPassword.Text = "admin1234@";
            }
            else
            {
                connectionOk = false;
                lblSplashMessage.Text = "Unable to connect to the server.";
                btnSplashGo.Visible = false;
                lblUserName.Visible = false;
                lblPassword.Visible = false;
                txtUserName.Visible = false;
                txtPassword.Visible = false;
            }
        }

        // One multi-purpose button
        private void cmdSplashGo_Click(object sender, EventArgs e)
        {

            // User login
            if (btnSplashGo.Text == "Login")
            {
                if (!connectionOk)
                {
                    lblSplashMessage.Text = "No connection to database.";
                    return;
                }

                string user = txtUserName.Text.Trim();
                string pass = txtPassword.Text.Trim();

                if (user == "" || pass == "")
                {
                    lblSplashMessage.Text = "Please enter both username and password.";
                    return;
                }

                // Encrypt entered password using Vernam cipher
                string encrypted = Ciphers.VernamEncrypt(pass, key);

                // Build SQL query and parameters to avoid sql injection
                string query = "SELECT * FROM dbo.SystemUser WHERE UserName = @UserName";
                var parameters = new Dictionary<string, object>()
                {                    
                    {"@UserName", user.Trim() } // To sanitise entry
                };

                // Read table using the shared DataRead utility. Table name could be generic but sql table names
                // is used for error tracing.
                DataTable dt = DataRead.GetDataAsDataTable(conString, "SystemUser", query, null, parameters);

                if (dt.Rows.Count == 0)
                {
                    lblSplashMessage.Text = "User not found.";
                    return;
                }

                // Only read the first row if multiple rows returned
                DataRow row = dt.Rows[0];
                string storedPass = row["Password"] != DBNull.Value ? row["Password"].ToString() : "";

                // the sql table stores if the user has access and the password must match
                if (encrypted == storedPass)
                {
                    userVerified = true;
                    AppGlobals.currentUser = user;

                    lblSplashMessage.Text = $"Welcome {user}!";
                    btnSplashGo.Text = "Continue";
                }
                else
                {
                    lblSplashMessage.Text = "Invalid password or access denied.";
                }

                return;
            }

            // Continue will close splash and pass control to MainForm
            if (btnSplashGo.Text == "Continue" && userVerified)
            {
                this.Close();
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
            return;
        }
    }
}
