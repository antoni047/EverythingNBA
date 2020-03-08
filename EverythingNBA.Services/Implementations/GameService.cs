namespace EverythingNBA.Services.Implementations
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
    using EverythingNBA.Services.Models.GameStatisticModels;
    using System.Globalization;

    public class GameService : IGameService
    {
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;
        private readonly IGameStatisticService gameStatisticService;

        public GameService(EverythingNBADbContext db, IMapper mapper, IGameStatisticService gameStatisticService)
        {
            this.db = db;
            this.mapper = mapper;
            this.gameStatisticService = gameStatisticService;
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

        public async Task<ICollection<GameDetailsServiceModel>> GetAllGamesBetweenTeamsAsync(string team1Name, string team2Name)
        {
            var games = await this.db.Games
                .Include(g => g.TeamHost)
                .Include(g => g.Team2)
                .Where(g => (g.TeamHost.Name == team1Name || g.Team2.Name == team1Name) && (g.TeamHost.Name == team2Name || g.Team2.Name == team2Name))
                .ToListAsync();

            var models = new List<GameDetailsServiceModel>();

            foreach (var game in games.OrderByDescending(g => g.Date))
            {
                var model = mapper.Map<GameDetailsServiceModel>(game);
                model.SeasonYear = this.GetCurrentSeasonYear(DateTime.ParseExact(model.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                model.Venue = game.TeamHost.Venue;

                models.Add(model);
            }

            return models;
        }

        public async Task<ICollection<GameDetailsServiceModel>> GetAllGamesBetweenTeamsBySeasonAsync(string team1Name, string team2Name, int seasonId)
        {
            var games = await this.db.Games
                .Include(g => g.TeamHost)
                .Include(g => g.Team2)
                .Where(g => (g.TeamHost.Name == team1Name || g.Team2.Name == team1Name) && (g.TeamHost.Name == team2Name || g.Team2.Name == team2Name) && g.SeasonId == seasonId)
                .ToListAsync();

            var models = new List<GameDetailsServiceModel>();

            foreach (var game in games.OrderByDescending(g => g.Date))
            {
                var model = mapper.Map<GameDetailsServiceModel>(game);

                models.Add(model);
            }

            return models;
        }

        public async Task<ICollection<GameDetailsServiceModel>> GetSeasonGamesAsync(int seasonId)
        {
            var currentSeasonGames = await this.db.Games.Where(g => g.SeasonId == seasonId).ToListAsync();

            var models = new List<GameDetailsServiceModel>();

            foreach (var game in currentSeasonGames)
            {
                var model = mapper.Map<GameDetailsServiceModel>(game);

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
                    .ThenInclude(ps => ps.Player)
                .Include(g => g.TeamHost)
                .Include(g => g.Team2)
                .Where(g => g.Id == gameId)
                .FirstOrDefaultAsync();

            if (game == null)
            {
                return null;
            }

            var model = mapper.Map<GameDetailsServiceModel>(game);

            model.Venue = game.TeamHost.Venue;

            return model;
        }

        public async Task<PlayerTopStatisticServiceModel> GetTopAssistsAsync(int gameId)
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

            var model = new PlayerTopStatisticServiceModel
            {
                PlayerName = playerStat.Name,
                Value = playerStat.Assists
            };

            return model;
        }
    
        public async Task<PlayerTopStatisticServiceModel> GetTopPointsAsync(int gameId)
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

            var model = new PlayerTopStatisticServiceModel
            {
                PlayerName = playerStat.Name,
                Value = playerStat.Points
            };

            return model;
        }

        public async Task<PlayerTopStatisticServiceModel> GetTopReboundsAsync(int gameId)
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

            var model = new PlayerTopStatisticServiceModel
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

        public async Task<GameDetailsServiceModel> GetGameOverview(int gameId)
        {
            var game = await this.db.Games.Where(g => g.Id == gameId).FirstOrDefaultAsync();

            var model = mapper.Map<GameDetailsServiceModel>(game);

            return model;
        }

        public async Task EditGameAsync(GameDetailsServiceModel model, int gameId)
        {
            var game = await this.db.Games.FindAsync(gameId);

            game.Team2Points = model.Team2Points;
            game.TeamHostPoints = model.TeamHostPoints;
            game.Date = DateTime.ParseExact(model.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            await this.db.SaveChangesAsync();
        }

        private int GetCurrentSeasonYear(DateTime date)
        {
            var seasonYear = 0;

            if (date.Month >= 9)
            {
                seasonYear = date.Year + 1;
            }
            else if (date.Month < 9)
            {
                seasonYear = date.Year;
            }

            return seasonYear;
        }
    }
}
