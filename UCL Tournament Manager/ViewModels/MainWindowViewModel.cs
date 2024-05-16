using System.Collections.ObjectModel;
using System.Windows.Input;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;
using UCL_Tournament_Manager.Views;

namespace UCL_Tournament_Manager.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;

        public ObservableCollection<Tournament> Tournaments { get; set; }
        public ICommand NavigateToCreateTournamentCommand { get; }

        public Action? NavigateToCreateTournamentView { get; set; }

        public MainWindowViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
            Tournaments = new ObservableCollection<Tournament>();

            NavigateToCreateTournamentCommand = new RelayCommand(() => NavigateToCreateTournamentView?.Invoke());

            LoadData();
        }

        private async void LoadData()
        {
            var tournaments = await _tournamentService.GetTournamentsAsync();
            Tournaments.Clear();
            foreach (var tournament in tournaments)
            {
                Tournaments.Add(tournament);
            }
        }
    }
}