
namespace EverythingNBA.Services.Implementations
{
    using EverythingNBA.Data;
    using EverythingNBA.Models;

    using System;
    using System.Threading.Tasks;
    using AutoMapper;

    public class GameStatisticService : IGameStatisticService
    {
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;

        public GameStatisticService(EverythingNBADbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<int> AddAsync(int gameId, int playerId, int minutesPlayed, int points, int assists, int rebounds, int blocks, int steals, 
            int freeThrowAttemps, int freeThrowsMade, int threeAttemps, int threeMade, int fieldGoalAttempts, int fieldGoalsMade)
        {
            var statObj = new GameStatistic
            {
                GameId = gameId,
                PlayerId = playerId,
                MinutesPlayed = minutesPlayed,
                Points = points,
                Rebounds = rebounds,
                Assists = assists,
                Blocks = blocks,
                Steals = steals,
                FreeThrowAttempts = freeThrowAttemps,
                FreeThrowsMade = freeThrowsMade,
                ThreeAttempts = threeAttemps,
                ThreeMade = threeMade,
                FieldGoalAttempts = fieldGoalAttempts,
                FieldGoalsMade = fieldGoalsMade
            };

            this.db.GameStatistics.Add(statObj);
            await this.db.SaveChangesAsync();

            return statObj.Id;
        }

        public async Task<bool> DeleteAsync(int gameStatisticId)
        {
            var statToDelete = await this.db.GameStatistics.FindAsync(gameStatisticId);

            if (statToDelete == null)
            {
                return false;
            }

            this.db.GameStatistics.Remove(statToDelete);
            await this.db.SaveChangesAsync();

            return true;
        }
    }
}
