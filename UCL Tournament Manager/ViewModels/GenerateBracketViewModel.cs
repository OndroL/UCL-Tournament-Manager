using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;

namespace UCL_Tournament_Manager.ViewModels
{
    public class GenerateBracketViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;
        private Tournament _selectedTournament;
        private int _selectedNumberOfTeams;
        private readonly List<int> _teamCounts = new List<int> { 4, 8, 16 };

        public ObservableCollection<Tournament> Tournaments { get; set; }
        public Tournament SelectedTournament
        {
            get => _selectedTournament;
            set => SetProperty(ref _selectedTournament, value);
        }

        public int SelectedNumberOfTeams
        {
            get => _selectedNumberOfTeams;
            set => SetProperty(ref _selectedNumberOfTeams, value);
        }

        public List<int> TeamCounts => _teamCounts;

        public ICommand GenerateBracketCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public Action? NavigateBack { get; set; }

        public GenerateBracketViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
            Tournaments = new ObservableCollection<Tournament>();
            LoadTournaments();

            GenerateBracketCommand = new RelayCommand(async () => await GenerateBracketAsync());
            NavigateBackCommand = new RelayCommand(() => NavigateBack?.Invoke());
        }

        private async void LoadTournaments()
        {
            var tournaments = await _tournamentService.GetTournamentsAsync();
            Tournaments.Clear();
            foreach (var tournament in tournaments)
            {
                Tournaments.Add(tournament);
            }
        }

        private async Task GenerateBracketAsync()
        {
            if (SelectedTournament != null)
            {
                await _tournamentService.GenerateBracketAsync(SelectedTournament.TournamentId, SelectedNumberOfTeams);
                NavigateBack?.Invoke();
            }
        }
    }
}
