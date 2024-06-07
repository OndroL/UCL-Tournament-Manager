using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;

namespace UCL_Tournament_Manager.ViewModels
{
    public class ExportTournamentViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;
        public ObservableCollection<Tournament> Tournaments { get; set; }
        private Tournament _selectedTournament;

        public Tournament SelectedTournament
        {
            get => _selectedTournament;
            set => SetProperty(ref _selectedTournament, value);
        }

        public ICommand ExportToCsvCommand { get; }
        public ICommand ExportToJsonCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public Action NavigateBack { get; set; }

        public ExportTournamentViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;

            Tournaments = new ObservableCollection<Tournament>();
            ExportToCsvCommand = new RelayCommand(async () => await ExportToCsv());
            ExportToJsonCommand = new RelayCommand(async () => await ExportToJson());
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

        private async Task ExportToCsv()
        {
            if (SelectedTournament == null)
            {
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                DefaultExt = "csv",
                AddExtension = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                await _tournamentService.ExportTournamentToCsvAsync(SelectedTournament.TournamentId, saveFileDialog.FileName);
            }
        }

        private async Task ExportToJson()
        {
            if (SelectedTournament == null)
            {
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON Files (*.json)|*.json",
                DefaultExt = "json",
                AddExtension = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                await _tournamentService.ExportTournamentToJsonAsync(SelectedTournament.TournamentId, saveFileDialog.FileName);
            }
        }
    }
}