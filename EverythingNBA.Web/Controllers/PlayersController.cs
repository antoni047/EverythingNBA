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

    public class PlayersController : Controller
    {
        private readonly IMapper mapper;
        private readonly EverythingNBADbContext db;
        private readonly IPlayerService playerService;


        public PlayersController(IPlayerService playerService, IMapper mapper, EverythingNBADbContext db)
        {
            this.db = db;
            this.mapper = mapper;
            this.playerService = playerService;
        }
        
        public IActionResult Index()
        {
            return View();
        }
    }
}