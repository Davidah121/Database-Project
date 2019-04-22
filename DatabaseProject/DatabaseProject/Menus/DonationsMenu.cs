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
using DatabaseProject;

namespace DatabaseProject
{
    partial class MainWindow : Window
    {
        private const string ANIMAL_ADOPTION = "Animal Adoption";
        private const string HABITAT_DONATION = "Habitat Donation";

        #region Properties


        private int DonationID
        {
            get
            {
                return int.TryParse(textbox_donation_id.Text.Trim(), out int i) ? i : -1;
            }
            set => textbox_donation_id.Text = value < 0 ? string.Empty : value.ToString();
        }

        private double DonationAmount
        {
            get
            {
                return double.TryParse(textbox_donation_id.Text.Trim(), out double i) ? i : 0;
            }
            set => textbox_donation_amount.Text = value < 0 ? string.Empty : value.ToString();
        }

        private string DonationType
        {
            get => dropdown_donation_type.Text;
            set => dropdown_donation_type.Text = value;
        }

        private string DonorFName
        {
            get => textbox_donation_donor_fname.Text;
            set => textbox_donation_donor_fname.Text = value;
        }

        private string DonorLName
        {
            get => textbox_donation_donor_lname.Text;
            set => textbox_donation_donor_lname.Text = value;
        }

        private string DonorEmail
        {
            get => textbox_donation_donor_email.Text;
            set => textbox_donation_donor_email.Text = value;
        }

        private Animal Animal
        {
            get => dropdown_donation_animal.SelectedItem as Animal;
            set => dropdown_donation_animal.SelectedItem = value;
        }

        private Habitat Habitat
        {
            get => dropdown_donation_habitat.SelectedItem as Habitat;
            set => dropdown_donation_habitat.SelectedItem = value;
        }

        private Donor Donor
        {
            get => dropdown_donation_donor.SelectedItem as Donor;
            set => dropdown_donation_donor.SelectedItem = value;
        }


        #endregion

        #region Events


        private void Menu_donations_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                // We were just been opened

                ClearDonationFields();
                PopulateDonorDropdown();
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

