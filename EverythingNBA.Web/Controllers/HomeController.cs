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

    public class HomeController : Controller
    {
        private readonly ITeamService teamService;
        private readonly IGameService gameService;
        private readonly ISeasonService seasonService;
        private readonly IImageService imageService;

        public HomeController(ITeamService teamService, IGameService gameService, ISeasonService seasonService,
            IImageService imageService)
        {
            this.teamService = teamService;
            this.gameService = gameService;
            this.seasonService = seasonService;
            this.imageService = imageService;
        }

        public async Task<IActionResult> Index()
        {
            var year = this.GetCurrentSeasonYear();
            var season = await this.seasonService.GetDetailsByYearAsync(year);

            var gamesYesterday = await this.gameService.GetGamesOnDateAsync(DateTime.UtcNow.AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
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
                model.ImageId = standing.ImageId;
                westernShortTeamStandings.Add(model);
            }

            foreach (var standing in teamStandings.EasternStandings)
            {
                var team = await this.teamService.GetTeamAsync(standing.Name);
                var model = new ShortTeamStandingsViewModel();
                model.TeamAbbreviation = team.AbbreviatedName;
                model.ImageId = standing.ImageId;
                easternShortTeamStandings.Add(model);
            }

            var viewModel = new IndexViewModel()
            {
                GamesYesterday = gamesYesterday,
                GamesToday = gamesToday,
                GamesTomorrow = gamesTomorrow,
                EasternTop8Standings = easternShortTeamStandings,
                WesternTop8Standings = westernShortTeamStandings,
            };

            return this.View(viewModel);
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost("AddPost")]
        public async Task<IActionResult> AddPost(ICollection<IFormFile> images)
        {
            foreach (var image in images)
            {
                await this.imageService.UploadImageAsync(image);
            }

            return RedirectToAction("Index");
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
