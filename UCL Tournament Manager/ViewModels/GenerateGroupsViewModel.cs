using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;

namespace UCL_Tournament_Manager.ViewModels
{
    public class GenerateGroupsViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;
        private Tournament _selectedTournament;
        private int _numberOfGroups;

        public ObservableCollection<Tournament> Tournaments { get; set; }
        public ObservableCollection<Group> Groups { get; set; }

        public Tournament SelectedTournament
        {
            get => _selectedTournament;
            set => SetProperty(ref _selectedTournament, value);
        }

        public int NumberOfGroups
        {
            get => _numberOfGroups;
            set => SetProperty(ref _numberOfGroups, value);
        }

        public ICommand GenerateGroupsCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public Action? NavigateBack { get; set; }

        public GenerateGroupsViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
            Tournaments = new ObservableCollection<Tournament>();
            Groups = new ObservableCollection<Group>();

            GenerateGroupsCommand = new RelayCommand(async () => await GenerateGroupsAsync());
            NavigateBackCommand = new RelayCommand(() => NavigateBack?.Invoke());

            LoadTournaments();
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

        private async Task GenerateGroupsAsync()
        {
            if (SelectedTournament != null && NumberOfGroups > 0)
            {
                await _tournamentService.GenerateGroupsAsync(SelectedTournament.TournamentId, NumberOfGroups);
                LoadGroups();
            }
        }

        private async void LoadGroups()
        {
            if (SelectedTournament != null)
            {
                var groups = await _tournamentService.GetGroupsByTournamentIdAsync(SelectedTournament.TournamentId);
                Groups.Clear();
                foreach (var group in groups)
                {
                    Groups.Add(group);
                }
            }
        }
    }
}