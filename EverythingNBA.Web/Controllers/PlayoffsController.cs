namespace EverythingNBA.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    using Services;
    using Web.Models.Playoff;

    public class PlayoffsController : Controller
    {
        private readonly IPlayoffService playoffService;
        private readonly ISeasonService seasonService;

        public PlayoffsController(IPlayoffService playoffService, ISeasonService seasonService)
        {
            this.playoffService = playoffService;
            this.seasonService = seasonService;
        }

        [Route("[controller]/[action]/{year:int}")]
        public async Task<IActionResult> PlayoffBracket(int year)
        {
            var model = await this.playoffService.GetDetailsBySeasonAsync(year);
            ViewBag.Year = year;

            return View(model);
        }


        [Route("[controller]/[action]/{playoffId:int}")]
        public async Task<IActionResult> FinishQuarterFinals(int playoffId)
        {
            var playoff = await this.playoffService.GetDetailsAsync(playoffId);
            var year = await this.seasonService.GetYearAsync((int)playoff.SeasonId);

            if (playoff.AreQuarterFinalsFinished == true)
            {
                return RedirectToAction("PlayoffBracket", new { year});
            }

            await this.playoffService.FinishQuarterFinals(playoffId);

            return RedirectToAction("PlayoffBracket", new { year });
        }

        [Route("[controller]/[action]/{playoffId:int}")]
        public async Task<IActionResult> FinishSemiFinals(int playoffId)
        {
            var playoff = await this.playoffService.GetDetailsAsync(playoffId);
            var year = await this.seasonService.GetYearAsync((int)playoff.SeasonId);

            if (playoff.AreSemiFinalsFinished == true)
            {
                return RedirectToAction("PlayoffBracket", new { year });
            }

            await this.playoffService.FinishSemiFinals(playoffId);

            return RedirectToAction("PlayoffBracket", new { year });
        }

        [Route("[controller]/[action]/{playoffId:int}")]
        public async Task<IActionResult> FinishConferenceFinals(int playoffId)
        {
            var playoff = await this.playoffService.GetDetailsAsync(playoffId);
            var year = await this.seasonService.GetYearAsync((int)playoff.SeasonId);

            if (playoff.AreConferenceFinalsFinished == true)
            {
                return RedirectToAction("PlayoffBracket", new { year });
            }

            await this.playoffService.FinishConferenceFinals(playoffId);

            return RedirectToAction("PlayoffBracket", new { year });
        }

        [Route("[controller]/[action]/{playoffId:int}")]
        public async Task<IActionResult> SetStartingSeries(int playoffId)
        {
            var playoff = await this.playoffService.GetDetailsAsync(playoffId);
            await this.playoffService.SetStartingSeries(playoffId);

            TempData["Message"] = "Playoff bracket created successfully";
            TempData["Type"] = "Success";

            return RedirectToAction("Index", "Home");
        }
    }
}