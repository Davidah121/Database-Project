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
                //First, try to parse all of the fields
                int idVal = int.Parse(txt_habitat_id.Text);
                string habName = txt_habitat_name.Text;
                int humiVal = int.Parse(txt_habitat_humidity.Text);
                int tempVal = int.Parse(txt_habitat_temperature.Text);

                //If the habitat name is blank, do not insert
                //and relay that to the user
                if (habName != "")
                {
                    //First create the base query to insert with placeholders
                    string query = "INSERT INTO HABITAT VALUES(@ID, @HABNAME, @HUMI, @TEMP);";

                    //Replace the placeholders with the user's values and
                    //then run the query.
                    bool valid = Database.NonQuery(query, ("@ID", idVal), ("@HABNAME", habName), ("@HUMI", humiVal),
                                            ("@TEMP", tempVal));

                    if (valid)
                        MessageBox.Show("Added a Habitat.");
                }
                else
                {
                    MessageBox.Show("One or more boxes were left blank.");
                }
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

                //Create the default query that will select all of the habitats
                string defaultQuery = "SELECT * FROM HABITAT;";

                //Create a separate query that can be used for more specific
                //searching.
                string query = "SELECT * FROM HABITAT WHERE ";

                //Only add to the Where statement if the field is filled
                //by something
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

                //Add a semicolon to end the statement
                query += ";";

                //Next, remove the additional add statement along with the
                //extra space before the semicolon.
                query = query.Remove(query.Length - 5, 4);

                //If the user did not enter any search parameters, use the
                //default query. Otherwise, use the query with the where statement.
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
                //Try to parse the fields
                int idVal = int.Parse(txt_habitat_id.Text);
                string habName = txt_habitat_name.Text;
                int humiVal = int.Parse(txt_habitat_humidity.Text);
                int tempVal = int.Parse(txt_habitat_temperature.Text);

                //If the habitat_name is blank, do not update the query
                //and report to the user why.
                if (habName != "")
                {
                    string query = "UPDATE HABITAT " +
                      "SET habitat_name=(@HABNAME), " +
                      "humidity=(@HUMIVAL), " +
                      "temperature=(@TEMP), " +
                      "WHERE habitat_id=(@ID);";

                    bool valid = Database.NonQuery(query, ("@ID", idVal), ("@HABNAME", habName), ("@HUMI", humiVal),
                                            ("@TEMP", tempVal));

                    if (valid)
                        MessageBox.Show("Updated a Habitat.");
                }
                else
                {
                    MessageBox.Show("One or more boxes were left blank.");
                }
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
                    //Parse the id of the habitat from the text boxes
                    int idVal = int.Parse(txt_habitat_id.Text);

                    //Next, build the query to delete the habitat at the
                    //specified id value.
                    string query = "DELETE FROM HABITAT WHERE habitat_id=(@ID);";

                    //Determine if the habitat is used by an animal
                    bool notInAnimal = Database.Query("SELECT * FROM ANIMAL WHERE habitat_id=(@ID);", ("@ID", idVal)).Rows.Count == 0;

                    //If the habitat is used by an animal, do not delete it
                    //and report that to the user
                    if (notInAnimal)
                    {
                        //Replace the placeholder, and run the query
                        bool validID = Database.NonQuery(query, ("@ID", idVal));

                        //If the query fails, the id was invalid. Report that to the
                        //user.
                        if (validID == false)
                        {
                            MessageBox.Show("Could not delete because no habitat has the ID value");
                        }
                        else
                        {
                            MessageBox.Show("Deleted a Habitat.");
                        }
                    }

                    //Report to the user if the habitat is still used by an animal.
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
