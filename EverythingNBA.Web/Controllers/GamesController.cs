namespace EverythingNBA.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using Services;
    using Services.Models.Game;
    using Services.Models.GameStatisticModels;
    using Web.Models.Games;
    using Data;
    using System.Globalization;

    public class GamesController : Controller
    {
        private readonly IMapper mapper;
        private readonly IGameService gameService;
        private readonly ISeasonService seasonService;
        private readonly IPlayerService playerService;
        private readonly ISeasonStatisticService statService;
        private readonly ITeamService teamService;
        private readonly IGameStatisticService gameStatisticService;

        public GamesController(IMapper mapper, IGameService gameService, ISeasonService seasonService, IPlayerService playerService,
            ISeasonStatisticService statService, ITeamService teamService, IGameStatisticService gameStatisticService)
        {
            this.mapper = mapper;
            this.gameService = gameService;
            this.seasonService = seasonService;
            this.playerService = playerService;
            this.statService = statService;
            this.teamService = teamService;
            this.gameStatisticService = gameStatisticService;
        }

        public async Task<IActionResult> Results(int seasonId)
        {
            var seasonGames = await this.gameService.GetSeasonGamesAsync(seasonId);

            var results = seasonGames.Where(g => g.IsFinished == true).
                OrderByDescending(g => DateTime.ParseExact(g.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();

            return this.View(results);
        }

        public async Task<IActionResult> Results()
        {
            var year = this.GetCurrentSeasonYear();
            var season = await this.seasonService.GetDetailsByYearAsync(year);

            return RedirectToAction("Results", season.Id);
        }

        public async Task<IActionResult> Fixtures()
        {
            var year = this.GetCurrentSeasonYear();
            var season = await this.seasonService.GetDetailsByYearAsync(year);

            var seasonGames = await this.gameService.GetSeasonGamesAsync(season.Id);

            var gamesNotPlayed = seasonGames.Where(g => g.IsFinished == false)
                .OrderBy(g => DateTime.ParseExact(g.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();

            return this.View(gamesNotPlayed);
        }

        public async Task<IActionResult> GameDetails(int gameId)
        {
            var currentYear = this.GetCurrentSeasonYear();
            var game = await this.gameService.GetGameAsync(gameId);
            var season = await this.seasonService.GetDetailsByYearAsync(currentYear);

            if (game == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var gameModel = mapper.Map<GameDetailsViewModel>(game);

            var teamHost = await this.teamService.GetTeamDetailsAsync(gameModel.TeamHostName, currentYear);
            var team2 = await this.teamService.GetTeamDetailsAsync(gameModel.Team2Name, currentYear);

            gameModel.TeamHostName = teamHost.Name;
            gameModel.Team2Name = team2.Name;

            var teamHostSeasonStats = await this.statService.GetDetailsAsync(season.Id, teamHost.Id);
            var team2SeasonStat = await this.statService.GetDetailsAsync(season.Id, team2.Id);

            gameModel.TeamHostSeasonStatistic = teamHostSeasonStats.Wins + "-" + teamHostSeasonStats.Losses;
            gameModel.Team2SeasonStatistic = team2SeasonStat.Wins + "-" + team2SeasonStat.Losses;

            foreach (var player in teamHost.Players)
            {
                var gameStat = await this.gameStatisticService.GetGameStatisticsAsync(game.Id, player.Name);

                gameModel.TeamHostPlayerStats.Add(gameStat);
            }

            foreach (var player in team2.Players)
            {
                var gameStat = await this.gameStatisticService.GetGameStatisticsAsync(game.Id, player.Name);

                gameModel.Team2PlayerStats.Add(gameStat);
            }

            return this.View(gameModel);
        }

        public async Task<IActionResult> HeadToHead(string team1Name, string team2Name)
        {
            var games = await this.gameService.GetAllGamesBetweenTeamsAsync(team1Name, team2Name);

            return this.View(games);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(GameInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var year = this.GetCurrentSeasonYear();
            var season = await this.seasonService.GetDetailsByYearAsync(year);

            var teamHost = await this.teamService.GetTeamDetailsAsync(inputModel.TeamHostName, year);
            var team2 = await this.teamService.GetTeamDetailsAsync(inputModel.Team2Name, year);

            var gameId = await this.gameService.AddGameAsync(season.Id, teamHost.Id, team2.Id, inputModel.TeamHostPoints,
                inputModel.Team2Points, inputModel.Date, inputModel.IsFinished);

            await this.seasonService.AddGameAsync(season.Id, gameId);
            await this.teamService.AddGameAsync(gameId, teamHost.Id);
            await this.teamService.AddGameAsync(gameId, team2.Id);

            return RedirectToAction("Fixtures");
        }

        [HttpGet]
        public async Task<IActionResult> EditGame(int gameId)
        {
            var gameModel = await this.gameService.GetGameAsync(gameId);

            if (gameModel == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return this.View(gameModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditGame(GameInputModel inputModel, int gameId)
        {
            var model = mapper.Map<GameDetailsServiceModel>(inputModel);

            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.gameService.EditGameAsync(model, gameId);

            return RedirectToAction($"GameDetails", gameId);
        }

        [HttpGet]
        public async Task<IActionResult> EditGameStatistic(int gameId, string playerName)
        {
            var gameStatistic = await this.gameStatisticService.GetGameStatisticsAsync(gameId, playerName);

            var model = mapper.Map<GameStatisticInputModel>(gameStatistic);
            model.GameId = gameId;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditGameStatistic(GameStatisticInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var editedStats = mapper.Map<PlayerGameStatisticServiceModel>(inputModel);

            await this.gameStatisticService.EditGameStatisticAsync(editedStats, inputModel.Id);

            return RedirectToAction("GameDetails", inputModel.GameId);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int gameId)
        {
            var game = await this.gameService.GetGameAsync(gameId);

            if (game == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return this.View(game);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int gameId, GameInputModel model)
        {
            var game = await this.gameService.GetGameAsync(gameId);
            var year = this.GetSeasonYear(DateTime.ParseExact(game.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            var season = await this.seasonService.GetDetailsByYearAsync(year);

            await this.seasonService.RemoveGameAsync(season.Id, gameId);
            await this.teamService.RemoveGameAsync(gameId, (int)game.TeamHostId);
            await this.teamService.RemoveGameAsync(gameId, (int)game.Team2Id);

            await this.gameService.DeleteGameAsync(gameId);

            return RedirectToAction("Index", "Home");
        }

        private int GetCurrentSeasonYear()
        {
            var currentYear = 0;

            if (DateTime.Now.Month >= 9)
            {
                currentYear = DateTime.Now.Year + 1;
            }
            else if (DateTime.Now.Month < 9)
            {
                currentYear = DateTime.Now.Year;
            }

            return currentYear;
        }

        private int GetSeasonYear(DateTime date)
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