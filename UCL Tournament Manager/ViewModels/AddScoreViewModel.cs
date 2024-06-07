using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;

namespace UCL_Tournament_Manager.ViewModels
{
    public class AddScoreViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;
        public ObservableCollection<Team> Teams { get; set; }
        public ObservableCollection<Match> Matches { get; set; }
        private int _team1Score;
        private int _team2Score;
        private Match _selectedMatch;
        private bool _isMatchSelected;
        private string _matchState;

        public bool IsMatchSelected
        {
            get => _isMatchSelected;
            set => SetProperty(ref _isMatchSelected, value);
        }

        public string MatchState
        {
            get => _matchState;
            set => SetProperty(ref _matchState, value);
        }

        public Match SelectedMatch
        {
            get => _selectedMatch;
            set
            {
                if (SetProperty(ref _selectedMatch, value))
                {
                    IsMatchSelected = value != null;
                    OnPropertyChanged(nameof(IsMatchSelected));
                    Team1Score = value?.Team1Score ?? 0;
                    Team2Score = value?.Team2Score ?? 0;
                    MatchState = value?.IsTeam1Winner.HasValue == true ? "Finished" : "Not played";
                }
            }
        }

        public int Team1Score
        {
            get => _team1Score;
            set => SetProperty(ref _team1Score, value);
        }

        public int Team2Score
        {
            get => _team2Score;
            set => SetProperty(ref _team2Score, value);
        }

        public ICommand SaveScoreCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public Action? NavigateBack { get; set; }

        public AddScoreViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
            Matches = new ObservableCollection<Match>();
            Teams = new ObservableCollection<Team>();

            SaveScoreCommand = new RelayCommand(async () => await SaveScoreAsync());
            NavigateBackCommand = new RelayCommand(() => NavigateBack?.Invoke());

            LoadMatches();
        }

        private async void LoadMatches()
        {
            var teams = await _tournamentService.GetTeamsAsync();
            Teams.Clear();
            foreach (var team in teams)
            {
                Teams.Add(team);
            }

            var matches = await _tournamentService.GetAllMatchesAsync();
            Matches.Clear();
            foreach (var match in matches)
            {
                Matches.Add(match);
            }
        }

        private async Task SaveScoreAsync()
        {
            if (SelectedMatch != null)
            {
                await _tournamentService.RecordMatchScoreAsync(SelectedMatch.MatchId, Team1Score, Team2Score);
                NavigateBack?.Invoke();
            }
        }
    }
}