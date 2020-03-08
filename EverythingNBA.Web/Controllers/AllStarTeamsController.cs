namespace EverythingNBA.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using Services;
    using Services.Models;
    using Web.Models;
    using Data;
    using EverythingNBA.Web.Models.AllStarTeams;

    public class AllStarTeamsController : Controller
    {
        private readonly IMapper mapper;
        private readonly EverythingNBADbContext db;
        private readonly IAllStarTeamService astService;
        private readonly ISeasonService seasonService;
        private readonly IPlayerService playerService;

        public AllStarTeamsController(IMapper mapper, EverythingNBADbContext db, IAllStarTeamService allStarTeamService, ISeasonService seasonService)
        {
            this.mapper = mapper;
            this.db = db;
            this.astService = allStarTeamService;
            this.seasonService = seasonService;
        }

        public async Task<IActionResult> SeasonAllStarTeams(int seasonId)
        {
            var year = await this.seasonService.GetYearAsync(seasonId);

            var astTeams = await this.astService.GetAllASTeamsAsync(year);

            return this.View(astTeams);
        }

        public async Task<IActionResult> AllStarTeamType(string type)
        {
            var astTeams = await this.astService.GetAllASTeamsAsync(type);

            return this.View(astTeams);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddAllStarTeamInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            if (model.PlayerNames.Any())
            {
                var id = (int)await this.astService.AddAllStarTeamAsync(model.Year, model.Type, model.PlayerNames);

                foreach (var name in model.PlayerNames)
                {
                    var player = await this.playerService.AddAllStarTeam(name, id);
                }
            }

            else
            {
                await this.astService.AddAllStarTeamAsync(model.Year, model.Type, null);
            }

            var season = await this.seasonService.GetDetailsByYearAsync(model.Year);

            return RedirectToAction($"SeasonAllStarTeams", season.Id);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int allStarTeamId)
        {
            var model = await this.astService.GetAllStarTeamAsync(allStarTeamId);

           

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete (GetSeasonDetailsServiceModel model, int allStarTeamId)
        {
            var astTeam = await this.astService.GetAllStarTeamAsync(allStarTeamId);
            var season = await this.seasonService.GetDetailsByYearAsync(astTeam.Year);

            if (astTeam.Players.Any())
            {
                foreach (var player in astTeam.Players)
                {
                    await this.playerService.RemoveAllStarTeam(player.FirstName + " " + player.LastName, allStarTeamId);
                }
            }

            await this.astService.DeleteAllStarTeamAsync(allStarTeamId);

            return RedirectToAction($"SeasonAllStarTeams", season.Id);
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