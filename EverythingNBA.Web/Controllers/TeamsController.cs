namespace EverythingNBA.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using Services;
    using Services.Models.Team;
    using Web.Models.Teams;
    using Microsoft.AspNetCore.Http;

    public class TeamsController : Controller
    {
        private readonly IMapper mapper;
        private readonly ITeamService teamService;
        private readonly IPlayerService playerService;

        public TeamsController(IMapper mapper, ITeamService teamService, IPlayerService playerService)
        {
            this.mapper = mapper;
            this.teamService = teamService;
            this.playerService = playerService;
        }
        public async Task<IActionResult> All()
        {
            var allTeams = await this.teamService.GetAllTeamsAsync();

            return View(allTeams);
        }

        [Route("[controller]/[action]/{teamId:int}")]
        public async Task<IActionResult> TeamDetails(int teamId)
        {
            var currentYear = this.GetCurrentSeasonYear();
            var teamDetailsModel = await this.teamService.GetTeamDetailsAsync(teamId, currentYear);

            if (teamDetailsModel == null)
            {
                return RedirectToAction("All");
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

            await this.teamService.AddTeamAsync(model.Name, model.Image ,model.Conference, model.Venue, model.Instagram, model.Twitter);

            return RedirectToAction("All");
        }

        [HttpGet]
        [Route("[controller]/[action]/{teamId:int}")]
        public async Task<IActionResult> Edit(int teamId)
        {
            var year = this.GetCurrentSeasonYear();
            var teamModel = await this.teamService.GetTeamDetailsAsync(teamId, year);

            if (teamModel == null)
            {
                return RedirectToAction("All");
            }

            return this.View(teamModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TeamInputModel inputModel, int teamId, IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var model = mapper.Map<GetTeamDetailsServiceModel>(inputModel);

            await this.teamService.EditTeamAsync(model, teamId, image);

            return RedirectToAction("TeamDetails");
        }

        [HttpGet]
        [Route("[controller]/[action]/{teamId:int}")]
        public async Task<IActionResult> Delete(int teamId)
        {
            var year = this.GetCurrentSeasonYear();
            var team = await this.teamService.GetTeamDetailsAsync(teamId, year);

            if (team == null)
            {
                return RedirectToAction("All");
            }

            return this.View(team);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int teamId, TeamInputModel model)
        {
            var year = this.GetCurrentSeasonYear();
            var team = await this.teamService.GetTeamDetailsAsync(teamId, year);

            foreach (var playerModel in team.Players)
            {
                await this.playerService.RemovePlayerFromTeamAsync(teamId, playerModel.Id);
            }

            await this.teamService.DeleteTeamAsync(teamId);

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