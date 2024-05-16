using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using UCL_Tournament_Manager.ViewModels;

namespace UCL_Tournament_Manager.Views
{
    public partial class CreateTeamWindow : Window
    {
        public CreateTeamWindow()
        {
            InitializeComponent();
            var viewModel = ((App)Application.Current).ServiceProvider.GetRequiredService<CreateTeamViewModel>();
            viewModel.NavigateBack = NavigateBack;
            DataContext = viewModel;
        }

        private void NavigateBack()
        {
            this.Close();
            var mainWindow = ((App)Application.Current).ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
