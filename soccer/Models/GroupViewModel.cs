using soccer.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace soccer.Models
{
    public class GroupViewModel: Group
    {
        public int TournamentId { get; set; }
    }
}
