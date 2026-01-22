using System;
using System.Data;
using Microsoft.Data.SqlClient;


/* The concept is to take a all the data from datagridview as a datatable. 
 * Create in the same session a sql global temp table using guid for unique naming
 * with the same schema as the target table. 
 * Use the stored procedure to truncate the target table and insert everything from the temp table.
 * The temp table is ##global and not #local to be visible across multiple sessions. 
 */

namespace Timetablez.Classes
{
    public static class DataBulkUpdate
    {
        public static void BulkUpdateAllData(
            string connectionString,
            string tableName,
            DataTable data)
        {
            var confirmation = MessageBox.Show(
                $"Are you sure you want to replace all data in {tableName}?",
                "Confirm Replace",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirmation != DialogResult.Yes)
                return;

            if (data == null || data.Rows.Count == 0)
                throw new ArgumentException("The DataTable is empty.");

            try
            {
                using (var sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    // Example: ##Temp_ExamBoards_c9fb7714781243ab94ad5ab59a60ce11
                    string tempTable = $"##Temp_{tableName}_{Guid.NewGuid():N}";

                    // This is never true and only the schema will be copied.
                    string createSql = $"SELECT * INTO {tempTable} FROM {tableName} WHERE 1 = 0;";

                    // Execution Session 1: Create a temp table with the same schema
                    using (SqlCommand createCmd = new SqlCommand(createSql, sqlConn))
                        createCmd.ExecuteNonQuery();

                    // Execution Session 2: Bulk copy new data into temp table
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConn))
                    {
                        bulkCopy.DestinationTableName = tempTable;
                        bulkCopy.WriteToServer(data);
                    }

                    // Execution Session 3: Call the stored procedure
                    using (var command = new SqlCommand("spCopyTable", sqlConn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@tblDestination", tableName);
                        command.Parameters.AddWithValue("@tblSource", tempTable);
                        command.ExecuteNonQuery();
                    }

                    // Execution Session 4: Drop the temp table for house keeping
                    using (var dropCmd = new SqlCommand($"DROP TABLE {tempTable};", sqlConn))
                        dropCmd.ExecuteNonQuery();

                    MessageBox.Show(
                        $"{tableName} replaced successfully!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error while replacing data: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
