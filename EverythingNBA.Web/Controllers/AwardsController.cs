namespace EverythingNBA.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using Services;
    using Services.Models;
    using Web.Models;
    using Data;

    public class AwardaController : Controller
    {
        private readonly IMapper mapper;
        private readonly EverythingNBADbContext db;
        private readonly IAwardService awardService;

        public AwardaController(IMapper mapper, EverythingNBADbContext db, IAwardService awardService)
        {
            this.mapper = mapper;
            this.db = db;
            this.awardService = awardService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}