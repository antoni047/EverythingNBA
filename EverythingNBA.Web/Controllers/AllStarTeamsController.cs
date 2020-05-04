namespace EverythingNBA.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    using Services;
    using EverythingNBA.Web.Models.AllStarTeams;
    using EverythingNBA.Services.Models.Season;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Authorization;

    public class AllStarTeamsController : Controller
    {
        private readonly IAllStarTeamService astService;
        private readonly ISeasonService seasonService;
        private readonly IPlayerService playerService;

        public AllStarTeamsController(IPlayerService playerService, IAllStarTeamService allStarTeamService, ISeasonService seasonService)
        {
            this.astService = allStarTeamService;
            this.seasonService = seasonService;
            this.playerService = playerService;
        }

        [Route("[controller]/[action]/{year:int}")]
        public async Task<IActionResult> SeasonAllStarTeams(int year)
        {
            var astTeams = await this.astService.GetAllASTeamsAsync(year);

            return this.View(astTeams);
        }

        [Route("[controller]/[action]/{type}")]
        public async Task<IActionResult> AllStarTeamType(string type)
        {
            var astTeams = await this.astService.GetAllASTeamsAsync(type);
            ViewBag.Type = this.ConvertToFriendlyType(type);

            return this.View(astTeams);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddAllStarTeamInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var playerNames = new List<string>() { model.FirstPlayer, model.SecondPlayer, model.SecondPlayer, model.FourthPayers, model.FifthPlayer, };

            var id = await this.astService.AddAllStarTeamAsync(model.Year, model.Type, playerNames);

            //if (id == null)
            //{

            //}

            var season = await this.seasonService.GetDetailsByYearAsync(model.Year);

            return RedirectToAction($"SeasonAllStarTeams", season.SeasonId);
        }

        [HttpGet]
        [Route("[controller]/[action]/{allStarTeamId:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int allStarTeamId)
        {
            var model = await this.astService.GetAllStarTeamAsync(allStarTeamId);

           

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete (GetSeasonDetailsServiceModel model, int allStarTeamId)
        {
            var astTeam = await this.astService.GetAllStarTeamAsync(allStarTeamId);
            var season = await this.seasonService.GetDetailsByYearAsync(astTeam.Year);

            if (astTeam.Players.Any())
            {
                foreach (var player in astTeam.Players)
                {
                    //await this.playerService.RemoveAllStarTeam(player.FirstName + " " + player.LastName, allStarTeamId);
                }
            }

            await this.astService.DeleteAllStarTeamAsync(allStarTeamId);

            return RedirectToAction($"SeasonAllStarTeams", season.SeasonId);
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

        private string ConvertToFriendlyType(string type)
        {
            switch (type)
            {
                case "FirstAllNBA":
                    return "First All NBA Team";
                case "SecondAllNBA":
                    return "Second All NBA Team";
                case "ThirdAllNBA":
                    return "Third All NBA Team";
                case "AllDefensive":
                    return "All Defense Team";
                case "AllRookie":
                    return "All Rookie Team";
                default:
                    return "All NBA Team";
            }
        }
    }
}