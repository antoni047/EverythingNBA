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
        private readonly IGameStatisticService statService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public HomeController(ITeamService teamService, IGameService gameService, ISeasonService seasonService,
            IImageService imageService, IGameStatisticService service, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.teamService = teamService;
            this.gameService = gameService;
            this.seasonService = seasonService;
            this.imageService = imageService;
            this.statService = service;
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

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost("AddPost")]
        public async Task<IActionResult> AddPost(ICollection<IFormFile> images)
        {
            var newUser = new IdentityUser()
            {
                UserName = "admin",
                Email = "admin@gmail.com",
            };

            var result = await userManager.CreateAsync(newUser, "adminpassword");

            var roleName = "Administrator";
            var roleExists = await roleManager.RoleExistsAsync(roleName);

            if (roleExists)
            {
                var user = await userManager.GetUserAsync(User);
                await userManager.AddToRoleAsync(user, roleName);
            }


            //await this.service.AddAsync(31, 1, 30, 21, 2, 10, 5, 1, 5, 5, 10, 10, 5, 18);
            //await this.service.AddAsync(31, 2, 30, 50, 3, 8, 5, 1, 5, 5, 10, 10, 5, 12);
            //await this.service.AddAsync(31, 3, 30, 23, 3, 12, 5, 1, 5, 5, 10, 10, 5, 8);
            //await this.service.AddAsync(31, 4, 30, 32, 4, 14, 5, 1, 5, 5, 10, 10, 5, 13);
            //await this.service.AddAsync(31, 5, 30, 11, 4, 8, 5, 1, 5, 5, 10, 10, 5, 8);
            //await this.service.AddAsync(31, 6, 30, 28, 21, 2, 5, 1, 5, 5, 10, 10, 5, 5);
            //await this.service.AddAsync(31, 11, 30, 42, 20, 5, 5, 1, 5, 5, 10, 10, 5, 5);
            //await this.service.AddAsync(31, 21, 30, 25, 5, 5, 5, 1, 5, 5, 10, 10, 5, 5);
            //await this.service.AddAsync(31, 23, 30, 36, 8, 5, 5, 1, 5, 5, 10, 10, 5, 14);
            //await this.service.AddAsync(31, 24, 30, 22, 10, 18, 5, 1, 5, 5, 10, 10, 5, 6);
            //await this.service.AddAsync(31, 25, 30, 21, 6, 8, 5, 1, 5, 5, 10, 10, 5, 6);
            //await this.service.AddAsync(32, 1, 30, 21, 8, 4, 5, 1, 5, 5, 10, 10, 5, 7);
            //await this.service.AddAsync(32, 2, 30, 24, 7, 3, 5, 1, 5, 5, 10, 10, 5, 8);
            //await this.service.AddAsync(32, 3, 30, 18, 6, 2, 5, 1, 5, 5, 10, 10, 5, 9);
            //await this.service.AddAsync(32, 4, 30, 12, 2, 1, 5, 1, 5, 5, 10, 10, 5, 10);
            //await this.service.AddAsync(32, 5, 30, 58, 2, 8, 5, 1, 5, 5, 10, 10, 5, 11);
            //await this.service.AddAsync(32, 6, 30, 10, 8, 15, 5, 1, 5, 5, 10, 10, 5, 12);
            //await this.service.AddAsync(32, 11, 30, 13, 2, 1, 5, 1, 5, 5, 10, 10, 5, 13);
            //await this.service.AddAsync(32, 21, 30, 22, 12, 0, 5, 1, 5, 5, 10, 10, 5, 14);
            //await this.service.AddAsync(32, 23, 30, 21, 2, 15, 5, 1, 5, 5, 10, 10, 5, 15);
            //await this.service.AddAsync(32, 21, 30, 25, 5, 5, 5, 1, 5, 5, 10, 10, 5, 5);
            //await this.service.AddAsync(32, 23, 30, 36, 8, 5, 5, 1, 5, 5, 10, 10, 5, 14);
            //await this.service.AddAsync(32, 24, 30, 22, 10, 18, 5, 1, 5, 5, 10, 10, 5, 6);
            //await this.service.AddAsync(32, 25, 30, 21, 6, 8, 5, 1, 5, 5, 10, 10, 5, 6);
            //await this.service.AddAsync(33, 1, 30, 21, 8, 4, 5, 1, 5, 5, 10, 10, 5, 7);
            //await this.service.AddAsync(33, 2, 30, 24, 7, 3, 5, 1, 5, 5, 10, 10, 5, 8);
            //await this.service.AddAsync(33, 3, 30, 18, 6, 2, 5, 1, 5, 5, 10, 10, 5, 9);
            //await this.service.AddAsync(33, 4, 30, 12, 2, 1, 5, 1, 5, 5, 10, 10, 5, 10);
            //await this.service.AddAsync(33, 5, 30, 58, 2, 8, 5, 1, 5, 5, 10, 10, 5, 11);
            //await this.service.AddAsync(33, 6, 30, 10, 8, 15, 5, 1, 5, 5, 10, 10, 5, 12);
            //await this.service.AddAsync(33, 11, 30, 13, 2, 1, 5, 1, 5, 5, 10, 10, 5, 13);
            //await this.service.AddAsync(33, 21, 30, 18, 6, 2, 5, 1, 5, 5, 10, 10, 5, 9);
            //await this.service.AddAsync(33, 23, 30, 12, 2, 1, 5, 1, 5, 5, 10, 10, 5, 10);
            //await this.service.AddAsync(33, 24, 30, 58, 2, 8, 5, 1, 5, 5, 10, 10, 5, 11);
            //await this.service.AddAsync(33, 25, 30, 10, 8, 15, 5, 1, 5, 5, 10, 10, 5, 12);
            //await this.service.AddAsync(34, 1, 30, 13, 2, 1, 5, 1, 5, 5, 10, 10, 5, 13);
            //await this.service.AddAsync(34, 2, 30, 22, 12, 0, 5, 1, 5, 5, 10, 10, 5, 14);
            //await this.service.AddAsync(34, 3, 30, 21, 8, 4, 5, 1, 5, 5, 10, 10, 5, 7);
            //await this.service.AddAsync(34, 4, 30, 24, 7, 3, 5, 1, 5, 5, 10, 10, 5, 8);
            //await this.service.AddAsync(34, 5, 30, 18, 6, 2, 5, 1, 5, 5, 10, 10, 5, 9);
            //await this.service.AddAsync(34, 6, 30, 12, 2, 1, 5, 1, 5, 5, 10, 10, 5, 10);
            //await this.service.AddAsync(34, 11, 30, 58, 2, 8, 5, 1, 5, 5, 10, 10, 5, 11);
            //await this.service.AddAsync(34, 21, 30, 10, 8, 15, 5, 1, 5, 5, 10, 10, 5, 12);
            //await this.service.AddAsync(34, 23, 30, 13, 2, 1, 5, 1, 5, 5, 10, 10, 5, 13);
            //await this.service.AddAsync(34, 24, 30, 22, 12, 0, 5, 1, 5, 5, 10, 10, 5, 14);
            //await this.service.AddAsync(34, 25, 30, 21, 2, 15, 5, 1, 5, 5, 10, 10, 5, 15);


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
