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

        private void Btn_diet_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idVal = int.Parse(txt_diet_id.Text);
                string dType = txt_diet_type.Text;
                string dRestrictions = txt_diet_restrictions.Text;
                string dPrimary = txt_diet_primary_food.Text;
                string dSecondary = txt_diet_secondary_food.Text;
                string dTreats = txt_diet_treats.Text;

                string query = "INSERT INTO DIET VALUES(@ID, @DTYPE, @RES, @PRIM, @SECO, @TREATS); ";
                
                Database.NonQuery(query, ("@ID", idVal), ("@DTYPE", dType), ("@RES", dRestrictions), 
                                ("@PRIM", dPrimary), ("@SECO", dSecondary), ("@TREATS", dTreats));
            }
            catch
            {
                MessageBox.Show("One or more boxes are invalid because they are not numbers");
            }
        }

        private void Btn_diet_get_Click(object sender, RoutedEventArgs e)
        {
            //If a text box is blank, do not include it in the search.
            try
            {
                bool hasParams = false;
                int idVal;
                string dType, dRestrictions, dPrimary, dSecondary, dTreats;

                string defaultQuery = "SELECT * FROM DIET;";

                string query = "SELECT * FROM DIET WHERE ";

                if (txt_diet_id.Text!="")
                {
                    hasParams = true;
                    idVal = int.Parse(txt_diet_id.Text);
                    query += "diet_id = " + idVal + " and ";
                }
                if (txt_diet_type.Text != "")
                {
                    hasParams = true;
                    dType = txt_diet_type.Text;
                    query += "dietary_type = " + dType + " and ";
                }
                if (txt_diet_restrictions.Text != "")
                {
                    hasParams = true;
                    dRestrictions = txt_diet_restrictions.Text;
                    query += "restrictions = " + dRestrictions + " and ";
                }
                if(txt_diet_primary_food.Text != "")
                {
                    hasParams = true;
                    dPrimary = txt_diet_primary_food.Text;
                    query += "primary_food = '" + dPrimary + "' and ";
                }
                
                if (txt_diet_secondary_food.Text != "")
                {
                    hasParams = true;
                    dSecondary = txt_diet_secondary_food.Text;
                    query += "secondary_food = '" + dSecondary + "' and ";
                }
                if (txt_diet_treats.Text != "")
                {
                    hasParams = true;
                    dTreats = txt_diet_treats.Text;
                    query += "treats = " + dTreats + " and ";
                }
                

                query += ";";
                
                query = query.Remove(query.Length - 5, 4);

                if (hasParams)
                    datagrid_diet.ItemsSource = Database.Query(query)?.DefaultView;
                else
                    datagrid_diet.ItemsSource = Database.Query(defaultQuery)?.DefaultView;
            }
            catch
            {
                MessageBox.Show("One or more boxes are invalid because they are not numbers");
            }
            
        }

        private void Btn_diet_update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idVal = int.Parse(txt_diet_id.Text);
                string dType = txt_diet_type.Text;
                string dRestrictions = txt_diet_restrictions.Text;
                string dPrimary = txt_diet_primary_food.Text;
                string dSecondary = txt_diet_secondary_food.Text;
                string dTreats = txt_diet_treats.Text;

                string query = "UPDATE DIET " +
                    "SET dietary_type=(@DTYPE), " +
                    "restrictions=(@RES), " +
                    "primary_food=(@PRIM), " +
                    "secondary_food=(@SECO), " +
                    "treats=(@TREATS) " +
                    "WHERE diet_id=(@ID);";


                Database.NonQuery(query, ("@ID", idVal), ("@DTYPE", dType), ("@RES", dRestrictions),
                                                ("@PRIM", dPrimary), ("@SECO", dSecondary), ("@TREATS", dTreats));
            }
            catch
            {
                MessageBox.Show("One or more boxes are invalid because they are not numbers");
            }
        }

        private void Btn_diet_delete_Click(object sender, RoutedEventArgs e)
        {
            //Can only delete one entry at a time. For safety reasons. So you don't accidentally delete tons of records by leaving something filled out.
            //Use expert mode to delete more by different parameters.
            if (MessageBox.Show("Are you sure you want to delete this record?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    int idVal = int.Parse(txt_diet_id.Text);

                    string query = "DELETE FROM DIET WHERE diet_id=(@ID);";

                    bool notInAnimal = Database.Query("SELECT * FROM ANIMAL WHERE diet_id=(@ID);", ("@ID", idVal)).Rows.Count > 0;
                    
                    if (notInAnimal)
                    {
                        bool validID = Database.NonQuery(query, ("@ID", idVal));

                        if (validID == false)
                        {
                            MessageBox.Show("Could not delete because no diet has the ID value");
                        }
                    }

                    if (notInAnimal == false)
                    {
                        MessageBox.Show("This Diet is still listed in Animal and can't be deleted");
                    }
                }
                catch
                {
                    MessageBox.Show("The Diet Id is not a number or is invalid.");
                }
            }
        }
        #endregion
    }
}