using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;

namespace UCL_Tournament_Manager.ViewModels
{
    public class EditTournamentViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;
        private Tournament _selectedTournament;
        private bool _isTournamentSelected;

        public ObservableCollection<Tournament> Tournaments { get; set; }

        public bool IsTournamentSelected
        {
            get => _isTournamentSelected;
            set => SetProperty(ref _isTournamentSelected, value);
        }

        public Tournament SelectedTournament
        {
            get => _selectedTournament;
            set
            {
                if (SetProperty(ref _selectedTournament, value))
                {
                    IsTournamentSelected = value != null;
                    OnPropertyChanged(nameof(IsTournamentSelected));
                }
            }
        }

        public ICommand SaveTournamentCommand { get; }
        public ICommand NavigateBackCommand { get; }
        public ICommand DeleteTournamentCommand { get; }

        public Action NavigateBack { get; set; }

        public EditTournamentViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
            Tournaments = new ObservableCollection<Tournament>();

            SaveTournamentCommand = new RelayCommand(async () => await SaveTournamentAsync());
            DeleteTournamentCommand = new RelayCommand(async () => await DeleteTournamentAsync());
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

        private async Task SaveTournamentAsync()
        {
            if (SelectedTournament != null)
            {
                await _tournamentService.UpdateTournamentAsync(SelectedTournament);
                NavigateBack?.Invoke();
            }
        }

        private async Task DeleteTournamentAsync()
        {
            if (SelectedTournament != null)
            {
                await _tournamentService.DeleteTournamentAsync(SelectedTournament);
                NavigateBack?.Invoke();
            }
        }
    }
}