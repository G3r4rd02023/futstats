using soccer.Data;
using soccer.Data.Entities;
using soccer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace soccer.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(DataContext context,ICombosHelper combosHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
        }

        public async Task<Group> ToGroupAsync(GroupViewModel model, bool isNew)
        {
            return new Group
            {
                GroupDetails = model.GroupDetails,
                Id = isNew ? 0 : model.Id,
                Matches = model.Matches,
                Name = model.Name,
                Tournament = await _context.Tournaments.FindAsync(model.TournamentId)
            };

        }

        public async Task<GroupDetail> ToGroupDetailAsync(GroupDetailViewModel model, bool isNew)
        {
            return new GroupDetail
            {
                GoalsAgainst = model.GoalsAgainst,
                GoalsFor = model.GoalsFor,
                Group = await _context.Groups.FindAsync(model.GroupId),
                Id = isNew ? 0 : model.Id,
                MatchesLost = model.MatchesLost,
                MatchesPlayed = model.MatchesPlayed,
                MatchesTied = model.MatchesTied,
                MatchesWon = model.MatchesWon,
                Team = await _context.Teams.FindAsync(model.TeamId)
            };
        }

        public GroupDetailViewModel ToGroupDetailViewModel(GroupDetail groupDetail)
        {
            return new GroupDetailViewModel
            {
                GoalsAgainst = groupDetail.GoalsAgainst,
                GoalsFor = groupDetail.GoalsFor,
                Group = groupDetail.Group,
                GroupId = groupDetail.Group.Id,
                Id = groupDetail.Id,
                MatchesLost = groupDetail.MatchesLost,
                MatchesPlayed = groupDetail.MatchesPlayed,
                MatchesTied = groupDetail.MatchesTied,
                MatchesWon = groupDetail.MatchesWon,
                Team = groupDetail.Team,
                TeamId = groupDetail.Team.Id,
                Teams = _combosHelper.GetComboTeams()
            };
        }

        public GroupViewModel ToGroupViewModel(Group group)
        {
            return new GroupViewModel
            {
                GroupDetails = group.GroupDetails,
                Id = group.Id,
                Matches = group.Matches,
                Name = group.Name,
                Tournament = group.Tournament,
                TournamentId = group.Tournament.Id
            };

        }

        public async Task<Match> ToMatchAsync(MatchViewModel model, bool isNew)
        {
            return new Match
            {
                Date = model.Date.ToUniversalTime(),
                GoalsLocal = model.GoalsLocal,
                GoalsVisitor = model.GoalsVisitor,
                Group = await _context.Groups.FindAsync(model.GroupId),
                Id = isNew ? 0 : model.Id,
                IsClosed = model.IsClosed,
                Local = await _context.Teams.FindAsync(model.LocalId),
                Visitor = await _context.Teams.FindAsync(model.VisitorId)
            };
        }

        public MatchViewModel ToMatchViewModel(Match match)
        {
            return new MatchViewModel
            {
                Date = match.Date.ToLocalTime(),
                GoalsLocal = match.GoalsLocal,
                GoalsVisitor = match.GoalsVisitor,
                Group = match.Group,
                GroupId = match.Group.Id,
                Id = match.Id,
                IsClosed = match.IsClosed,
                Local = match.Local,
                LocalId = match.Local.Id,
                Teams = _combosHelper.GetComboTeams(match.Group.Id),
                Visitor = match.Visitor,
                VisitorId = match.Visitor.Id
            };
        }

        public Team ToTeam(TeamViewModel model, string path, bool isNew)
        {
            return new Team
            {
                Id = isNew ? 0 : model.Id,
                LogoPath = path,
                Name = model.Name
            };

        }

        public TeamViewModel ToTeamViewModel(Team team)
        {
            return new TeamViewModel
            {
                Id = team.Id,
                LogoPath = team.LogoPath,
                Name = team.Name
            };

        }

        public Tournament ToTournament(TournamentViewModel model, string path, bool isNew)
        {
            return new Tournament
            {
                EndDate = model.EndDate.ToUniversalTime(),
                Groups = model.Groups,
                Id = isNew ? 0 : model.Id,
                IsActive = model.IsActive,
                LogoPath = path,
                Name = model.Name,
                StartDate = model.StartDate.ToUniversalTime()
            };

        }

        public TournamentViewModel ToTournamentViewModel(Tournament tournament)
        {
            return new TournamentViewModel
            {
                EndDate = tournament.EndDate,
                Groups = tournament.Groups,
                Id = tournament.Id,
                IsActive = tournament.IsActive,
                LogoPath = tournament.LogoPath,
                Name = tournament.Name,
                StartDate = tournament.StartDate
            };

        }
    }
}
