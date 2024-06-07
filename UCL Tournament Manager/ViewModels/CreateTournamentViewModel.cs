using System.Collections.ObjectModel;
using System.Windows.Input;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;

namespace UCL_Tournament_Manager.ViewModels
{
    public class CreateTournamentViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;

        public ObservableCollection<Tournament> Tournaments { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICommand? CreateTournamentCommand { get; }
        public ICommand? NavigateBackCommand { get; }

        public Action? NavigateBack { get; set; }

        public CreateTournamentViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
            Tournaments = new ObservableCollection<Tournament>();

            Name = "Enter Name of Tournament";
            Location = "Enter location of Tournament";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(7);

            CreateTournamentCommand = new RelayCommand(async () => await CreateTournamentAsync());
            NavigateBackCommand = new RelayCommand(() => NavigateBack?.Invoke());

            LoadData();
        }

        private async void LoadData()
        {
            var tournaments = await _tournamentService.GetTournamentsAsync();
            Tournaments.Clear();
            foreach (var tournament in tournaments)
            {
                Tournaments.Add(tournament);
            }
        }

        private async Task CreateTournamentAsync()
        {
            await _tournamentService.CreateTournamentAsync(Name, Location, StartDate, EndDate);
            LoadData();
        }
    }
}