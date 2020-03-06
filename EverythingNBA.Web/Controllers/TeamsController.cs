namespace EverythingNBA.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using Services;
    using Services.Models.Team;
    using Web.Models.Teams;
    using Data;

    public class TeamsController : Controller
    {
        private readonly IMapper mapper;
        private readonly EverythingNBADbContext db;
        private readonly ITeamService teamService;

        public TeamsController(IMapper mapper, EverythingNBADbContext db, ITeamService teamService)
        {
            this.db = db;
            this.mapper = mapper;
            this.teamService = teamService;
        }
        public async Task<IActionResult> All()
        {
            var allTeams = await this.teamService.GetAllTeamsAsync();

            return View(allTeams);
        }

        public async Task<IActionResult> TeamDetails(int teamId)
        {
            var currentYear = this.GetCurrentSeasonYear();
            var teamDetailsModel = await this.teamService.GetTeamDetailsAsync(teamId, currentYear);

            if (teamDetailsModel == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return this.View(teamDetailsModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(TeamInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.teamService.AddTeamAsync(model.Name, model.Conference, model.Venue, model.Instagram, model.Twitter);

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