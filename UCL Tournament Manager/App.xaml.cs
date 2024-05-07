using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using System.Windows;
using UCL_Tournament_Manager.Data;
using UCL_Tournament_Manager.Helpers;

namespace UCL_Tournament_Manager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TournamentContext>();
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=UCL_Tournament;User Id=sa;Password=YourStrong!Passw0rd;Encrypt=False;");
            using (var context = new TournamentContext(optionsBuilder.Options))
            {
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    // Log the error or notify the user
                    MessageBox.Show($"An error occurred while setting up the database: {ex.Message}");
                }
            }

            // Initialize the main window
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            // Create and test database connection
            DatabaseHelper dbHelper = new DatabaseHelper();
            dbHelper.TestDatabaseConnection();
        }
    }
}
