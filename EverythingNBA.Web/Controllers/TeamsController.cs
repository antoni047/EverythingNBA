namespace EverythingNBA.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using Services;
    using Services.Models;
    using Web.Models;
    using Data;

    public class TeamsController : Controller
    {
        private readonly IMapper mapper;
        private readonly EverythingNBADbContext db;
        private readonly ITeamService teamService;

        public TeamsController(IMapper mapper, EverythingNBADbContext db, ITeamService teamService)
        {
            this.db = db;
            this.mapper = mapper;
            this.teamService = teamService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}