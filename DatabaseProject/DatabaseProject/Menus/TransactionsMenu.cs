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
        #region Properties
        
        public string TransactionID
        {
            get => txt_transID.Text;
            set => txt_transID.Text = value;
        }

        public string EmployeeID
        {
            get => txt_empID.Text;
            set => txt_empID.Text = value;
        }

        public string Date
        {
            get => txt_date.Text;
            set => txt_date.Text = value;
        }

        public string PaymentMethod
        {
            get => combo_payMethod.Text;
            set => combo_payMethod.Text = value;
        }

        public string TransactionAmount
        {
            get => txt_trans_amount.Text;
            set => txt_trans_amount.Text = value;
        }

        #endregion

        #region Button Events


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

        #endregion

        #region Methods

        private void NewTransaction()
        {
            if (string.IsNullOrWhiteSpace(TransactionID)) return;
            if (string.IsNullOrWhiteSpace(TransactionAmount)) return;

            string query = $"INSERT INTO Transaction VALUES({TransactionID}, {TransactionAmount}, 1);";

            NonQuery(query);
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
            if (string.IsNullOrWhiteSpace(TransactionID)) return;


            if (MessageBox.Show("Are you sure you want to update this transaction?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            string query = $"UPDATE Transaction SET amount = '{TransactionAmount}' WHERE donation_id = {TransactionID};";


            NonQuery(query);
        }

        private void DeleteTransaction()
        {
            if (string.IsNullOrWhiteSpace(TransactionID)) return;

            if (MessageBox.Show("Are you sure you want to delete this donation?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            string query = $"DELETE FROM Donation WHERE donation_id = {TransactionID};";


            if (NonQuery(query))
            {
                TransactionID = string.Empty;
                ViewTransaction();
            }
        }
    }
}
