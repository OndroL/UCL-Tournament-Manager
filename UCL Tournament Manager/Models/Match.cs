namespace UCL_Tournament_Manager.Models
{
    public class Match
    {
        public int MatchId { get; set; }
        public int? Team1Id { get; set; }
        public Team? Team1 { get; set; }
        public int? Team2Id { get; set; }
        public Team? Team2 { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public int? GroupId { get; set; }
        public Group? Group { get; set; }
        public int TournamentId { get; set; }
        public required Tournament Tournament { get; set; }

        public int? NextMatchId { get; set; }
        public Match? NextMatch { get; set; }
        public bool? IsTeam1Winner { get; set; }
    }
}