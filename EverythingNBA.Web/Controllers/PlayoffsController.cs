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

    public class PlayoffsController : Controller
    {
        private readonly IMapper mapper;
        private readonly EverythingNBADbContext db;
        private readonly IPlayoffService playoffService;

        public PlayoffsController(IMapper mapper, EverythingNBADbContext db, IPlayoffService playoffService)
        {
            this.mapper = mapper;
            this.db = db;
            this.playoffService = playoffService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}