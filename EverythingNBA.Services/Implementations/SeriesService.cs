﻿namespace EverythingNBA.Services.Implementations
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using System.Linq;

    using EverythingNBA.Data;
    using EverythingNBA.Models;
    using EverythingNBA.Services.Models.Series;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    public class SeriesService : ISeriesService
    {
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;
        private readonly IGameService gameService;

        public SeriesService(EverythingNBADbContext db, IMapper mapper, IGameService gameService)
        {
            this.db = db;
            this.mapper = mapper;
            this.gameService = gameService;
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
                Team2Id = team2Id,
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
            var series = await this.db.Series
                .Include(s => s.Team1)
                .Include(s => s.Team2)
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            var model = mapper.Map<GetSeriesDetailsServiceModel>(series);

            var mostPoints = string.Empty;
            var mostAssists = string.Empty;
            var mostRebounds = string.Empty;

            var stats = await this.GetTopStats(model);

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

        private async Task<IDictionary<string, int>> GetTopStats(GetSeriesDetailsServiceModel seriesModel)
        {
            var mostPoints = double.MinValue;
            var mostPointsPlayerName = string.Empty;

            var mostAssists = double.MinValue;
            var mostAssistsPlayerName = string.Empty;

            var mostRebounds = double.MinValue;
            var mostReboundsPlayerName = string.Empty;

            foreach (var game in seriesModel.Games)
            {
                var playerPointsDict = new Dictionary<string, double>();
                var playerAssistsDict = new Dictionary<string, double>();
                var playerReboundsDict = new Dictionary<string, double>();

                var playerStats = await this.db.Games
                    .Include(g => g.PlayerStats).ThenInclude(ps => ps.Player)
                    .Where(g => g.Id == game.Id)
                    .Select(g => g.PlayerStats)
                    .FirstOrDefaultAsync();

                foreach (var stat in playerStats)
                {
                    var playerFullName = stat.Player.FirstName + " " + stat.Player.LastName;

                    if (playerPointsDict.ContainsKey(playerFullName) &&
                        playerAssistsDict.ContainsKey(playerFullName) &&
                        playerReboundsDict.ContainsKey(playerFullName))
                    {
                        playerPointsDict[playerFullName] += stat.Points;
                        playerAssistsDict[playerFullName] += stat.Assists;
                        playerReboundsDict[playerFullName] += stat.Rebounds;
                    }
                    else
                    {
                        playerPointsDict.Add(playerFullName, stat.Points);
                        playerAssistsDict.Add(playerFullName, stat.Assists);
                        playerReboundsDict.Add(playerFullName, stat.Rebounds);
                    }
                }

                var pointsList = new List<double>();
                var assitsList = new List<double>();
                var reboundsList = new List<double>();

                foreach (var playerStat in playerPointsDict)
                {
                    for (int i = 0; i < playerPointsDict.Count(); i++)
                    {
                        pointsList[i] = playerStat.Value;
                    }

                    if (pointsList.Sum() > mostPoints)
                    {
                        mostPoints = pointsList.Sum();
                        mostPointsPlayerName = playerStat.Key;
                    }
                }

                foreach (var playerStat in playerAssistsDict)
                {
                    for (int i = 0; i < playerAssistsDict.Count(); i++)
                    {
                        assitsList[i] = playerStat.Value;
                    }

                    if (pointsList.Sum() > mostPoints)
                    {
                        mostAssists = pointsList.Sum();
                        mostAssistsPlayerName = playerStat.Key;
                    }
                }

                foreach (var playerStat in playerReboundsDict)
                {
                    for (int i = 0; i < playerReboundsDict.Count(); i++)
                    {
                        reboundsList[i] = playerStat.Value;
                    }

                    if (pointsList.Sum() > mostPoints)
                    {
                        mostRebounds = pointsList.Sum();
                        mostReboundsPlayerName = playerStat.Key;
                    }
                }
            }

            var pointsDict = new Dictionary<string, double>();
            pointsDict.Add(mostPointsPlayerName, mostPoints);

            var assistsDict = new Dictionary<string, double>();
            assistsDict.Add(mostAssistsPlayerName, mostAssists);

            var reboundsDict = new Dictionary<string, double>();
            reboundsDict.Add(mostReboundsPlayerName, mostRebounds);
        }
    }
}
