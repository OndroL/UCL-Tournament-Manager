using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;

namespace UCL_Tournament_Manager.ViewModels
{
    public class GenerateBracketViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;

        public int TournamentId { get; set; }
        public ObservableCollection<Models.Match> Matches { get; set; }
        public ICommand GenerateBracketCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public Action? NavigateBack { get; set; }

        public GenerateBracketViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
            Matches = new ObservableCollection<Models.Match>();

            GenerateBracketCommand = new RelayCommand(async () => await GenerateBracketAsync());
            NavigateBackCommand = new RelayCommand(() => NavigateBack?.Invoke());
        }

        private async Task GenerateBracketAsync()
        {
            await _tournamentService.GenerateBracketAsync(TournamentId);
            var matches = await _tournamentService.GetMatchesByTournamentIdAsync(TournamentId);
            Matches.Clear();
            foreach (var match in matches)
            {
                Matches.Add(match);
            }
        }
    }
}
