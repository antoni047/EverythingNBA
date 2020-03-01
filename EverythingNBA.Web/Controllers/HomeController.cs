using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using EverythingNBA.Web.Models;
using EverythingNBA.Services;
using EverythingNBA.Data;
using EverythingNBA.Services.Models;

namespace EverythingNBA.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITeamService teamService;
        private readonly IGameService gameService;
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;

        public HomeController(ITeamService teamService, EverythingNBADbContext db, IGameService gameService, IMapper mapper)
        {
            this.teamService = teamService;
            this.gameService = gameService;
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
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
