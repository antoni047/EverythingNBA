namespace EverythingNBA.Services.Implementations
{
    using System.Threading.Tasks;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper;

    using EverythingNBA.Data;
    using EverythingNBA.Models;
    using EverythingNBA.Services.Models;

    public class SeasonStatisticService : ISeasonStatisticService
    {
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;

        public SeasonStatisticService(EverythingNBADbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
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

        public async Task<bool> AddGameAsync(int id, bool isWon)
        {
            var statistic = await this.db.SingleSeasonStatistics.FindAsync(id);

            if (statistic == null)
            {
                return false;
            }

            if (isWon)
            {
                statistic.Wins++;
            }
            else
            {
                statistic.Losses++;
            }

            await this.db.SaveChangesAsync();
            return true;
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

        public async Task<GetSeasonStatisticDetailsServiceModel> GetDetailsAsync(int id)
        {
            var statistic = await this.db.SingleSeasonStatistics.FindAsync(id);

            var model = mapper.Map<GetSeasonStatisticDetailsServiceModel>(statistic);

            return model;
        }

        public async Task<GetSeasonStatisticDetailsServiceModel> GetDetailsAsync(int seasonId, int teamId)
        {
            var statistic = await this.db.SingleSeasonStatistics.Where(ss => ss.TeamId == teamId && ss.SeasonId == seasonId).FirstOrDefaultAsync();

            var model = mapper.Map<GetSeasonStatisticDetailsServiceModel>(statistic);

            return model;
        }

        public async Task<string> GetWinPercentageAsync(int seasonStatisticId)
        {
            var statistic = await this.db.SingleSeasonStatistics.FindAsync(seasonStatisticId);
            var season = await this.db.Seasons.FindAsync(statistic.SeasonId);

            double result = ((double)statistic.Wins / (double)season.GamesPlayed) * 100;

            return (result / 100).ToString("0.000");
        }//OK
    }
}
