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

        Tournament ToTournament(TournamentViewModel model, string path, bool isNew);

        TournamentViewModel ToTournamentViewModel(Tournament tournament);

        Task<Group> ToGroupAsync(GroupViewModel model, bool isNew);

        GroupViewModel ToGroupViewModel(Group group);

        Task<GroupDetail> ToGroupDetailAsync(GroupDetailViewModel model, bool isNew);

        GroupDetailViewModel ToGroupDetailViewModel(GroupDetail groupDetail);

        Task<Match> ToMatchAsync(MatchViewModel model, bool isNew);

        MatchViewModel ToMatchViewModel(Match match);

    }
}
