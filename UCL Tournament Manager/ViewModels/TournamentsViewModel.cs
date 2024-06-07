using System.Windows.Input;
using UCL_Tournament_Manager.Services;
using UCL_Tournament_Manager.Views;

namespace UCL_Tournament_Manager.ViewModels
{
    public class TournamentsViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;
        private object? _currentView;

        public object? CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public ICommand NavigateToCreateTournamentCommand { get; }
        public ICommand NavigateToEditTournamentCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public Action? NavigateBack { get; set; }

        public TournamentsViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;

            NavigateToCreateTournamentCommand = new RelayCommand(NavigateToCreateTournament);
            NavigateToEditTournamentCommand = new RelayCommand(NavigateToEditTournament);
            NavigateBackCommand = new RelayCommand(() => NavigateBack?.Invoke());
        }

        private void NavigateToCreateTournament()
        {
            var createTournamentViewModel = new CreateTournamentViewModel(_tournamentService);
            createTournamentViewModel.NavigateBack = () => CurrentView = null;
            CurrentView = new CreateTournamentView { DataContext = createTournamentViewModel };
        }

        private void NavigateToEditTournament()
        {
            var editTournamentViewModel = new EditTournamentViewModel(_tournamentService);
            editTournamentViewModel.NavigateBack = () => CurrentView = null;
            CurrentView = new EditTournamentView { DataContext = editTournamentViewModel };
        }
    }
}