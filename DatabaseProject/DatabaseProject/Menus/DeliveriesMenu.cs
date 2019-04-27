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
                ViewItems();
            }
            else
            {
                // We were just closed
            }
        }

        //BUTTONS

        private void btn_item_Click(object sender, RoutedEventArgs e)
        {
            NewItem();
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

        private void NewItem()
        {
            if (string.IsNullOrWhiteSpace(DeliveryID))
            {
                MessageBox.Show("Please enter a DeliveryID.");
                return;
            }

            if (string.IsNullOrWhiteSpace(DeliveryType))
            {
                MessageBox.Show("Please enter a DeliveryType.");
                return;
            }

            if (string.IsNullOrWhiteSpace(ItemID))
            {
                MessageBox.Show("Please enter a ItemID.");
                return;
            }

            if (string.IsNullOrWhiteSpace(QuantityDelivered))
            {
                MessageBox.Show("Please enter a QuantityDelivered.");
                return;
            }

            if (string.IsNullOrWhiteSpace(EmployeeID))
            {
                MessageBox.Show("Please enter a EmployeeID.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(DeliveryID))
            {
                string query = $"INSERT INTO Delivery VALUES(@DELIVERY_ID, @DELIVERY_TYPE);";
                Database.NonQuery(query, ("@DELIVERY_ID", DeliveryID), ("@DELIVERY_TYPE", DeliveryType));
            }

            if (!string.IsNullOrWhiteSpace(DeliveryID))
            {
                string query = $"INSERT INTO Delivery_Item VALUES(@DELIVERY_ID, @ITEM_ID, @QUANTITY_DELIVERED);";
                Database.NonQuery(query, ("@DELIVERY_ID", DeliveryID), ("@ITEM_ID", ItemID), ("@QUANTITY_DELIVERED", QuantityDelivered));
            }

            if (!string.IsNullOrWhiteSpace(DeliveryID))
            {
                string query = $"INSERT INTO Delivery_Unloader VALUES(@DELIVERY_ID, @EMPLOYEE_ID);";
                Database.NonQuery(query, ("@DELIVERY_ID", DeliveryID), ("@EMPLOYEE_ID", EmployeeID));
            }

            ViewItems();
        }

        private void ViewItems()
        {
            string query;

            if (!string.IsNullOrWhiteSpace(DeliveryID))
            {
                query = "SELECT DELIVERY.delivery_id, delivery_type, item_id, quantity_delivered FROM Delivery RIGHT JOIN DELIVERY_ITEM ON DELIVERY_ITEM.delivery_id = DELIVERY.delivery_id WHERE DELIVERY.delivery_id = @DELIVERY_ID";
            }
            else
            {
                query = "SELECT DELIVERY.delivery_id, delivery_type, item_id, quantity_delivered FROM Delivery RIGHT JOIN DELIVERY_ITEM ON DELIVERY_ITEM.delivery_id = DELIVERY.delivery_id";
            }
            DataTable table = Database.Query(query, ("@DELIVERY_ID", DeliveryID));
            delivery_dataGrid.ItemsSource = table?.DefaultView;
        }


        //checks to see if any elements of a delivery are attempting to be updated
        private void UpdateItems()
        {
            if (string.IsNullOrWhiteSpace(DeliveryID)) return;
            if (MessageBox.Show("Are you sure you want to update this delivery?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(DeliveryType)) 
            { 
                string query = $"UPDATE Delivery SET DELIVERY.delivery_type = @DELIVERY_TYPE WHERE delivery_id = @DELIVERY_ID;";
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
            ViewItems();
        }

        //Remove Items boi
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
        //adds employees to the delivery unloader table
        private void NewUnloaders()
        {
            if (string.IsNullOrWhiteSpace(DeliveryID)) return;
            if (string.IsNullOrWhiteSpace(EmployeeID)) return;

          //  int id = FindFirstNonIndex("SELECT Employee_id FROM Delivery_Unloader order by 1");
 
            string query = $"INSERT INTO Delivery_Unloader VALUES(@DELIVERY_ID, @EMPLOYEE_ID);";


            if (Database.NonQuery(query, ("@DELIVERY_ID", DeliveryID), ("@EMPLOYEE_ID", EmployeeID)))
            {
                ClearDeliveryFields();
                ViewUnloaders();
            }
        }
        //look at them unloading bois
        private void ViewUnloaders()
        {
            string query;
            if (!string.IsNullOrWhiteSpace(DeliveryID))
            {
                query = "SELECT Delivery_ID, Employee.employee_ID, first_name, last_name FROM DELIVERY_UNLOADER JOIN Employee ON Delivery_Unloader.employee_id = employee.employee_id WHERE Delivery_Unloader.delivery_id = @delivery_id";
            }
            else
            {
                query = "SELECT Delivery_ID, Employee.employee_ID, first_name, last_name FROM DELIVERY_UNLOADER JOIN Employee ON Delivery_Unloader.employee_id = employee.employee_id ";
            }
            DataTable table = Database.Query(query, ("@delivery_id", DeliveryID));
            delivery_dataGrid.ItemsSource = table?.DefaultView;
        }

        //remove an unloader in case wrongly entered
        private void RemoveUnloaders()
        {
            if (string.IsNullOrWhiteSpace(DeliveryID)) return;
            if (MessageBox.Show("Are you sure you want to delete this unloader?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
          
            string query = $"DELETE FROM Delivery_Unloader WHERE delivery_id = @DELIVERY_ID;";
            
           
            if (Database.NonQuery(query, ("@DELIVERY_ID", DeliveryID)))
            {
                DeliveryID = string.Empty;
                ViewUnloaders();
            }
        }
    

        private void ClearDeliveryFields()
        {
            DeliveryID = string.Empty;
            DeliveryType = string.Empty;
            ItemID = string.Empty;
            QuantityDelivered = string.Empty;
            EmployeeID = string.Empty;

        }

            #endregion
        }
        #endregion
    }




