using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soccer.Data.Entities
{
    public class Team
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = "El {0} campo no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Equipo")]
        public string Name { get; set; }

        [Display(Name = "Logo")]
        public string LogoPath { get; set; }

        public ICollection<GroupDetail> GroupDetails { get; set; }
    }
}
