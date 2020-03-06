namespace EverythingNBA.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using EverythingNBA.Data;
    using EverythingNBA.Models;
    using EverythingNBA.Services.Models.Playoff;
    using EverythingNBA.Services.Models.Series;

    public class PlayoffService : IPlayoffService
    {
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;
        private readonly ISeriesService seriesService;

        public PlayoffService(EverythingNBADbContext db, IMapper mapper, ISeriesService seriesService)
        {
            this.db = db;
            this.mapper = mapper;
            this.seriesService = seriesService;
        }

        public async Task<int> AddPlayoffAsync(int? seasonId)
        {
            var playoffObj = new Playoff()
            {
                SeasonId = seasonId
            };

            this.db.Playoffs.Add(playoffObj);

            await this.db.SaveChangesAsync();

            return playoffObj.Id;
        }

        public async Task AddSeriesAsync(int playoffId, int seriesId)
        {
            var playoff = await this.db.Playoffs.FindAsync(playoffId);
            var series = await this.db.Series.FindAsync(seriesId);

            if (series != null)
            {
                playoff.Series.Add(series);
                await this.db.SaveChangesAsync();
            }
        }

        public async Task<bool> DeletePlayoffAsync(int playoffId)
        {
            var playoffToDelete = await this.db.Playoffs.FindAsync(playoffId);

            if (playoffToDelete == null)
            {
                return false;
            }

            this.db.Playoffs.Remove(playoffToDelete);
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<GetPlayoffServiceModel> GetDetailsBySeasonAsync(int seasonId)
        {
            var playoffId = await this.db.Seasons.Where(s => s.Id == seasonId).Select(s => s.PlayoffId).FirstOrDefaultAsync();

            if (playoffId == null)
            {
                return null;
            }

            return await this.GetDetailsAsync((int)playoffId);
        }

        public async Task<GetPlayoffServiceModel> GetDetailsAsync(int playoffId)
        {
            var playoff = await this.db.Playoffs
                .Where(p => p.Id == playoffId)
                .Include(p => p.Series)
                .FirstOrDefaultAsync();

            if (playoff == null)
            {
                return null;
            }

            var finalSeries = playoff.Series.Where(s => s.Stage == "Final").FirstOrDefault();
            var winnerName = await this.seriesService.GetWinnerAsync(finalSeries.Id);

            var seriesModels = new List<SeriesOverviewServiceModel>();

            foreach (var serie in playoff.Series)
            {
                var serieModel = mapper.Map<SeriesOverviewServiceModel>(serie);

                seriesModels.Add(serieModel);
            }

            var model = new GetPlayoffServiceModel
            {
                SeasonId = playoff.SeasonId,
                Series = seriesModels,
                WinnerName = winnerName
            };

            return model;
        }
    }
}
