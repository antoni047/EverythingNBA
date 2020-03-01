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

    public class SeasonsController : Controller
    {
        private readonly IMapper mapper;
        private readonly EverythingNBADbContext db;
        private readonly ISeasonService seasonService;

        public SeasonsController(ISeasonService seasonService, EverythingNBADbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
            this.seasonService = seasonService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}