using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DatabaseProject
{
    class SQLConnection
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["connectstring"].ConnectionString;

        private SqlConnection connection;

        public bool Connected
        {
            get => connection != null && (connection.State == ConnectionState.Open || connection.State == ConnectionState.Executing);
        }

        public SQLConnection()
        {
            connection = new SqlConnection(connectionString);
        }

        public bool Connect()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Disconnect()
        {
            connection.Close();
        }

        public DataTable Query(string command)
        {

            try
            {
                DataTable table = new DataTable();

                SqlCommand cmd = new SqlCommand(command, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(table);

                return table;
            }
            catch
            {
                return null;
            }
        }
    }
}
