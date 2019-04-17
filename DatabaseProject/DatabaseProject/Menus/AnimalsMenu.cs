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
            OpenMenu(menu_animals_habitat);
        }
        private void OpenAnimalsMenu(object sender, RoutedEventArgs e)
        {
            OpenMenu(menu_animals);
        }
        private void OpenSpeciesMenu(object sender, RoutedEventArgs e)
        {
            OpenMenu(menu_animals_species);
        }

        private void Btn_animals_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idVal = int.Parse(txt_animal_id.Text);
                int habIdVal = int.Parse(txt_animal_habitat_id.Text);
                int speIdVal = int.Parse(txt_animal_species_id.Text);
                string animalName = txt_animal_name.Text;
                string animalDate = txt_animal_birthdate.Text;
                int wei = int.Parse(txt_animal_weight.Text);
                int dietID = int.Parse(txt_animal_diet_id.Text);

                NonQuery("INSERT INTO ANIMALS (animal_id, habitat_id, species_id, animal_name, birthday, weight, diet_id) " +
                    "VALUES ('" + idVal + "', '" + habIdVal + "', '" + speIdVal + "', '" + animalName + "', '" + animalDate + "', '" + wei + "', '" + dietID + "');");

                MessageBox.Show("Add Stuff");

            }
            catch (Exception ex)
            {
                MessageBox.Show("One or more boxes are invalid because they are not numbers");
            }
        }

        private void Btn_animals_get_Click(object sender, RoutedEventArgs e)
        {
            //If a text box is blank, do not include it in the search.
            MessageBox.Show("Get Stuff");
        }

        private void Btn_animals_update_Click(object sender, RoutedEventArgs e)
        {
            int idVal = 0;
            int.TryParse(txt_animal_id.Text, out idVal);

            MessageBox.Show("Update Stuff with ID: "+idVal);
        }

        private void Btn_animals_delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this record?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                MessageBox.Show("Deleted Stuff");
                //OpenMenu(menu_expmode);
            }
        }
        #endregion
    }
}
