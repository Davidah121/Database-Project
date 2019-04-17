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

namespace DatabaseProject
{
    partial class MainWindow : Window 
    {
        #region Button Events


        private void Btn_new_trans_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("New transaction!");
        }

        private void Btn_view_trans_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Viewing transaction!");
        }

        private void Btn_update_trans_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Updating transaction!");
        }

        private void Btn_delete_trans_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Deleting transaction!");
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        #endregion
    }
}
