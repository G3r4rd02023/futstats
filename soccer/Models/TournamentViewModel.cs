using Microsoft.AspNetCore.Http;
using soccer.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soccer.Models
{
    public class TournamentViewModel :Tournament
    {
        [Display(Name = "Logo")]
        public IFormFile LogoFile { get; set; }

    }
}
