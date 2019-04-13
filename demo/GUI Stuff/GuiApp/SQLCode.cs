using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;

namespace GuiApp
{
    class SQLCode
    {
        private static SqlConnection cnn;
        private static bool valid = false;

        /*
         * Connects to the database. Should be called at the start of the program.
         */
        public static bool connect()
        {
            string connectionString;

            connectionString = @"Data Source=MSI; Initial Catalog=SmallAnimalDB;Integrated Security=true";

            cnn = new SqlConnection(connectionString);

            try
            {
                cnn.Open();
                valid = true;
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            valid = false;
            return false;
        }

        /*
        * Connects to the database. Should be called at the start of the program.
        */
        public static bool connect(string connectString)
        {
            cnn = new SqlConnection(connectString);

            try
            {
                cnn.Open();
                valid = true;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            valid = false;
            return false;
        }
        /*
         * Disconnects from the database. Should be called at the end of the program.
         */
        public static bool disconnect()
        {
            if (valid == true)
            {
                try
                {
                    cnn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            return false;
        }

        /*
         * Executes a sql query on the connected database. Always returns an output string
         * Columns are separated by the ascii character 28. This will not appear when printed.
         * Rows are separated by a line break.
         */ 
        public static string executeCommand(string commandText)
        {
            string output = "";
            SqlCommand com;
            SqlDataReader daRead;
            SqlDataAdapter adapter = new SqlDataAdapter();

            com = new SqlCommand("Set Identity_Insert Animals ON", cnn);
            com.ExecuteNonQuery();

            com = new SqlCommand(commandText, cnn);

            //com = new SqlCommand("Insert into Animals (SpeciesID, SpeciesName) values(" + id + ", '" + name + "')", cnn);
            
            adapter.InsertCommand = com;
            adapter.InsertCommand.ExecuteNonQuery();
            
            daRead = com.ExecuteReader();

            while (daRead.Read())
            {
                for (int i = 0; i < daRead.FieldCount; i++)
                {
                    output += daRead.GetValue(i);
                    if(i!=daRead.FieldCount-1)
                    {
                        output += (char)28;
                    }
                }
                output += "\n";
            }

            com.Dispose();
            return output;
        }
    }
}
