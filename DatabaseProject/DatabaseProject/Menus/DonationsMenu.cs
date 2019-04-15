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


        private void Btn_new_donation_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("New donation");

            DataTable table = Query($"select * from Addresses where CustomerID = {textbox_donator_id.Text}");

            if (table == null)
            {
                return;
            }

            MessageBox.Show(table.Rows[0]["Line1"].ToString());
        }

        private void Btn_view_donation_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("View donation");
        }

        private void Btn_update_donation_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Update donation");
        }

        private void Btn_delete_donation_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete donation");
        }


        #endregion
    }
}
