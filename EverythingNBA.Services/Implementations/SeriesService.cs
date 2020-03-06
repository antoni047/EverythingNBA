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
    using System.Collections.Generic;

    public class SeriesService : ISeriesService
    {
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;
        private readonly IGameService gameService;
        private readonly ITeamService teamService;

        public SeriesService(EverythingNBADbContext db, IMapper mapper, IGameService gameService, ITeamService teamService)
        {
            this.db = db;
            this.mapper = mapper;
            this.gameService = gameService;
            this.teamService = teamService;
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

        public async Task<int> AddSeriesAsync(int playoffId, int team1Id, int team2Id, int team1GameWon, int team2GamesWon, int? game1Id, int? game2Id,
            int? game3Id, int? game4Id, int? game5Id, int? game6Id, int? game7Id, string conference, string stage, int stageNumber)
        {
            var seriesObj = new Series
            {
                PlayoffId = playoffId,
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
                Game7Id = game7Id,
                Conference = conference,
                Stage = stage,
                StageNumber = stageNumber
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

            model.Team1Name = series.Team1.Name;
            model.Team2Name = series.Team2.Name;

            var topStatsModel = await this.GetTopStats(model);

            model = mapper.Map<GetSeriesDetailsServiceModel>(topStatsModel);
          
            return model;
        }

        public async Task<SeriesOverviewServiceModel> GetSeriesOverview(int seriesId)
        {
            var series = await this.GetSeriesAsync(seriesId);

            var model = mapper.Map<SeriesOverviewServiceModel>(series);

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

        private async Task<TopStatsServiceModel> GetTopStats(GetSeriesDetailsServiceModel seriesModel)
        {
            var mostPoints = int.MinValue;
            var mostPointsPlayerName = string.Empty;

            var mostAssists = int.MinValue;
            var mostAssistsPlayerName = string.Empty;

            var mostRebounds = int.MinValue;
            var mostReboundsPlayerName = string.Empty;

            var stats = new List<Dictionary<string, int>>();

            foreach (var game in seriesModel.Games)
            {
                var playerPointsDict = new Dictionary<string, int>();
                var playerAssistsDict = new Dictionary<string, int>();
                var playerReboundsDict = new Dictionary<string, int>();

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
                        playerPointsDict[playerFullName] += (int)stat.Points;
                        playerAssistsDict[playerFullName] += (int)stat.Assists;
                        playerReboundsDict[playerFullName] += (int)stat.Rebounds;
                    }
                    else
                    {
                        playerPointsDict.Add(playerFullName, (int)stat.Points);
                        playerAssistsDict.Add(playerFullName, (int)stat.Assists);
                        playerReboundsDict.Add(playerFullName, (int)stat.Rebounds);
                    }
                }

                var pointsList = new List<int>();
                var assitsList = new List<int>();
                var reboundsList = new List<int>();

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

            var topStats = new TopStatsServiceModel
            {
                MostPoints = mostPoints,
                MostPointsPlayerName = mostPointsPlayerName,
                MostAssists = mostAssists,
                MostAssistsPlayerName = mostAssistsPlayerName,
                MostRebounds = mostRebounds,
                MostReboundsPlayerName = mostReboundsPlayerName
            };
            
            return topStats;
        }
    }
}
