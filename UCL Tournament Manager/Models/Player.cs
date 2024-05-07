using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCL_Tournament_Manager.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public int TeamId { get; set; }  // Foreign key to the Team
        public Team Team { get; set; }   // Navigation property
    }
}
