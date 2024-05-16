using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using UCL_Tournament_Manager.ViewModels;

namespace UCL_Tournament_Manager.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = ((App)Application.Current).ServiceProvider.GetRequiredService<MainWindowViewModel>();
            viewModel.NavigateToCreateTournamentView = NavigateToCreateTournamentView;
            DataContext = viewModel;
        }

        private void NavigateToCreateTournamentView()
        {
            var createTournamentWindow = new CreateTournamentWindow();
            var createTournamentViewModel = ((App)Application.Current).ServiceProvider.GetRequiredService<CreateTournamentViewModel>();
            createTournamentViewModel.NavigateBack = () =>
            {
                createTournamentWindow.Close();
                this.Show();
            };
            createTournamentWindow.DataContext = createTournamentViewModel;
            createTournamentWindow.Show();
            this.Hide();
        }
    }
}