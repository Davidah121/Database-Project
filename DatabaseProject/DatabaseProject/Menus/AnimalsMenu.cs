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

        private void OpenHabitatsMenu(object sender, RoutedEventArgs e)
        {
            //Opens the habitat menu
            OpenMenu(menu_animals_habitat);
        }
        private void OpenAnimalsMenu(object sender, RoutedEventArgs e)
        {
            //Opens the animal menu
            OpenMenu(menu_animals);
        }
        private void OpenSpeciesMenu(object sender, RoutedEventArgs e)
        {
            //Opens the species menu
            OpenMenu(menu_animals_species);
        }
        private void OpenDietMenu(object sender, RoutedEventArgs e)
        {
            //Opens the diet menu
            OpenMenu(menu_animals_diet);
        }

        private void Btn_animals_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //First, try to parse all of the fields
                int idVal = int.Parse(txt_animal_id.Text);
                int habIdVal = int.Parse(txt_animal_habitat_id.Text);
                int speIdVal = int.Parse(txt_animal_species_id.Text);
                string animalName = txt_animal_name.Text;
                string animalDate = date_animal_birthday.Text;
                int wei = int.Parse(txt_animal_weight.Text);
                int dietID = int.Parse(txt_animal_diet_id.Text);

                //Prevent adding into the table if the animal has no name
                //or birthday.
                if (animalName != "" && animalDate != "")
                {
                    //Create the query with placeholders that will insert into the table
                    string query = "INSERT INTO ANIMAL VALUES(@ID, @HABID, @SPEID, @NAME, @BIRTHDAY, @WEIGHT, @DIETID);";

                    //Replace the placeholders with the values to insert and run the query
                    bool valid = Database.NonQuery(query, ("@ID", idVal), ("@HABID", habIdVal), ("@SPEID", speIdVal),
                                            ("@NAME", animalName), ("@BIRTHDAY", animalDate), ("@WEIGHT", wei),
                                            ("@DIETID", dietID));

                    if(valid)
                        MessageBox.Show("Added an Animal.");
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

        private void Btn_animals_get_Click(object sender, RoutedEventArgs e)
        {
            //If a text box is blank, do not include it in the search.
            try
            {
                bool hasParams = false;
                int idVal, habIdVal, speIdVal, wei, dietID;
                string animalName, animalDate;

                //Create the default query which will select every animal
                string defaultQuery = "SELECT * FROM ANIMAL;";

                //Create the query that will select animals with some
                //specified attribute
                string query = "SELECT * FROM ANIMAL WHERE ";

                //Check each field to determine if it should be added
                //to the search attributes. If it is blank, it is left out
                if (txt_animal_id.Text!="")
                {
                    hasParams = true;
                    idVal = int.Parse(txt_animal_id.Text);
                    query += "animal_id = " + idVal + " and ";
                }
                if (txt_animal_habitat_id.Text != "")
                {
                    hasParams = true;
                    habIdVal = int.Parse(txt_animal_habitat_id.Text);
                    query += "habitat_id = " + habIdVal + " and ";
                }
                if (txt_animal_species_id.Text != "")
                {
                    hasParams = true;
                    speIdVal = int.Parse(txt_animal_species_id.Text);
                    query += "species_id = " + speIdVal + " and ";
                }
                if(txt_animal_name.Text != "")
                {
                    hasParams = true;
                    animalName = txt_animal_name.Text;
                    query += "animal_name = '" + animalName + "' and ";
                }
                
                if (date_animal_birthday.Text != "")
                {
                    hasParams = true;
                    animalDate = date_animal_birthday.Text;
                    query += "birthday = '" + animalDate + "' and ";
                }
                if (txt_animal_weight.Text != "")
                {
                    hasParams = true;
                    wei = int.Parse(txt_animal_weight.Text);
                    query += "weight = " + wei + " and ";
                }
                if (txt_animal_diet_id.Text != "")
                {
                    hasParams = true;
                    dietID = int.Parse(txt_animal_diet_id.Text);
                    query += "diet_id = " + dietID + " and ";
                }

                //add a semicolon to end the specific search query
                query += ";";
                
                //Take out the last and with the additional space from
                //that query.
                query = query.Remove(query.Length - 5, 4);


                //If the user did not enter any data, use the default query
                //else use the query that we have been modifying.
                if (hasParams)
                    datagrid_animals.ItemsSource = Database.Query(query)?.DefaultView;
                else
                    datagrid_animals.ItemsSource = Database.Query(defaultQuery)?.DefaultView;
            }
            catch
            {
                MessageBox.Show("One or more boxes are invalid because they are not numbers");
            }
            
        }

        private void Btn_animals_update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //First try to parse all of the fields
                int idVal = int.Parse(txt_animal_id.Text);
                int habIdVal = int.Parse(txt_animal_habitat_id.Text);
                int speIdVal = int.Parse(txt_animal_species_id.Text);

                string animalName = txt_animal_name.Text;
                string animalDate = date_animal_birthday.Text;

                int wei = int.Parse(txt_animal_weight.Text);
                int dietID = int.Parse(txt_animal_diet_id.Text);

                if (animalName != "" && animalDate != "")
                {
                    //Create the query to update an animal that has the
                    //specified id value leaving placeholders.
                    string query = "UPDATE ANIMAL " +
                        "SET habitat_id=(@HABID), " +
                        "species_id=(@SPEID), " +
                        "animal_name=(@NAME), " +
                        "birthday=(@BIRTHDAY), " +
                        "weight=(@WEIGHT), " +
                        "diet_id=(@DIETID), " +
                        "WHERE animal_id=(@ID);";

                    //Replace the placeholders with the users values and
                    //then run the query.
                    bool valid = Database.NonQuery(query, ("@ID", idVal), ("@HABID", habIdVal), ("@SPEID", speIdVal),
                                            ("@NAME", animalName), ("@BIRTHDAY", animalDate), ("@WEIGHT", wei),
                                            ("@DIETID", dietID));

                    if (valid)
                        MessageBox.Show("Updated an Animal.");
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

        private void Btn_animals_delete_Click(object sender, RoutedEventArgs e)
        {
            //Can only delete one entry at a time. For safety reasons. So you don't accidentally delete tons of records by leaving something filled out.
            //Use expert mode to delete more by different parameters.
            if (MessageBox.Show("Are you sure you want to delete this record?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    //First try to parse the id field
                    int idVal = int.Parse(txt_animal_id.Text);

                    //Create the query that delete the animal with the
                    //specified field
                    string query = "DELETE FROM ANIMAL WHERE animal_id=(@ID);";

                    //Check if the animal is listed in the handlers table or
                    //the animal_adoption table
                    bool notInHandler = Database.Query("SELECT * FROM HANDLER WHERE animal_id=(@ID);", ("@ID", idVal)).Rows.Count == 0;
                    bool notInAdopt = Database.Query("SELECT * FROM ANIMAL_ADOPTION WHERE animal_id=(@ID);", ("@ID", idVal)).Rows.Count == 0;
                    

                    //If it is not in those tables, it is safe to delete
                    if(notInAdopt && notInAdopt)
                    {
                        bool validID = Database.NonQuery(query, ("@ID", idVal));

                        //If the id wasn't valid, report that to the user
                        if(validID==false)
                        {
                            MessageBox.Show("Could not delete because no animal has the ID value");
                        }
                        else
                        {
                            MessageBox.Show("Deleted the animal.");
                        }
                    }

                    //Report whether the animal is listed in either the animal_adoption or
                    //Handlers table.
                    if(notInAdopt==false)
                    {
                        MessageBox.Show("This animal is still listed in Animal Adoption and can't be deleted");
                    }

                    if (notInHandler == false)
                    {
                        MessageBox.Show("This animal is still listed in Handlers and can't be deleted");
                    }
                }
                catch
                {
                    MessageBox.Show("The Animal Id is not a number.");
                }
            }
        }
        #endregion
    }
}
