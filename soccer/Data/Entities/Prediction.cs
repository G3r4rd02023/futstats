using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soccer.Data.Entities
{
    public class Prediction
    {
        public int Id { get; set; }

        public Match Match { get; set; }

        public User User { get; set; }

        [Display(Name = "Goles Local")]
        public int? GoalsLocal { get; set; }

        [Display(Name = "Goles Visita")]
        public int? GoalsVisitor { get; set; }

        public int Points { get; set; }
    }
}
