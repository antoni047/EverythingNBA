using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EverythingNBA.Web.Areas.Archive.Controllers
{
    [Area("Archive")]
    [Route("Archive/[controller]/[action]")]
    public class TeamsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}