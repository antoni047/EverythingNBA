namespace EverythingNBA.Services.Implementations
{
    using System.Threading.Tasks;
    using System;

    using EverythingNBA.Data;
    using EverythingNBA.Models;

    public class SeasonStatisticService : ISeasonStatisticService;
    {
        private readonly EverythingNBADbContext db;

        public SeasonStatisticService(EverythingNBADbContext db)
        {
            this.db = db;
        }

        public async Task<int> AddAsync(int seasonId, int wins, int losses)
        {
            var seasonStatisticObj = new SeasonStatistic
            {
                SeasonId = seasonId,
                Wins = wins,
                Losses = losses
            };

            this.db.SingleSeasonStatistics.Add(seasonStatisticObj);
            await this.db.SaveChangesAsync();

            return seasonStatisticObj.Id;
        }

        public async Task<bool> DeleteAsync(int seasonStatisticId)
        {
            var statisticToDelete = await this.db.SingleSeasonStatistics.FindAsync(seasonStatisticId);

            if (statisticToDelete == null)
            {
                return false;
            }

            this.db.SingleSeasonStatistics.Remove(statisticToDelete);
            await this.db.SaveChangesAsync();

            return true;
        }
    }
}
