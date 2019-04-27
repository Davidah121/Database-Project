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
using System.Data;

//most comments are incomplete attempts of replicating Nick's code.
namespace DatabaseProject
{
    partial class MainWindow : Window
    {

        #region Properties

        private string DeliveryID
        {
            get => delivery_ID_textBox.Text;
            set => delivery_ID_textBox.Text = value;
        }
        private string DeliveryType
        {
            get => delivery_type_textBox.Text;
            set => delivery_type_textBox.Text = value;
        }
        private string ItemID
        {
            get => item_ID_textBox.Text;
            set => item_ID_textBox.Text = value;
        }

        private string QuantityDelivered
        {
            get => quantity_delivered_textBox.Text;
            set => quantity_delivered_textBox.Text = value;
        }

        private string EmployeeID
        {
            get => Employee_ID_textBox.Text;
            set => Employee_ID_textBox.Text = value;
        }

        #region Events

        private void Menu_delivery_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                // We were just opened

                ClearDeliveryFields();
                //PopulateDonorDropdown();
                ViewItems();
                //ViewDonor();

                //Donor.onAllUpdated += PopulateDonorDropdown;
                //Animal.onAllUpdated += PopulateAnimalDropdown;
                //Habitat.onAllUpdated += PopulateHabitatDropdown;
            }
            else
            {
                // We were just closed
                //Donor.onAllUpdated -= PopulateDonorDropdown;
                //Animal.onAllUpdated -= PopulateAnimalDropdown;
                //Habitat.onAllUpdated -= PopulateHabitatDropdown;
            }
        }

        //BUTTONS

        private void btn_item_Click(object sender, RoutedEventArgs e)
        {
            NewItems();
        }
        private void btn_view_items_Click(object sender, RoutedEventArgs e)
        {
            ViewItems();
        }

        private void btn_rmv_item_Click(object sender, RoutedEventArgs e)
        {
            RemoveItems();
        }

        private void btn_unloader_Click(object sender, RoutedEventArgs e)
        {
            NewUnloaders();
        }
        private void btn_view_unloaders_Click(object sender, RoutedEventArgs e)
        {
            ViewUnloaders();
        }

        private void btn_rmv_unloaders_Click(object sender, RoutedEventArgs e)
        {
            RemoveUnloaders();
        }

        private void btn_update_item_Click(object sender, RoutedEventArgs e)
        {
            UpdateItems();
        }

        //ITEMS



        //Trying to copy Nick's 
        private void NewItems()
        {
            if (DeliveryID != null && MessageBox.Show("Do you want to create a new donation? A new donation id will be generated.", "Warning!", MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            if (ItemID == null)
            {
                MessageBox.Show("Please enter the item ID.");
                return;
            }
            if (string.IsNullOrWhiteSpace(QuantityDelivered))
            {
                MessageBox.Show("Please enter the quantity delivered.");
                return;
            }

            if (DeliveryType == null)
            {
                MessageBox.Show("Please select a delivery type.");
                return;
            }

            if (EmployeeID == null)
            {
                MessageBox.Show("Please enter an Employee ID.");
                return;
            }

            //obviously I don't have a delivery constructor, because I was unsure if I needed to make one. but this bit would reference that?
           // Delivery delivery = CreateDelivery();

            //if (delivery == null)
            //{
            //    MessageBox.Show("Could not create new donation.");
            //    return;
            //}
            //DEFINE A dIFFERENT UPDATE METHODDDDDDDD!!!!!
           // Delivery.UpdateItems();

            ClearDonationFields();
            ViewDonation();
        }

        private void ViewItems()
        {
            string query;
            //delete repeated column
            if (!string.IsNullOrWhiteSpace(DeliveryID))
            {
                query = "SELECT DELIVERY.delivery_id, deliery_type, item_id, quantity_delivered FROM Delivery RIGHT JOIN DELIVERY_ITEM ON DELIVERY_ITEM.delivery_id = DELIVERY.delivery_id WHERE delivery_id = @DELIVERY_ID";
            }
            else
            {
                query = "SELECT DELIVERY.delivery_id, deliery_type, item_id, quantity_delivered FROM Delivery RIGHT JOIN DELIVERY_ITEM ON DELIVERY_ITEM.delivery_id = DELIVERY.delivery_id";
            }
            DataTable table = Database.Query(query, ("@DELIVERY_ID", DeliveryID));
            delivery_dataGrid.ItemsSource = table?.DefaultView;
        }


        //does not work big time
        private void UpdateItems()
        {
            if (string.IsNullOrWhiteSpace(DeliveryID)) return;
            if (MessageBox.Show("Are you sure you want to update this delivery?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(DeliveryType)) 
            { //change when pushing to git (mispelled table)
                string query = $"UPDATE Delivery SET DELIVERY.deliery_type = @DELIVERY_TYPE WHERE delivery_id = @DELIVERY_ID;";
                Database.NonQuery(query, ("@DELIVERY_TYPE", DeliveryType), ("@DELIVERY_ID", DeliveryID));
            }

            if (!string.IsNullOrWhiteSpace(ItemID)) 
            {
                string query = $"UPDATE Delivery_Item SET DELIVERY_ITEM.item_id = @ITEM_ID WHERE delivery_id = @DELIVERY_ID;";
                Database.NonQuery(query, ("@ITEM_ID", ItemID), ("@DELIVERY_ID", DeliveryID));
            }

            if (!string.IsNullOrWhiteSpace(QuantityDelivered))
            {
                string query = $"UPDATE Delivery_Item SET DELIVERY_ITEM.quantity_delivered = @QUANTITY_DELIVERED WHERE delivery_id = @DELIVERY_ID;";
                Database.NonQuery(query, ("@QUANTITY_DELIVERED", QuantityDelivered), ("@DELIVERY_ID", DeliveryID));
            }

            if (!string.IsNullOrWhiteSpace(EmployeeID))
            {
                string query = $"UPDATE Delivery_Unloader SET DELIVERY_UNLOADER.employee_id = @EMPLOYEE_ID WHERE delivery_id = @DELIVERY_ID;";
                Database.NonQuery(query, ("@EMPLOYEE_ID", EmployeeID), ("@DELIVERY_ID", DeliveryID));
            }
        }

        private void RemoveItems()
        {
            if (string.IsNullOrWhiteSpace(DeliveryID)) return;
            if (MessageBox.Show("Are you sure you want to delete this delivery?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            string query = $"DELETE FROM Delivery WHERE delivery_id = @DELIVERY_ID; DELETE FROM Delivery_Item WHERE delivery_id = @DELIVERY_ID;";
            DataTable table = Database.Query(query, ("@DELIVERY_ID", DeliveryID));
            delivery_dataGrid.ItemsSource = table?.DefaultView;

            if (Database.NonQuery(query))
            {
                DeliveryID = string.Empty;
                ViewItems();
            }
        }

        //UNLOADERS
        //also does not work big time. I'm pretty sure it's not referencing the right stuff
        private void NewUnloaders()
        {
            if (!string.IsNullOrWhiteSpace(DeliveryID)) return;
            if (string.IsNullOrWhiteSpace(EmployeeID)) return;

            int id = FindFirstNonIndex("SELECT Employee_id FROM Delivery_Unloader order by 1");
            //fix
            string query = $"INSERT INTO Delivery VALUES({id}, {QuantityDelivered});";

            if (Database.NonQuery(query))
            {
                ClearFields();
                ViewUnloaders();
            }
        }

        private void ViewUnloaders()
        {
            string query;
            if (!string.IsNullOrWhiteSpace(DeliveryID))
            {
                query = "SELECT * FROM DELIVERY_UNLOADER WHERE employee_id = @EMPLOYEE_ID";
            }
            else
            {
                query = "SELECT * FROM DELIVERY_UNLOADER";
            }
            DataTable table = Database.Query(query, ("@EMPLOYEE_ID", EmployeeID));
            delivery_dataGrid.ItemsSource = table?.DefaultView;
        }

        private void RemoveUnloaders()
        {
            if (string.IsNullOrWhiteSpace(DeliveryID)) return;
            if (MessageBox.Show("Are you sure you want to delete this unloader?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
          
            string query = $"DELETE FROM Delivery_Unloader WHERE delivery_id = @DELIVERY_ID;";
            Database.Query(query, ("@DELIVERY_ID", DeliveryID));
           
            if (Database.NonQuery(query))
            {
                DeliveryID = string.Empty;
                ViewUnloaders();
            }
        }
    

        private void ClearDeliveryFields()
        {
            DeliveryID = null;
            DeliveryType = string.Empty;
            ItemID = null;
            QuantityDelivered = null;
            EmployeeID = null;

        }

            #endregion
        }
        #endregion
    }




