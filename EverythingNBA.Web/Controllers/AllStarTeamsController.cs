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

    public class AllStarTeamsController : Controller
    {
        private readonly IMapper mapper;
        private readonly EverythingNBADbContext db;
        private readonly IAllStarTeamService astService;

        public AllStarTeamsController(IMapper mapper, EverythingNBADbContext db, IAllStarTeamService allStarTeamService)
        {
            this.mapper = mapper;
            this.db = db;
            this.astService = allStarTeamService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}