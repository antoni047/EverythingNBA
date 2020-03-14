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

        public async Task<IActionResult> TeamDetails(string teamName)
        {
            //checking if the name is an abbreviation
            string fullName = teamName.Length == 3 ? this.GetFullTeamName(teamName) : teamName;

            var currentYear = this.GetCurrentSeasonYear();
            var teamDetailsModel = await this.teamService.GetTeamDetailsAsync(fullName, currentYear);

            return RedirectToAction("TeamDetails", teamDetailsModel.Id);
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

            await this.teamService.AddTeamAsync(model.Name, model.FullImage, model.SmallImage, model.Conference, model.Venue,
                model.Instagram, model.Twitter);

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
        public async Task<IActionResult> Edit(TeamInputModel inputModel, int teamId, IFormFile fullImageId, IFormFile smallImageId)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var model = mapper.Map<GetTeamDetailsServiceModel>(inputModel);

            await this.teamService.EditTeamAsync(model, teamId, fullImageId, smallImageId);

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

        private string GetFullTeamName(string shortName)
        {
            switch (shortName)
            {
                case "ATL":
                    return "Atlanta Hawks";
                case "BKN":
                    return "Brooklyn Nets";
                case "BOS":
                    return "Boston Celtics";
                case "CHA":
                    return "Charlotte Hornets";
                case "CHI":
                    return "Chicago Bulls";
                case "CLE":
                    return "Cleveland Cavaliers";
                case "DAL":
                    return "Dallas Mavericks";
                case "DEN":
                    return "Denver Nuggets";
                case "DET":
                    return "Detroit Pistons";
                case "GSW":
                    return "Golden State Warriors";
                case "HOU":
                    return "Houston Rockets";
                case "IND":
                    return "Indiana Pacers";
                case "LAC":
                    return "Los Angeles Clippers";
                case "LAL":
                    return "Los Angeles Lakers";
                case "MEM":
                    return "Memphis Grizzlies";
                case "MIA":
                    return "Miami Heat";
                case "MIL":
                    return "Milwaukee Bucks";
                case "MIN":
                    return "Minnesota Timberwolves";
                case "NOP":
                    return "New Orleans Pelicans";
                case "NYK":
                    return "New York Knicks";
                case "OKC":
                    return "Oklahoma City Thunder";
                case "ORL":
                    return "Orlando Magic";
                case "PHI":
                    return "Philadelphia 76ers";
                case "POR":
                    return "Portland Trail Blazers";
                case "SAC":
                    return "Sacramento Kings";
                case "TOR":
                    return "Toronto Raptors";
                case "UTA":
                    return "Utah Jazz";
                case "WAS":
                    return "Washington Wizards";
                case "PHX":
                    return "Phoenix Suns";
                case "SAS":
                    return "San Antonio Spurs";
                default:
                    return "Error";
            }
        }
    }
}