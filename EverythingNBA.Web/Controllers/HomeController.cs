namespace EverythingNBA.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;
    using System.Globalization;
    using Microsoft.AspNetCore.Http;

    using EverythingNBA.Web.Models.Home;
    using EverythingNBA.Web.Models;
    using EverythingNBA.Services;
    using System.Linq;
    using Microsoft.AspNetCore.Identity;

    public class HomeController : Controller
    {
        private readonly ITeamService teamService;
        private readonly IGameService gameService;
        private readonly ISeasonService seasonService;
        private readonly IImageService imageService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public HomeController(ITeamService teamService, IGameService gameService, ISeasonService seasonService,
            IImageService imageService, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.teamService = teamService;
            this.gameService = gameService;
            this.seasonService = seasonService;
            this.imageService = imageService;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var year = this.GetCurrentSeasonYear();
            var season = await this.seasonService.GetDetailsByYearAsync(year);

            var gamesYesterday = await this.gameService.GetGamesOnDateAsync(DateTime.UtcNow.AddDays(-1));
            var gamesToday = await this.gameService.GetGamesOnDateAsync(DateTime.UtcNow);
            var gamesTomorrow = await this.gameService.GetGamesOnDateAsync(DateTime.UtcNow.AddDays(1));
            var teamStandings = await this.teamService.GetStandingsAsync(season.SeasonId);

            var westernShortTeamStandings = new List<ShortTeamStandingsViewModel>();
            var easternShortTeamStandings = new List<ShortTeamStandingsViewModel>();

            foreach (var standing in teamStandings.WesternStandings.Take(8))
            {
                var team = await this.teamService.GetTeamAsync(standing.Name);

                var model = new ShortTeamStandingsViewModel();
                model.TeamAbbreviation = team.AbbreviatedName;
                model.ImageURL = standing.ImageURL;

                westernShortTeamStandings.Add(model);
            }

            foreach (var standing in teamStandings.EasternStandings.Take(8))
            {
                var team = await this.teamService.GetTeamAsync(standing.Name);

                var model = new ShortTeamStandingsViewModel();
                model.TeamAbbreviation = team.AbbreviatedName;
                model.ImageURL = standing.ImageURL;

                easternShortTeamStandings.Add(model);
            }

            var viewModel = new IndexViewModel()
            {
                GamesYesterday = gamesYesterday.ToList(),
                GamesToday = gamesToday.ToList(),
                GamesTomorrow = gamesTomorrow.ToList(),
                EasternTop8Standings = easternShortTeamStandings,
                WesternTop8Standings = westernShortTeamStandings,
            };

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
