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

            return RedirectToAction("TeamDetails", new { teamDetailsModel.Name });
        }

        [Route("[controller]/[action]/{teamName}")]
        public async Task<IActionResult> TeamDetails(string teamName)
        {
            //checking if the name is an abbreviation or short version
            string fullName = teamName.Split(" ").Length < 2 ? this.GetFullTeamName(teamName) : teamName;

            var currentYear = this.GetCurrentSeasonYear();
            var teamDetailsModel = await this.teamService.GetTeamDetailsAsync(fullName, currentYear);

            if (teamDetailsModel == null)
            {
                return RedirectToAction("All");
            }

            ViewBag.CurrentYear = this.GetCurrentSeasonYear();
            ViewBag.Year = currentYear;
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

            var model = mapper.Map<TeamInputModel>(teamModel);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TeamInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var model = mapper.Map<GetTeamDetailsServiceModel>(inputModel);

            await this.teamService.EditTeamAsync(model, model.Id, inputModel.FullImage, inputModel.SmallImage);

            var teamName = model.Name;

            return RedirectToAction("TeamDetails", new { teamName});
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

            var model = mapper.Map<TeamInputModel>(team);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(TeamInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var year = this.GetCurrentSeasonYear();
            var team = await this.teamService.GetTeamDetailsAsync(model.Id, year);

            foreach (var playerModel in team.Players)
            {
                await this.playerService.RemovePlayerFromTeamAsync(model.Id, playerModel.Id);
            }

            await this.teamService.DeleteTeamAsync(model.Id);

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
            if (shortName == "ATL" || shortName == "Atlanta")
            {
                return "Atlanta Hawks";
            }
            else if (shortName == "BKN" || shortName == "Brooklyn")
            {
                return "Brooklyn Nets";
            }
            else if(shortName == "BOS" || shortName == "Boston")
            {
                return "Boston Celtics";
            }
            else if (shortName == "CHA" || shortName == "Charlotte")
            {
                return "Charlotte Hornets";
            }
            else if (shortName == "CHI" || shortName == "Chicago")
            {
                return "Chicago Bulls";
            }
            else if (shortName == "CLE" || shortName == "Cleveland")
            {
                return "Cleveland Cavaliers";
            }
            else if (shortName == "DAL" || shortName == "Dallas")
            {
                return "Dallas Mavericks";
            }
            else if (shortName == "DEN" || shortName == "Denver")
            {
                return "Denver Nuggets";
            }
            else if (shortName == "DET" || shortName == "Detroit")
            {
                return "Detroit Pistons";
            }
            else if (shortName == "GSW" || shortName == "Golden State")
            {
                return "Golden State Warriors";
            }
            else if (shortName == "HOU" || shortName == "Houston")
            {
                return "Houston Rockets";
            }
            else if (shortName == "IND" || shortName == "Indiana")
            {
                return "Indiana Pacers";
            }
            else if (shortName == "LAC" || shortName == "Los Angeles")
            {
                return "Los Angeles Clippers";
            }
            else if (shortName == "LAL" || shortName == "Los Angeles")
            {
                return "Los Angeles Lakers";
            }
            else if (shortName == "MEM" || shortName == "Memphis")
            {
                return "Memphis Grizzlies";
            }
            else if (shortName == "MIA" || shortName == "Miami")
            {
                return "Miami Heat";
            }
            else if (shortName == "MIL" || shortName == "Milwaukee")
            {
                return "Milwaukee Bucks";
            }
            else if (shortName == "MIN" || shortName == "Minnesota")
            {
                return "Minnesota Timberwolves";
            }
            else if (shortName == "NOP" || shortName == "New Orleans")
            {
                return "New Orleans Pelicans";

            }
            else if (shortName == "NYK" || shortName == "New York")
            {
                return "New York Knicks";
            }
            else if (shortName == "OKC" || shortName == "Oklahoma City")
            {
                return "Oklahoma City Thunder";
            }
            else if (shortName == "ORL" || shortName == "Orlando")
            {
                return "Orlando Magic";
            }
            else if (shortName == "PHI" || shortName == "Philadelphia")
            {
                return "Philadelphia 76ers";
            }
            else if (shortName == "POR" || shortName == "Portland Trail")
            {
                return "Portland Trail Blazers";
            }
            else if (shortName == "SAC" || shortName == "Sacramento ")
            {
                return "Sacramento Kings";
            }
            else if (shortName == "TOR" || shortName == "Toronto")
            {
                return "Toronto Raptors";
            }
            else if (shortName == "UTA" || shortName == "Utah")
            {
                return "Utah Jazz";
            }
            else if (shortName == "WAS" || shortName == "Washington")
            {
                return "Washington Wizards";
            }
            else if (shortName == "PHX" || shortName == "Phoenix")
            {
                return "Phoenix Suns";
            }
            else if (shortName == "SAS" || shortName == "San Antonio")
            {
                return "San Antonio Spurs";
            }
            else
            {
                return "Error";
            }

            //switch (shortName)
            //{
            //    case "ATL":
            //        return "Atlanta Hawks";
            //    case "BKN":
            //        return "Brooklyn Nets";
            //    case "BOS":
            //        return "Boston Celtics";
            //    case "CHA":
            //        return "Charlotte Hornets";
            //    case "CHI":
            //        return "Chicago Bulls";
            //    case "CLE":
            //        return "Cleveland Cavaliers";
            //    case "DAL":
            //        return "Dallas Mavericks";
            //    case "DEN":
            //        return "Denver Nuggets";
            //    case "DET":
            //        return "Detroit Pistons";
            //    case "GSW":
            //        return "Golden State Warriors";
            //    case "HOU":
            //        return "Houston Rockets";
            //    case "IND":
            //        return "Indiana Pacers";
            //    case "LAC":
            //        return "Los Angeles Clippers";
            //    case "LAL":
            //        return "Los Angeles Lakers";
            //    case "MEM":
            //        return "Memphis Grizzlies";
            //    case "MIA":
            //        return "Miami Heat";
            //    case "MIL":
            //        return "Milwaukee Bucks";
            //    case "MIN":
            //        return "Minnesota Timberwolves";
            //    case "NOP":
            //        return "New Orleans Pelicans";
            //    case "NYK":
            //        return "New York Knicks";
            //    case "OKC":
            //        return "Oklahoma City Thunder";
            //    case "ORL":
            //        return "Orlando Magic";
            //    case "PHI":
            //        return "Philadelphia 76ers";
            //    case "POR":
            //        return "Portland Trail Blazers";
            //    case "SAC":
            //        return "Sacramento Kings";
            //    case "TOR":
            //        return "Toronto Raptors";
            //    case "UTA":
            //        return "Utah Jazz";
            //    case "WAS":
            //        return "Washington Wizards";
            //    case "PHX":
            //        return "Phoenix Suns";
            //    case "SAS":
            //        return "San Antonio Spurs";
            //    default:
            //        return "Error";
            //}
        }
    }
}