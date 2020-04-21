namespace EverythingNBA.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using System.Globalization;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using EverythingNBA.Models;
    using Services.Models.Game;
    using Services.Models.GameStatistic;

    public class GameService : IGameService
    {
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;

        public GameService(EverythingNBADbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<string> AddGameAsync(int seasonId, int teamHostId, int team2Id, int teamHostPoints, int team2Points, string date, bool isFinished, bool isPlayoffGame)
        {
            var gameObj = new Game
            {
                SeasonId = seasonId,
                TeamHostId = teamHostId,
                Team2Id = team2Id,
                TeamHostPoints = teamHostPoints,
                Team2Points = team2Points,
                Date = DateTime.ParseExact(date, "yyyy-MM-dd", null),
                IsFinished = isFinished,
                IsPlayoffGame = isPlayoffGame,
            };

            if (this.db.Games.Contains(gameObj))
            {
                return "Game_already_exists" + " " + gameObj.Id;
            }

            this.db.Games.Add(gameObj);
            await this.db.SaveChangesAsync();

            return "Success" + " " + gameObj.Id;
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

                models.Add(model);
            }

            return models.OrderByDescending(g => g.Date).ToList();
        }

        public async Task<ICollection<GameDetailsServiceModel>> GetSeasonGamesAsync(int seasonId)
        {
            var currentSeasonGames = await this.db.Games.Include(g => g.TeamHost).Include(g => g.Team2).Where(g => g.SeasonId == seasonId).ToListAsync();

            var models = new List<GameDetailsServiceModel>();

            foreach (var game in currentSeasonGames)
            {
                var model = mapper.Map<GameDetailsServiceModel>(game);
                model.SeasonYear = this.GetCurrentSeasonYear(game.Date);

                models.Add(model);
            }

            return models;
        }

        public async Task<ICollection<GameDetailsServiceModel>> GetGamesOnDateAsync(DateTime date)
        {
            //var parsedDate = DateTime.ParseExact(date, "dd/MM/yyyy", null);

            var games = await this.db.Games
                .Include(g => g.PlayerStats)
                .Include(g => g.Team2)
                .Include(g => g.TeamHost)
                .Where(g => g.Date.Date == date.Date)
                .ToListAsync();

            var gameModels = new List<GameDetailsServiceModel>();

            foreach (var game in games)
            {
                var model = mapper.Map<GameDetailsServiceModel>(game);
                model.Team2ShortName = game.Team2.AbbreviatedName;
                model.TeamHostShortName = game.TeamHost.AbbreviatedName;
                model.SeasonYear = this.GetCurrentSeasonYear(game.Date);

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
            model.SeasonYear = this.GetCurrentSeasonYear(game.Date);

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

            var winner = game.TeamHostPoints > game.Team2Points ? game.TeamHost.Name : game.Team2.Name;

            return winner;
        }

        public async Task<bool> SetScoreAsync(int gameId, int teamHostScore, int team2Score)
        {
            var game = await this.db.Games
                .Include(g => g.Team2)
                    .ThenInclude(t => t.SeasonsStatistics)
                .Include(g => g.TeamHost)
                    .ThenInclude(t => t.SeasonsStatistics)
                .Where(g => g.Id == gameId)
                .FirstOrDefaultAsync();

            if (game.TeamHostPoints > 0 && game.Team2Points > 0) { return false; }

            game.TeamHostPoints = teamHostScore;
            game.Team2Points = team2Score;

            var teamHostStats = game.TeamHost.SeasonsStatistics.Where(ss => ss.SeasonId == game.SeasonId).FirstOrDefault();
            var team2Stats = game.Team2.SeasonsStatistics.Where(ss => ss.SeasonId == game.SeasonId).FirstOrDefault();

            if (teamHostScore > team2Score)
            {
                teamHostStats.Wins++;
                team2Stats.Losses++;
            }
            else
            {
                teamHostStats.Losses++;
                team2Stats.Wins++;
            }

            game.IsFinished = true;
            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<GameDetailsServiceModel> GetGameOverview(int gameId)
        {
            var game = await this.db.Games
                .Include(g => g.Team2)
                .Include(g => g.TeamHost)
                .Where(g => g.Id == gameId)
                .FirstOrDefaultAsync();

            var model = mapper.Map<GameDetailsServiceModel>(game);
            model.SeasonYear = this.GetCurrentSeasonYear(game.Date);

            return model;
        }

        public async Task<GameListingServiceModel> GetFixturesAsync(int seasonId, int page = 1)
        {
            var season = await this.db.Seasons.FindAsync(seasonId);
            var seasonGames = await this.GetSeasonGamesAsync(season.Id);

            var totalDays = (new DateTime(season.Year, 7, 1) - DateTime.Now).TotalDays;
            var dates = new List<DateTime>();

            for (int i = 0; i < (int)totalDays; i++)
            {
                dates.Add(DateTime.UtcNow.AddDays(i));
            }

            var datesList = dates.Skip((page - 1) * 10).Take(10).ToList();

            var gamesNotPlayed = seasonGames.Where(g => g.IsFinished == false)
                .OrderBy(g => DateTime.ParseExact(g.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .Where(g => DateTime.ParseExact(g.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= datesList.First()
                        && DateTime.ParseExact(g.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= datesList.Last())
                .ToList();

            var model = new GameListingServiceModel
            {
                Games = gamesNotPlayed,
                Dates = datesList,
                Total = dates.Count,
                CurrentPage = page,
            };

            return model;
        }

        public async Task<GameListingServiceModel> GetResultsAsync(int seasonId, int page = 1)
        {
            var season = await this.db.Seasons.FindAsync(seasonId);
            var seasonGames = await this.GetSeasonGamesAsync(season.Id);

            var totalDays = (DateTime.UtcNow - season.SeasonStartDate).TotalDays;
            var dates = new List<DateTime>();

            for (int i = 0; i < (int)totalDays; i++)
            {
                dates.Add(DateTime.UtcNow.AddDays(-i));
            }

            var datesList = dates.Skip((page - 1) * 10).Take(10).ToList();

            var gamesPlayed = seasonGames.Where(g => g.IsFinished == true)
                .OrderBy(g => DateTime.ParseExact(g.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .Where(g => DateTime.ParseExact(g.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= datesList.First()
                        && DateTime.ParseExact(g.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= datesList.Last())
                .ToList();

            var model = new GameListingServiceModel
            {
                Games = gamesPlayed,
                Dates = datesList,
                Total = dates.Count,
                CurrentPage = page,
            };

            return model;
        }

        public async Task<string> EditGameAsync(GameDetailsServiceModel model, int gameId)
        {
            var game = await this.db.Games.FindAsync(gameId);

            if (game.Team2Points != 0 && game.TeamHostPoints != 0 && game.Team2Points == game.TeamHostPoints)
            {
                return "Error";
            }

            var scoreSet = await this.SetScoreAsync(game.Id, (int)model.TeamHostPoints, (int)model.Team2Points);
            if (scoreSet == false)
            {
                game.Team2Points = model.Team2Points;
                game.TeamHostPoints = model.TeamHostPoints;
            }

            game.Date = DateTime.ParseExact(model.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            await this.db.SaveChangesAsync();
            return "Success";
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
