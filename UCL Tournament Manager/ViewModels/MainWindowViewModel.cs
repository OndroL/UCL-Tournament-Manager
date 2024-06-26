﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using UCL_Tournament_Manager.Models;
using UCL_Tournament_Manager.Services;
using UCL_Tournament_Manager.Views;

namespace UCL_Tournament_Manager.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly TournamentService _tournamentService;
        private readonly System.Timers.Timer? _timer;
        private object? _currentView ;

        private bool _isMainViewVisible;

        public ObservableCollection<Tournament> Tournaments { get; set; }
        public ICommand NavigateToTournamentsCommand { get; }
        public ICommand NavigateToTeamsCommand { get; }
        public ICommand NavigateToGenerateBracketCommand { get; }
        public ICommand NavigateToAddScoreCommand { get; }
        public ICommand NavigateToGenerateGroupsViewCommand { get; }
        public ICommand NavigateToExportTournamentCommand { get; }

        public object? CurrentView
        {
            get => _currentView;
            set
            {
                SetProperty(ref _currentView, value);
                IsMainViewVisible = value == null;
            }
        }

        public bool IsMainViewVisible
        {
            get => _isMainViewVisible;
            set => SetProperty(ref _isMainViewVisible, value);
        }

        public MainWindowViewModel(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
            Tournaments = new ObservableCollection<Tournament>();

            NavigateToTournamentsCommand = new RelayCommand(NavigateToTournamentsView);
            NavigateToTeamsCommand = new RelayCommand(NavigateToTeamsView);
            NavigateToGenerateBracketCommand = new RelayCommand(NavigateToGenerateBracketView);
            NavigateToAddScoreCommand = new RelayCommand(NavigateToAddScoreView);
            NavigateToGenerateGroupsViewCommand = new RelayCommand(NavigateToGenerateGroupsView);
            NavigateToExportTournamentCommand = new RelayCommand(NavigateToExportTournamentView);


            _timer = new System.Timers.Timer(5000);
            _timer.Elapsed += TimerElapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;

            CurrentView = null;

            LoadData();
        }

        private async void TimerElapsed(object? sender, ElapsedEventArgs e)
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

        private void NavigateToTournamentsView()
        {
            var tournamentsViewModel = new TournamentsViewModel(_tournamentService);
            tournamentsViewModel.NavigateBack = () => CurrentView = null;
            CurrentView = new TournamentsView { DataContext = tournamentsViewModel };
        }

        private void NavigateToTeamsView()
        {
            var teamsViewModel = new TeamsViewModel(_tournamentService);
            teamsViewModel.NavigateBack = () => CurrentView = null;
            CurrentView = new TeamsView { DataContext = teamsViewModel };
        }

        private void NavigateToGenerateBracketView()
        {
            var generateBracketViewModel = new GenerateBracketViewModel(_tournamentService);
            generateBracketViewModel.NavigateBack = () => CurrentView = null;
            CurrentView = new GenerateBracketView { DataContext = generateBracketViewModel };
        }

        private void NavigateToAddScoreView()
        {
            var addScoreViewModel = new AddScoreViewModel(_tournamentService);
            addScoreViewModel.NavigateBack = () => CurrentView = null;
            CurrentView = new AddScoreView { DataContext = addScoreViewModel };
        }

        private void NavigateToGenerateGroupsView()
        {
            var generateGroupsViewModel = new GenerateGroupsViewModel(_tournamentService);
            generateGroupsViewModel.NavigateBack = () => CurrentView = null;
            CurrentView = new GenerateGroupsView { DataContext = generateGroupsViewModel };
        }

        private void NavigateToExportTournamentView()
        {
            var exportTournamentViewModel = new ExportTournamentViewModel(_tournamentService);
            exportTournamentViewModel.NavigateBack = () => CurrentView = null;
            CurrentView = new ExportTournamentView { DataContext = exportTournamentViewModel };
        }


    }
}