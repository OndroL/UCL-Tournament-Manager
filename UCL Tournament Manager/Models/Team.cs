namespace UCL_Tournament_Manager.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public required string Name { get; set; }
        public ICollection<Player> Players { get; set; } = new List<Player>();
        public int? GroupId { get; set; }
        public Group? Group { get; set; }
        public int? TournamentId { get; set; }
        public Tournament? Tournament { get; set; }
    }
}
