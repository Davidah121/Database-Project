﻿using System;
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

                string query = "INSERT INTO ANIMAL (animal_id, habitat_id, species_id, animal_name, birthday, weight, diet_id) " +
                    "VALUES ('" + idVal + "', '" + habIdVal + "', '" + speIdVal + "', '" + animalName + "', '" + animalDate + "', '" + wei + "', '" + dietID + "');";

                query.Replace("''", "NULL");
                NonQuery(query);
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
                int idVal, habIdVal, speIdVal, wei, dietID;
                string animalName, animalDate;

                string query = "SELECT * FROM ANIMAL WHERE ";

                if (txt_animal_id.Text!="")
                {
                    idVal = int.Parse(txt_animal_id.Text);
                    query += "animal_id = " + idVal + ", ";
                }
                if (txt_animal_habitat_id.Text != "")
                {
                    habIdVal = int.Parse(txt_animal_habitat_id.Text);
                    query += "habitat_id = " + habIdVal + ", ";
                }
                if (txt_animal_species_id.Text != "")
                {
                    speIdVal = int.Parse(txt_animal_habitat_id.Text);
                    query += "species_id = " + speIdVal + ", ";
                }
                if(txt_animal_name.Text != "")
                {
                    animalName = txt_animal_name.Text;
                    query += "animal_name = '" + animalName + "', ";
                }
                if (txt_animal_birthdate.Text != "")
                {
                    animalDate = txt_animal_birthdate.Text;
                    query += "birthday = '" + animalDate + "', ";
                }
                if (txt_animal_weight.Text != "")
                {
                    wei = int.Parse(txt_animal_weight.Text);
                    query += "weight = " + wei + ", ";
                }
                if (txt_animal_diet_id.Text != "")
                {
                    dietID = int.Parse(txt_animal_diet_id.Text);
                    query += "diet_id = " + dietID + ", ";
                }

                query += ";";
                query.Replace("'NULL'", "NULL");
                
                query = query.Remove(query.Length - 3, 2);

                datagrid_animals.ItemsSource = Query(query)?.DefaultView;
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
                int idVal = int.Parse(txt_animal_id.Text);
                int habIdVal = int.Parse(txt_animal_habitat_id.Text);
                int speIdVal = int.Parse(txt_animal_species_id.Text);

                string animalName = txt_animal_name.Text;
                string animalDate = txt_animal_birthdate.Text;

                int wei = int.Parse(txt_animal_weight.Text);
                int dietID = int.Parse(txt_animal_diet_id.Text);

                string query = "UPDATE ANIMAL " +
                    "SET habitat_id='" + habIdVal +
                    "', species_id='" + speIdVal +
                    "', animal_name='" + animalName +
                    "', birthday='" + animalDate +
                    "', weight='" + wei +
                    "', diet_id='" + dietID +
                    "' WHERE animal_id='" + idVal + "';";

                query.Replace("''", "NULL");

                NonQuery(query);
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
    }
}
