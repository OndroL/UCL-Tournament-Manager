using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;

namespace UCL_Tournament_Manager.ViewModels
{
    public class CreateTeamViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;

        public ObservableCollection<Team> Teams { get; set; }
        public string TeamName { get; set; }
        public ICommand CreateTeamCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public Action? NavigateBack { get; set; }

        public CreateTeamViewModel(TournamentService tournamentService)
        {
            TeamName = "Enter Name of Team";
            _tournamentService = tournamentService;
            Teams = new ObservableCollection<Team>();

            CreateTeamCommand = new RelayCommand(async () => await CreateTeamAsync());
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
        }

        private async Task CreateTeamAsync()
        {
            await _tournamentService.CreateTeamAsync(TeamName);
            LoadData();
        }
    }
}