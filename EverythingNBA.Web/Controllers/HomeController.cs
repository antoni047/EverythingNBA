using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EverythingNBA.Web.Models;
using EverythingNBA.Services;
using EverythingNBA.Data;

namespace EverythingNBA.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISeasonService seasonService;
        private readonly ITeamService teamService;
        private readonly ISeasonStatisticService statService;
        private readonly IPlayerService playerService;
        private readonly ISeasonStatisticService seasonStatisticService;
        private readonly IAwardService awardService;
        private readonly EverythingNBADbContext db;

        public HomeController(ISeasonService seasonService, ITeamService teamService, ISeasonStatisticService statService,
            EverythingNBADbContext db, IPlayerService playerService, ISeasonStatisticService seasonStatisticService, IAwardService awardService)
        {
            this.seasonService = seasonService;
            this.teamService = teamService;
            this.statService = statService;
            this.db = db;
            this.playerService = playerService;
            this.seasonStatisticService = seasonStatisticService;
            this.awardService = awardService;
        }

        public async Task<IActionResult> Index()
        {
            await this.teamService.AddPlayerAsync(13, 1);
            //await this.teamService.AddPlayerAsync(9, 1);

            //var result2 = await this.teamService.AddTitleAsync(1, 2);
            //var result = await this.teamService.AddSeasonStatistic(1, 9);


            //var model = await this.teamService.GetTeamDetailsAsync("Lakers");

            //await this.awardService.AddAwardAsync("MVP", 2019, 7);

            return this.View();
        }

        public async Task<IActionResult> Privacy()
        {
            var list = await this.playerService.GetPlayerDetailsAsync(13); 

            return this.View(list);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
