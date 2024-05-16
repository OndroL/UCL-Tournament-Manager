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
            viewModel.NavigateToCreateTeamView = NavigateToCreateTeamWindow;
            viewModel.NavigateToGenerateBracketView = NavigateToGenerateBracketWindow;
            viewModel.NavigateToAddScoreView = NavigateToAddScoreView;

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

        private void NavigateToCreateTeamWindow()
        {
            var createTeamWindow = new CreateTeamWindow();
            var createTeamViewModel = ((App)Application.Current).ServiceProvider.GetRequiredService<CreateTeamViewModel>();
            createTeamViewModel.NavigateBack = () =>
            {
                createTeamWindow.Close();
                this.Show();
            };
            createTeamWindow.DataContext = createTeamViewModel;
            createTeamWindow.Show();
            this.Hide();
        }

        private void NavigateToGenerateBracketWindow()
        {
            var generateBracketWindow = new GenerateBracketWindow();
            var generateBracketViewModel = ((App)Application.Current).ServiceProvider.GetRequiredService<GenerateBracketViewModel>();
            generateBracketViewModel.NavigateBack = () =>
            {
                generateBracketWindow.Close();
                this.Show();
            };
            generateBracketWindow.DataContext = generateBracketViewModel;
            generateBracketWindow.Show();
            this.Hide();
        }
        private void NavigateToAddScoreView()
        {
            var addScoreWindow = new AddScoreWindow();
            var addScoreViewModel = ((App)Application.Current).ServiceProvider.GetRequiredService<AddScoreViewModel>();
            addScoreViewModel.NavigateBack = () =>
            {
                addScoreWindow.Close();
                this.Show();
            };
            addScoreWindow.DataContext = addScoreViewModel;
            addScoreWindow.Show();
            this.Hide();
        }
    }
}