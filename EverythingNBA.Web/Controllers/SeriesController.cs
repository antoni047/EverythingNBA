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

    public class SeriesController : Controller
    {
        private readonly IMapper mapper;
        private readonly EverythingNBADbContext db;
        private readonly ISeriesService seriesService;

        public SeriesController(IMapper mapper, EverythingNBADbContext db, ISeriesService seriesService)
        {
            this.mapper = mapper;
            this.db = db;
            this.seriesService = seriesService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}