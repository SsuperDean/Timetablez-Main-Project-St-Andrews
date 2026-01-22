// ================================
// File: DataRead.cs
// ================================
using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;

namespace Timetablez.Classes
{
    public static class DataRead
    {
        // =============================================================================================
        // GET DATA AS DATA TABLE
        // =============================================================================================
        public static DataTable GetDataAsDataTable
            (
            string conStr,
            string tableName=null,
            string sqlQuery =null, 
            string storedProcedure = null, 
            Dictionary<string, object>? parameters = null
            )
        {

            // Assign the tableName parameter as table name. Could have been given a generic name.
            DataTable DBtable = new DataTable();

            // Either must be provided
            if (string.IsNullOrEmpty(storedProcedure) && string.IsNullOrEmpty(sqlQuery))            
            { 
                return DBtable;
            }

            using (SqlConnection sqlCon = new SqlConnection(conStr))
            {
                try
                {
                    sqlCon.Open();                                                           
                    
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        // Standard ADO.Net
                        cmd.Connection = sqlCon;
                        
                        if (storedProcedure != null)
                        {
                            // which stored procedure to run
                            cmd.CommandText = storedProcedure;
                            cmd.CommandType = CommandType.StoredProcedure;
                        }
                        else
                        {
                            // what query to run
                            cmd.CommandText = sqlQuery;
                            cmd.CommandType = CommandType.Text;
                        }

                        // Always add parameters, regardless of query type
                        // A query example: SELECT * FROM ATABLE WHERE ID=@ID
                        if (parameters != null)
                        {
                            foreach (var p in parameters)
                            {
                                cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
                            }
                        }

                        // All data processing happens in data grid 
                        // So no need for adapter
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DBtable.Load(reader);
                        }

                    }
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            }
            return DBtable;
        }

        // =============================================================================================
        // GET DATA AS LIST
        // =============================================================================================
        public static List<T> GetDataAsList<T>
            (
            string conStr, 
            string sqlQuery
            ) where T : new()
        {

            List<T> result = new List<T>();

            using (SqlConnection sqlCon = new SqlConnection(conStr))
            {
                try
                {
                    sqlCon.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, sqlCon))
                    {
                        using (SqlDataReader rs = cmd.ExecuteReader())
                        {

                            //Get all the public instance properties of the type T
                            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                            if (rs != null)
                            {
                                while (rs.Read())
                                {
                                    T obj = new T();

                                    foreach (var prop in properties)
                                    {
                                        if (!rs.HasColumn(prop.Name)) continue;

                                        object value = rs[prop.Name];
                                        if (value != DBNull.Value)
                                        {
                                            prop.SetValue(obj, Convert.ChangeType(value, prop.PropertyType));
                                        }
                                    }

                                    result.Add(obj);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            }
            return result;
        }

        // =============================================================================================
        // EXTENSION METHOD TO CHECK IF READER HAS A COLUMN
        // =============================================================================================
        private static bool HasColumn(this SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).ToLower() == columnName.ToLower())
                    return true;
            }
            return false;
        }


        public static void ExecuteNonQuery
            (
            string conStr,
            string sqlCommand,
            Dictionary<string, object>? parameters = null,
            bool isStoredProcedure = false
            )
        {

            using (SqlConnection sqlCon = new SqlConnection(conStr))
            {
                try
                {
                    sqlCon.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlCommand, sqlCon))
                    {
                        if (isStoredProcedure)
                            cmd.CommandType = CommandType.StoredProcedure;

                        if (parameters != null)
                        {
                            foreach (var p in parameters)
                            {
                                cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
                            }
                        }

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            }
        }

        private static void LogError(Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}

