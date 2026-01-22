using Microsoft.Data.SqlClient;


namespace Timetablez.Classes
{
    public class SQLTablesHelper
    {
        public static void CopyTable(string connectionString, string sourceTable, string destinationTable, string procedureName)
        {

            if (string.IsNullOrEmpty(sourceTable) || string.IsNullOrEmpty(destinationTable) || string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("Table or Connection Is Missing!",
                    "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to copy " + sourceTable + " to " + destinationTable + " ?",
                "Confirm Table Copy", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (result != DialogResult.Yes)
            {
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tblSource", sourceTable);
                    cmd.Parameters.AddWithValue("@tblDestination", destinationTable);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show(
                "Copy " + sourceTable + " to " + destinationTable + " completed successfully!",
                "Table Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
