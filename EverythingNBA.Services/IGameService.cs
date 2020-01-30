

namespace EverythingNBA.Services
{
    using System;
    using System.Threading.Tasks;

    public interface IGameService
    {
        Task<int> AddGamesAsync(int seasonId, int teamHostId, int team2Id, int winnerPoints, int loserPoints, DateTime date, bool isFinished);

        Task<bool> DeletePlayerAsync(int gameId);
    }
}
