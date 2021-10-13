using soccer.Data.Entities;
using soccer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace soccer.Helpers
{
    public interface IConverterHelper
    {
        Team ToTeam(TeamViewModel model, string path, bool isNew);

        TeamViewModel ToTeamViewModel(Team team);

    }
}
