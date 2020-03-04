namespace EverythingNBA.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using Services;
    using Services.Models;
    using Web.Models.Awards;
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

        public async Task<IActionResult> SeasonAwards(int seasonId)
        {
            var awards = await this.awardService.GetSeasonAwardsAsync(seasonId);

            return this.View(awards);
        }

        public async Task<IActionResult> All()
        {
            var awards = this.awardService.GetAllAwards();

            return this.View(awards);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddAwardInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.awardService.AddAwardAsync(model.Type, model.Year, model.WinnerName, model.WinnerTeamName);

            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int awardId)
        {
            var award = await this.awardService.GetAwardDetails(awardId);

            return this.View(award);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AwardDetailsInputModel model, int awardId)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.awardService.EditAwardWinnerAsync(model.Winner, awardId);

            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int awardId)
        {
            var award = await this.awardService.GetAwardDetails(awardId);

            return this.View(award);
        }

        [HttpPost]
        public async Task<IActionResult> Delete (AwardDetailsInputModel model, int awardId)
        {
            await this.awardService.DeleteAwardAsync(awardId);

            return RedirectToAction("All");
        }
    }
}