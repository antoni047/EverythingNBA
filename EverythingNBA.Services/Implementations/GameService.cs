﻿namespace EverythingNBA.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    using EverythingNBA.Data;
    using EverythingNBA.Models;
    using EverythingNBA.Services.Models.Game;
    using EverythingNBA.Services.Models.GameStatistic;

    public class GameService : IGameService
    {
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;

        public GameService(EverythingNBADbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<int> AddGameAsync(int seasonId, int teamHostId, int team2Id, int teamHostPoints, int team2Points, string date, bool isFinished)
        {
            var gameObj = new Game
            {
                SeasonId = seasonId,
                TeamHostId = teamHostId,
                Team2Id = team2Id,
                TeamHostPoints = teamHostPoints,
                Team2Points = team2Points,
                Date = DateTime.ParseExact(date, "dd/MM/yyyy", null),
                IsFinished = isFinished
            };

            this.db.Games.Add(gameObj);
            await this.db.SaveChangesAsync();

            return gameObj.Id;
        }

        public async Task<bool> DeleteGameAsync(int gameId)
        {
            var gameToDelete = await this.db.Games.FindAsync(gameId);

            if (gameToDelete == null)
            {
                return false;
            }

            this.db.Games.Remove(gameToDelete);
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<GameOverviewServiceModel>> GetAllGamesBetweenTeamsAsync(int team1Id, int team2Id)
        {
            var games = await this.db.Games
                .Where(g => (g.TeamHostId == team1Id || g.Team2Id == team1Id) && (g.TeamHostId == team2Id || g.Team2Id == team2Id))
                .ToListAsync();

            var models = new List<GameOverviewServiceModel>();

            foreach (var game in games.OrderByDescending(g => g.Date))
            {
                var model = mapper.Map<GameOverviewServiceModel>(game);

                models.Add(model);
            }

            return models;
        }

        public async Task<ICollection<GameOverviewServiceModel>> GetAllGamesBetweenTeamsBySeasonAsync(int team1Id, int team2Id, int seasonId)
        {
            var games = await this.db.Games
                .Where(g => (g.TeamHostId == team1Id || g.Team2Id == team1Id) && (g.TeamHostId == team2Id || g.Team2Id == team2Id) && g.SeasonId == seasonId)
                .ToListAsync();

            var models = new List<GameOverviewServiceModel>();

            foreach (var game in games.OrderByDescending(g => g.Date))
            {
                var model = mapper.Map<GameOverviewServiceModel>(game);

                models.Add(model);
            }

            return models;
        }

        public async Task<ICollection<GameOverviewServiceModel>> GetCurrentSeasonGamesAsync(int seasonId)
        {
            var currentSeasonGames = await this.db.Games.Where(g => g.SeasonId == seasonId).ToListAsync();

            var models = new List<GameOverviewServiceModel>();

            foreach (var game in currentSeasonGames)
            {
                var model = mapper.Map<GameOverviewServiceModel>(game);

                models.Add(model);
            }

            return models;
        }

        public async Task<ICollection<GameDetailsServiceModel>> GetGamesOnDateAsync(string date)
        {
            var parsedDate = DateTime.ParseExact(date, "dd/MM/yyyy", null);
            var games = await this.db.Games.Include(g => g.PlayerStats).Where(g => DateTime.Compare(g.Date, parsedDate) == 0).ToListAsync();

            var gameModels = new List<GameDetailsServiceModel>();

            foreach (var game in games)
            {
                var model = mapper.Map<GameDetailsServiceModel>(game);

                gameModels.Add(model);
            }

            return gameModels;
        }

        public async Task<GameDetailsServiceModel> GetGameAsync(int gameId)
        {
            var game = await this.db.Games
                .Include(g => g.PlayerStats)
                .Where(g => g.Id == gameId)
                .FirstOrDefaultAsync();

            if (game == null)
            {
                return null;
            }

            var model = mapper.Map<GameDetailsServiceModel>(game);

            return model;
        }

        public async Task<PlayerGameStatisticServiceModel> GetTopAssistsAsync(int gameId)
        {
            var game = await this.db.Games
                .Include(g => g.PlayerStats).ThenInclude(ps => ps.Player)
                .Where(g => g.Id == gameId)
                .FirstOrDefaultAsync();

            var playerStat = game.PlayerStats.Select(ps => new
            {
                ps.Assists,
                Name = ps.Player.FirstName + " " + ps.Player.LastName,
            }).OrderByDescending(ps => ps.Assists)
              .FirstOrDefault();

            var model = new PlayerGameStatisticServiceModel
            {
                PlayerName = playerStat.Name,
                Value = playerStat.Assists
            };

            return model;
        }
    
        public async Task<PlayerGameStatisticServiceModel> GetTopPointsAsync(int gameId)
        {
            var game = await this.db.Games
                .Include(g => g.PlayerStats).ThenInclude(ps => ps.Player)
                .Where(g => g.Id == gameId)
                .FirstOrDefaultAsync();

            var playerStat = game.PlayerStats.Select(ps => new
            {
                ps.Points,
                Name = ps.Player.FirstName + " " + ps.Player.LastName,
            }).OrderByDescending(ps => ps.Points)
              .FirstOrDefault();

            var model = new PlayerGameStatisticServiceModel
            {
                PlayerName = playerStat.Name,
                Value = playerStat.Points
            };

            return model;
        }

        public async Task<PlayerGameStatisticServiceModel> GetTopReboundsAsync(int gameId)
        {
            var game = await this.db.Games
                 .Include(g => g.PlayerStats).ThenInclude(ps => ps.Player)
                 .Where(g => g.Id == gameId)
                 .FirstOrDefaultAsync();

            var playerStat = game.PlayerStats.Select(ps => new
            {
                ps.Rebounds,
                Name = ps.Player.FirstName + " " + ps.Player.LastName,
            }).OrderByDescending(ps => ps.Rebounds)
              .FirstOrDefault();

            var model = new PlayerGameStatisticServiceModel
            {
                PlayerName = playerStat.Name,
                Value = playerStat.Rebounds
            };

            return model;
        }

        public async Task<string> GetWinnerAsync(int gameId)
        {
            var game = await this.db.Games
                 .Include(g => g.Team2)
                 .Include(g => g.TeamHost)
                 .Where(g => g.Id == gameId)
                 .FirstOrDefaultAsync();

            if (!game.IsFinished)
            {
                return "Game has not finished.";
            }

            var winner = game.TeamHostPoints > game.Team2Points ? game.TeamHost.Name : game.TeamHost.Name;

            return winner;
        }

        public async Task<bool> SetScoreAsync(int gameId, int teamHostScore, int team2Score)
        {
            var game = await this.db.Games.Where(g => g.Id == gameId).FirstOrDefaultAsync();

            if (game.IsFinished)
            {
                return false;
            }
            game.TeamHostPoints = teamHostScore;
            game.Team2Points = team2Score;

            await this.db.SaveChangesAsync();
            return true;
        }
    }
}
