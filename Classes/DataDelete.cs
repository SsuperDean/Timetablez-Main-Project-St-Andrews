using Microsoft.Data.SqlClient;

namespace Timetablez.Classes
{
    public static class DataDelete
    {
        public static void DeleteSelectedRows(
            string connectionString, 
            string tableName, 
            List<object> idsToDelete, 
            string keyColumn)
        {
            DialogResult result = MessageBox.Show(
                $"{idsToDelete.Count} item(s) are marked for deletion. Would you like to proceed?",
                "Confirm Delete",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (result != DialogResult.Yes)
                return;

            int itemsDeleted = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (var id in idsToDelete)
                    {
                        string deleteQuery = $"DELETE FROM {tableName} WHERE {keyColumn} = @Key";
                        using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@Key", id);
                            cmd.ExecuteNonQuery();
                            itemsDeleted += 1;
                        }
                    }
                }

                MessageBox.Show(
                    $"{itemsDeleted} item(s) deleted successfully!",
                    "Delete Confirmation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

