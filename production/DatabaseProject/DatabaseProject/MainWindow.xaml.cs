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
    public partial class MainWindow : Window
    {
        UIElement currentMenu = null;

        public MainWindow()
        {
            InitializeComponent();

            currentMenu = menu_main;
        }


        private void OpenMenu(UIElement menu)
        {
            if (menu == null)
            {
                return;
            }

            currentMenu.Visibility = Visibility.Hidden;

            menu.Visibility = Visibility.Visible;

            currentMenu = menu;
        }
    }
}
