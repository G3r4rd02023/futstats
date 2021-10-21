using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace soccer.Helpers
{
    public interface IMatchHelper
    {
        Task CloseMatchAsync(int matchId, int goalsLocal, int goalsVisitor);
    }
}
