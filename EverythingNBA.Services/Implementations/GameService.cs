namespace EverythingNBA.Services.Implementations
{
    using System;
    using System.Threading.Tasks;

    using EverythingNBA.Data;
    using EverythingNBA.Models;

    public class GameService : IGameService
    {
        private readonly EverythingNBADbContext db;

        public GameService(EverythingNBADbContext db)
        {
            this.db = db;
        }

        public async Task<int> AddGamesAsync(int seasonId, int teamHostId, int team2Id, int winnerPoints, int loserPoints, DateTime date, bool isFinished)
        {
            var gameObj = new Game
            {
                SeasonId = seasonId,
                TeamHostId = teamHostId,
                Team2Id = team2Id,
                WinnerPoints = winnerPoints,
                LoserPoints = loserPoints,
                IsFinished = isFinished
            };

            this.db.Games.Add(gameObj);
            await this.db.SaveChangesAsync();

            return gameObj.Id;
        }

        public async Task<bool> DeletePlayerAsync(int gameId)
        {
            var gameToDelete = await this.db.Games.FindAsync(gameId);

            if (gameToDelete == null)
            {
                return false;
            }

            this.db.Games.Remove(gameToDelete);
            await this.db.SaveChangesAsync();

            return true;
        }
    }
}
