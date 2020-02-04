﻿namespace EverythingNBA.Services.Implementations
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using System.Linq;

    using EverythingNBA.Data;
    using EverythingNBA.Models;
    using EverythingNBA.Services.Models.Series;

    public class SeriesService : ISeriesService
    {
        private readonly EverythingNBADbContext db;

        public SeriesService(EverythingNBADbContext db)
        {
            this.db = db;
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
            } //sets game to appropriate game number

            await this.db.SaveChangesAsync();
        }

        public async Task<int> AddSeriesAsync(int team1Id, int team2Id, int winnerGamesWon, int loserGamesWon, int game1Id, int game2Id, int game3Id, int game4Id, int? game5Id, int? game6Id, int? game7Id)
        {
            var seriesObj = new Series
            {
                Team1Id = team1Id,
                Team2Id =team2Id,
                WinnerGamesWon = winnerGamesWon,
                LoserGamesWon = loserGamesWon,
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

            var model = Mapper.Map<GetSeriesDetailsServiceModel>(series);

            return model;
        }

        public Task<string> GetWinnerAsync(int seriesId)
        {
            throw new NotImplementedException();

            
        }
    }
}
