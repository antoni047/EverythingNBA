namespace EverythingNBA.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using Services;
    using Services.Models.Series;
    using Web.Models.Series;
    using Data;

    public class SeriesController : Controller
    {
        private readonly IMapper mapper;
        private readonly EverythingNBADbContext db;
        private readonly ISeriesService seriesService;
        private readonly ITeamService teamService;
        private readonly IPlayoffService playoffService;

        public SeriesController(IMapper mapper, EverythingNBADbContext db, ISeriesService seriesService, ITeamService teamService, 
            IPlayoffService playoffService)
        {
            this.mapper = mapper;
            this.db = db;
            this.seriesService = seriesService;
            this.teamService = teamService;
            this.playoffService = playoffService;
        }

        public async Task<IActionResult> SeriesOverview(int seriesId, string conference, string stage)
        {
            var seriesModel = await this.seriesService.GetSeriesAsync(seriesId);
            seriesModel.Conference = conference;
            seriesModel.Stage = stage;

            ViewBag.WinnerName = await this.seriesService.GetWinnerAsync(seriesId);

            return View(seriesModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(SeriesInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var team1 = await this.teamService.GetTeamDetailsAsync(inputModel.Team1Name);
            var team2 = await this.teamService.GetTeamDetailsAsync(inputModel.Team2Name);

            if (team1 == null || team2 == null)
            {
                return this.View(inputModel);
            }


            var seriesId = await this.seriesService.AddSeriesAsync(inputModel.PlayoffId ,team1.Id, team2.Id, inputModel.Team1GamesWon, inputModel.Team2GamesWon,
                            inputModel.Game1Id, inputModel.Game2Id, inputModel.Game3Id, inputModel.Game4Id, inputModel.Game5Id,
                            inputModel.Game6Id, inputModel.Game7Id, inputModel.Conference, inputModel.Stage, inputModel.StageNumber);

            await this.playoffService.AddSeriesAsync(inputModel.PlayoffId, seriesId);

            return this.View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int seriesId)
        {
            var series = await this.seriesService.GetSeriesAsync(seriesId);

            if (series == null)
            {
                return this.View();
            }

            return this.View(series);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int seriesId, SeriesInputModel model)
        {
            var series = await this.seriesService.GetSeriesAsync(seriesId);

            await this.seriesService.DeleteSeriesAsync(seriesId);

            return RedirectToAction("PlayoffBracket", "Playoffs");
        }
    }
}