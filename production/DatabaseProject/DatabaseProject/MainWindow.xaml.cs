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
        SQLConnection connection;

        List<Grid> menus;

        public MainWindow()
        {
            InitializeComponent();


            // Initialize connection object. This is what we use to connect to the database
            connection = new SQLConnection();


            // Initialize list of all menus
            menus = new List<Grid>()
            {
                menu_main,
                menu_employees,
                menu_animals,
                menu_donations,
                menu_deliveries,
                menu_trans,
                menu_expmode
            };


            // Open the main menu
            OpenMenu(menu_main);


            // Connect to the database
            if (connection.Connect() == false)
            {
                MessageBox.Show("Could not connect to database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Disconnect from the database when the application is closing
            connection?.Disconnect();
        }


        private void OpenMenu(Grid menu)
        {
            if (menu == null)
            {
                return;
            }


            // Loop through all the menus. If the menu is the one specified, set it to visible. Otherwise, hidden
            foreach (Grid m in menus)
            {
                m.Visibility = m == menu ? Visibility.Visible : Visibility.Hidden;
            }
        }
    }
}
