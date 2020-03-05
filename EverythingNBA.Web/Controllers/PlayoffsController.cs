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
    using EverythingNBA.Web.Models.Playoffs;

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
        public async Task<IActionResult> Add(AddPlayoffInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var playoffId = await this.playoffService.AddPlayoffAsync(inputModel.SeasonId, inputModel.WesternQuarterFinalFirstId, 
                inputModel.WesternQuarterFinalSecondId, inputModel.WesternQuarterFinalThirdId, inputModel.WesternQuarterFinalFourthId, 
                inputModel.WesternSemiFinalFirstId, inputModel.WesternSemiFinalSecondId, inputModel.WesternFinalId, inputModel.EasternQuarterFinalFirstId,
                inputModel.EasternQuarterFinalSecondId, inputModel.EasternQuarterFinalThirdId, inputModel.EasternQuarterFinalFourthId, 
                inputModel.EasternSemiFinalFirstId,inputModel.EasternSemiFinalSecondId, inputModel.EasternFinalId, inputModel.FinalId);

            await this.seasonService.AddPlayoffAsync(inputModel.SeasonId, playoffId);

            return RedirectToAction($"PlayoffBracket?{playoffId}");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int playoffId)
        {
            var playoff = await this.playoffService.GetDetailsAsync(playoffId);

            return this.View(playoff);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int playoffId, AddPlayoffInputModel inputModel)
        {
            var playoff = await this.playoffService.GetDetailsAsync(playoffId);

            if (playoff == null)
            {
                return this.View();
            }

            await this.playoffService.DeletePlayoffAsync(playoffId);

            return RedirectToAction($"Home/Index/");
        }

        [HttpGet]
        public IActionResult AddSeries(int seriesId)
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

            await this.playoffService.AddSeriesAsync(playoffId, inputModel.Id, inputModel.Conference, inputModel.Stage, inputModel.Number);

            return RedirectToAction($"PlayoffBracket?{playoffId}");
        }
    }
}