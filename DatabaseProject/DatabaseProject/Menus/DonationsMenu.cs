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
        #region Properties


        public string DonationID
        {
            get => textbox_donation_id.Text;
            set => textbox_donation_id.Text = value;
        }

        public string DonationAmount
        {
            get => textbox_donation_amount.Text;
            set => textbox_donation_amount.Text = value;
        }

        public string DonatorName
        {
            get => textbox_donator_name.Text;
            set => textbox_donator_name.Text = value;
        }


        public string DonatorEmail
        {
            get => textbox_donator_email.Text;
            set => textbox_donator_email.Text = value;
        }

        public string DonationType
        {
            get => dropdown_donation_type.Text;
            set => dropdown_donation_type.Text = value;
        }


        #endregion

        #region Events


        private void Btn_new_donation_Click(object sender, RoutedEventArgs e)
        {
            NewDonation();
        }

        private void Btn_view_donation_Click(object sender, RoutedEventArgs e)
        {
            ViewDonation();
        }


        private void Btn_update_donation_Click(object sender, RoutedEventArgs e)
        {
            UpdateDonation();
        }

        private void Btn_delete_donation_Click(object sender, RoutedEventArgs e)
        {
            DeleteDonation();
        }


        #endregion


        #region Methods


        private void NewDonation()
        {
            if (string.IsNullOrWhiteSpace(DonationID)) return;
            if (string.IsNullOrWhiteSpace(DonationAmount)) return;

            string query = $"INSERT INTO Donation VALUES({DonationID}, {DonationAmount}, 1);";

            NonQuery(query);
        }


        private void ViewDonation()
        {
            string query = string.Empty;

            if (string.IsNullOrWhiteSpace(textbox_donation_id.Text))
            {
                query = $"SELECT * FROM Donation;";
            }
            else
            {
                query = $"SELECT * FROM Donation WHERE donation_id = {textbox_donation_id.Text};";
            }

            DataTable table = Query(query);

            if (table == null)
            {
                return;
            }

            datatable_donations.ItemsSource = table.DefaultView;
        }


        private void UpdateDonation()
        {
            if (string.IsNullOrWhiteSpace(DonationID)) return;


            if (MessageBox.Show("Are you sure you want to update this donation?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            string query = $"UPDATE Donation SET amount = '{DonationAmount}' WHERE donation_id = {DonationID};";


            NonQuery(query);
        }


        private void DeleteDonation()
        {
            if (string.IsNullOrWhiteSpace(DonationID)) return;

            if (MessageBox.Show("Are you sure you want to delete this donation?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            string query = $"DELETE FROM Donation WHERE donation_id = {DonationID};";


            if (NonQuery(query))
            {
                DonationID = string.Empty;
                ViewDonation();
            }
        }


        #endregion
    }
}
