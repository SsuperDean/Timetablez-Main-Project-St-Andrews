using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Timetablez.Classes
{
    internal class Connections
    {
        public static bool CheckConnection(string connectionString)
        {
            SqlConnection con = null;
            try
            {
                using (con = new SqlConnection(connectionString))
                {
                    con.Open(); // Will throw if can't connect
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
