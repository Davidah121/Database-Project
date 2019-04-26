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
        #region Events


        private void Btn_expmode_submit_Click(object sender, RoutedEventArgs e)
        {
            string cmd = textbox_expmode_sql.Text;

            if (string.IsNullOrWhiteSpace(cmd))
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
                datagrid_expmode.ItemsSource = Database.Query(query)?.DefaultView;
            }
            else
            {
                if (Database.NonQuery(query))
                {
                    MessageBox.Show("Query executed successfully!");
                }
            }
        }
    }
}
