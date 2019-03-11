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

namespace GuiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("Item1 Text Field: " + Item1Info.Text);
            MessageBox.Show("Item2 Text Field: " + Item2Info.Text);
            MessageBox.Show("Item3 Text Field: " + Item3Info.Text);
            MessageBox.Show("Item4 Text Field: " + Item4Info.Text);
            MessageBox.Show("Item5 Text Field: " + Item5Info.Text);
            MessageBox.Show("Item6 Text Field: " + Item6Info.Text);
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
