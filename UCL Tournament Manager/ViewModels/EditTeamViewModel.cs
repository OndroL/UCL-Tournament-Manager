using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;

namespace UCL_Tournament_Manager.ViewModels
{
    public class EditTeamViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;
        private Team _selectedTeam;
        private bool _isTeamSelected;

        public ObservableCollection<Team> Teams { get; set; }

        public bool IsTeamSelected
        {
            get => _isTeamSelected;
            set => SetProperty(ref _isTeamSelected, value);
        }

        public Team SelectedTeam
        {
            get => _selectedTeam;
            set
            {
                if (SetProperty(ref _selectedTeam, value))
                {
                    IsTeamSelected = value != null;
                    OnPropertyChanged(nameof(IsTeamSelected));
                }
            }
        }

        public ICommand SaveTeamCommand { get; }
        public ICommand DeleteTeamCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public Action? NavigateBack { get; set; }

        public EditTeamViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
            Teams = new ObservableCollection<Team>();

            SaveTeamCommand = new RelayCommand(async () => await SaveTeamAsync());
            DeleteTeamCommand = new RelayCommand(async () => await DeleteTeamAsync());
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

        private async Task SaveTeamAsync()
        {
            if (SelectedTeam != null)
            {
                await _tournamentService.UpdateTeamAsync(SelectedTeam);
                NavigateBack?.Invoke();
            }
        }

        private async Task DeleteTeamAsync()
        {
            if (SelectedTeam != null)
            {
                await _tournamentService.DeleteTeamAsync(SelectedTeam);
                NavigateBack?.Invoke();
            }
        }
    }
}