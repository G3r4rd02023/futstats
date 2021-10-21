using soccer.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soccer.Models
{
    public class CloseMatchViewModel
    {
        public int MatchId { get; set; }

        public int GroupId { get; set; }

        public int LocalId { get; set; }

        public int VisitorId { get; set; }

        [Display(Name = "Goles Local")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int? GoalsLocal { get; set; }

        [Display(Name = "Goals Visita")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int? GoalsVisitor { get; set; }

        public Group Group { get; set; }

        public Team Local { get; set; }

        public Team Visitor { get; set; }
    }  
}
