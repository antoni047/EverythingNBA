namespace EverythingNBA.Services.Implementations
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using System.Linq;

    using EverythingNBA.Data;
    using EverythingNBA.Models;
    using EverythingNBA.Services.Models.Series;
    using Microsoft.EntityFrameworkCore;

    public class SeriesService : ISeriesService
    {
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;

        public SeriesService(EverythingNBADbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task AddGameAsync(int seriesId, int gameId, int gameNumber)
        {
            var series = await this.db.Series.FindAsync(seriesId);

            switch (gameNumber)
            {
                case 1:
                    series.Game1Id = gameId;
                    break;
                case 2:
                    series.Game2Id = gameId;
                    break;
                case 3:
                    series.Game3Id = gameId;
                    break;
                case 4:
                    series.Game4Id = gameId;
                    break;
                case 5:
                    series.Game5Id = gameId;
                    break;
                case 6:
                    series.Game6Id = gameId;
                    break;
                case 7:
                    series.Game7Id = gameId;
                    break;
            } //sets gameId by the given game number

            await this.db.SaveChangesAsync();
        }

        public async Task<int> AddSeriesAsync(int team1Id, int team2Id, int team1GameWon, int team2GamesWon, int game1Id, int game2Id, 
            int game3Id, int game4Id, int? game5Id, int? game6Id, int? game7Id)
        {
            var seriesObj = new Series
            {
                Team1Id = team1Id,
                Team2Id =team2Id,
                Team1GamesWon = team1GameWon,
                Team2GamesWon = team2GamesWon,
                Game1Id = game1Id,
                Game2Id = game2Id,
                Game3Id = game3Id,
                Game4Id = game4Id,
                Game5Id = game5Id,
                Game6Id = game6Id,
                Game7Id = game7Id
            };

            this.db.Series.Add(seriesObj);
            await this.db.SaveChangesAsync();

            return seriesObj.Id;
        }

        public async Task<bool> DeleteSeriesAsync(int seriesId)
        {
            var seriesToDelete = await this.db.Series.FindAsync(seriesId);

            if (seriesToDelete == null)
            {
                return false;
            }

            this.db.Series.Remove(seriesToDelete);
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<GetSeriesDetailsServiceModel> GetSeriesAsync(int id)
        {
            var series = await this.db.Series.FindAsync(id);

            var model = mapper.Map<GetSeriesDetailsServiceModel>(series);

            return model;
        }

        public async Task<string> GetWinnerAsync(int seriesId)
        {
            var series = await this.db.Series
                .Include(s => s.Team1)
                .Include(s => s.Team2)
                .Where(s => s.Id == seriesId)
                .FirstOrDefaultAsync();

            var winner = string.Empty; 

            if (series.Team1GamesWon > series.Team2GamesWon)
            {
                winner = series.Team1.Name;
            }
            else
            {
                winner = series.Team2.Name;
            }

            return winner;
        }
    }
}
