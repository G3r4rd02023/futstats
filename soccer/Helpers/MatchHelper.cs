using Microsoft.EntityFrameworkCore;
using soccer.Data;
using soccer.Data.Entities;
using soccer.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace soccer.Helpers
{
    public class MatchHelper : IMatchHelper
    {
        private readonly DataContext _context;
        private Match _match;
        private MatchStatus _matchStatus;

        public MatchHelper(DataContext context)
        {
            _context = context;
        }

        public async Task CloseMatchAsync(int matchId, int goalsLocal, int goalsVisitor)
        {
                _match = await _context.Matches
                .Include(m => m.Local)
                .Include(m => m.Visitor)
                .Include(m => m.Predictions)
                .Include(m => m.Group)
                .ThenInclude(g => g.GroupDetails)
                .ThenInclude(gd => gd.Team)
                .FirstOrDefaultAsync(m => m.Id == matchId);

            _match.GoalsLocal = goalsLocal;
            _match.GoalsVisitor = goalsVisitor;
            _match.IsClosed = true;
            _matchStatus = GetMatchStatus(_match.GoalsLocal.Value, _match.GoalsVisitor.Value);

            UpdatePointsInpredictions();
            UpdatePositions();

            await _context.SaveChangesAsync();
        }
      

        private void UpdatePointsInpredictions()
        {
            foreach (Prediction prediction in _match.Predictions)
            {
                prediction.Points = GetPoints(prediction);
            }
        }

        private int GetPoints(Prediction prediction)
        {
            int points = 0;
            if (prediction.GoalsLocal == _match.GoalsLocal)
            {
                points += 2;
            }

            if (prediction.GoalsVisitor == _match.GoalsVisitor)
            {
                points += 2;
            }

            if (_matchStatus == GetMatchStatus(prediction.GoalsLocal.Value, prediction.GoalsVisitor.Value))
            {
                points++;
            }

            return points;
        }

        private MatchStatus GetMatchStatus(int goalsLocal, int goalsVisitor)
        {
            if (goalsLocal > goalsVisitor)
            {
                return MatchStatus.LocalWin;
            }

            if (goalsVisitor > goalsLocal)
            {
                return MatchStatus.VisitorWin;
            }

            return MatchStatus.Tie;
        }

        private void UpdatePositions()
        {
            GroupDetail local = _match.Group.GroupDetails.FirstOrDefault(gd => gd.Team == _match.Local);
            GroupDetail visitor = _match.Group.GroupDetails.FirstOrDefault(gd => gd.Team == _match.Visitor);

            local.MatchesPlayed++;
            visitor.MatchesPlayed++;

            local.GoalsFor += _match.GoalsLocal.Value;
            local.GoalsAgainst += _match.GoalsVisitor.Value;
            visitor.GoalsFor += _match.GoalsVisitor.Value;
            visitor.GoalsAgainst += _match.GoalsLocal.Value;

            if (_matchStatus == MatchStatus.LocalWin)
            {
                local.MatchesWon++;
                visitor.MatchesLost++;
            }
            else if (_matchStatus == MatchStatus.VisitorWin)
            {
                visitor.MatchesWon++;
                local.MatchesLost++;
            }
            else
            {
                local.MatchesTied++;
                visitor.MatchesTied++;
            }
        }
    }
}
