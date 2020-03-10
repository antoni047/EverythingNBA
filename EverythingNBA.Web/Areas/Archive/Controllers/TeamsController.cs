
namespace EverythingNBA.Web.Areas.Archive.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    using Services;

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
                return RedirectToAction("All");
            }

            return this.View(teamDetailsModel);
        }
    }
}