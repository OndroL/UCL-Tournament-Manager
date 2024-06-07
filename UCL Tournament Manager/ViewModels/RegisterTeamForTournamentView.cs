using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;

namespace UCL_Tournament_Manager.ViewModels
{
    public class RegisterTeamForTournamentViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;
        private Team _selectedTeam;
        private Tournament _selectedTournament;

        public ObservableCollection<Team> Teams { get; set; }
        public ObservableCollection<Tournament> Tournaments { get; set; }

        public Team SelectedTeam
        {
            get => _selectedTeam;
            set => SetProperty(ref _selectedTeam, value);
        }

        public Tournament SelectedTournament
        {
            get => _selectedTournament;
            set => SetProperty(ref _selectedTournament, value);
        }

        public ICommand RegisterTeamCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public Action? NavigateBack { get; set; }

        public RegisterTeamForTournamentViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;

            Teams = new ObservableCollection<Team>();
            Tournaments = new ObservableCollection<Tournament>();

            RegisterTeamCommand = new RelayCommand(async () => await RegisterTeamAsync());
            NavigateBackCommand = new RelayCommand(() => NavigateBack?.Invoke());

            LoadData();
        }

        private async void LoadData()
        {
            var teams = await _tournamentService.GetTeamsAsync();
            Teams.Clear();
            foreach (var team in teams)
            {
                Teams.Add(team);
            }

            var tournaments = await _tournamentService.GetTournamentsAsync();
            Tournaments.Clear();
            foreach (var tournament in tournaments)
            {
                Tournaments.Add(tournament);
            }
        }

        private async Task RegisterTeamAsync()
        {
            if (SelectedTeam != null && SelectedTournament != null)
            {
                await _tournamentService.RegisterTeamAsync(SelectedTournament.TournamentId, SelectedTeam.TeamId);
                NavigateBack?.Invoke();
            }
        }
    }
}
