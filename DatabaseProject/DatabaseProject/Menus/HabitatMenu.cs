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

                string query = "INSERT INTO HABITAT VALUES(@ID, @HABNAME, @HUMI, @TEMP);";
                Database.NonQuery(query, ("@ID", idVal), ("@HABNAME", habName), ("@HUMI", humiVal),
                                        ("@TEMP", tempVal));
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
                bool hasParams = false;
                int idVal, humiVal, tempVal;
                string habName;

                string defaultQuery = "SELECT * FROM HABITAT;";

                string query = "SELECT * FROM HABITAT WHERE ";

                if (txt_habitat_id.Text != "")
                {
                    hasParams = true;
                    idVal = int.Parse(txt_habitat_id.Text);
                    query += "habitat_id = " + idVal + " and ";
                }
                if (txt_habitat_name.Text != "")
                {
                    hasParams = true;
                    habName = txt_habitat_name.Text;
                    query += "habitat_name = " + habName + " and ";
                }
                if (txt_habitat_humidity.Text != "")
                {
                    hasParams = true;
                    humiVal = int.Parse(txt_habitat_humidity.Text);
                    query += "humidity = " + humiVal + " and ";
                }
                if (txt_habitat_temperature.Text != "")
                {
                    tempVal = int.Parse(txt_habitat_temperature.Text);
                    query += "temperature = '" + tempVal + "' and ";
                }

                query += ";";

                query = query.Remove(query.Length - 5, 4);

                if(hasParams)
                    datagrid_habitat.ItemsSource = Database.Query(query)?.DefaultView;
                else
                    datagrid_habitat.ItemsSource = Database.Query(defaultQuery)?.DefaultView;
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


                string query = "UPDATE HABITAT " +
                                "SET habitat_name=(@HABNAME), " +
                                "humidity=(@HUMIVAL), " +
                                "temperature=(@TEMP), " +
                                "WHERE habitat_id=(@ID);";

                Database.NonQuery(query, ("@ID", idVal), ("@HABNAME", habName), ("@HUMI", humiVal),
                                        ("@TEMP", tempVal));
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

                    string query = "DELETE FROM HABITAT WHERE habitat_id=(@ID);";

                    bool notInAnimal = Database.Query("SELECT * FROM ANIMAL WHERE habitat_id=(@ID);", ("@ID", idVal)).Rows.Count > 0;

                    if (notInAnimal)
                    {
                        bool validID = Database.NonQuery(query, ("@ID", idVal));

                        if (validID == false)
                        {
                            MessageBox.Show("Could not delete because no habitat has the ID value");
                        }
                    }

                    if (notInAnimal == false)
                    {
                        MessageBox.Show("This Habitat is still listed in Animal and can't be deleted");
                    }
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
