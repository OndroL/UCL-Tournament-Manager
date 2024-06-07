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


        public Task GenerateGroupsAsync(int tournamentId, int groupCount)
        {
            throw new NotImplementedException();
        }

        public async Task GenerateBracketAsync(int tournamentId)
        {
            var tournament = await _repository.GetByIdAsync<Tournament>(tournamentId);

            if (tournament == null)
            {
                throw new Exception("Tournament not found");
            }
            if (tournament.Teams != null && tournament.Teams.Count > 0)
            {
                throw new Exception($"Tournament has already generated Bracket ${tournament.Teams.Count}");
            }

            var teams = await _repository.GetAllAsync<Team>();
            var teamsList = teams.ToList();

            if (teamsList.Count < 4)
            {
                throw new Exception("There must be at least 4 teams to generate the bracket");
            }

            var random = new Random();
            var selectedTeams = teamsList.OrderBy(t => random.Next()).Take(4).ToList();
            tournament.Teams = selectedTeams;

            var match1 = new Match
            {
                TournamentId = tournamentId,
                Tournament = tournament,
                Team1 = selectedTeams[0],
                Team1Id = selectedTeams[0].TeamId,
                Team2 = selectedTeams[1],
                Team2Id = selectedTeams[1].TeamId
            };

            var match2 = new Match
            {
                TournamentId = tournamentId,
                Tournament = tournament,
                Team1 = selectedTeams[2],
                Team1Id = selectedTeams[2].TeamId,
                Team2 = selectedTeams[3],
                Team2Id = selectedTeams[3].TeamId
            };

            await _repository.AddAsync(match1);
            await _repository.AddAsync(match2);
            await _repository.SaveChangesAsync();

            var finalMatch = new Match
            {
                TournamentId = tournamentId,
                Tournament = tournament
            };

            await _repository.AddAsync(finalMatch);
            await _repository.SaveChangesAsync();

            match1.NextMatchId = finalMatch.MatchId;
            match2.NextMatchId = finalMatch.MatchId;

            await _repository.UpdateAsync(match1);
            await _repository.UpdateAsync(match2);
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
    }
}
