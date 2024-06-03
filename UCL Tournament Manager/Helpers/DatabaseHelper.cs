using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;

namespace UCL_Tournament_Manager.Helpers
{
    public class DatabaseHelper
    {
        private readonly string connectionString;

        public DatabaseHelper()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public void TestDatabaseConnection()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    MessageBox.Show("Connection successful!");
                }
            }
            catch (SqlException sqlEx)
            {
                string detailedErrors = string.Join("\n", sqlEx.Errors.Cast<SqlError>().Select(e => e.Message));
                MessageBox.Show($"Failed to connect to the database. SQL Errors: {detailedErrors}\n\nException: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
