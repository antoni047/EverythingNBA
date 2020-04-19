namespace EverythingNBA.Services
{
    using System.Threading.Tasks;

    using Services.Models.GameStatistic;

    public interface IGameStatisticService
    {
        Task<int> AddAsync(int gameId, int playerId, int minutesPlayed, int points, int assists, int rebounds, int blocks, int steals,
            int freeThrowAttemps, int freeThrowsMade, int threeAttemps, int threeMade, int fieldGoalAttempts, int fieldGoalsMade);

        Task<bool> DeleteAsync(int gameStatisticId);

        Task<PlayerGameStatisticServiceModel> GetGameStatisticsAsync(int gameId, string playerName);

        Task<int> GetThreePointsPercentage(int gameStatisticId);

        Task<int> GetFieldGoalPercentage(int gameStatisticId);

        Task<int> GetFreeThrowPercentage(int gameStatisticId);

        Task EditGameStatisticAsync(PlayerGameStatisticServiceModel editedStats, int statId);
    }
}
