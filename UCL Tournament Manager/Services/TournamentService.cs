using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task CreateTournamentAsync(string name)
        {
            var tournament = new Tournament { Name = name };
            await _repository.AddAsync(tournament);
            await _repository.SaveChangesAsync();
        }

        public async Task RegisterTeamAsync(int tournamentId, string teamName)
        {
            var team = new Team { Name = teamName };
            var tournament = await _repository.GetByIdAsync<Tournament>(tournamentId);
            tournament.Teams.Add(team);
            await _repository.SaveChangesAsync();
        }

        public async Task GenerateGroupsAsync(int tournamentId, int groupCount)
        {
            var tournament = await _repository.GetByIdAsync<Tournament>(tournamentId);
            var teams = tournament.Teams.ToList();
            var groups = new List<Group>();

            for (int i = 0; i < groupCount; i++)
            {
                groups.Add(new Group { Name = $"Group {i + 1}" });
            }

            for (int i = 0; i < teams.Count; i++)
            {
                groups[i % groupCount].Teams.Add(teams[i]);
            }

            tournament.Groups = groups;
            await _repository.SaveChangesAsync();
        }

        public async Task GenerateSpiderAsync(int tournamentId)
        {
            // Implementation for generating spider
        }

        public async Task RecordMatchScoreAsync(int matchId, int scoreA, int scoreB)
        {
            var match = await _repository.GetByIdAsync<Match>(matchId);
            match.ScoreA = scoreA;
            match.ScoreB = scoreB;
            await _repository.SaveChangesAsync();
        }
    }
}
