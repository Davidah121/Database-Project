using System;
using System.Collections.Generic;
using System.Data;
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
using System.Data.SqlClient;

namespace DatabaseProject
{
    partial class MainWindow : Window 
    {

        #region Button Events

        private void Menu_transactions_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                ClearFields();
                ViewTransaction();
            }
        }

        private void Btn_new_trans_Click(object sender, RoutedEventArgs e)
        {
            NewTransaction();
            ClearFields();
        }

        private void Btn_view_trans_Click(object sender, RoutedEventArgs e)
        {
            ViewTransaction();
            ClearFields();
        }

        private void Btn_view_item_Click(object sender, RoutedEventArgs e)
        {
            ViewItemTransaction();
            ClearFields();
        }

        private void Btn_view_ticket_Click(object sender, RoutedEventArgs e)
        {
            ViewTicketTransaction();
            ClearFields();
        }

        private void Btn_update_trans_Click(object sender, RoutedEventArgs e)
        {
            UpdateTransaction();
            ClearFields();
        }

        private void Btn_delete_trans_Click(object sender, RoutedEventArgs e)
        {
            DeleteTransaction();
            ClearFields();
        }

        private void Btn_clear_fields_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void Btn_add_ticket_to_cart_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_ticket_id.Text)) return;
            if (string.IsNullOrWhiteSpace(combo_ticket_selection.Text)) return;
            list_ticket_cart.Items.Add(txt_ticket_id.Text + "', '" + combo_ticket_selection.Text);
        }

        private void Btn_add_item_to_cart_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_item_id.Text)) return;
            if (string.IsNullOrWhiteSpace(txt_item_quantity.Text)) return;
            list_item_cart.Items.Add(txt_item_id.Text + "', '" + txt_item_quantity.Text);
        }

        private void combo_trans_type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string value = (e.AddedItems[0] as ComboBoxItem).Content as string;

                if (value == "Item Sale")
                {
                    TicketLabel.Visibility = Visibility.Collapsed;
                    combo_ticket_selection.Visibility = Visibility.Collapsed;
                    TicketIDLabel.Visibility = Visibility.Collapsed;
                    txt_ticket_id.Visibility = Visibility.Collapsed;
                    list_ticket_cart.Visibility = Visibility.Collapsed;
                    btn_add_ticket_to_cart.Visibility = Visibility.Collapsed;

                    ItemIDLabel.Visibility = Visibility.Visible;
                    txt_item_id.Visibility = Visibility.Visible;
                    list_item_cart.Visibility = Visibility.Visible;
                    btn_add_item_to_cart.Visibility = Visibility.Visible;
                    txt_item_quantity.Visibility = Visibility.Visible;
                    ItemQuantityLabel.Visibility = Visibility.Visible;
                }
                if (value == "Ticket Sale")
                {
                    ItemIDLabel.Visibility = Visibility.Collapsed;
                    txt_item_id.Visibility = Visibility.Collapsed;
                    list_item_cart.Visibility = Visibility.Collapsed;
                    btn_add_item_to_cart.Visibility = Visibility.Collapsed;
                    txt_item_quantity.Visibility = Visibility.Collapsed;
                    ItemQuantityLabel.Visibility = Visibility.Collapsed;

                    TicketLabel.Visibility = Visibility.Visible;
                    combo_ticket_selection.Visibility = Visibility.Visible;
                    TicketIDLabel.Visibility = Visibility.Visible;
                    txt_ticket_id.Visibility = Visibility.Visible;
                    list_ticket_cart.Visibility = Visibility.Visible;
                    btn_add_ticket_to_cart.Visibility = Visibility.Visible;
                }

            }
            catch
            {

            }
        }

        #endregion

        #region Methods

        private void NewTransaction()
        {
            string Transaction_ID = txt_transID.Text;
            string Employee_ID = txt_empID.Text;
            string Date_Of_Transaction = txt_date.Text;
            string Payment_Method = combo_payMethod.Text;
            string Transaction_Amount = txt_trans_amount.Text;
            string Ticket_ID = txt_ticket_id.Text;
            string Ticket_Type = combo_ticket_selection.Text;

            if (!string.IsNullOrWhiteSpace(Transaction_ID)) return;
            if (string.IsNullOrWhiteSpace(Employee_ID)) return;
            if (string.IsNullOrWhiteSpace(Date_Of_Transaction)) return;
            if (string.IsNullOrWhiteSpace(Payment_Method)) return;
            if (string.IsNullOrWhiteSpace(Transaction_Amount)) return;

            int id = FindFirstNonIndex("Select transaction_id from Transactions order by 1");
            string query = $"INSERT INTO Transactions VALUES('{id}','{Employee_ID}', '{Date_Of_Transaction}', '{Payment_Method}', '{Transaction_Amount}')";
            Database.Query(query);
            // Use foreach loop to get all tickets in cart to the right id
            if (list_ticket_cart.HasItems)
            {
                foreach (var listBoxItem in list_ticket_cart.Items)
                {
                    query = $"INSERT INTO Ticket (transaction_id, ticket_id, ticket_type) VALUES('{id}', '{listBoxItem.ToString()}');";
                    Database.Query(query);
                }
            }
            else if (list_item_cart.HasItems)
            {
                foreach (var listBoxItem in list_item_cart.Items)
                {
                    query = $"INSERT INTO Item_Sale (transaction_id, item_id, quantity) VALUES('{id}', '{listBoxItem.ToString()}');";
                    Database.Query(query);
                }
            }

            ViewTransaction();
            ClearFields();
        }

        private void ViewTransaction()
        {
            string query = string.Empty;

            if (string.IsNullOrWhiteSpace(txt_transID.Text))
            {
                query = $"SELECT * FROM Transactions;";
            }
            else
            {
                query = $"SELECT * FROM Transactions WHERE Transactions.transaction_id = {txt_transID.Text};";
            }

            DataTable table = Query(query);

            if (table == null)
            {
                return;
            }

            datatable_transactions.ItemsSource = table.DefaultView;
            ClearFields();
        }

        private void ViewTicketTransaction()
        {
            string query = string.Empty;

            if (string.IsNullOrWhiteSpace(txt_transID.Text))
            {
                query = $"SELECT * FROM Transactions FULL OUTER JOIN Ticket ON Transactions.transaction_id = Ticket.transaction_id";
            }
            else
            {
                query = $"SELECT * FROM Transactions FULL OUTER JOIN Ticket ON Transactions.transaction_id = Ticket.transaction_id WHERE Transactions.transaction_id = {txt_transID.Text}";
            }

            DataTable table = Query(query);

            if (table == null)
            {
                return;
            }

            datatable_transactions.ItemsSource = table.DefaultView;
        }

        private void ViewItemTransaction()
        {
            string query = string.Empty;

            if (string.IsNullOrWhiteSpace(txt_transID.Text))
            {
                query = $"SELECT * FROM Transactions FULL OUTER JOIN Item_Sale ON Transactions.transaction_id = Item_Sale.transaction_id";
            }
            else
            {
                query = $"SELECT * FROM Transactions FULL OUTER JOIN Item_Sale ON Transactions.transaction_id = Item_Sale.transaction_id WHERE Transactions.transaction_id = {txt_transID.Text}";
            }

            DataTable table = Query(query);

            if (table == null)
            {
                return;
            }

            datatable_transactions.ItemsSource = table.DefaultView;
        }



        private void UpdateTransaction()
        {
            if (string.IsNullOrWhiteSpace(txt_transID.Text)) return;

            if (MessageBox.Show("Are you sure you want to update this transaction?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            //Handling Employee ID?
            string query = $"UPDATE Transactions SET amount = '{txt_trans_amount.Text}', employee_id = '{txt_empID.Text}', transaction_date = '{txt_date.Text}', payment_method = '{combo_payMethod.Text}' WHERE transaction_id = '{txt_transID.Text}';";
            Database.Query(query);
            if (list_ticket_cart.HasItems)
            {
                query = $"DELETE FROM Ticket WHERE transaction_id = {txt_transID.Text};";
                Database.Query(query);
                foreach (var listBoxItem in list_ticket_cart.Items)
                {
                    query = $"INSERT INTO Ticket (transaction_id, ticket_id, ticket_type) VALUES('{txt_transID.Text}', '{listBoxItem.ToString()}');";
                    Database.Query(query);
                }
            }
            else if (list_item_cart.HasItems)
            {
                query = $"DELETE FROM Item_Sale WHERE transaction_id = {txt_transID.Text};";
                Database.Query(query);
                foreach (var listBoxItem in list_item_cart.Items)
                {
                    query = $"INSERT INTO Item_Sale (transaction_id, item_id, quantity) VALUES('{txt_transID.Text}', '{listBoxItem.ToString()}');";
                    Database.Query(query);
                }
            }

            ViewTransaction();
            ClearFields();
        }

        private void DeleteTransaction()
        {
            if (string.IsNullOrWhiteSpace(txt_transID.Text)) return;

            if (MessageBox.Show("Are you sure you want to delete this transaction?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            string query = $"DELETE FROM Transactions WHERE transaction_id = {txt_transID.Text};";
            Database.Query(query);
            query = $"DELETE FROM Ticket WHERE transaction_id = {txt_transID.Text};";
            Database.Query(query);
            query = $"Delete FROM Item_Sale WHERE transaction_id = {txt_transID.Text};";
            Database.Query(query);
            ViewTransaction();
            ClearFields();
        }
        
    
        private void ClearFields()
        {
            txt_transID.Text = string.Empty;
            txt_empID.Text = string.Empty;
            txt_date.Text = string.Empty;
            combo_payMethod.Text = string.Empty;
            txt_trans_amount.Text = string.Empty;
            combo_trans_type.Text = string.Empty;
            txt_ticket_id.Text = string.Empty;
            combo_ticket_selection.Text = string.Empty;
            list_ticket_cart.Items.Clear();
            list_item_cart.Items.Clear();
            txt_item_quantity.Text = string.Empty;

        }
    }

    #endregion
}
