using System.Collections.ObjectModel;
using System.Windows.Input;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;

namespace UCL_Tournament_Manager.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;

        public ObservableCollection<Tournament> Tournaments { get; set; }
        public ICommand NavigateToCreateTournamentCommand { get; }
        public ICommand NavigateToCreateTeamCommand { get; }
        public ICommand NavigateToGenerateBracketCommand { get; }

        public Action? NavigateToCreateTournamentView { get; set; }
        public Action? NavigateToCreateTeamView { get; set; }
        public Action? NavigateToGenerateBracketView { get; set; }


        public MainWindowViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
            Tournaments = new ObservableCollection<Tournament>();

            NavigateToCreateTournamentCommand = new RelayCommand(() => NavigateToCreateTournamentView?.Invoke());
            NavigateToCreateTeamCommand = new RelayCommand(() => NavigateToCreateTeamView?.Invoke());
            NavigateToGenerateBracketCommand = new RelayCommand(() => NavigateToGenerateBracketView?.Invoke());
            LoadData();
        }

        public async void LoadData()
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