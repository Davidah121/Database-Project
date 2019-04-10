using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        }

        private void pretendImNotHear()
        {
            string connectionString;
            SqlConnection cnn;

            connectionString = @"Data Source=MSI; Initial Catalog=SmallAnimalDB;Integrated Security=true";

            cnn = new SqlConnection(connectionString);

            try
            {

                int id = Convert.ToInt32(Item1Info.Text);
                string name = Item2Info.Text;

                cnn.Open();
                MessageBox.Show("Connection Open!");

                string output = "";

                SqlCommand com;
                SqlDataReader daRead;
                SqlDataAdapter adapter = new SqlDataAdapter();

                com = new SqlCommand("Set Identity_Insert Animals ON", cnn);
                com.ExecuteNonQuery();

                com = new SqlCommand("Insert into Animals (SpeciesID, SpeciesName) values(" + id + ", '" + name + "')", cnn);


                adapter.InsertCommand = com;
                adapter.InsertCommand.ExecuteNonQuery();

                com = new SqlCommand("Select * from Animals", cnn);

                daRead = com.ExecuteReader();

                while (daRead.Read())
                {
                    output += daRead.GetValue(0) + " - " + daRead.GetValue(1) + "\n";
                }

                MessageBox.Show("All the species in the table: \n" + output);

                com.Dispose();
                cnn.Close();
            }
            catch (SqlException q)
            {
                for (int i = 0; i < q.Errors.Count; i++)
                    MessageBox.Show(q.Errors[i].ToString());
            }
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            SQLCode.connect();
            string output = SQLCode.executeCommand("Select * from Animals");
            string[] output2 = output.Split((char)28);

            for(int i=0; i<output2.Length; i++)
            {
                MessageBox.Show(output2[i]);
            }
            
            SQLCode.disconnect();
        }
    }
}
