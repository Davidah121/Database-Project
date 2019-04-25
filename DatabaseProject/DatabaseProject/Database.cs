using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseProject
{
    class Database
    {
        const string connectionString = "Data Source=DESKTOP-V87K7CH;Initial Catalog=Zoo;Integrated Security=True";

        public static DataTable Query(string query, params (string key, object value)[] p)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    if (p.Length > 0)
                    {
                        SqlParameter[] sqlParameters = p.Select(x => new SqlParameter(x.key, x.value)).ToArray();
                        command.Parameters.AddRange(sqlParameters);
                    }

                    connection.Open();
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
