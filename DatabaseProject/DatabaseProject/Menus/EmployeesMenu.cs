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
                if (MessageBox.Show("Could not find the specified department with name: " + department_name, "Warning!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    query = "insert "
                }
                else
                {

                }
                stringDep_id = "-1";
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
                    query = "update employee set employee_id="+emp_field_id.Text+"";
                }
                catch
                {
                    //Add in ability to add specified department
                    MessageBox.Show("Could not find the specified employee with id: " + emp_field_id.Text);
                }
            }
        }
    }
}
