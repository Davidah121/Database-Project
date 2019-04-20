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
        private const string NEW_DONATOR = "<New Donator>";
        private const string ANIMAL_ADOPTION = "Animal Adoption";
        private const string HABITAT_DONATION = "Habitat Donation";

        #region Properties


        private string DonationID
        {
            get => textbox_donation_id.Text;
            set => textbox_donation_id.Text = value;
        }

        private string DonationAmount
        {
            get => textbox_donation_amount.Text;
            set => textbox_donation_amount.Text = value;
        }

        private string DonationType
        {
            get => dropdown_donation_type.Text;
            set => dropdown_donation_type.Text = value;
        }

        private string Animal
        {
            get => dropdown_donation_animal.Text;
            set => dropdown_donation_animal.Text = value;
        }

        private string Habitat
        {
            get => dropdown_donation_habitat.Text;
            set => dropdown_donation_habitat.Text = value;
        }

        private Donator Donator
        {
            get => dropdown_donation_donator.SelectedItem as Donator;
            set => dropdown_donation_donator.SelectedItem = value;
        }


        #endregion

        #region Events


        private void Menu_donations_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                // We were just been opened

                ClearFields();
                PopulateDonatorDropdown();
                ViewDonation();
            }
            else
            {
                // We were just closed
            }
        }


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


        private void Dropdown_donation_type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string value = (e.AddedItems[0] as ComboBoxItem).Content as string;

                label_donation_habitat.Visibility = value == HABITAT_DONATION ? Visibility.Visible : Visibility.Collapsed;
                dropdown_donation_habitat.Visibility = value == HABITAT_DONATION ? Visibility.Visible : Visibility.Collapsed;

                label_donation_animal.Visibility = value == ANIMAL_ADOPTION ? Visibility.Visible : Visibility.Collapsed;
                dropdown_donation_animal.Visibility = value == ANIMAL_ADOPTION ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
                label_donation_habitat.Visibility = Visibility.Collapsed;
                dropdown_donation_habitat.Visibility = Visibility.Collapsed;

                label_donation_animal.Visibility = Visibility.Collapsed;
                dropdown_donation_animal.Visibility = Visibility.Collapsed;
            }
        }


        private void Dropdown_donation_donator_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dropdown_donation_donator.Text == NEW_DONATOR)
                {
                    MessageBox.Show("Create new donator!!");
                }
            }
            catch
            {

            }
        }


        private void Datatable_donations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView data = e.AddedItems[0] as DataRowView;

                if (data == null) return;

                DonationID = data.Row[0].ToString().Trim();
                DonationAmount = data.Row[1].ToString().Trim();
                // dropdown_donation_donator.SelectedItem = dropdown_donation_donator.Item
            }
            catch
            {

            }
        }


        #endregion


        #region Methods


        private void NewDonation()
        {
            if (!string.IsNullOrWhiteSpace(DonationID)) return;
            if (string.IsNullOrWhiteSpace(DonationAmount)) return;
            if (Donator == null) return;

            int id = FindFirstNonIndex("sElEcT donation_id fRoM Donation order by 1");

            string query = $"INSERT INTO Donation VALUES({id}, {DonationAmount}, {Donator.ID});";

            if (NonQuery(query))
            {
                ClearFields();
                ViewDonation();
            }
        }


        private void ViewDonation()
        {
            string query = string.Empty;

            if (string.IsNullOrWhiteSpace(textbox_donation_id.Text))
            {
                query = $"SELECT donation_id, amount, Donation.donator_id, first_name, last_name, email FROM Donation join Donator on Donation.donator_id = Donator.donator_id;";
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
            DonationID = string.Empty;
            DonationAmount = string.Empty;
            DonationType = string.Empty;
            Donator = null;
        }


        private void PopulateDonatorDropdown()
        {
            try
            {
                string query = "SELECT * FROM Donator";
                DataTable table = Query(query);

                if (table == null)
                {
                    return;
                }

                dropdown_donation_donator.Items.Clear();

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string id = table.Rows[i]["donator_id"].ToString().Trim();
                    string firstName = table.Rows[i]["first_name"].ToString().Trim();
                    string lastName = table.Rows[i]["last_name"].ToString().Trim();
                    string email = table.Rows[i]["email"].ToString().Trim();

                    Donator donator = new Donator(id, firstName, lastName, email);

                    dropdown_donation_donator.Items.Add(donator);
                }

                dropdown_donation_donator.Items.Add(NEW_DONATOR);
            }
            catch
            {

            }
        }


        #endregion
    }
}

class Donator
{
    public string ID
    {
        get;
        private set;
    }

    public string FirstName
    {
        get;
        private set;
    }

    public string LastName
    {
        get;
        private set;
    }

    public string Email
    {
        get;
        private set;
    }

    public Donator(string id, string firstName, string lastName, string email)
    {
        this.ID = id;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Email = email;
    }

    public override string ToString()
    {
        return $"{this.ID}: {this.FirstName} {this.LastName} - {this.Email}";
    }
}