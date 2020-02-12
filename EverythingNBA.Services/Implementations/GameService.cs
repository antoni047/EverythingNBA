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

    public class GameService : IGameService
    {
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;

        public GameService(EverythingNBADbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<int> AddGamesAsync(int seasonId, int teamHostId, int team2Id, int teamHostPoints, int team2Points, DateTime date, bool isFinished)
        {
            var gameObj = new Game
            {
                SeasonId = seasonId,
                TeamHostId = teamHostId,
                Team2Id = team2Id,
                TeamHostPoints = teamHostPoints,
                Team2Points = team2Points,
                Date = date,
                IsFinished = isFinished
            };

            this.db.Games.Add(gameObj);
            await this.db.SaveChangesAsync();

            return gameObj.Id;
        }

        public async Task<bool> DeletePlayerAsync(int gameId)
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
            var games = await this.db.Games.Where(g => (g.TeamHostId == team1Id || g.Team2Id == team1Id) && (g.TeamHostId == team2Id || g.Team2Id == team2Id))
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

        public async Task<GameDetailsServiceModel> GetGameAsync(DateTime date)
        {
            var game = await this.db.Games.Where(g => g.Date == date).FirstOrDefaultAsync();

            var result = await this.GetGameAsync(game.Id);

            return result;
        }

        public async Task<GameDetailsServiceModel> GetGameAsync(int gameId)
        {
            var game = await this.db.Games.FindAsync(gameId);

            if (game == null)
            {
                return null;
            }

            var model = mapper.Map<GameDetailsServiceModel>(game);

            return model;
        }

        public async Task<PlayerGameStatisticServiceModel> GetTopAssistsAsync(int gameId)
        {
            throw new NotImplementedException();
        }
    

        public async Task<PlayerGameStatisticServiceModel> GetTopPointsAsync(int gameId)
        {
            throw new NotImplementedException();
        }

        public async Task<PlayerGameStatisticServiceModel> GetTopReboundsAsync(int gameId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetWinnerAsync(int gameId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SetScoreAsync(int gameId)
        {
            throw new NotImplementedException();
        }
    }
}
