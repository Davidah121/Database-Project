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
        private const string NEW_TRANSACTION = "<New Transaction>";
        private const string ITEM_SALE = "Item Sale";
        private const string TICKET_SALE = "Ticket Sale";
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

        //Add item id
        //add ticket type

        #endregion

        #region Button Events

        private void Menu_transactions_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                ClearFields();
                //PopulateTransactionDropdown();
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

        private void combo_trans_type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string value = (e.AddedItems[0] as ComboBoxItem).Content as string;

                //Add Item ID + quantity visibility
                //Add TicketType visibility

            }
            catch
            {

            }
        }

        #endregion

        #region Methods

        private void NewTransaction()
        {
            if (!string.IsNullOrWhiteSpace(TransactionID)) return;
            if (string.IsNullOrWhiteSpace(TransactionAmount)) return;
            //if (Transaction == null) return;

            int TransactionID = FindFirstNonIndex("Select transaction_id from Transaction order by 1");

            string query = $"INSERT INTO Transaction VALUES({TransactionID}, {TransactionAmount}, 1);";

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
                //TO DO
                //query = $"SELECT * FROM Transaction;";
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

            string query = $"UPDATE Transaction SET amount = '{TransactionAmount}' WHERE transaction_id = {TransactionID};";


            NonQuery(query);
        }

        private void DeleteTransaction()
        {
            if (string.IsNullOrWhiteSpace(TransactionID)) return;

            if (MessageBox.Show("Are you sure you want to delete this transaction?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            string query = $"DELETE FROM Transaction WHERE transaction_id = {TransactionID};";


            if (NonQuery(query))
            {
                TransactionID = string.Empty;
                ViewTransaction();
            }
        }

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

        private void ClearFields()
        {
            TransactionID = string.Empty;
            EmployeeID = string.Empty;
            Date = string.Empty;
            PaymentMethod = string.Empty;
            TransactionAmount = string.Empty;

        }

        private void PopulateTransactionDropdown()
        {
            try
            {
                string query = "SELECT * FROM Transaction";
                DataTable table = Query(query);

                if (table == null)
                {
                    return;
                }

                //dropdown_donation_donator.Items.Clear();

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string TransactionID = table.Rows[i]["transaction_id"].ToString().Trim();
                    string EmployeeID = table.Rows[i]["employee_id"].ToString().Trim();
                    string Date = table.Rows[i]["date"].ToString().Trim();
                    string PaymentMethod = table.Rows[i]["payment_method"].ToString().Trim();
                    string TransactionAmount = table.Rows[i]["transaction_amount"].ToString().Trim();
                    string TransactionType = table.Rows[i]["transaction_type"].ToString().Trim();

                    Transaction transaction = new Transaction(TransactionID, EmployeeID, Date, PaymentMethod, TransactionAmount, TransactionType);

                    //dropdown_donation_donator.Items.Add(transaction);
                }

                dropdown_donation_donator.Items.Add(NEW_TRANSACTION);
            }
            catch
            {

            }
        }
    }

    class Transaction
    {
        public string TransactionID
        {
            get;
            private set;
        }

        public string EmployeeID
        {
            get;
            private set;
        }

        public string Date
        {
            get;
            private set;
        }

        public string PaymentMethod
        {
            get;
            private set;
        }

        public string TransactionAmount
        {
            get;
            private set;
        }
        
        public string TransactionType
        {
            get;
            private set;
        }

        public Transaction(string TransactionID, string EmployeeID, string Date, string PaymentMethod, string TransactionAmount, string TransactionType)
        {
            this.TransactionID = TransactionID;
            this.EmployeeID = EmployeeID;
            this.Date = Date;
            this.PaymentMethod = PaymentMethod;
            this.TransactionAmount = TransactionAmount;
            this.TransactionType = TransactionType;
        }

        //To Do
        public override string ToString()
        {
            return "string";
        }
    }
}
