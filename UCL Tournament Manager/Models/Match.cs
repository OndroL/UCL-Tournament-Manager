using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UCL_Tournament_Manager.Models
{
    public class Match
    {
        public int MatchId { get; set; }
        public int Team1Id { get; set; }
        public required Team Team1 { get; set; }   // Navigation property
        public int Team2Id { get; set; }
        public required Team Team2 { get; set; }   // Navigation property
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public int? GroupId { get; set; }  // Foreign key to Group (nullable if knockout)
        public Group? Group { get; set; }   // Navigation property (nullable)
        public int? TournamentId { get; set; }  // Foreign key to Tournament
        public required Tournament Tournament { get; set; }   // Navigation property
    }
}
