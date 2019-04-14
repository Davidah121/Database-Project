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

namespace DatabaseProject
{
    partial class MainWindow : Window 
    {
        #region Events


        private void Textbox_expmode_sql_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Btn_expmode_submit_Click(object sender, RoutedEventArgs e)
        {
            string cmd = textbox_expmode_sql.Text;

            if (String.IsNullOrWhiteSpace(cmd))
            {
                return;
            }

            DataTable table = connection.Query(cmd);

            if (table == null)
            {
                MessageBox.Show("The query was unsuccessful.", "Unsuccessful Query", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            datagrid_expmode.ItemsSource = table?.DefaultView;
        }


        #endregion
    }
}
