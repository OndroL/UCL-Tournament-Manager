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

        private string? _teamName;

        public string? TeamName
        {
            get => _teamName;
            set => SetProperty(ref _teamName, value);
        }
        public ICommand CreateTeamCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public Action? NavigateBack { get; set; }

        public CreateTeamViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
            Teams = new ObservableCollection<Team>();

            CreateTeamCommand = new RelayCommand(async () => await CreateTeamAsync());
            NavigateBackCommand = new RelayCommand(() => NavigateBack?.Invoke());
        }

        private async Task CreateTeamAsync()
        {
            if (TeamName != null)
            {
                await _tournamentService.CreateTeamAsync(TeamName);
            }
        }
    }
}