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
        #region Events





        #endregion
        private string Get_Department(string department_name)
        {
            if (department_name.Equals(""))
            {
                return "" + -1;
            }
            //MessageBox.Show(department_name);
            string query = "select department_id from department where department_name='" + department_name + "';";
            string stringDep_id;
            try
            {
                stringDep_id = Query(query).Rows[0].ItemArray.Select(x => x.ToString().Trim()).ToArray()[0];
            }
            catch
            {
                //Add in ability to add specified department
                if (MessageBox.Show("Could not find the specified department with name: " + department_name +"\nWould you like to add it?", "Warning!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    query = "select department_id from department;";
                    int next_id = FindFirstNonIndex(query);
                    query = "insert into department (department_id, department_name) values (" + next_id + ", '" + department_name + "');";
                    NonQuery(query);
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
                query = $"select * from employee where employee_id={emp_field_id.Text};";
            }
            else
            {
                string get_dep_id = Get_Department(emp_field_department.Text);
                //MessageBox.Show(get_dep_id);
                query = $"select * from employee where"+" department_id="+get_dep_id + ((!emp_field_ssn.Password.Equals("")) ? " AND ssn=" + emp_field_ssn.Password : "") + ((!emp_field_email.Text.Equals("")) ? " AND email='" + emp_field_email.Text + "'" : "") + ((!emp_field_lname.Text.Equals("")) ? " AND last_name='" + emp_field_lname.Text + "'" : "") + ((!emp_field_fname.Text.Equals("")) ? " AND first_name='" + emp_field_fname.Text + "'" : "") + ((!emp_field_phone.Text.Equals("")) ? " AND phone_number='" + emp_field_phone.Text + "'" : "") + ((!emp_field_type.Text.Equals("")) ? " AND employment_type='" + emp_field_type.Text + "'" : "") + ((!emp_field_date.Text.Equals("")) ? " AND birthday='" + emp_field_date.Text + "'" : "") + ";";
                if (query.Equals("select * from employee where;")){
                    query = "select * from employee;";
                }
            }
            //MessageBox.Show(query);
            datagrid_emp.ItemsSource = Query(query)?.DefaultView;
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
                query = "select * from employee where employee_id=" + emp_field_id.Text;
                try
                {
                    var testEmployee = Query(query).Rows[0].ItemArray.Select(x => x.ToString().Trim()).ToArray()[0];
                    query = "update employee set employee_id="+emp_field_id.Text + ((!get_dep_id.Equals("-1")) ? ", department_id=" + get_dep_id : "")  + ((!emp_field_ssn.Password.Equals("")) ? ", ssn=" + emp_field_ssn.Password : "") + ((!emp_field_email.Text.Equals("")) ? ", email='" + emp_field_email.Text + "'" : "") + ((!emp_field_lname.Text.Equals("")) ? ", last_name='" + emp_field_lname.Text + "'" : "") + ((!emp_field_fname.Text.Equals("")) ? ", first_name='" + emp_field_fname.Text + "'" : "") + ((!emp_field_phone.Text.Equals("")) ? ", phone_number='" + emp_field_phone.Text + "'" : "") + ((!emp_field_type.Text.Equals("")) ? ", employment_type='" + emp_field_type.Text + "'" : "") + ((!emp_field_date.Text.Equals("")) ? ", birthday='" + emp_field_date.Text + "'" : "") + " where employee_id="+ emp_field_id.Text +";";
                    NonQuery(query);
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
                    query = "select * from employee where employee_id="+emp_field_id.Text;
                    var employee_exists = Query(query).Rows[0].ItemArray.Select(x => x.ToString().Trim()).ToArray()[0];
                    if(MessageBox.Show("Are you sure you want to remove employee with id: " + emp_field_id.Text + "?", "Warning!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        query = "delete from employee where employee_id=" + emp_field_id.Text;
                        NonQuery(query);
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
                    NonQuery(query);
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
            
        }
        private void populateEmpAnimalCombo()
        {
            try
            {

            }
            catch
            {

            }
        }
    }
}
