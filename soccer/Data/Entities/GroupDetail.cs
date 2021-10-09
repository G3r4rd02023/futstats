using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soccer.Data.Entities
{
    public class GroupDetail
    {
        public int Id { get; set; }

        public Team Team { get; set; }

        [Display(Name = "PJ")]
        public int MatchesPlayed { get; set; }

        [Display(Name = "PG")]
        public int MatchesWon { get; set; }

        [Display(Name = "PE")]
        public int MatchesTied { get; set; }

        [Display(Name = "PP")]
        public int MatchesLost { get; set; }

        [Display(Name = "Pts")]
        public int Points => MatchesWon * 3 + MatchesTied;

        [Display(Name = "GF")]
        public int GoalsFor { get; set; }

        [Display(Name = "GC")]
        public int GoalsAgainst { get; set; }

        [Display(Name = "DIF")]
        public int GoalDifference => GoalsFor - GoalsAgainst;

        public Group Group { get; set; }

    }
}
