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

namespace DatabaseProject
{
    partial class MainWindow : Window 
    {
        private object datatable_deliveries;

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

        private void NewItems()
        {
            if (!string.IsNullOrWhiteSpace(DeliveryID)) return;
            if (string.IsNullOrWhiteSpace(QuantityDelivered)) return;

            int id = FindFirstNonIndex("SELECT delivery_id FROM Delivery order by 1");
            //fix
            string query = $"INSERT INTO Delivery VALUES({delivery_ID}, {delivery_type});";

            if (NonQuery(query))
            {
                ClearFields();
                ViewItems();
            }
        }

        private void ViewItems()
        {
            string query;
            //delete repeated column
            query = "SELECT DELIVERY.delivery_id, deliery_type, item_id, quantity_delivered FROM Delivery RIGHT JOIN DELIVERY_ITEM ON DELIVERY_ITEM.delivery_id = DELIVERY.delivery_id";
            DataTable table = Query(query);
            delivery_dataGrid.ItemsSource = table?.DefaultView;
        }
        
        private void UpdateItems()
        {
            if (string.IsNullOrWhiteSpace(DeliveryID)) return;
            if (MessageBox.Show("Are you sure you want to update this delivery?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            //Finish to update every possible part of the Delivery
            string query = $"UPDATE Delivery SET quantity_delivered = '{quantity_delivered}' WHERE delivery_id = {DeliveryID};";
            NonQuery(query);
        }

        private void RemoveItems()
        {
            if (string.IsNullOrWhiteSpace(DeliveryID)) return;
            if (MessageBox.Show("Are you sure you want to delete this delivery?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            string query = $"DELETE FROM Delivery WHERE delivery_id = {DeliveryID}; DELETE FROM Delivery_Item WHERE delivery_id = {DeliveryID};";

            if (NonQuery(query))
            {
                DeliveryID = string.Empty;
                ViewItems();
            }
        }

        //UNLOADERS

        private void NewUnloaders()
        {
            if (!string.IsNullOrWhiteSpace(DeliveryID)) return;
            if (string.IsNullOrWhiteSpace(EmployeeID)) return;
            
            int id = FindFirstNonIndex("SELECT Employee_id FROM Delivery_Unloader order by 1");
            //fix
            string query = $"INSERT INTO Delivery VALUES({id}, {quantity_delivered}, {Donator.ID});";

            if (NonQuery(query))
            {
                ClearFields();
                ViewUnloaders();
            }
        }

        private void ViewUnloaders()
        {
            string query;
            query = "SELECT * FROM DELIVERY_UNLOADER";
            DataTable table = Query(query);
            delivery_dataGrid.ItemsSource = table?.DefaultView;
        }

        private void RemoveUnloaders()
        {
            if (string.IsNullOrWhiteSpace(DeliveryID)) return;
            if (MessageBox.Show("Are you sure you want to delete this unloader?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            //fix
            string query = $"DELETE FROM Delivery_Unloader WHERE delivery_id = {DeliveryID};";
            //fix
            if (NonQuery(query))
            {
                DonationID = string.Empty;
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