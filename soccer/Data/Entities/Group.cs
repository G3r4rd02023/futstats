using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soccer.Data.Entities
{
    public class Group
    {
        public int Id { get; set; }

        [MaxLength(30, ErrorMessage = "El {0} campo no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Grupo")]
        public string Name { get; set; }

        public Tournament Tournament { get; set; }

        public ICollection<GroupDetail> GroupDetails { get; set; }

        public ICollection<Match> Matches { get; set; }

    }
}
