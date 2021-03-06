﻿namespace EverythingNBA.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using Services;
    using Services.Models.Game;
    using Services.Models.GameStatistic;
    using Web.Models.Games;
    using System.Globalization;
    using Microsoft.AspNetCore.Authorization;

    public class GamesController : Controller
    {
        private readonly IMapper mapper;
        private readonly IGameService gameService;
        private readonly ISeasonService seasonService;
        private readonly ISeasonStatisticService statService;
        private readonly ITeamService teamService;
        private readonly IGameStatisticService gameStatisticService;

        public GamesController(IMapper mapper, IGameService gameService, ISeasonService seasonService, ITeamService teamService,
            ISeasonStatisticService statService, IGameStatisticService gameStatisticService)
        {
            this.mapper = mapper;
            this.gameService = gameService;
            this.seasonService = seasonService;
            this.statService = statService;
            this.teamService = teamService;
            this.gameStatisticService = gameStatisticService;
        }

        [Route("[controller]/[action]/{year:int}")]
        public async Task<IActionResult> Results(int year, int page = 1)
        {
            var season = await this.seasonService.GetDetailsByYearAsync(year);

            var model = await this.gameService.GetResultsAsync(season.SeasonId, page);

            ViewBag.Year = year;

            return this.View(model);
        }

        public IActionResult CurrentResults()
        {
            var year = this.GetCurrentSeasonYear();

            return RedirectToAction("Results", "Games", new { year });
        }

        public async Task<IActionResult> Fixtures(int page = 1)
        {
            var year = this.GetCurrentSeasonYear();
            var season = await this.seasonService.GetDetailsByYearAsync(year);

            var model = await this.gameService.GetFixturesAsync(season.SeasonId, page);

            return this.View(model);
        }

        [Route("[controller]/[action]/{gameId:int}")]
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

            var teamHostFullName = this.GetFullTeamName(game.TeamHostShortName);
            var team2FullName = this.GetFullTeamName(game.Team2ShortName);
            var teamHost = await this.teamService.GetTeamDetailsAsync(teamHostFullName, currentYear);
            var team2 = await this.teamService.GetTeamDetailsAsync(team2FullName, currentYear);

            gameModel.TeamHostName = teamHost.Name;
            gameModel.Team2Name = team2.Name;
            gameModel.TeamHostPrimaryColor = teamHost.PrimaryColorHex;
            gameModel.TeamHostSecondaryColor = teamHost.SecondaryColorHex;
            gameModel.Team2PrimaryColor = team2.PrimaryColorHex;
            gameModel.Team2SecondaryColor = team2.SecondaryColorHex;

            var teamHostSeasonStats = await this.statService.GetDetailsAsync(season.SeasonId, teamHost.Id);
            var team2SeasonStat = await this.statService.GetDetailsAsync(season.SeasonId, team2.Id);

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

        [Route("[controller]/[action]/{team1Name}&{team2Name}")]
        public async Task<IActionResult> HeadToHead(string team1Name, string team2Name)
        {
            var games = await this.gameService.GetAllGamesBetweenTeamsAsync(team1Name, team2Name);

            var yearsList = new HashSet<int>();
            foreach (var game in games)
            {
                yearsList.Add(game.SeasonYear);
            }

            ViewBag.yearsList = yearsList;

            return this.View(games);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
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

            var gameAdded = await this.gameService.AddGameAsync(season.SeasonId, teamHost.Id, team2.Id, inputModel.TeamHostPoints,
                inputModel.Team2Points, inputModel.Date, inputModel.IsFinished, false);

            //Success notification data 
            if (gameAdded.Split(" ")[0] == "Success")
            {
                TempData["Message"] = "Game added successfully";
                TempData["Type"] = "Success";
            }
            else
            {
                TempData["Message"] = "Game already exists";
                TempData["Type"] = "Error";
            }

            if (inputModel.IsFinished == true)
            {
                return RedirectToAction("Results", new { year});
            }
            else
            {
                return RedirectToAction("Fixtures");
            }
            
        }

        [HttpGet]
        [Authorize]
        [Route("[controller]/[action]/{gameId:int}")]
        public async Task<IActionResult> EditGame(int gameId)
        {
            var gameModel = await this.gameService.GetGameAsync(gameId);

            if (gameModel == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = mapper.Map<GameInputModel>(gameModel);
            model.Team2Name = this.GetFullTeamName(gameModel.Team2ShortName);
            model.TeamHostName = this.GetFullTeamName(gameModel.TeamHostShortName);


            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditGame(GameInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var model = mapper.Map<GameDetailsServiceModel>(inputModel);

            var result = await this.gameService.EditGameAsync(model, inputModel.Id);

            if (result == "Error")
            {
                TempData["Message"] = "Points of the two teams cannot be the same";
                TempData["Type"] = result;

                return this.View(inputModel);
            }

            //Success notification data 
            TempData["Message"] = "Game edited successfully";
            TempData["Type"] = result;

            return RedirectToAction($"GameDetails", new { inputModel.Id });
        }

        [HttpGet]
        [Route("[controller]/[action]/{gameStatisticId:int}")]
        [Authorize]
        public async Task<IActionResult> EditGameStatistic(int gameStatisticId)
        {
            var gameStatistic = await this.gameStatisticService.GetGameStatisticsAsync(gameStatisticId);

            if (gameStatistic == null)
            {
                return NotFound();
            }

            var model = mapper.Map<GameStatisticInputModel>(gameStatistic);
            model.GameId = gameStatistic.GameId;

            var game = await this.gameService.GetGameOverview(model.GameId);
            ViewBag.Name = gameStatistic.PlayerName;
            ViewBag.Team1 = game.TeamHostShortName;
            ViewBag.Team2 = game.Team2ShortName;
            ViewBag.Date = game.Date;

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditGameStatistic(GameStatisticInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var editedStats = mapper.Map<PlayerGameStatisticServiceModel>(inputModel);

            await this.gameStatisticService.EditGameStatisticAsync(editedStats, inputModel.Id);

            //Success notification data 
            TempData["Message"] = "Game statistic edited successfully";
            TempData["Type"] = "Success";

            return RedirectToAction("GameDetails", new { inputModel.GameId });
        }

        [HttpGet]
        [Route("[controller]/[action]/{gameId:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int gameId)
        {
            var game = await this.gameService.GetGameAsync(gameId);

            if (game == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = mapper.Map<GameInputModel>(game);
            model.TeamHostName = this.GetFullTeamName(game.TeamHostShortName);
            model.Team2Name = this.GetFullTeamName(game.Team2ShortName);

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(GameInputModel model)
        {
            var game = await this.gameService.GetGameAsync(model.Id);

            var year = this.GetSeasonYear(DateTime.ParseExact(game.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            var season = await this.seasonService.GetDetailsByYearAsync(year);

            var teamHostName = this.GetFullTeamName(game.TeamHostShortName);
            var team2Name = this.GetFullTeamName(game.Team2ShortName);

            if (teamHostName != "Error" || team2Name != "Error")
            {
                var teamHost = await this.teamService.GetTeamDetailsAsync(teamHostName, year);
                var team2 = await this.teamService.GetTeamDetailsAsync(team2Name, year);

                await this.teamService.RemoveGameAsync(model.Id, team2.Id);
                await this.teamService.RemoveGameAsync(model.Id, teamHost.Id);
            }
            
            var deleted = await this.gameService.DeleteGameAsync(model.Id);

            //Success notification data 
            if (deleted == true)
            {
                TempData["Message"] = "Game deleted successfully";
                TempData["Type"] = "Success";
            }
            else
            {
                TempData["Message"] = "Game not deleted";
                TempData["Type"] = "Error";
            }
            

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

        private string GetFullTeamName (string shortName)
        {
            switch (shortName)
            {
                case "ATL":
                    return "Atlanta Hawks";
                case "BKN":
                    return "Brooklyn Nets";
                case "BOS":
                    return "Boston Celtics";
                case "CHA":
                    return "Charlotte Hornets";
                case "CHI":
                    return "Chicago Bulls";
                case "CLE":
                    return "Cleveland Cavaliers";
                case "DAL":
                    return "Dallas Mavericks";
                case "DEN":
                    return "Denver Nuggets";
                case "DET":
                    return "Detroit Pistons";
                case "GSW":
                    return "Golden State Warriors";
                case "HOU":
                    return "Houston Rockets";
                case "IND":
                    return "Indiana Pacers";
                case "LAC":
                    return "Los Angeles Clippers";
                case "LAL":
                    return "Los Angeles Lakers";
                case "MEM":
                    return "Memphis Grizzlies";
                case "MIA":
                    return "Miami Heat";
                case "MIL":
                    return "Milwaukee Bucks";
                case "MIN":
                    return "Minnesota Timberwolves";
                case "NOP":
                    return "New Orleans Pelicans";
                case "NYK":
                    return "New York Knicks";
                case "OKC":
                    return "Oklahoma City Thunder";
                case "ORL":
                    return "Orlando Magic";
                case "PHI":
                    return "Philadelphia 76ers";
                case "POR":
                    return "Portland Trail Blazers";
                case "SAC":
                    return "Sacramento Kings";
                case "TOR":
                    return "Toronto Raptors";
                case "UTA":
                    return "Utah Jazz";
                case "WAS":
                    return "Washington Wizards";
                case "PHX":
                    return "Phoenix Suns";
                case "SAS":
                    return "San Antonio Spurs";
                default:
                    return "Error";
            }
        }
    }
}