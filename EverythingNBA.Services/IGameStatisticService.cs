namespace EverythingNBA.Services
{
    using System;
    using System.Threading.Tasks;

    public interface IGameStatisticService
    {
        Task<int> AddAsync(int gameId, int playerId, int minutesPlayed, int points, int assists, int rebounds, int blocks, int steals,
            int freeThrowAttemps, int freeThrowsMade, int threeAttemps, int threeMade, int fieldGoalAttempts, int fieldGoalsMade);

        Task<bool> DeleteAsync(int gameStatisticId);

        Task<double> GetThreePointsPercentage(int gameStatisticId);

        Task<double> GetFieldGoalPercentage(int gameStatisticId);

        Task<double> GetFreeThrowPercentage(int gameStatisticId);
    }
}
