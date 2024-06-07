using System.Collections.ObjectModel;
using System.Windows.Input;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;

namespace UCL_Tournament_Manager.ViewModels
{
    public class AddPlayerViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;
        private Team _selectedTeam;
        private string _firstName;
        private string _lastName;
        private string _selectedPosition;

        public ObservableCollection<Team> Teams { get; set; }
        public ObservableCollection<string> Positions { get; set; }

        public Team SelectedTeam
        {
            get => _selectedTeam;
            set => SetProperty(ref _selectedTeam, value);
        }

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public string SelectedPosition
        {
            get => _selectedPosition;
            set => SetProperty(ref _selectedPosition, value);
        }

        public ICommand AddPlayerCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public Action? NavigateBack { get; set; }

        public AddPlayerViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;

            Teams = new ObservableCollection<Team>();
            Positions = new ObservableCollection<string> { "ST", "CM", "CB", "GK" };

            AddPlayerCommand = new RelayCommand(async () => await AddPlayerAsync());
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

        private async Task AddPlayerAsync()
        {
            if (SelectedTeam != null && !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(SelectedPosition))
            {
                var player = new Player
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Position = SelectedPosition,
                    TeamId = SelectedTeam.TeamId
                };
                await _tournamentService.AddPlayerAsync(player);
                NavigateBack?.Invoke();
            }
        }
    }
}
