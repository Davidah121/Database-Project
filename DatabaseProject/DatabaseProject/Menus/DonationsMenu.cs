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
using System.Data.SqlClient;
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
                return double.TryParse(textbox_donation_amount.Text.Trim(), out double i) ? i : 0;
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
                ViewDonor();
                
                // Add event listeners
                Donor.onAllUpdated += PopulateDonorDropdown;
                Animal.onAllUpdated += PopulateAnimalDropdown;
                Habitat.onAllUpdated += PopulateHabitatDropdown;
            }
            else
            {
                // We were just closed
                
                // Remove event listeners
                Donor.onAllUpdated -= PopulateDonorDropdown;
                Animal.onAllUpdated -= PopulateAnimalDropdown;
                Habitat.onAllUpdated -= PopulateHabitatDropdown;
            }
        }


        private void Btn_new_donation_Click(object sender, RoutedEventArgs e)
        {
            NewDonation();
        }

        private void Btn_view_donation_Click(object sender, RoutedEventArgs e)
        {
            ViewDonation(true);
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
                // Show or hide menu items based on the type of donation
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

        private void Btn_donation_view_donor_Click(object sender, RoutedEventArgs e)
        {
            ViewDonor();
        }

        private void Btn_donation_new_donor_Click(object sender, RoutedEventArgs e)
        {
            NewDonor();
        }

        private void Btn_donation_update_donor_Click(object sender, RoutedEventArgs e)
        {
            UpdateDonor();
        }

        private void Btn_donation_delete_donor_Click(object sender, RoutedEventArgs e)
        {
            DeleteDonor();
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
                Donor = Donor.All.Find(x => x.ID == donation.Donor.ID);

                if (donation is AnimalAdoption adoption)
                {
                    DonationType = ANIMAL_ADOPTION;
                    Animal = adoption.Animal;
                    Habitat = null;
                }
                else if (donation is HabitatDonation habitat)
                {
                    DonationType = HABITAT_DONATION;
                    Habitat = habitat.Habitat;
                    Animal = null;
                }
            }
            catch { }
        }

        private void Datatable_donors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!(e.AddedItems[0] is DataRowView data)) return;

                int id = int.TryParse(data?.Row[0]?.ToString().Trim() ?? string.Empty, out int i) ? i : -1;

                Donor = Donor.All.Find(x => x.ID == id);
            }
            catch { }
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

            Donation donation = CreateDonation();

            if (donation == null)
            {
                MessageBox.Show("Could not create new donation.");
                return;
            }

            donation.Update();

            ClearDonationFields();
            ViewDonation();
        }


        private void ViewDonation(bool reduced = false)
        {
            string query;

            if (!string.IsNullOrWhiteSpace(textbox_donation_id.Text) && reduced)
            {
                query = $"SELECT d.donation_id, amount, animal_id, habitat_id, d.donor_id FROM Donation as d left join Animal_Adoption as a on d.donation_id = a.donation_id left join Habitat_Donation as h on d.donation_id = h.donation_id WHERE donation_id = @ID;";
            }
            else
            {
                query = $"SELECT d.donation_id, amount, animal_id, habitat_id, d.donor_id FROM Donation as d left join Animal_Adoption as a on d.donation_id = a.donation_id left join Habitat_Donation as h on d.donation_id = h.donation_id;";
            }

            DataTable table = Database.Query(query, ("@ID", DonationID));

            datatable_donations.ItemsSource = table?.DefaultView;
        }


        private void UpdateDonation()
        {
            if (DonationID < 0) return;

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
            if (DonationType == HABITAT_DONATION && Habitat == null)
            {
                MessageBox.Show("Please select a habitat.");
                return;
            }
            if (Donor == null)
            {
                MessageBox.Show("Please select a donor.");
                return;
            }

            Donation donation = Donation.All.Find(x => x.ID == DonationID);

            if (donation == null) return;

            if (donation is AnimalAdoption && DonationType == HABITAT_DONATION || donation is HabitatDonation && DonationType == ANIMAL_ADOPTION)
            {
                donation.Delete();
                donation = CreateDonation(donation.ID);
                donation?.Update();
            }

            if (donation is AnimalAdoption adoption)
            {
                adoption.Animal = Animal;
            }
            else if (donation is HabitatDonation habitat)
            {
                habitat.Habitat = Habitat;
            }

            donation.Amount = DonationAmount;
            donation.Donor = Donor;


            donation.Update();


            ClearDonationFields();
            ViewDonation();
        }


        private void DeleteDonation()
        {
            if (DonationID < 0)
            {
                MessageBox.Show("Please enter a valid id.");
                return;
            }

            Donation donation = Donation.All.Find(x => x.ID == DonationID);

            donation.Delete();

            ClearDonationFields();
            ViewDonation();
        }


        private Donation CreateDonation(int id = -1)
        {
            if (id < 0)
            {
                id = FindFirstNonIndex("SELECT donation_id FROM Donation");
            }

            if (DonationType == ANIMAL_ADOPTION)
            {
                return new AnimalAdoption(id, DonationAmount, Donor, Animal);
            }
            else if (DonationType == HABITAT_DONATION)
            {
                return new HabitatDonation(id, DonationAmount, Donor, Habitat);
            }

            return null;
        }

        private void NewDonor()
        {
            if (string.IsNullOrEmpty(DonorFName))
            {
                MessageBox.Show("Please enter a first name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(DonorLName))
            {
                MessageBox.Show("Please enter a last name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(DonorEmail))
            {
                MessageBox.Show("Please enter an email.");
                return;
            }

            Donor donor = CreateDonor();

            donor?.Update();

            Donor = Donor.All.Find(x => x.ID == donor.ID);
            ViewDonor();
        }


        private void ViewDonor()
        {
            string query = $"SELECT * FROM Donor";

            DataTable table = Database.Query(query);

            datatable_donors.ItemsSource = table?.DefaultView;
        }

        private void UpdateDonor()
        {
            if (string.IsNullOrWhiteSpace(DonorFName))
            {
                MessageBox.Show("Please enter a first name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(DonorLName))
            {
                MessageBox.Show("Please enter a last name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(DonorEmail))
            {
                MessageBox.Show("Please enter a email.");
                return;
            }

            int id = Donor.ID;

            Donor.FirstName = DonorFName;
            Donor.LastName = DonorLName;
            Donor.Email = DonorEmail;

            Donor.Update();

            Donor = Donor.All.Find(x => x.ID == id);

            ViewDonor();
            ViewDonation();
        }

        private void DeleteDonor()
        {
            if (Donor == null)
            {
                MessageBox.Show("No donor selected");
                return;
            }

            Donor.Delete();

            ClearDonationFields();

            ViewDonor();
            ViewDonation();
        }

        private Donor CreateDonor()
        {
            int id = FindFirstNonIndex($"SELECT donor_id FROM Donor");

            return new Donor(id, DonorFName, DonorLName, DonorEmail);
        }


        private int FindFirstNonIndex(string query)
        {
            DataTable table = Database.Query(query);

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
    public static event Action onAllUpdated;

    protected static List<Donation> all;

    private static bool _allDirty;
    protected static bool allDirty
    {
        get => _allDirty;
        set
        {
            _allDirty = value;
            if (_allDirty) onAllUpdated?.Invoke();
        }
    }

    public static List<Donation> All
    {
        get
        {
            if (all == null || allDirty)
            {
                all = GetAll();
                allDirty = false;
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
        set;
    }

    public Donor Donor
    {
        get;
        set;
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
                // Get data out of data table
                if (!int.TryParse(table.Rows[i]["donation_id"].ToString().Trim(), out int id)) continue;
                if (!double.TryParse(table.Rows[i]["amount"].ToString().Trim(), out double amount)) continue;
                if (!int.TryParse(table.Rows[i]["donor_id"].ToString().Trim(), out int donor_id)) continue;
                Donor donor = Donor.All.Find(x => x.ID == donor_id);


                Animal a = AnimalAdoption.GetAnimal(id);
                Habitat h = HabitatDonation.GetHabitat(id);
                if (a != null)
                {
                    // if the donation is an animal adoption, make a new animal adoption
                    list.Add(new AnimalAdoption(id, amount, donor, a));
                }
                else if (h != null)
                {
                    // otherwise we are probably a habitat donation. make a new habitat donation object.
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
        set;
    }


    public AnimalAdoption(int id, double amount, Donor donor, Animal animal) : base(id, amount, donor)
    {
        this.Animal = animal;
    }

    public static Animal GetAnimal(int donationid)
    {
        // Initialize parameterized query
        string query = $"SELECT animal_id FROM Animal_Adoption WHERE donation_id = @ID";

        // Query the database and retrieve the table
        DataTable table = Database.Query(query, ("@ID", donationid));

        // If there are zero rows, return null
        if (table?.Rows.Count == 0) return null;

        // Parse the animal id from the first row of the table
        if (!int.TryParse(table.Rows[0]["animal_id"].ToString(), out int id)) return null;

        // Find and return the animal object, null if does not exist
        return Animal.All.Find(x => x.ID == id);
    }

    public override bool Update()
    {
        if (!All.Exists(x => x.ID == this.ID))
        {
            if (Database.NonQuery($"INSERT INTO Donation VALUES(@ID, @Amount, @Donor);", ("@ID", ID), ("@Amount", Amount), ("@Donor", Donor.ID)))
            {
                Database.NonQuery($"INSERT INTO Animal_Adoption VALUES(@ID, @Animal)", ("@ID", ID), ("@Animal", Animal.ID));
                allDirty = true;
                return true;
            }
            return false;
        }

        if (MessageBox.Show("Are you sure you want to update this donation?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
        {
            return false;
        }

        string query = $"UPDATE Donation SET amount = @Amount, donor_id = @Donor WHERE donation_id = @ID;";


        if (Database.NonQuery(query, ("@ID", ID), ("@Amount", Amount), ("@Donor", Donor.ID)))
        {
            Database.NonQuery($"UPDATE Animal_Adoption set animal_id = @Animal WHERE donation_id = @ID;", ("@ID", ID), ("@Animal", Animal.ID));
            allDirty = true;
            return true;
        }
        return false;
    }

    public override bool Delete()
    {
        if (MessageBox.Show("Are you sure you want to delete this donation?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
        {
            return false;
        }

        if (Database.NonQuery($"DELETE FROM Donation WHERE donation_id = @ID;", ("@ID", ID)))
        {
            Database.NonQuery($"DELETE FROM Animal_Adoption WHERE donation_id = @ID;", ("@ID", ID));
            allDirty = true;
            return true;
        }

        return false;
    }
}

class HabitatDonation : Donation
{
    public Habitat Habitat
    {
        get;
        set;
    }


    public HabitatDonation(int id, double amount, Donor donor, Habitat habitat) : base(id, amount, donor)
    {
        this.Habitat = habitat;
    }

    public static Habitat GetHabitat(int donationid)
    {
        string query = $"SELECT habitat_id FROM Habitat_Donation WHERE donation_id = @ID";

        DataTable table = Database.Query(query, ("@ID", donationid));

        if (table?.Rows?.Count == 0) return null;

        int id = int.TryParse(table.Rows[0]["habitat_id"].ToString().Trim(), out int i) ? i : -1;

        return Habitat.All.Find(x => x.ID == id);
    }

    public override bool Update()
    {
        if (!All.Exists(x => x.ID == this.ID))
        {
            MessageBox.Show("Adding new donation!");
            if (Database.NonQuery($"INSERT INTO Donation VALUES(@ID, @Amount, @Donor);", ("@ID", ID), ("@Amount", Amount), ("@Donor", Donor.ID)))
            {
                Database.NonQuery($"INSERT INTO Habitat_Donation VALUES(@ID, @Habitat)", ("@ID", ID), ("@Habitat", Habitat.ID));
                allDirty = true;
                return true;
            }
            return false;
        }

        if (MessageBox.Show("Are you sure you want to update this donation?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
        {
            return false;
        }

        string update_amount = Amount <= 0 ? string.Empty : $"";
        string update_donor = Donor == null ? string.Empty : $",donor_id = {Donor.ID}";

        if (Database.NonQuery($"UPDATE Donation SET donation_id = @ID, amount = @Amount, donor_id = @Donor WHERE donation_id = @ID;", ("@ID", ID), ("@Amount", Amount), ("@Donor", Donor.ID)))
        {
            Database.NonQuery($"UPDATE Habitat_Donation set habitat_id = @Habitat WHERE donation_id = @ID;", ("ID", ID), ("@Habitat", Habitat.ID));
            allDirty = true;
            return true;
        }
        return false;
    }

    public override bool Delete()
    {
        if (MessageBox.Show("Are you sure you want to delete this donation?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
        {
            return false;
        }

        if (Database.NonQuery($"DELETE FROM Donation WHERE donation_id = @ID;", ("@ID", ID)))
        {
            Database.NonQuery($"DELETE FROM Habitat_Donation WHERE donation_id = @ID;", ("@ID", ID));
            allDirty = true;
            return true;
        }
        return false;
    }
}

class Donor : IDatabaseObject
{
    public static event Action onAllUpdated;

    private static List<Donor> all;

    private static bool _allDirty;
    private static bool allDirty
    {
        get => _allDirty;
        set
        {
            _allDirty = value;
            if (_allDirty) onAllUpdated?.Invoke();
        }
    }

    public static List<Donor> All
    {
        get
        {
            if (all == null || allDirty)
            {
                all = GetAll();
                allDirty = false;
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
        set;
    }

    public string LastName
    {
        get;
        set;
    }

    public string Email
    {
        get;
        set;
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
            string query = "SELECT * FROM Donor order by donor_id";
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
        if (!All.Exists(x => x.ID == this.ID))
        {
            if (Database.NonQuery($"INSERT INTO Donor VALUES(@ID, @FName, @LName, @Email);", ("@ID", ID), ("@FName", FirstName), ("@LName", LastName), ("@Email", Email)))
            {
                allDirty = true;
                return true;
            }
            return false;
        }

        if (MessageBox.Show("Are you sure you want to update this donor?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
        {
            return false;
        }

        string query = $"UPDATE Donor SET first_name = @FName, last_name = @LName, email = @Email WHERE donor_id = @ID;";

        if (Database.NonQuery(query, ("@ID", ID), ("@FName", FirstName), ("@LName", LastName), ("@Email", Email)))
        {
            allDirty = true;
            return true;
        }
        return false;
    }

    public bool Delete()
    {
        if (MessageBox.Show("Are you sure you want to delete this donor?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
        {
            return false;
        }

        if (Database.NonQuery($"DELETE FROM Donor WHERE donor_id = @ID", ("@ID", ID)))
        {
            allDirty = true;
            return true;
        }

        return false;
    }
}

class Habitat : IDatabaseObject
{
    public static event Action onAllUpdated;

    private static List<Habitat> all;

    private static bool _allDirty;
    private static bool allDirty
    {
        get => _allDirty;
        set
        {
            _allDirty = value;
            if (_allDirty) onAllUpdated?.Invoke();
        }
    }

    public static List<Habitat> All
    {
        get
        {
            if (all == null || allDirty)
            {
                all = GetAll();
                allDirty = false;
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
            string query = "SELECT * FROM Habitat order by habitat_id";
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
        if (Database.NonQuery($"DELETE FROM Habitat WHERE habitat_id = @ID", ("@ID", ID)))
        {
            allDirty = true;
            return true;
        }

        return false;
    }

    public override string ToString()
    {
        return $"{this.ID}: {this.Name}";
    }
}

class Animal : IDatabaseObject
{
    public static event Action onAllUpdated;

    private static List<Animal> all;

    private static bool _allDirty;
    private static bool allDirty
    {
        get => _allDirty;
        set
        {
            _allDirty = value;
            if (_allDirty) onAllUpdated?.Invoke();
        }
    }

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
            string query = "SELECT * FROM Animal order by animal_id";
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
        if (Database.NonQuery($"DELETE FROM Animal WHERE animal_id = @ID", ("@ID", ID)))
        {
            allDirty = true;
            return true;
        }

        return false;
    }

    public override string ToString()
    {
        return $"{this.ID}: {this.Name} in {Habitat.Name}";
    }
}