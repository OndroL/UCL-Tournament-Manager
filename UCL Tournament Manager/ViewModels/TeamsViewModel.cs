using System.Windows.Input;
using UCL_Tournament_Manager.Services;
using UCL_Tournament_Manager.Views;

namespace UCL_Tournament_Manager.ViewModels
{
    public class TeamsViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;
        private object? _currentView;

        public object? CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public ICommand NavigateToCreateTeamCommand { get; }
        public ICommand NavigateToEditTeamCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public Action? NavigateBack { get; set; }

        public TeamsViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;

            NavigateToCreateTeamCommand = new RelayCommand(NavigateToCreateTeam);
            NavigateToEditTeamCommand = new RelayCommand(NavigateToEditTeam);
            NavigateBackCommand = new RelayCommand(() => NavigateBack?.Invoke());

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
    }
}
