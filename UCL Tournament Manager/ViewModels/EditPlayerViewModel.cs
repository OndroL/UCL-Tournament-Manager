using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;

namespace UCL_Tournament_Manager.ViewModels
{
    public class EditPlayerViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;
        private Team _selectedTeam;
        private Player _selectedPlayer;

        public ObservableCollection<Team> Teams { get; set; }
        public ObservableCollection<Player> Players { get; set; }
        public ObservableCollection<string> Positions { get; set; } = new ObservableCollection<string> { "ST", "CM", "CB", "GK" };

        public Team SelectedTeam
        {
            get => _selectedTeam;
            set
            {
                if (SetProperty(ref _selectedTeam, value))
                {
                    LoadPlayers();
                    OnPropertyChanged(nameof(IsTeamSelected));
                }
            }
        }

        public Player SelectedPlayer
        {
            get => _selectedPlayer;
            set
            {
                if (SetProperty(ref _selectedPlayer, value))
                {
                    OnPropertyChanged(nameof(IsPlayerSelected));
                }
            }
        }

        public bool IsTeamSelected => SelectedTeam != null;
        public bool IsPlayerSelected => SelectedPlayer != null;

        public ICommand UpdatePlayerCommand { get; }
        public ICommand DeletePlayerCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public Action? NavigateBack { get; set; }

        public EditPlayerViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;

            Teams = new ObservableCollection<Team>();
            Players = new ObservableCollection<Player>();

            UpdatePlayerCommand = new RelayCommand(async () => await UpdatePlayerAsync());
            DeletePlayerCommand = new RelayCommand(async () => await DeletePlayerAsync());
            NavigateBackCommand = new RelayCommand(() => NavigateBack?.Invoke());

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

        private async void LoadPlayers()
        {
            if (SelectedTeam != null)
            {
                var players = await _tournamentService.GetPlayersByTeamIdAsync(SelectedTeam.TeamId);
                Players.Clear();
                foreach (var player in players)
                {
                    Players.Add(player);
                }
            }
        }

        private async Task UpdatePlayerAsync()
        {
            if (SelectedPlayer != null)
            {
                await _tournamentService.UpdatePlayerAsync(SelectedPlayer);
                NavigateBack?.Invoke();
            }
        }

        private async Task DeletePlayerAsync()
        {
            if (SelectedPlayer != null)
            {
                await _tournamentService.DeletePlayerAsync(SelectedPlayer.PlayerId);
                NavigateBack?.Invoke();
            }
        }
    }
}