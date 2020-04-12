﻿namespace EverythingNBA.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    using Services;
    using Web.Models.Playoff;

    public class PlayoffsController : Controller
    {
        private readonly IPlayoffService playoffService;
        private readonly ISeasonService seasonService;

        public PlayoffsController(IPlayoffService playoffService, ISeasonService seasonService)
        {
            this.playoffService = playoffService;
            this.seasonService = seasonService;
        }

        [Route("[controller]/[action]/{year:int}")]
        public async Task<IActionResult> PlayoffBracket(int year)
        {
            var model = await this.playoffService.GetDetailsBySeasonAsync(year);
            ViewBag.Year = year;

            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(int seasonId)
        {
            var season = await this.seasonService.GetDetailsAsync(seasonId);

            if (season == null)
            {
                return RedirectToAction("All", "Seasons");
            }

            var playoffId = await this.playoffService.AddPlayoffAsync(seasonId);

            await this.playoffService.SetStartingSeries(playoffId);

            await this.seasonService.AddPlayoffAsync(seasonId, playoffId);

            return RedirectToAction($"PlayoffBracket", playoffId);
        }

        [HttpGet]
        [Route("[controller]/[action]/{playoffId:int}")]
        public async Task<IActionResult> Delete(int playoffId)
        {
            var playoff = await this.playoffService.GetDetailsAsync(playoffId);

            return this.View(playoff);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int playoffId, int seasonId)
        {
            var playoff = await this.playoffService.GetDetailsAsync(playoffId);

            if (playoff == null)
            {
                return this.View();
            }

            var playoffRemovedFromSeason = await this.seasonService.RemovePlayoffAsync(seasonId, playoffId);

            if (!playoffRemovedFromSeason)
            {
                return this.View(playoff);
            }

            await this.playoffService.DeletePlayoffAsync(playoffId);

            return RedirectToAction("Index", "Home");
        }


        [Route("[controller]/[action]/{playoffId:int}")]
        public async Task<IActionResult> FinishQuarterFinals(int playoffId)
        {
            var playoff = await this.playoffService.GetDetailsAsync(playoffId);
            var year = await this.seasonService.GetYearAsync((int)playoff.SeasonId);

            if (playoff.AreQuarterFinalsFinished == true)
            {
                return RedirectToAction("PlayoffBracket", new { year});
            }

            await this.playoffService.FinishQuarterFinals(playoffId);

            return RedirectToAction("PlayoffBracket", new { year });
        }

        [Route("[controller]/[action]/{playoffId:int}")]
        public async Task<IActionResult> FinishSemiFinals(int playoffId)
        {
            var playoff = await this.playoffService.GetDetailsAsync(playoffId);
            var year = await this.seasonService.GetYearAsync((int)playoff.SeasonId);

            if (playoff.AreSemiFinalsFinished == true)
            {
                return RedirectToAction("PlayoffBracket", new { year });
            }

            await this.playoffService.FinishSemiFinals(playoffId);

            return RedirectToAction("PlayoffBracket", new { year });
        }

        [Route("[controller]/[action]/{playoffId:int}")]
        public async Task<IActionResult> FinishConferenceFinals(int playoffId)
        {
            var playoff = await this.playoffService.GetDetailsAsync(playoffId);
            var year = await this.seasonService.GetYearAsync((int)playoff.SeasonId);

            if (playoff.AreConferenceFinalsFinished == true)
            {
                return RedirectToAction("PlayoffBracket", new { year });
            }

            await this.playoffService.FinishConferenceFinals(playoffId);

            return RedirectToAction("PlayoffBracket", new { year });
        }


        [HttpGet]
        public IActionResult AddSeries()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSeries(AddSeriesInputModel inputModel, int playoffId)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.playoffService.AddSeriesAsync(playoffId, inputModel.Id);

            return RedirectToAction("PlayoffBracket", playoffId);
        }

        [HttpPost]
        [Route("[controller]/[action]/{playoffId:int}")]
        public async Task<IActionResult> SetStartingSeries(int playoffId)
        {
            var playoff = await this.playoffService.GetDetailsAsync(playoffId);
            var season = await this.seasonService.GetDetailsAsync((int)playoff.SeasonId);
            await this.playoffService.SetStartingSeries(playoffId);

            TempData["Message"] = "Playoff bracket created successfully";
            TempData["Type"] = "Success";

            return RedirectToAction("Index", "Home");
        }
    }
}