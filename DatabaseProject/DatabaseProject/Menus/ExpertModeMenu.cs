using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseProject
{
    partial class MainWindow : Window
    {
        const string connectionString = "Data Source=DESKTOP-V87K7CH;Initial Catalog=Zoo;Integrated Security=True";

        #region Events


        private void Btn_expmode_submit_Click(object sender, RoutedEventArgs e)
        {
            string cmd = textbox_expmode_sql.Text;

            if (String.IsNullOrWhiteSpace(cmd))
            {
                return;
            }

            Submit(cmd);
        }


        #endregion


        private void Submit(string query)
        {
            if (query.StartsWith("select", StringComparison.OrdinalIgnoreCase))
            {
                datagrid_expmode.ItemsSource = Query(query)?.DefaultView;
            }
            else
            {
                NonQuery(query);
            }
        }


        private DataTable Query(string query)
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


        private bool NonQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Non-query executed successfully!");
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