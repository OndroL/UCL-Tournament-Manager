using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;

namespace UCL_Tournament_Manager.ViewModels
{
    public class ShowPlayersViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;
        private Team _selectedTeam;

        public ObservableCollection<Team> Teams { get; set; }
        public ObservableCollection<Player> Players { get; set; }

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

        public bool IsTeamSelected => SelectedTeam != null;

        public ICommand NavigateBackCommand { get; }

        public Action? NavigateBack { get; set; }

        public ShowPlayersViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;

            Teams = new ObservableCollection<Team>();
            Players = new ObservableCollection<Player>();

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
    }
}