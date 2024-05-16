using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UCL_Tournament_Manager.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public required string Name { get; set; }
        public ICollection<Player> Players { get; set; } = new List<Player>();
        public int? GroupId { get; set; }   // Foreign key to the Group
        public Group? Group { get; set; }   // Navigation property
        public int? TournamentId { get; set; }  // Foreign key to Tournament
        public Tournament? Tournament { get; set; }   // Navigation property
    }
}
