namespace EverythingNBA.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using Services;
    using Services.Models.Playoff;
    using Web.Models.Playoff;
    using Data;

    public class PlayoffsController : Controller
    {
        private readonly IMapper mapper;
        private readonly EverythingNBADbContext db;
        private readonly IPlayoffService playoffService;
        private readonly ISeasonService seasonService;
        private readonly ISeriesService seriesService;

        public PlayoffsController(IMapper mapper, EverythingNBADbContext db, IPlayoffService playoffService, ISeasonService seasonService,
            ISeriesService seriesService)
        {
            this.mapper = mapper;
            this.db = db;
            this.playoffService = playoffService;
            this.seasonService = seasonService;
            this.seriesService = seriesService;
        }

        [Route("[controller]/[action]/{playoffId:int}")]
        public async Task<IActionResult> PlayoffBracket(int playoffId)
        {
            var model = await this.playoffService.GetDetailsAsync(playoffId);

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
    }
}