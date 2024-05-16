using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;

namespace UCL_Tournament_Manager.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;

        public ObservableCollection<Tournament> Tournaments { get; set; }
        public ObservableCollection<Team> Teams { get; set; }
        public ObservableCollection<Group> Groups { get; set; }
        public ObservableCollection<Match> Matches { get; set; }

        public ICommand CreateTournamentCommand { get; }
        public ICommand RegisterTeamCommand { get; }
        public ICommand GenerateGroupsCommand { get; }
        public ICommand GenerateSpiderCommand { get; }
        public ICommand RecordMatchScoreCommand { get; }

        public MainWindowViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
            Tournaments = new ObservableCollection<Tournament>();
            Teams = new ObservableCollection<Team>();
            Groups = new ObservableCollection<Group>();
            Matches = new ObservableCollection<Match>();

            CreateTournamentCommand = new RelayCommand(async () => await CreateTournament());
            RegisterTeamCommand = new RelayCommand(async () => await RegisterTeam());
            GenerateGroupsCommand = new RelayCommand(async () => await GenerateGroups());
            GenerateSpiderCommand = new RelayCommand(async () => await GenerateSpider());
            RecordMatchScoreCommand = new RelayCommand(async () => await RecordMatchScore());

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

        private async Task CreateTournament()
        {
            await _tournamentService.CreateTournamentAsync("New Tournament", "Location", DateTime.Now, DateTime.Now.AddDays(10));
            LoadData();
        }

        private async Task RegisterTeam()
        {
            var selectedTournament = Tournaments.FirstOrDefault();
            if (selectedTournament != null)
            {
                await _tournamentService.RegisterTeamAsync(selectedTournament.TournamentId, "New Team");
                LoadData();
            }
        }

        private async Task GenerateGroups()
        {
            var selectedTournament = Tournaments.FirstOrDefault();
            if (selectedTournament != null)
            {
                await _tournamentService.GenerateGroupsAsync(selectedTournament.TournamentId, 4);
                LoadData();
            }
        }

        private async Task GenerateSpider()
        {
            var selectedTournament = Tournaments.FirstOrDefault();
            if (selectedTournament != null)
            {
                await _tournamentService.GenerateSpiderAsync(selectedTournament.TournamentId);
                LoadData();
            }
        }

        private async Task RecordMatchScore()
        {
            var match = Matches.FirstOrDefault();
            if (match != null)
            {
                await _tournamentService.RecordMatchScoreAsync(match.MatchId, 1, 2);
                LoadData();
            }
        }
    }
}