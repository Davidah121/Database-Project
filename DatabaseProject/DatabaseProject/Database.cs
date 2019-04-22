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
        const string connectionString = "Data Source=NICK-Laptop;Initial Catalog=Zoo;Integrated Security=True";

        public static DataTable Query(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
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


        public static bool NonQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
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
