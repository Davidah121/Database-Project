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

        private void OpenMainMenu(object sender, RoutedEventArgs e)
        {
            OpenMenu(menu_main);
        }


        #region Menu Buttons

        private void Btn_open_emp_Click(object sender, RoutedEventArgs e)
        {
            OpenMenu(menu_employees);
        }

        private void Btn_open_animals_Click(object sender, RoutedEventArgs e)
        {
            OpenMenu(menu_animals);
        }

        private void Btn_open_trans_Click(object sender, RoutedEventArgs e)
        {
            OpenMenu(menu_trans);
        }

        private void Btn_open_expmode_Click(object sender, RoutedEventArgs e)
        {
            OpenMenu(menu_expmode);
        }

        private void Btn_open_deliveries_Click(object sender, RoutedEventArgs e)
        {
            OpenMenu(menu_deliveries);
        }

        private void Btn_open_donations_Click(object sender, RoutedEventArgs e)
        {
            OpenMenu(menu_donations);
        }

        #endregion
    }
}
