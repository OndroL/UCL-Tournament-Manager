using System.Collections.Generic;
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

        public async Task<IEnumerable<Tournament>> GetTournamentsAsync()
        {
            return await _repository.GetAllAsync<Tournament>();
        }

        public async Task CreateTournamentAsync(string name, string location, DateTime startDate, DateTime endDate)
        {
            var tournament = new Tournament
            {
                Name = name,
                Location = location,
                StartDate = startDate,
                EndDate = endDate
            };
            await _repository.AddAsync(tournament);
            await _repository.SaveChangesAsync();
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

        public async Task GenerateGroupsAsync(int tournamentId, int groupCount)
        {
            // Implementation of group generation, no idea how this will work yet
        }

        public async Task GenerateSpiderAsync(int tournamentId)
        {
            // Implement of knockout spider generation, no idea how this will work yet
        }

        public async Task RecordMatchScoreAsync(int matchId, int scoreA, int scoreB)
        {
            var match = await _repository.GetByIdAsync<Match>(matchId);
            if (match != null)
            {
                match.Team1Score = scoreA;
                match.Team2Score = scoreB;
                await _repository.UpdateAsync(match);
                await _repository.SaveChangesAsync();
            }
        }
    }
}
