﻿namespace EverythingNBA.Services.Implementations
{
    using System.Threading.Tasks;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper;

    using EverythingNBA.Models;
    using Data;
    using Services.Models.SeasonStatistic;

    public class SeasonStatisticService : ISeasonStatisticService
    {
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;

        public SeasonStatisticService(EverythingNBADbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<int> AddAsync(int seasonId, int teamId, int wins, int losses)
        {
            var seasonStatisticObj = new SeasonStatistic
            {
                SeasonId = seasonId,
                TeamId = teamId,
                Wins = wins,
                Losses = losses
            };

            this.db.SeasonStatistics.Add(seasonStatisticObj);
            await this.db.SaveChangesAsync();

            return seasonStatisticObj.Id;
        }

        public async Task<GetSeasonStatisticDetailsServiceModel> GetDetailsAsync(int id)
        {
            var statistic = await this.db.SeasonStatistics.FindAsync(id);

            var model = mapper.Map<GetSeasonStatisticDetailsServiceModel>(statistic);

            return model;
        }

        public async Task<GetSeasonStatisticDetailsServiceModel> GetDetailsAsync(int seasonId, int teamId)
        {
            var statisticId = await this.db.SeasonStatistics
                .Where(ss => ss.TeamId == teamId && ss.SeasonId == seasonId)
                .Select(ss => ss.Id)
                .FirstOrDefaultAsync();

            return await this.GetDetailsAsync(statisticId);
        }

        public async Task<string> GetWinPercentageAsync(int seasonStatisticId)
        {
            var statistic = await this.db.SeasonStatistics.Where(ss => ss.Id == seasonStatisticId).FirstOrDefaultAsync();
            var season = await this.db.Seasons.Where(ss => ss.Id == statistic.SeasonId).FirstOrDefaultAsync();

            if (season.GamesPlayed == 0)
            {
                return "0";
            }

            double result = ((double)statistic.Wins / (double)season.GamesPlayed) * 100;

            return (result / 100).ToString(".000");
        }
    }
}
