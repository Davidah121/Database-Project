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
        #region Buttons

        private void Btn_habitat_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idVal = int.Parse(txt_habitat_id.Text);
                string habName = txt_habitat_name.Text;
                int humiVal = int.Parse(txt_habitat_humidity.Text);
                int tempVal = int.Parse(txt_habitat_temperature.Text);

                NonQuery("INSERT INTO HABITAT (habitat_id, habitat_name, humidity, temperature) " +
                    "VALUES ('" + idVal + "', '" + habName + "', '" + humiVal + "', '" + tempVal + "');");
            }
            catch
            {
                MessageBox.Show("One or more boxes are invalid because they are not numbers");
            }
        }

        private void Btn_habitat_get_Click(object sender, RoutedEventArgs e)
        {
            //If a text box is blank, do not include it in the search.
            try
            {
                int idVal, humiVal, tempVal;
                string habName;

                string query = "SELECT * FROM HABITAT WHERE ";

                if (txt_habitat_id.Text != "")
                {
                    idVal = int.Parse(txt_habitat_id.Text);
                    query += "habitat_id = " + idVal + ", ";
                }
                if (txt_habitat_name.Text != "")
                {
                    habName = txt_habitat_name.Text;
                    query += "habitat_name = " + habName + ", ";
                }
                if (txt_habitat_humidity.Text != "")
                {
                    humiVal = int.Parse(txt_habitat_humidity.Text);
                    query += "humidity = " + humiVal + ", ";
                }
                if (txt_habitat_temperature.Text != "")
                {
                    tempVal = int.Parse(txt_habitat_temperature.Text);
                    query += "temperature = '" + tempVal + "', ";
                }

                query += ";";
                query.Replace("'NULL'", "NULL");

                query = query.Remove(query.Length - 3, 2);

                datagrid_habitat.ItemsSource = Query(query)?.DefaultView;
            }
            catch
            {
                MessageBox.Show("One or more boxes are invalid because they are not numbers");
            }
        }

        private void Btn_habitat_update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idVal = int.Parse(txt_habitat_id.Text);
                string habName = txt_habitat_name.Text;
                int humiVal = int.Parse(txt_habitat_humidity.Text);
                int tempVal = int.Parse(txt_habitat_temperature.Text);

                NonQuery("UPDATE ANIMAL " +
                    "SET habitat_name='" + habName +
                    "', humidity='" + humiVal +
                    "', temperature='" + tempVal +
                    "' WHERE habitat_id='" + idVal + "';");
            }
            catch
            {
                MessageBox.Show("One or more boxes are invalid because they are not numbers");
            }
        }

        private void Btn_habitat_delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this record?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    int idVal = int.Parse(txt_habitat_id.Text);

                    NonQuery("DELETE FROM HABITAT WHERE habitat_id='" + idVal + "';");
                }
                catch
                {
                    MessageBox.Show("The Habitat Id is not a number.");
                }
            }
        }
        #endregion
    }
}
