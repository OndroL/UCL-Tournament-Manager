using System.Collections.ObjectModel;
using System.Windows.Input;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;
using UCL_Tournament_Manager.Views;

namespace UCL_Tournament_Manager.ViewModels
{
    public class TeamsViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;
        private object? _currentView;
        public ObservableCollection<Team> Teams { get; set; }

        public object? CurrentView
        {
            get => _currentView;
            set
            {
                SetProperty(ref _currentView, value);
                OnPropertyChanged(nameof(IsMainViewVisible));
            }
        }

        public bool IsMainViewVisible => CurrentView == null;

        public ICommand NavigateToCreateTeamCommand { get; }
        public ICommand NavigateToEditTeamCommand { get; }
        public ICommand NavigateToAddPlayerCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public Action? NavigateBack { get; set; }

        public TeamsViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;

            NavigateToCreateTeamCommand = new RelayCommand(NavigateToCreateTeam);
            NavigateToEditTeamCommand = new RelayCommand(NavigateToEditTeam);
            NavigateToAddPlayerCommand = new RelayCommand(NavigateToAddPlayer);
            NavigateBackCommand = new RelayCommand(() => NavigateBack?.Invoke());

            Teams = new ObservableCollection<Team>();
            LoadTeams();

        }

        private async void LoadTeams()
        {
            var teams = await _tournamentService.GetTeamsAsync();
            Teams.Clear();
            foreach (var team in teams)
            {
                Teams.Add(team);
            }
        }

        private void NavigateToCreateTeam()
        {
            var createTeamViewModel = new CreateTeamViewModel(_tournamentService);
            createTeamViewModel.NavigateBack = () => CurrentView = null;
            CurrentView = new CreateTeamView { DataContext = createTeamViewModel };
        }

        private void NavigateToEditTeam()
        {
            var editTeamViewModel = new EditTeamViewModel(_tournamentService);
            editTeamViewModel.NavigateBack = () => CurrentView = null;
            CurrentView = new EditTeamView { DataContext = editTeamViewModel };
        }

        private void NavigateToAddPlayer()
        {
            var addPlayerViewModel = new AddPlayerViewModel(_tournamentService);
            addPlayerViewModel.NavigateBack = () => CurrentView = null;
            CurrentView = new AddPlayerView { DataContext = addPlayerViewModel };
        }
    }
}
