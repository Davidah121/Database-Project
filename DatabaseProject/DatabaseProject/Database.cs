using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DatabaseProject
{
    class Database
    {
        private static readonly string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

        public static DataTable Query(string query, params (string key, object value)[] p)
        {
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    // Add parameters
                    if (p.Length > 0)
                    {
                        SqlParameter[] sqlParameters = p.Select(x => new SqlParameter(x.key, x.value)).ToArray();
                        command.Parameters.AddRange(sqlParameters);
                    }

                    // Open the connection to the database
                    connection.Open();


                    // Create an adapter object. This will help us retrieve the rows from the database
                    SqlDataAdapter adapter = new SqlDataAdapter(command);


                    // Create an new table
                    DataTable table = new DataTable();


                    // Populate the table with the rows from the database
                    adapter.Fill(table);

                    return table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Something went wrong.");
                    return null;
                }
            }
        }


        public static bool NonQuery(string query, params (string key, object value)[] p)
        {
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    // Add parameters
                    if (p.Length > 0)
                    {
                        SqlParameter[] sqlParameters = p.Select(x => new SqlParameter(x.key, x.value)).ToArray();
                        command.Parameters.AddRange(sqlParameters);
                    }

                    // Open the connection to the database
                    connection.Open();


                    // Return true if at least 1 row was affected.
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Something went wrong.");
                    return false;
                }
            }
        }
    }
}
