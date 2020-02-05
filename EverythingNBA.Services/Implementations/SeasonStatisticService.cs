namespace EverythingNBA.Services.Implementations
{
    using System.Threading.Tasks;
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using EverythingNBA.Data;
    using EverythingNBA.Models;
    using EverythingNBA.Services.Models;
    using EverythingNBA.Services.Mapping;

    public class SeasonStatisticService : ISeasonStatisticService
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

        public async Task AddGameAsync(int id, bool isWon)
        {
            var statistic = await this.db.SingleSeasonStatistics.FindAsync(id);

            if (isWon)
            {
                statistic.Wins++;
            }
            else
            {
                statistic.Losses++;
            }

            await this.db.SaveChangesAsync();
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

            var model = Mapping.Mapper.Map<GetSeasonStatisticDetailsServiceModel>(statistic);

            return model;
        }

        public async Task<GetSeasonStatisticDetailsServiceModel> GetDetailsAsync(int seasonId, int teamId)
        {
            var statistic = await this.db.SingleSeasonStatistics.Where(ss => ss.TeamId == teamId && ss.SeasonId == seasonId).FirstOrDefaultAsync();

            var model = Mapping.Mapper.Map<GetSeasonStatisticDetailsServiceModel>(statistic);

            return model;
        }

        public async Task<string> GetWinPercentageAsync(int seasonStatisticId)
        {
            var statistic = await this.db.SingleSeasonStatistics.FindAsync(seasonStatisticId);

            var result = statistic.Wins / statistic.Season.GamesPlayed;

            return result.ToString("0.000");

        }
    }
}
