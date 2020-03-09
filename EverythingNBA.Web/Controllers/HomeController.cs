using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using EverythingNBA.Web.Models.Home;
using EverythingNBA.Web.Models;
using EverythingNBA.Services;
using EverythingNBA.Data;
using EverythingNBA.Services.Models.Game;
using System.Globalization;

namespace EverythingNBA.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITeamService teamService;
        private readonly IGameService gameService;
        private readonly ISeasonService seasonService;
        private readonly IMapper mapper;

        public HomeController(ITeamService teamService, IGameService gameService, IMapper mapper, ISeasonService seasonService)
        {
            this.teamService = teamService;
            this.gameService = gameService;
            this.mapper = mapper;
            this.seasonService = seasonService;
        }

        public async Task<IActionResult> Index()
        {
            var year = this.GetCurrentSeasonYear();
            var season = await this.seasonService.GetDetailsByYearAsync(year);

            var gamesToday = await this.gameService.GetGamesOnDateAsync(DateTime.UtcNow.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            var gamesTomorrow = await this.gameService.GetGamesOnDateAsync(DateTime.UtcNow.AddDays(1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            var teamStandings = await this.teamService.GetStandingsAsync(season.SeasonId);

            var westernShortTeamStandings = new List<ShortTeamStandingsViewModel>();
            var easternShortTeamStandings = new List<ShortTeamStandingsViewModel>();

            foreach (var standing in teamStandings.WesternStandings)
            {
                var team = await this.teamService.GetTeamAsync(standing.Name);
                var model = new ShortTeamStandingsViewModel();
                model.TeamAbbreviation = team.AbbreviatedName;
                //model.ImageId = standing.TeamLogoImageURL;
                westernShortTeamStandings.Add(model);
            }

            foreach (var standing in teamStandings.EasternStandings)
            {
                var team = await this.teamService.GetTeamAsync(standing.Name);
                var model = new ShortTeamStandingsViewModel();
                model.TeamAbbreviation = team.AbbreviatedName;
                //model.ImageId = standing.TeamLogoImageURL;
                easternShortTeamStandings.Add(model);
            }

            var viewModel = new IndexViewModel();

            viewModel.GamesToday = gamesToday;
            viewModel.GamesTomorrow = gamesTomorrow;
            viewModel.EasternTop8Standings = easternShortTeamStandings;
            viewModel.WesternTop8Standings = westernShortTeamStandings;

            return this.View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
