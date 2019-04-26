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
                int idVal = int.Parse(txt_species_id.Text);
                string specName = txt_species_name.Text;
                string specClass = txt_species_class.Text;

                string query = "INSERT INTO SPECIES VALUES(@ID, @SPENAME, @CLASSNAME);";
                Database.NonQuery(query, ("@ID", idVal), ("@SPENAME", specName), ("@CLASSNAME", specClass));
            }
            catch
            {
                MessageBox.Show("One or more boxes are invalid because they are not numbers");
            }
        }

        private void Btn_species_get_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool hasParams = false;
                int idVal;
                string specName, specClass;

                string defaultQuery = "SELECT * FROM SPECIES;";
                string query = "SELECT * FROM SPECIES WHERE ";

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

                query += ";";

                query = query.Remove(query.Length - 5, 4);

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
                int idVal = int.Parse(txt_species_id.Text);
                string specName = txt_species_name.Text;
                string specClass = txt_species_class.Text;

                string query = "UPDATE SPECIES " +
                            "SET species_name=(@SPENAME), " +
                            "class_name=(@CLASSNAME) " +
                            "WHERE species_id=(@ID);";

                Database.NonQuery(query, ("@ID", idVal), ("@SPENAME", specName), ("@CLASSNAME", specClass));
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
                    int idVal = int.Parse(txt_species_id.Text);

                    string query = "DELETE FROM SPECIES WHERE species_id=(@ID);";

                    bool notInAnimal = Database.Query("SELECT * FROM ANIMAL WHERE species_id=(@ID);", ("@ID", idVal)).Rows.Count > 0;

                    if (notInAnimal)
                    {
                        bool validID = Database.NonQuery(query, ("@ID", idVal));

                        if (validID == false)
                        {
                            MessageBox.Show("Could not delete because no species has the ID value");
                        }
                    }

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
