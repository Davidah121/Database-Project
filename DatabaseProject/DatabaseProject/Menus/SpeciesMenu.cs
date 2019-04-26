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

        private void Btn_species_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //First, try to parse all of the entries into usable values.
                //If any can't be parsed or are empty, we stop.
                int idVal = int.Parse(txt_species_id.Text);
                string specName = txt_species_name.Text;
                string specClass = txt_species_class.Text;

                if(specName!="" && specClass!="")
                {
                    //Make the base of the query
                    string query = "INSERT INTO SPECIES VALUES(@ID, @SPENAME, @CLASSNAME);";
                    
                    //Then run the query while replacing the placeholders with our values.
                    Database.NonQuery(query, ("@ID", idVal), ("@SPENAME", specName), ("@CLASSNAME", specClass));
                }
                else
                {
                    MessageBox.Show("One or more boxes were left blank");
                }
            }
            catch
            {
                MessageBox.Show("One or more boxes are invalid because they are not numbers.");
            }
        }

        private void Btn_species_get_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool hasParams = false;
                int idVal;
                string specName, specClass;

                //Make two queries. One if they enter nothing and
                //one if they have entered something

                string defaultQuery = "SELECT * FROM SPECIES;";
                string query = "SELECT * FROM SPECIES WHERE ";

                //Check each indivual field to determine if it is empty
                //If the field is filled, add it to the where statement
                //by just adding to the end.
                if (txt_species_id.Text != "")
                {
                    hasParams = true;
                    idVal = int.Parse(txt_species_id.Text);
                    query += "species_id = " + idVal + " and ";
                }
                if (txt_species_name.Text != "")
                {
                    hasParams = true;
                    specName = txt_species_name.Text;
                    query += "species_name = '" + specName + "' and ";
                }
                if (txt_species_class.Text != "")
                {
                    hasParams = true;
                    specClass = txt_species_class.Text;
                    query += "class_name = '" + specClass + "' and ";
                }

                //Add a semicolon to the end to signify the end of the query
                query += ";";

                //Lastly, remove the additional and statement along with the
                //addition space.
                query = query.Remove(query.Length - 5, 4);

                //If there are parameters to search by, use the made query
                //otherwise use the default query
                if(hasParams)
                    datagrid_species.ItemsSource = Database.Query(query)?.DefaultView;
                else
                    datagrid_species.ItemsSource = Database.Query(defaultQuery)?.DefaultView;
            }
            catch
            {
                MessageBox.Show("One or more boxes are invalid because they are not numbers");
            }
        }

        private void Btn_species_update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //First try to parse all fields
                int idVal = int.Parse(txt_species_id.Text);
                string specName = txt_species_name.Text;
                string specClass = txt_species_class.Text;

                //If species_name or species_class are empty,
                //do not update. Otherwise create a query to update
                //the species at the specified id.
                if(specName!="" && specClass!="")
                {
                    string query = "UPDATE SPECIES " +
                                "SET species_name=(@SPENAME), " +
                                "class_name=(@CLASSNAME) " +
                                "WHERE species_id=(@ID);";

                    Database.NonQuery(query, ("@ID", idVal), ("@SPENAME", specName), ("@CLASSNAME", specClass));
                }
                else
                {
                    MessageBox.Show("One or more boxes were left blank");
                }
        }
            catch
            {
                MessageBox.Show("One or more boxes are invalid because they are not numbers");
            }
        }

        private void Btn_species_delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this record?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    //First, parse the id if you can.
                    int idVal = int.Parse(txt_species_id.Text);

                    //Create the query to delete that species
                    string query = "DELETE FROM SPECIES WHERE species_id=(@ID);";

                    //Lastly, check if the species is used. If it is,
                    //Don't delete this species.
                    bool notInAnimal = Database.Query("SELECT * FROM ANIMAL WHERE species_id=(@ID);", ("@ID", idVal)).Rows.Count > 0;

                    if (notInAnimal)
                    {
                        //If the id does not exist, let the user know.
                        bool validID = Database.NonQuery(query, ("@ID", idVal));

                        if (validID == false)
                        {
                            MessageBox.Show("Could not delete because no species has the ID value");
                        }
                    }

                    //Report to the user that the habitat is still used by
                    //animals in the zoo.
                    if (notInAnimal == false)
                    {
                        MessageBox.Show("This Habitat is still listed in Animal and can't be deleted");
                    }
                }
                catch
                {
                    MessageBox.Show("The species Id is not a number.");
                }
            }
        }
        #endregion
    }
}
