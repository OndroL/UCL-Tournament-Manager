using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Timers;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;
using System.Windows;

namespace UCL_Tournament_Manager.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;
        private readonly System.Timers.Timer _timer;


        public ObservableCollection<Tournament> Tournaments { get; set; }
        public ICommand NavigateToCreateTournamentCommand { get; }
        public ICommand NavigateToCreateTeamCommand { get; }
        public ICommand NavigateToGenerateBracketCommand { get; }
        public ICommand NavigateToAddScoreCommand { get; }

        public Action? NavigateToCreateTournamentView { get; set; }
        public Action? NavigateToCreateTeamView { get; set; }
        public Action? NavigateToGenerateBracketView { get; set; }
        public Action? NavigateToAddScoreView { get; set; }



        public MainWindowViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
            Tournaments = new ObservableCollection<Tournament>();

            NavigateToCreateTournamentCommand = new RelayCommand(() => NavigateToCreateTournamentView?.Invoke());
            NavigateToCreateTeamCommand = new RelayCommand(() => NavigateToCreateTeamView?.Invoke());
            NavigateToGenerateBracketCommand = new RelayCommand(() => NavigateToGenerateBracketView?.Invoke());
            NavigateToAddScoreCommand = new RelayCommand(() => NavigateToAddScoreView?.Invoke());

            // Configure the timer
            _timer = new System.Timers.Timer(5000); // 5 seconds interval
            _timer.Elapsed += TimerElapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;

            LoadData();
        }

        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            await LoadDataAsync();
        }

        public async void LoadData()
        {
            await LoadDataAsync();
        }
        private async Task LoadDataAsync()
        {
            var tournaments = await _tournamentService.GetTournamentsAsync();

            Application.Current.Dispatcher.Invoke(() =>
            {
                Tournaments.Clear();
                foreach (var tournament in tournaments)
                {
                    Tournaments.Add(tournament);
                }
            });
        }
    }
}