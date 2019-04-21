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
        }

        private void Btn_view_trans_Click(object sender, RoutedEventArgs e)
        {
            ViewTransaction();
        }

        private void Btn_update_trans_Click(object sender, RoutedEventArgs e)
        {
            UpdateTransaction();
        }

        private void Btn_delete_trans_Click(object sender, RoutedEventArgs e)
        {
            DeleteTransaction();
        }

        private void Btn_clear_fields_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
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

                    ItemLabel.Visibility = Visibility.Visible;
                    combo_item_selection.Visibility = Visibility.Visible;
                    QuantityLabel.Visibility = Visibility.Visible;
                    txt_item_quantity.Visibility = Visibility.Visible;
                    list_item_cart.Visibility = Visibility.Visible;
                    btn_add_item_to_cart.Visibility = Visibility.Visible;
                }
                if (value == "Ticket Sale")
                {
                    ItemLabel.Visibility = Visibility.Collapsed;
                    combo_item_selection.Visibility = Visibility.Collapsed;
                    QuantityLabel.Visibility = Visibility.Collapsed;
                    txt_item_quantity.Visibility = Visibility.Collapsed;
                    list_item_cart.Visibility = Visibility.Collapsed;
                    btn_add_item_to_cart.Visibility = Visibility.Collapsed;

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
            if (!string.IsNullOrWhiteSpace(txt_transID.Text)) return;

            /*
            int id = FindFirstNonIndex("Select transaction_id from Transaction order by 1");
            */
            string query = $"INSERT INTO Transaction VALUES(1);";

            if (NonQuery(query))
            {
                ClearFields();
                ViewTransaction();
            }
        }

        private void ViewTransaction()
        {
            string query = string.Empty;

            if (string.IsNullOrWhiteSpace(txt_transID.Text))
            {
                query = $"SELECT * FROM Transaction;";
            }
            else
            {
                query = $"SELECT * FROM Transaction WHERE transaction_id = {txt_transID.Text}";
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
            string query = $"UPDATE Transaction SET amount = '{txt_trans_amount.Text}', employee_id = '{txt_empID.Text}', transaction_date = '{txt_date.Text}', payment_method = '{combo_payMethod.Text}, transaction_type = '{combo_trans_type}' WHERE transaction_id = {txt_transID.Text};";

            NonQuery(query);
        }

        private void DeleteTransaction()
        {
            if (string.IsNullOrWhiteSpace(txt_transID.Text)) return;

            if (MessageBox.Show("Are you sure you want to delete this transaction?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            string query = $"DELETE FROM Transaction WHERE transaction_id = {txt_transID.Text};";


            if (NonQuery(query))
            {
                txt_transID.Text = string.Empty;
                ViewTransaction();
            }
        }
        /*
        private int FindFirstNonIndex(string query)
        {
            DataTable table = Query(query);

            List<int> values = new List<int>();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (int.TryParse(table.Rows[i][0].ToString(), out int val))
                {
                    values.Add(val);
                }
            }

            int firstAvailable = Enumerable.Range(0, int.MaxValue).Except(values).FirstOrDefault();

            return firstAvailable;
        }
        
    */
        private void ClearFields()
        {
            txt_transID.Text = string.Empty;
            txt_empID.Text = string.Empty;
            txt_date.Text = string.Empty;
            combo_payMethod.Text = string.Empty;
            txt_trans_amount.Text = string.Empty;
            combo_trans_type.Text = string.Empty;
        }
    }

    #endregion
}
