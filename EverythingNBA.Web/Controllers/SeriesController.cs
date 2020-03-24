﻿namespace EverythingNBA.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;
    using System;

    using Services;
    using Web.Models.Series;
    using Web.Models.Games;

    public class SeriesController : Controller
    {
        private readonly IMapper mapper;
        private readonly ISeriesService seriesService;
        private readonly ITeamService teamService;
        private readonly IPlayoffService playoffService;
        private readonly IGameService gameServce;

        public SeriesController(IMapper mapper, ISeriesService seriesService, ITeamService teamService,
            IPlayoffService playoffService, IGameService gameServce)
        {
            this.mapper = mapper;
            this.seriesService = seriesService;
            this.teamService = teamService;
            this.playoffService = playoffService;
            this.gameServce = gameServce;
        }

        [Route("[controller]/[action]/{conference}&{stage}/{seriesId:int}")]
        public async Task<IActionResult> SeriesOverview(int seriesId, string conference, string stage)
        {
            var seriesModel = await this.seriesService.GetSeriesAsync(seriesId);
            seriesModel.Conference = conference;
            seriesModel.Stage = stage;

            ViewBag.WinnerName = await this.seriesService.GetWinnerAsync(seriesId);

            return View(seriesModel);
        }

        [HttpGet]
        [Route("[controller]/[action]/{playoffId:int}")]
        public IActionResult Add(int playoffId)
        {
            var model = new SeriesInputModel();
            model.PlayoffId = playoffId;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SeriesInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var year = this.GetCurrentSeasonYear();

            var team1 = await this.teamService.GetTeamDetailsAsync(inputModel.Team1Name, year);
            var team2 = await this.teamService.GetTeamDetailsAsync(inputModel.Team2Name, year);

            if (team1 == null || team2 == null)
            {
                return this.View(inputModel);
            }


            var seriesId = await this.seriesService.AddSeriesAsync(inputModel.PlayoffId, team1.Name, team2.Name, inputModel.Team1GamesWon,
                inputModel.Team2GamesWon, null, null, null, null, null, null, null, inputModel.Conference, inputModel.Stage, inputModel.StageNumber, null, null);

            await this.playoffService.AddSeriesAsync(inputModel.PlayoffId, seriesId);

            return RedirectToAction("PlayoffBracket", "Playoffs", new { inputModel.PlayoffId});
        }

        [HttpGet]
        [Route("[controller]/[action]/{seriesId:int}")]
        public async Task<IActionResult> Delete(int seriesId)
        {
            var series = await this.seriesService.GetSeriesOverview(seriesId);

            if (series == null)
            {
                return this.View();
            }

            var model = mapper.Map<SeriesInputModel>(series);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SeriesInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.playoffService.RemoveSeriesAsync(model.PlayoffId, model.Id);

            await this.seriesService.DeleteSeriesAsync(model.Id);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("[controller]/[action]/{seriesId:int}")]
        public async Task<IActionResult> AddGame(int seriesId)
        {
            var series = await this.seriesService.GetSeriesOverview(seriesId);

            //if (series == null)
            //{
            //    return NotFound();
            //}

            //var model = new SeriesInputModel()
            //{
            //    Team1Name = series.Team1Name,
            //    Team2Name = series.Team2Name,
            //    Conference = series.Conference,
            //    Stage = series.Stage
            //};

            var model = mapper.Map<SeriesGameInputModel>(series);
            model.TeamHostName = series.Team1Name;
            model.StageNumber = series.StageNumber;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddGame(SeriesGameInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var series = await this.seriesService.GetSeriesAsync(model.SeriesId);

            var playoff = await this.playoffService.GetDetailsAsync(series.PlayoffId);
            var team1 = await this.teamService.GetTeamAsync(model.TeamHostName);
            var team2 = await this.teamService.GetTeamAsync(model.Team2Name);

            var gameId = await this.gameServce.AddGameAsync((int)playoff.SeasonId, team1.Id, team2.Id, model.TeamHostPoints, 
                model.Team2Points, model.Date, model.IsFinished);

            await this.seriesService.AddGameAsync(model.SeriesId, gameId, model.GameNumber);

            var winner = await this.gameServce.GetWinnerAsync(gameId);
            await this.seriesService.SetGameWon(series.Id, winner);

            return RedirectToAction("SeriesOverview", new { model.SeriesId, model.Conference, model.Stage});
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