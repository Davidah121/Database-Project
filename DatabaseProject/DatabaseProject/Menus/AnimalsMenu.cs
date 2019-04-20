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

                NonQuery("INSERT INTO ANIMAL (animal_id, habitat_id, species_id, animal_name, birthday, weight, diet_id) " +
                    "VALUES ('" + idVal + "', '" + habIdVal + "', '" + speIdVal + "', '" + animalName + "', '" + animalDate + "', '" + wei + "', '" + dietID + "');");
            }
            catch
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
            try
            {
                int idVal = int.Parse(txt_animal_id.Text);
                int habIdVal = int.Parse(txt_animal_habitat_id.Text);
                int speIdVal = int.Parse(txt_animal_species_id.Text);
                string animalName = txt_animal_name.Text;
                string animalDate = txt_animal_birthdate.Text;
                int wei = int.Parse(txt_animal_weight.Text);
                int dietID = int.Parse(txt_animal_diet_id.Text);

                NonQuery("UPDATE ANIMAL " +
                    "SET habitat_id='" + habIdVal +
                    "', species_id='" + speIdVal +
                    "', animal_name='" + animalName +
                    "', birthday='" + animalDate +
                    "', weight='" + wei +
                    "', diet_id='" + dietID +
                    "' WHERE animal_id='" + idVal + "';");
            }
            catch
            {
                MessageBox.Show("One or more boxes are invalid because they are not numbers");
            }
        }

        private void Btn_animals_delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this record?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    int idVal = int.Parse(txt_animal_id.Text);

                    NonQuery("DELETE FROM ANIMAL WHERE animal_id='" + idVal + "';");
                }
                catch
                {
                    MessageBox.Show("The Animal Id is not a number.");
                }
            }
        }
        #endregion

        private void Btn_habitat_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idVal = int.Parse(txt_habitat_id.Text);
                string habName = txt_habitat_name.Text;
                int humiVal = int.Parse(txt_habitat_humidity.Text);
                int tempVal = int.Parse(txt_habitat_temperature.Text);

                NonQuery("INSERT INTO HABITAT (habitat_id, habitat_name, habitat_humidity, habitat_temperature) " +
                    "VALUES ('" + idVal + "', '" + habName + "', '" + humiVal + "', '" + tempVal + "');");
            }
            catch
            {
                MessageBox.Show("One or more boxes are invalid because they are not numbers");
            }
        }

        private void Btn_habitat_get_Click(object sender, RoutedEventArgs e)
        {

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
                    "', habitat_humidity='" + humiVal +
                    "', habitat_temperature='" + tempVal +
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

        private void Btn_species_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idVal = int.Parse(txt_species_id.Text);
                string specName = txt_species_name.Text;
                string specClass = txt_species_class.Text;

                NonQuery("INSERT INTO HABITAT (species_id, species_name, species_class) " +
                    "VALUES ('" + idVal + "', '" + specName + "', '" + specClass + "');");
            }
            catch
            {
                MessageBox.Show("One or more boxes are invalid because they are not numbers");
            }
        }

        private void Btn_species_get_Click(object sender, RoutedEventArgs e)
        {

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
                    "', species_class='" + specClass +
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
    }
}
