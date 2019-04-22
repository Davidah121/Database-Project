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

                NonQuery("INSERT INTO SPECIES (species_id, species_name, class_name) " +
                    "VALUES ('" + idVal + "', '" + specName + "', '" + specClass + "');");
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
                query.Replace("'NULL'", "NULL");

                query = query.Remove(query.Length - 5, 4);

                if(hasParams)
                    datagrid_species.ItemsSource = Query(query)?.DefaultView;
                else
                    datagrid_species.ItemsSource = Query(defaultQuery)?.DefaultView;
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

                NonQuery("UPDATE SPECIES " +
                    "SET species_name='" + specName +
                    "', class_name='" + specClass +
                    "' WHERE species_id='" + idVal + "';");
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

                    NonQuery("DELETE FROM SPECIES WHERE species_id='" + idVal + "';");
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
