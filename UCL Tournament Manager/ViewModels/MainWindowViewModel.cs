using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;
using UCL_Tournament_Manager.Views;

namespace UCL_Tournament_Manager.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;
        private readonly System.Timers.Timer _timer;
        private object _currentView;

        public ObservableCollection<Tournament> Tournaments { get; set; }
        public ICommand NavigateToCreateTournamentCommand { get; }
        public ICommand NavigateToCreateTeamCommand { get; }
        public ICommand NavigateToGenerateBracketCommand { get; }
        public ICommand NavigateToAddScoreCommand { get; }

        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public MainWindowViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
            Tournaments = new ObservableCollection<Tournament>();

            NavigateToCreateTournamentCommand = new RelayCommand(NavigateToCreateTournamentView);
            NavigateToCreateTeamCommand = new RelayCommand(NavigateToCreateTeamView);
            NavigateToGenerateBracketCommand = new RelayCommand(NavigateToGenerateBracketView);
            NavigateToAddScoreCommand = new RelayCommand(NavigateToAddScoreView);

            // Configure the timer
            _timer = new System.Timers.Timer(5000);
            _timer.Elapsed += TimerElapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;

            LoadData();
        }

        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            await LoadDataAsync();
        }

        public async void LoadData()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var tournaments = await _tournamentService.GetTournamentsAsync();

            Application.Current.Dispatcher.Invoke(() =>
            {
                Tournaments.Clear();
                foreach (var tournament in tournaments)
                {
                    Tournaments.Add(tournament);
                }
            });
        }

        private void NavigateToCreateTournamentView()
        {
            var createTournamentViewModel = new CreateTournamentViewModel(_tournamentService);
            createTournamentViewModel.NavigateBack = () => CurrentView = this;
            CurrentView = new CreateTournamentView { DataContext = createTournamentViewModel };
        }

        private void NavigateToCreateTeamView()
        {
            var createTeamViewModel = new CreateTeamViewModel(_tournamentService);
            createTeamViewModel.NavigateBack = () => CurrentView = this;
            CurrentView = new CreateTeamView { DataContext = createTeamViewModel };
        }

        private void NavigateToGenerateBracketView()
        {
            var generateBracketViewModel = new GenerateBracketViewModel(_tournamentService);
            generateBracketViewModel.NavigateBack = () => CurrentView = this;
            CurrentView = new GenerateBracketView { DataContext = generateBracketViewModel };
        }

        private void NavigateToAddScoreView()
        {
            var addScoreViewModel = new AddScoreViewModel(_tournamentService);
            addScoreViewModel.NavigateBack = () => CurrentView = this;
            CurrentView = new AddScoreView { DataContext = addScoreViewModel };
        }
    }
}