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
        private readonly IAllStarTeamService astService;
        private readonly IGameStatisticService gameStatisticService;
        private readonly IGameService gameService;
        private readonly EverythingNBADbContext db;

        public HomeController(ISeasonService seasonService, ITeamService teamService, ISeasonStatisticService statService,
            EverythingNBADbContext db, IPlayerService playerService, ISeasonStatisticService seasonStatisticService, IAwardService awardService,
            IAllStarTeamService astService, IGameService gameService, IGameStatisticService gameStatisticService)
        {
            this.seasonService = seasonService;
            this.teamService = teamService;
            this.statService = statService;
            this.db = db;
            this.playerService = playerService;
            this.seasonStatisticService = seasonStatisticService;
            this.awardService = awardService;
            this.astService = astService;
            this.gameService = gameService;
            this.gameStatisticService = gameStatisticService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await this.playerService.GetPlayerDetailsAsync(12);

            return this.View(model);
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
