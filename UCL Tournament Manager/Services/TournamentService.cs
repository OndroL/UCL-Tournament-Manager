using UCL_Tournament_Manager.Data;
using UCL_Tournament_Manager.Models;

namespace UCL_Tournament_Manager.Services
{
    public class TournamentService
    {
        private readonly IRepository _repository;

        public TournamentService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsAsync()
        {
            return await _repository.GetAllAsync<Tournament>();
        }

        public async Task<IEnumerable<Team>> GetTeamsAsync()
        {
            return await _repository.GetAllAsync<Team>();
        }

        public async Task CreateTournamentAsync(string name, string location, DateTime startDate, DateTime endDate)
        {
            var tournament = new Tournament
            {
                Name = name,
                Location = location,
                StartDate = startDate,
                EndDate = endDate,
                Teams = null
            };
            await _repository.AddAsync(tournament);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateTournamentAsync(Tournament tournament)
        {
            await _repository.UpdateAsync(tournament);
        }

        public async Task DeleteTournamentAsync(Tournament tournament)
        {
            var teams = await _repository.GetAllAsync<Team>();
            var groups = await _repository.GetAllAsync<Group>();
            var matches = await _repository.GetAllAsync<Match>();

            var tournamentTeams = teams.Where(t => t.TournamentId == tournament.TournamentId).ToList();
            foreach (var team in tournamentTeams)
            {
                await _repository.DeleteAsync(team);
            }

            var tournamentGroups = groups.Where(g => g.TournamentId == tournament.TournamentId).ToList();
            foreach (var group in tournamentGroups)
            {
                var groupMatches = matches.Where(m => m.GroupId == group.GroupId).ToList();
                foreach (var match in groupMatches)
                {
                    await _repository.DeleteAsync(match);
                }
                await _repository.DeleteAsync(group);
            }
            await _repository.DeleteAsync(tournament);
        }

        public async Task RegisterTeamAsync(int tournamentId, string teamName)
        {
            var team = new Team
            {
                Name = teamName,
                TournamentId = tournamentId
            };
            await _repository.AddAsync(team);
            await _repository.SaveChangesAsync();
        }

        public async Task CreateTeamAsync(string teamName)
        {
            var team = new Team
            {
                Name = teamName,
            };
            await _repository.AddAsync(team);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateTeamAsync(Team team)
        {
            await _repository.UpdateAsync(team);
        }

        public async Task DeleteTeamAsync(Team team)
        {
            await _repository.DeleteAsync(team);
        }

        public async Task RegisterTeamAsync(int tournamentId, int teamId)
        {
            var team = await _repository.GetByIdAsync<Team>(teamId);
            if (team != null)
            {
                team.TournamentId = tournamentId;
                await _repository.UpdateAsync(team);
                await _repository.SaveChangesAsync();
            }
        }

        public async Task AddPlayerAsync(Player player)
        {
            await _repository.AddAsync(player);
            await _repository.SaveChangesAsync();
        }

        public async Task RecordMatchScoreAsync(int matchId, int scoreA, int scoreB)
        {
            var match = await _repository.GetByIdAsync<Match>(matchId);
            if (match != null)
            {
                match.Team1Score = scoreA;
                match.Team2Score = scoreB;

                match.IsTeam1Winner = scoreA > scoreB;

                if (match.NextMatchId.HasValue)
                {
                    var nextMatch = await _repository.GetByIdAsync<Match>(match.NextMatchId.Value);
                    if (nextMatch != null)
                    {
                        if (!nextMatch.Team1Id.HasValue)
                        {
                            nextMatch.Team1Id = (bool)match.IsTeam1Winner ? match.Team1Id : match.Team2Id;
                        }
                        else if (!nextMatch.Team2Id.HasValue)
                        {
                            nextMatch.Team2Id = (bool)match.IsTeam1Winner ? match.Team1Id : match.Team2Id;
                        }
                        await _repository.UpdateAsync(nextMatch);
                    }
                }
                await _repository.UpdateAsync(match);
                await _repository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Match>> GetMatchesByTournamentIdAsync(int tournamentId)
        {
            var matches = await _repository.GetAllAsync<Match>();
            return matches.Where(m => m.TournamentId == tournamentId).ToList();
        }

        public async Task<IEnumerable<Match>> GetAllMatchesAsync()
        {
            return await _repository.GetAllAsync<Match>();
        }

        public async Task<IEnumerable<Player>> GetPlayersByTeamIdAsync(int teamId)
        {
            var players = await _repository.GetAllAsync<Player>();
            return players.Where(p => p.TeamId == teamId);
        }

        public async Task UpdatePlayerAsync(Player player)
        {
            await _repository.UpdateAsync(player);
            await _repository.SaveChangesAsync();
        }

        public async Task DeletePlayerAsync(int playerId)
        {
            var player = await _repository.GetByIdAsync<Player>(playerId);
            if (player != null)
            {
                await _repository.DeleteAsync(player);
                await _repository.SaveChangesAsync();
            }
        }

        public async Task GenerateBracketAsync(int tournamentId, int selectedNumberOfTeams)
        {
            if (selectedNumberOfTeams != 4 && selectedNumberOfTeams != 8 && selectedNumberOfTeams != 16)
            {
                throw new ArgumentException("Number of teams must be 4, 8, or 16.");
            }

            var tournament = await _repository.GetByIdAsync<Tournament>(tournamentId);

            if (tournament == null)
            {
                throw new Exception("Tournament not found");
            }
            if (tournament.Teams != null && tournament.Teams.Count > 0)
            {
                throw new Exception($"Tournament has already generated Bracket {tournament.Teams.Count}");
            }

            var teams = await _repository.GetAllAsync<Team>();
            var teamsList = teams.ToList();

            if (teamsList.Count < selectedNumberOfTeams)
            {
                throw new Exception($"There must be at least {selectedNumberOfTeams} teams to generate the bracket");
            }

            var random = new Random();
            var selectedTeams = teamsList.OrderBy(t => random.Next()).Take(selectedNumberOfTeams).ToList();
            tournament.Teams = selectedTeams;

            await GenerateMatchesAsync(tournament, selectedTeams, selectedNumberOfTeams);
        }

        private async Task GenerateMatchesAsync(Tournament tournament, List<Team> selectedTeams, int teamCount)
        {
            var matches = new List<Match>();
            int matchCount = teamCount / 2;

            for (int i = 0; i < matchCount; i++)
            {
                var match = new Match
                {
                    TournamentId = tournament.TournamentId,
                    Tournament = tournament,
                    Team1 = selectedTeams[i * 2],
                    Team1Id = selectedTeams[i * 2].TeamId,
                    Team2 = selectedTeams[i * 2 + 1],
                    Team2Id = selectedTeams[i * 2 + 1].TeamId
                };
                matches.Add(match);
            }

            foreach (var match in matches)
            {
                await _repository.AddAsync(match);
            }

            await _repository.SaveChangesAsync();

            var nextRoundMatches = await CreateNextRoundMatchesAsync(tournament, matches, matchCount);

            while (nextRoundMatches.Count > 1)
            {
                nextRoundMatches = await CreateNextRoundMatchesAsync(tournament, nextRoundMatches, nextRoundMatches.Count);
            }
        }

        private async Task<List<Match>> CreateNextRoundMatchesAsync(Tournament tournament, List<Match> currentRoundMatches, int matchCount)
        {
            var nextRoundMatches = new List<Match>();

            for (int i = 0; i < matchCount / 2; i++)
            {
                var nextMatch = new Match
                {
                    TournamentId = tournament.TournamentId,
                    Tournament = tournament
                };

                nextRoundMatches.Add(nextMatch);
                await _repository.AddAsync(nextMatch);
                await _repository.SaveChangesAsync();

                currentRoundMatches[i * 2].NextMatchId = nextMatch.MatchId;
                currentRoundMatches[i * 2 + 1].NextMatchId = nextMatch.MatchId;

                await _repository.UpdateAsync(currentRoundMatches[i * 2]);
                await _repository.UpdateAsync(currentRoundMatches[i * 2 + 1]);
            }

            await _repository.SaveChangesAsync();

            return nextRoundMatches;
        }
    }
}
