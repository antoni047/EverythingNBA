namespace EverythingNBA.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using Services;
    using Services.Models;
    using Web.Models;
    using Data;
    using EverythingNBA.Web.Models.Seasons;

    public class SeasonsController : Controller
    {
        private readonly IMapper mapper;
        private readonly EverythingNBADbContext db;
        private readonly ISeasonService seasonService;
        private readonly ITeamService teamService;

        public SeasonsController(ISeasonService seasonService, EverythingNBADbContext db, IMapper mapper, ITeamService teamService)
        {
            this.db = db;
            this.mapper = mapper;
            this.seasonService = seasonService;
            this.teamService = teamService;
        }

        [Route("[controller]/[action]/{seasonId:int}")]
        public async Task<IActionResult> Standings(int seasonId)
        {
            var allTeamsStandings = await this.teamService.GetStandingsAsync(seasonId);

            return this.View(allTeamsStandings);
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

            await this.seasonService.AddAsync(season.Year, season.TitleWinnerName, season.GamesPlayed);



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

            return this.View(seasonDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SeasonDetailsInputModel inputModel, int seasonId)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var model = mapper.Map<GetSeasonDetailsServiceModel>(inputModel);

            await this.seasonService.EditSeasonAsync(model, seasonId);

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
            await this.seasonService.DeleteAsync(seasonId);

            return RedirectToAction("All");
        }
    }
}