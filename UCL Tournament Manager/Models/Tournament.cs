namespace UCL_Tournament_Manager.Models
{
    public class Tournament
    {
        public int TournamentId { get; set; }
        public required string Name { get; set; }
        public required string Location { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public ICollection<Group>? Groups { get; set; } = new List<Group>();
        public ICollection<Team>? Teams { get; set; } = new List<Team>();
    }
}
