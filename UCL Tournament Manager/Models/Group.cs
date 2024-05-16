namespace UCL_Tournament_Manager.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string? GroupName { get; set; }
        public ICollection<Team> Teams { get; set; } = new List<Team>();
        public ICollection<Match> Matches { get; set; } = new List<Match>();
        public int TournamentId { get; set; }
        public Tournament? Tournament { get; set; }
    }
}
