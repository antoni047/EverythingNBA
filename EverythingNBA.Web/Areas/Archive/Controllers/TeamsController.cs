
namespace EverythingNBA.Web.Areas.Archive.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    using Services;
    using System;

    [Area("Archive")]
    [Route("Archive/[controller]/[action]")]
    public class TeamsController : Controller
    {
        private readonly ITeamService teamService;

        public TeamsController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        [Route("Archive/[controller]/[action]/{teamId:int}/{year:int}")]
        public async Task<IActionResult> TeamDetails(int teamId, int year)
        {
            var teamDetailsModel = await this.teamService.GetTeamDetailsAsync(teamId, year);

            if (teamDetailsModel == null)
            {
                return RedirectToAction("Teams", "All");
            }

            ViewBag.CurrentYear = this.GetCurrentSeasonYear();
            ViewBag.Year = year;
            return this.View(teamDetailsModel);
        }

        private int GetCurrentSeasonYear()
        {
            var currentYear = 0;

            if (DateTime.Now.Month >= 9)
            {
                currentYear = DateTime.Now.Year + 1;
            }
            else if (DateTime.Now.Month < 9)
            {
                currentYear = DateTime.Now.Year;
            }

            return currentYear;
        }
    }
}