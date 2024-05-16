using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using UCL_Tournament_Manager.Data;
using UCL_Tournament_Manager.Services;
using UCL_Tournament_Manager.ViewModels;
using UCL_Tournament_Manager.Views;

namespace UCL_Tournament_Manager
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Migrate the database
            try
            {
                using (var scope = ServiceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<TournamentContext>();
                    context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while setting up the database: {ex.Message}");
            }

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TournamentContext>(options =>
                options.UseSqlServer("Server=localhost,1433;Database=UCL_Tournament;User Id=sa;Password=YourStrong!Passw0rd;Encrypt=False;"),
                ServiceLifetime.Scoped);

            services.AddScoped<IRepository, Repository>();
            services.AddScoped<TournamentService>();
            services.AddScoped<MainWindowViewModel>();
            services.AddScoped<CreateTournamentViewModel>();
            services.AddScoped<CreateTeamViewModel>();
            services.AddScoped<GenerateBracketViewModel>();

            services.AddTransient<MainWindow>();
            services.AddTransient<CreateTournamentWindow>();
            services.AddTransient<CreateTeamWindow>();
            services.AddTransient<GenerateBracketWindow>();
        }
    }
}
