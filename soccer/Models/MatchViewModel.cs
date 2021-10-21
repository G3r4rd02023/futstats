using Microsoft.AspNetCore.Mvc.Rendering;
using soccer.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soccer.Models
{
    public class MatchViewModel : Match
    {
        public int GroupId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Local")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un equipo.")]
        public int LocalId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Visita")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un equipo.")]
        public int VisitorId { get; set; }

        public IEnumerable<SelectListItem> Teams { get; set; }
    }
}
