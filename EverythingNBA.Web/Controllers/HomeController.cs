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
            await this.teamService.RemovePlayerAsync(13, 1);
            await this.teamService.RemovePlayerAsync(9, 1);
            await this.teamService.RemovePlayerAsync(2, 1);

            var model = await this.teamService.GetTeamDetailsAsync(1);

            return this.View(model);

            //return this.View();
        }

        public async Task<IActionResult> Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
