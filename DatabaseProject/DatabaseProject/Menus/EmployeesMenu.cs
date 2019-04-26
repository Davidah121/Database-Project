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
        #region Events





        #endregion
        private string Get_AnimalId(string animal_rawSelected)
        {
            string possibleStr = animal_rawSelected[animal_rawSelected.Length-1] + "";
            if(possibleStr=="0"|| possibleStr == "1" || possibleStr == "2" || possibleStr == "3" || possibleStr == "4" || possibleStr == "5" || possibleStr == "6" || possibleStr == "7" || possibleStr == "8" || possibleStr == "9")
            {
                return possibleStr;
            }
            else
            {
                return "-1";
            }
        }
        private bool isHandlerOf(string animal_id, string handler_id)
        {
            string query;
            if (animal_id == "")
            {
                query = "select * from handler where employee_id=@HANDLER_ID;";
            }
            else
            {
                query = $"select * from handler where animal_id=@Animal AND employee_id=@HANDLER_ID;";
            }
            try
            {
                var testHandler = Database.Query(query, ("@HANDLER_ID", handler_id), ("@Animal", animal_id)).Rows[0].ItemArray.Select(x => x.ToString().Trim()).ToArray()[0];
                return true;
            }
            catch
            {
                return false;
            }
        }
        private string Get_Department(string department_name)
        {
            if (department_name.Equals(""))
            {
                return "" + -1;
            }
            //MessageBox.Show(department_name);
            string query = "select department_id from department where department_name=@Dept;";
            string stringDep_id;
            try
            {
                stringDep_id = Database.Query(query, ("@Dept", department_name)).Rows[0].ItemArray.Select(x => x.ToString().Trim()).ToArray()[0];
            }
            catch
            {
                //Add in ability to add specified department
                if (MessageBox.Show("Could not find the specified department with name: " + department_name +"\nWould you like to add it?", "Warning!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    query = "select department_id from department;";
                    int next_id = FindFirstNonIndex(query);
                    query = "insert into department (department_id, department_name) values(@next, @DeptName);";
                    Database.NonQuery(query, ("@next", next_id), ("@DeptName", department_name));
                    MessageBox.Show("Added Department with name: " + department_name);
                    stringDep_id = "" + next_id;
                }
                else
                {
                    stringDep_id = "-1";
                }
                
            }
            
            //MessageBox.Show(stringDep_id);
            return stringDep_id;
        }

        private void Btn_emp_get_Click(object sender, RoutedEventArgs e)
        {
            string query;
            if (!(emp_field_id.Text.Equals("")))
            {
                query = $"select * from employee where employee_id=@EID;";
            }
            else
            {
                string get_dep_id = Get_Department(emp_field_department.Text);
                //MessageBox.Show(get_dep_id);
                if (emp_combo_animal.Text == "" || emp_combo_animal.Text == "None")
                {
                    query = $"select * from employee where" + ((!get_dep_id.Equals("-1")) ? " department_id=" + get_dep_id : "") + ((!emp_field_ssn.Password.Equals("")) ? ((!get_dep_id.Equals("-1")) ? " AND" : "") + " ssn=" + emp_field_ssn.Password : "") + ((!emp_field_email.Text.Equals("")) ? ((!get_dep_id.Equals("-1")) || (!emp_field_ssn.Password.Equals("")) ? " AND" : "") + " email='" + emp_field_email.Text + "'" : "") + ((!emp_field_lname.Text.Equals("")) ? ((!get_dep_id.Equals("-1")) || (!emp_field_ssn.Password.Equals("")) || (!emp_field_email.Text.Equals("")) ? " AND" : "") + " last_name='" + emp_field_lname.Text + "'" : "") + ((!emp_field_fname.Text.Equals("")) ? ((!get_dep_id.Equals("-1")) || (!emp_field_ssn.Password.Equals("")) || (!emp_field_email.Text.Equals("")) || (!emp_field_lname.Text.Equals("")) ? " AND" : "") + " first_name='" + emp_field_fname.Text + "'" : "") + ((!emp_field_phone.Text.Equals("")) ? ((!get_dep_id.Equals("-1")) || (!emp_field_ssn.Password.Equals("")) || (!emp_field_email.Text.Equals("")) || (!emp_field_lname.Text.Equals("")) || (!emp_field_fname.Text.Equals("")) ? " AND" : "") + " phone_number='" + emp_field_phone.Text + "'" : "") + ((!emp_field_type.Text.Equals("")) ? ((!get_dep_id.Equals("-1")) || (!emp_field_ssn.Password.Equals("")) || (!emp_field_email.Text.Equals("")) || (!emp_field_lname.Text.Equals("")) || (!emp_field_fname.Text.Equals("")) || (!emp_field_phone.Text.Equals("")) ? " AND" : "") + " employment_type='" + emp_field_type.Text + "'" : "") + ((!emp_field_date.Text.Equals("")) ? ((!get_dep_id.Equals("-1")) || (!emp_field_ssn.Password.Equals("")) || (!emp_field_email.Text.Equals("")) || (!emp_field_lname.Text.Equals("")) || (!emp_field_fname.Text.Equals("")) || (!emp_field_phone.Text.Equals("")) || (!emp_field_type.Text.Equals("")) ? " AND" : "") + " birthday='" + emp_field_date.Text + "'" : "") + ";";
                    if (query.Equals("select * from employee where;"))
                    {
                        query = "select * from employee;";
                    }
                }
                else
                {
                    query = $"select animal_id,employee.employee_id,department_id,first_name,last_name,email,phone_number,employment_type,birthday,ssn from employee join handler on employee.employee_id=handler.employee_id where animal_id=" + Get_AnimalId(emp_combo_animal.Text) + ((!get_dep_id.Equals("-1")) ? " AND department_id=" + get_dep_id : "") + ((!emp_field_ssn.Password.Equals("")) ? " AND ssn=" + emp_field_ssn.Password : "") + ((!emp_field_email.Text.Equals("")) ? " AND email='" + emp_field_email.Text + "'" : "") + ((!emp_field_lname.Text.Equals("")) ? " AND last_name='" + emp_field_lname.Text + "'" : "") + ((!emp_field_fname.Text.Equals("")) ? " AND first_name='" + emp_field_fname.Text + "'" : "") + ((!emp_field_phone.Text.Equals("")) ? " AND phone_number='" + emp_field_phone.Text + "'" : "") + ((!emp_field_type.Text.Equals("")) ? " AND employment_type='" + emp_field_type.Text + "'" : "") + ((!emp_field_date.Text.Equals("")) ? " AND birthday='" + emp_field_date.Text + "'" : "") + ";";
                    MessageBox.Show(query);
                }
            }
            //MessageBox.Show(query);
            datagrid_emp.ItemsSource = Database.Query(query, ("@EID", emp_field_id.Text))?.DefaultView;
        }
        private void Btn_emp_update_Click(object sender, RoutedEventArgs e)
        {
            string query;
            if (emp_field_id.Text.Equals(""))
            {
                MessageBox.Show("Please specifiy the employee to update with their id");
            }
            else
            {
                string get_dep_id = Get_Department(emp_field_department.Text);
                query = "select * from employee where employee_id=@EID;";
                try
                {
                    var testEmployee = Database.Query(query, ("@EID", emp_field_id.Text)).Rows[0].ItemArray.Select(x => x.ToString().Trim()).ToArray()[0];
                    query = "update employee set employee_id="+emp_field_id.Text + ((!get_dep_id.Equals("-1")) ? ", department_id=" + get_dep_id : "")  + ((!emp_field_ssn.Password.Equals("")) ? ", ssn=" + emp_field_ssn.Password : "") + ((!emp_field_email.Text.Equals("")) ? ", email='" + emp_field_email.Text + "'" : "") + ((!emp_field_lname.Text.Equals("")) ? ", last_name='" + emp_field_lname.Text + "'" : "") + ((!emp_field_fname.Text.Equals("")) ? ", first_name='" + emp_field_fname.Text + "'" : "") + ((!emp_field_phone.Text.Equals("")) ? ", phone_number='" + emp_field_phone.Text + "'" : "") + ((!emp_field_type.Text.Equals("")) ? ", employment_type='" + emp_field_type.Text + "'" : "") + ((!emp_field_date.Text.Equals("")) ? ", birthday='" + emp_field_date.Text + "'" : "") + " where employee_id="+ emp_field_id.Text +";";
                    Database.NonQuery(query);
                    if (!emp_combo_animal.Text.Equals("")) {
                        if (emp_combo_animal.Text.Equals("None"))
                        {
                            query = "delete from handler where employee_id=" + emp_field_id.Text;
                            Database.NonQuery(query);
                        }
                        else
                        {
                            if (isHandlerOf("", emp_field_id.Text))
                            {
                                query = "delete from handler where employee_id=@EID;";
                                Database.NonQuery(query, ("@EID", emp_field_id.Text));
                            }
                            query = "insert into handler (employee_id, animal_id) values (@EID, @Animal);";
                            Database.NonQuery(query, ("@EID", emp_field_id.Text), ("@Animal", Get_AnimalId(emp_combo_animal.Text)));
                        }
                    }
                    MessageBox.Show("Updated Employee with id: " + emp_field_id.Text);
                }
                catch
                {
                    //Add in ability to add specified department
                    MessageBox.Show("Could not find the specified employee with id: " + emp_field_id.Text);
                }
            }
        }
        private void Btn_emp_remove_Click(object sender, RoutedEventArgs e)
        {
            string query;
            if (emp_field_id.Text.Equals(""))
            {
                MessageBox.Show("Please specifiy the employee to update with their id");
            }
            else
            {
                try
                {
                    query = "select * from employee where employee_id=@EID;";
                    var employee_exists = Database.Query(query).Rows[0].ItemArray.Select(x => x.ToString().Trim()).ToArray()[0];
                    if(MessageBox.Show("Are you sure you want to remove employee with id: " + emp_field_id.Text + "?", "Warning!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        query = "delete from employee where employee_id=@EID";
                        Database.NonQuery(query, ("@EID", emp_field_id.Text));
                        query = "delete from handler where employee_id=@EID";
                        Database.NonQuery(query, ("@EID", emp_field_id.Text));
                    }
                }
                catch
                {
                    MessageBox.Show("Employee with id: " + emp_field_id + " does not exists.");
                }
            }

        }
        private void Btn_emp_add_Click(object sender, RoutedEventArgs e)
        {
            string query;
            if (emp_field_id.Text.Equals(""))
            {
                if(emp_field_date.Text.Equals("")|| emp_field_department.Text.Equals("") || emp_field_email.Text.Equals("") || emp_field_fname.Text.Equals("") || emp_field_lname.Text.Equals("") || emp_field_phone.Text.Equals("") || emp_field_ssn.Password.Equals("") || emp_field_type.Text.Equals(""))
                {
                    MessageBox.Show("Please fill out all fields except for the employee id field");
                }
                else
                {
                    string get_dep_id = Get_Department(emp_field_department.Text);
                    query = "select employee_id from employee;";
                    int next_emp_id = FindFirstNonIndex(query);
                    query = "insert into employee (employee_id, department_id, first_name, last_name, email, phone_number, employment_type, birthday, ssn) VALUES ("+ next_emp_id +", "+ get_dep_id +", '"+ emp_field_fname.Text + "', '" + emp_field_lname.Text + "', '" + emp_field_email.Text + "', '" + emp_field_phone.Text + "', '" + emp_field_type.Text + "', '" + emp_field_date.Text + "', '" + emp_field_ssn.Password +"');";
                    Database.NonQuery(query);
                    if (!emp_combo_animal.Text.Equals("") && !emp_combo_animal.Text.Equals("None"))
                    {
                        query = "insert into handler (employee_id, animal_id) values ("+next_emp_id+", "+Get_AnimalId(emp_combo_animal.Text)+");";
                        Database.NonQuery(query);
                    }
                    MessageBox.Show("Added " + emp_field_fname.Text + " to the database.");
                }
            }
            else
            {
                MessageBox.Show("You are not allowed to specify the employee id.");
                
            }
        }
        private void Btn_dpt_remove_Click(object sender, RoutedEventArgs e)
        {
            string query;
            if (dpt_field_name.Text.Equals(""))
            {
                MessageBox.Show("Please specfiy the department by name to remove.");
            }
            else
            {
                string get_dep_id = Get_Department(dpt_field_name.Text);
                if (get_dep_id.Equals("-1"))
                {
                    MessageBox.Show("Department with name: " + dpt_field_name.Text);
                }
                else
                {
                    query = "select * from employee where department_id=@Dept;";
                    if (Database.Query(query, ("@Dept", get_dep_id)).Rows.Count>0)
                    {
                        MessageBox.Show("Cannot delete department: " + dpt_field_name.Text + ", because " + Database.Query(query).Rows.Count + " user(s) are still in that department.");
                    }
                    else
                    {
                        query = $"delete from department where department_id=@Dept;";
                        Database.NonQuery(query, ("@Dept", get_dep_id));
                        MessageBox.Show("Successfully removed the department with name: " + dpt_field_name.Text);
                    }
                }
            }
        }
        private void Btn_dpt_get_Click(object sender, RoutedEventArgs e)
        {
            string get_dep_id = Get_Department(dpt_field_name.Text);
            string query;
            if (dpt_field_name.Text.Equals(""))
            {
                query = $"select * from department;";
            }
            else
            {
                query = "select * from department where department_id=@Dept";
            }
            //MessageBox.Show(query);
            datagrid_emp.ItemsSource = Database.Query(query, ("@Dept", get_dep_id))?.DefaultView;
        }

        private void populateEmpAnimalCombo()
        {
            try
            {
                string query = "SELECT * FROM animal";
                DataTable table = Database.Query(query);

                if (table == null)
                {
                    return;
                }

                emp_combo_animal.Items.Clear();
                emp_combo_animal.Items.Add("");
                emp_combo_animal.Items.Add("None");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string id = table.Rows[i]["animal_id"].ToString().Trim();
                    string name = table.Rows[i]["animal_name"].ToString().Trim();

                    string item = name + ", id=" + id;

                    emp_combo_animal.Items.Add(item);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