                PopulateHabitatDropdown();
                PopulateAnimalDropdown();
            }
            catch
            {
                label_donation_habitat.Visibility = Visibility.Collapsed;
                dropdown_donation_habitat.Visibility = Visibility.Collapsed;

                label_donation_animal.Visibility = Visibility.Collapsed;
                dropdown_donation_animal.Visibility = Visibility.Collapsed;
            }
        }


        private void Btn_donation_update_donor_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Dropdown_donation_donor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                DonorFName = string.Empty;
                DonorLName = string.Empty;
                DonorEmail = string.Empty;
                return;
            }

            if (!(e.AddedItems[0] is Donor donor)) return;

            DonorFName = donor.FirstName;
            DonorLName = donor.LastName;
            DonorEmail = donor.Email;
        }


        private void Datatable_donations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!(e.AddedItems[0] is DataRowView data)) return;

                int id = int.TryParse(data?.Row[0]?.ToString().Trim() ?? string.Empty, out int i) ? i : -1;

                Donation donation = Donation.All?.Find(x => x.ID == id);

                if (donation == null)
                {
                    ClearDonationFields();
                    return;
                }

                DonationID = donation.ID;
                DonationAmount = donation.Amount;
                Donor = donation.Donor;

                if (donation is AnimalAdoption adoption)
                {
                    DonationType = ANIMAL_ADOPTION;
                    Animal = adoption.Animal;
                    Habitat = null;
                }
                else if(donation is HabitatDonation habitat)
                {
                    DonationType = HABITAT_DONATION;
                    Habitat = habitat.Habitat;
                    Animal = null;
                }
            }
            catch(Exception ex)
            {
            }
        }


        #endregion


        #region Methods


        private void NewDonation()
        {
            if (DonationID >= 0 && MessageBox.Show("Do you want to create a new donation? A new donation id will be generated.", "Warning!", MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            if (DonationAmount <= 0)
            {
                MessageBox.Show("Please enter a positive donation amount.");
                return;
            }
            if (string.IsNullOrWhiteSpace(DonationType))
            {
                MessageBox.Show("Please select a donation type.");
                return;
            }
            if (DonationType == ANIMAL_ADOPTION && Animal == null)
            {
                MessageBox.Show("Please select an animal.");
                return;
            }
            else if (DonationType == HABITAT_DONATION && Habitat == null)
            {
                MessageBox.Show("Please select a habitat.");
                return;
            }

            if (Donor == null)
            {
                MessageBox.Show("Please select a donor.");
                return;
            }

            int id = FindFirstNonIndex("SELECT donation_id FROM Donation");

            string query = $"INSERT INTO Donation VALUES({id}, {DonationAmount}, {Donor.ID});";

            if (NonQuery(query))
            {
                if (DonationType == ANIMAL_ADOPTION)
                {
                    query = $"INSERT INTO Animal_Adoption VALUES({id}, {Animal.ID})";
                    NonQuery(query);
                }
                else if (DonationType == HABITAT_DONATION)
                {
                    query = $"INSERT INTO Habitat_Donation VALUES({id}, {Habitat.ID})";
                    NonQuery(query);
                }

                ClearDonationFields();
                ViewDonation();
            }
        }


        private void ViewDonation()
        {
            string query;

            if (string.IsNullOrWhiteSpace(textbox_donation_id.Text))
            {
                query = $"SELECT donation_id, amount, Donation.donor_id, first_name, last_name, email FROM Donation left join Donor on Donation.donor_id = Donor.donor_id;";
            }
            else
            {
                query = $"SELECT * FROM Donation WHERE donation_id = {textbox_donation_id.Text};";
            }

            DataTable table = Query(query);

            datatable_donations.ItemsSource = table?.DefaultView;
        }


        private void UpdateDonation()
        {
            if (DonationID < 0) return;


            if (MessageBox.Show("Are you sure you want to update this donation?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            string update_amount = DonationAmount <= 0 ? string.Empty : $",amount = '{DonationAmount}'";
            string update_donor = Donor == null ? string.Empty : $",donor_id = {Donor.ID}";

            string query = $"UPDATE Donation SET donation_id = {DonationID}{update_amount}{update_donor} WHERE donation_id = {DonationID};";


            if (NonQuery(query))
            {
                if (Animal != null)
                {
                    NonQuery($"UPDATE Animal_Adoption set animal_id = {Animal.ID} WHERE donation_id = {DonationID};");
                }
                else if (Habitat != null)
                {
                    NonQuery($"UPDATE Habitat_Donation set habitat_id = {Habitat.ID} WHERE donation_id = {DonationID};");
                }

                ClearDonationFields();
                ViewDonation();
            }
        }


        private void DeleteDonation()
        {

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


        private void ClearDonationFields()
        {
            DonationID = -1;
            DonationAmount = -1;
            DonationType = string.Empty;
            Donor = null;
        }


        private void PopulateDonorDropdown()
        {
            dropdown_donation_donor.ItemsSource = Donor.All;
        }


        private void PopulateHabitatDropdown()
        {
            dropdown_donation_habitat.ItemsSource = Habitat.All;
        }


        private void PopulateAnimalDropdown()
        {
            dropdown_donation_animal.ItemsSource = Animal.All;
        }


        #endregion
    }
}

interface IDatabaseObject
{
    bool Update();
    bool Delete();
}

abstract class Donation : IDatabaseObject
{
    protected static List<Donation> all;
    protected static bool allDirty;

    public static List<Donation> All
    {
        get
        {
            if (all == null || allDirty)
            {
                all = GetAll();
            }

            return all;
        }
    }

    public int ID
    {
        get;
        protected set;
    }

    public double Amount
    {
        get;
        protected set;
    }

    public Donor Donor
    {
        get;
        protected set;
    }

    public Donation(int id, double amount, Donor donor)
    {
        this.ID = id;
        this.Amount = amount;
        this.Donor = donor;
    }

    protected static List<Donation> GetAll()
    {
        List<Donation> list = new List<Donation>();

        try
        {
            string query = "SELECT * FROM Donation";
            DataTable table = Database.Query(query);
            int count = table?.Rows?.Count ?? 0;

            for (int i = 0; i < count; i++)
            {
                if (!int.TryParse(table.Rows[i]["donation_id"].ToString().Trim(), out int id)) continue;
                if (!double.TryParse(table.Rows[i]["amount"].ToString().Trim(), out double amount)) continue;
                if (!int.TryParse(table.Rows[i]["donor_id"].ToString().Trim(), out int donor_id)) continue;
                Donor donor = Donor.All.Find(x => x.ID == donor_id);


                Animal a = AnimalAdoption.GetAnimal(id);
                Habitat h = HabitatDonation.GetHabitat(id);
                if (a != null)
                {
                    list.Add(new AnimalAdoption(id, amount, donor, a));
                }
                else if (h != null)
                {
                    list.Add(new HabitatDonation(id, amount, donor, h));
                }
            }
        }
        catch
        {

        }

        return list;
    }

    public override string ToString()
    {
        return $"{this.ID}: {this.Amount}";
    }

    public abstract bool Update();
    public abstract bool Delete();
}

class AnimalAdoption : Donation
{
    public Animal Animal
    {
        get;
        private set;
    }


    public AnimalAdoption(int id, double amount, Donor donor, Animal animal) : base(id, amount, donor)
    {
        this.Animal = animal;
    }

    public static Animal GetAnimal(int donationid)
    {
        string query = $"SELECT animal_id FROM Animal_Adoption WHERE donation_id = {donationid}";

        DataTable table = Database.Query(query);

        if (table?.Rows?.Count == 0) return null;

        int id = int.TryParse(table.Rows[0]["animal_id"].ToString().Trim(), out int i) ? i : -1;

        return Animal.All.Find(x => x.ID == id);
    }

    public override bool Update()
    {
        return false;
    }

    public override bool Delete()
    {
        if (MessageBox.Show("Are you sure you want to delete this donation?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
        {
            return false;
        }

        return Database.NonQuery($"DELETE FROM Donation WHERE donation_id = {this.ID};") && Database.NonQuery($"DELETE FROM Animal_Adoption WHERE donation_id = {this.ID};");
    }
}

class HabitatDonation : Donation
{
    public Habitat Habitat
    {
        get;
        private set;
    }


    public HabitatDonation(int id, double amount, Donor donor, Habitat habitat) : base(id, amount, donor)
    {
        this.Habitat = habitat;
    }

    public static Habitat GetHabitat(int donationid)
    {
        string query = $"SELECT habitat_id FROM Habitat_Donation WHERE donation_id = {donationid}";

        DataTable table = Database.Query(query);

        if (table?.Rows?.Count == 0) return null;

        int id = int.TryParse(table.Rows[0]["habitat_id"].ToString().Trim(), out int i) ? i : -1;

        return Habitat.All.Find(x => x.ID == id);
    }

    public override bool Update()
    {
        throw new NotImplementedException();
    }

    public override bool Delete()
    {
        if (MessageBox.Show("Are you sure you want to delete this donation?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
        {
            return false;
        }


        return (Database.NonQuery($"DELETE FROM Donation WHERE donation_id = {this.ID};") && Database.NonQuery($"DELETE FROM Habitat_Donation WHERE donation_id = {this.ID};"));
    }
}

class Donor : IDatabaseObject
{
    private static List<Donor> all;
    private static bool allDirty;

    public static List<Donor> All
    {
        get
        {
            if (all == null || allDirty)
            {
                all = GetAll();
            }

            return all;
        }
    }

    public int ID
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

    public Donor(int id, string firstName, string lastName, string email)
    {
        this.ID = id;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Email = email;
    }

    private static List<Donor> GetAll()
    {
        List<Donor> list = new List<Donor>();

        try
        {
            string query = "SELECT * FROM Donor";
            DataTable table = Database.Query(query);
            int count = table?.Rows?.Count ?? 0;

            for (int i = 0; i < count; i++)
            {
                int id = int.Parse(table.Rows[i]["donor_id"].ToString().Trim());
                string firstName = table.Rows[i]["first_name"].ToString().Trim();
                string lastName = table.Rows[i]["last_name"].ToString().Trim();
                string email = table.Rows[i]["email"].ToString().Trim();

                Donor donor = new Donor(id, firstName, lastName, email);

                list.Add(donor);
            }
        }
        catch
        {

        }

        return list;
    }

    public override string ToString()
    {
        return $"{this.ID}: {this.FirstName} {this.LastName} - {this.Email}";
    }

    public bool Update()
    {
        return false;//TODO
    }

    public bool Delete()
    {
        return false;//TODO
    }
}

class Habitat : IDatabaseObject
{
    private static List<Habitat> all;
    private static bool allDirty;

    public static List<Habitat> All
    {
        get
        {
            if (all == null || allDirty)
            {
                all = GetAll();
            }

            return all;
        }
    }

    public int ID
    {
        get;
        private set;
    }

    public string Name
    {
        get;
        private set;
    }

    public Habitat(int id, string name)
    {
        this.ID = id;
        this.Name = name;
    }

    private static List<Habitat> GetAll()
    {
        List<Habitat> list = new List<Habitat>();

        try
        {

            string query = "SELECT * FROM Habitat";
            DataTable table = Database.Query(query);
            int count = table?.Rows?.Count ?? 0;

            for (int i = 0; i < count; i++)
            {
                int id = int.Parse(table.Rows[i]["habitat_id"].ToString().Trim());
                string name = table.Rows[i]["habitat_name"].ToString().Trim();

                Habitat habitat = new Habitat(id, name);

                list.Add(habitat);
            }


        }
        catch { }
        return list;
    }

    public bool Update()
    {
        return false; //TODO
    }

    public bool Delete()
    {
        return false; //TODO
    }

    public override string ToString()
    {
        return $"{this.ID}: {this.Name}";
    }
}

class Animal : IDatabaseObject
{
    private static List<Animal> all;
    private static bool allDirty;

    public static List<Animal> All
    {
        get
        {
            if (all == null || allDirty)
            {
                all = GetAll();
            }

            return all;
        }
    }

    public int ID
    {
        get;
        private set;
    }

    public string Name
    {
        get;
        private set;
    }

    public Habitat Habitat
    {
        get;
        private set;
    }

    public Animal(int id, string name, Habitat habitat)
    {
        this.ID = id;
        this.Name = name;
        this.Habitat = habitat;
    }

    private static List<Animal> GetAll()
    {
        List<Animal> list = new List<Animal>();

        try
        {
            string query = "SELECT * FROM Animal";
            DataTable table = Database.Query(query);
            int count = table?.Rows?.Count ?? 0;

            for (int i = 0; i < count; i++)
            {
                int id = int.Parse(table.Rows[i]["animal_id"].ToString().Trim());
                int habitat_id = int.Parse(table.Rows[i]["habitat_id"].ToString().Trim());
                string name = table.Rows[i]["animal_name"].ToString().Trim();

                Habitat habitat = Habitat.All.Find(x => x.ID == habitat_id);

                Animal animal = new Animal(id, name, habitat);

                list.Add(animal);
            }
        }
        catch { }

        return list;
    }

    public bool Update()
    {
        return false; // TODO
    }

    public bool Delete()
    {
        return false; // TODO
    }

    public override string ToString()
    {
        return $"{this.ID}: {this.Name} in {Habitat.Name}";
    }
}