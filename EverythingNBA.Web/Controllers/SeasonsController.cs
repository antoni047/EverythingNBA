namespace EverythingNBA.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;
    using System;

    using Services;
    using EverythingNBA.Web.Models.Seasons;
    using EverythingNBA.Services.Models.Season;

    public class SeasonsController : Controller
    {
        private readonly IMapper mapper;
        private readonly ISeasonService seasonService;
        private readonly ITeamService teamService;
        private readonly IPlayoffService playoffService;

        public SeasonsController(ISeasonService seasonService, IMapper mapper, ITeamService teamService, IPlayoffService playoffService)
        {
            this.mapper = mapper;
            this.seasonService = seasonService;
            this.teamService = teamService;
            this.playoffService = playoffService;
        }

        [Route("[controller]/[action]/{seasonId:int}")]
        public async Task<IActionResult> Standings(int seasonId)
        {
            var allTeamsStandings = await this.teamService.GetStandingsAsync(seasonId);

            var seasonYear = await this.seasonService.GetYearAsync(seasonId);
            var playoff = await this.playoffService.GetDetailsBySeasonAsync(seasonYear);
            ViewBag.Year = seasonYear;

            return this.View(allTeamsStandings);
        }

        public async Task<IActionResult> CurrentStandings()
        {
            var year = this.GetCurrentSeasonYear();
            var season = await this.seasonService.GetDetailsByYearAsync(year);

            return RedirectToAction("Standings", new { season.SeasonId });
        }
        public async Task<IActionResult> All()
        {
            var seasons = await this.seasonService.GetAllSeasonsAsync();

            return this.View(seasons);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(SeasonDetailsInputModel season)
        {
            if (!ModelState.IsValid)
            {
                return this.View(season);
            }

            var seasonId = await this.seasonService.AddAsync(season.Year, season.TitleWinnerName, season.GamesPlayed);

            var playoffId = await this.playoffService.AddPlayoffAsync(seasonId);

            await this.playoffService.SetStartingSeries(playoffId);

            await this.seasonService.AddPlayoffAsync(seasonId, playoffId);

            return RedirectToAction("All");
        }

        [HttpGet]
        [Route("[controller]/[action]/{seasonId:int}")]
        public async Task<IActionResult> Edit(int seasonId)
        {
            var seasonDetails = await this.seasonService.GetDetailsAsync(seasonId);
            if (seasonDetails == null)
            {
                return RedirectToAction("All"); 
            }

            var model = mapper.Map<SeasonDetailsInputModel>(seasonDetails);
            model.Id = seasonDetails.SeasonId;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SeasonDetailsInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var model = mapper.Map<GetSeasonDetailsServiceModel>(inputModel);
            var team = await this.teamService.GetTeamAsync(inputModel.TitleWinnerName);
            model.TitleWinnerId = team.Id;

            await this.seasonService.EditSeasonAsync(model, inputModel.Id);

            return RedirectToAction("All");
        }

        [HttpGet]
        [Route("[controller]/[action]/{seasonId:int}")]
        public async Task<IActionResult> Delete(int seasonId)
        {
            var season = await this.seasonService.GetDetailsAsync(seasonId);
            if (season == null)
            {
                return RedirectToAction("All");
            }

            return this.View(season);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SeasonDetailsInputModel model, int seasonId)
        {
            var season = await this.seasonService.GetDetailsAsync(seasonId);
            await this.playoffService.DeletePlayoffAsync((int)season.PlayoffId);
            await this.seasonService.DeleteAsync(seasonId);

            return RedirectToAction("All");
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
    }
}