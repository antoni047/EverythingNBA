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
        private readonly ISeasonService seasonService;
        private readonly IPlayerService playerService;

        public AwardaController(IMapper mapper, EverythingNBADbContext db, IAwardService awardService, ISeasonService seasonService, 
            IPlayerService playerService)
        {
            this.mapper = mapper;
            this.db = db;
            this.awardService = awardService;
            this.seasonService = seasonService;
            this.playerService = playerService;
        }

        [Route("[controller]/[action]/{seasonId:int}")]
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

            var awardId = await this.awardService.AddAwardAsync(model.Type, model.Year, model.WinnerName, model.WinnerTeamName);

            var season = await this.seasonService.GetDetailsByYearAsync(model.Year);
            await this.seasonService.AddAwardAsync(season.SeasonId, awardId);

            var player = await this.playerService.GetPlayerDetailsAsync(model.WinnerName);
            await this.playerService.AddAward(player.Id, awardId);

            return RedirectToAction("All");
        }

        [HttpGet]
        [Route("[controller]/[action]/{awardId:int}")]
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

            var oldAwardWinner = await this.playerService.GetPlayerDetailsAsync(model.Winner);
            await this.playerService.RemoveAward(oldAwardWinner.Id, awardId);

            await this.awardService.EditAwardWinnerAsync(model.Winner, awardId);
            var award = await this.awardService.GetAwardDetails(awardId);

            var newAwardWinner = await this.playerService.GetPlayerDetailsAsync(award.Winner);
            await this.playerService.AddAward(newAwardWinner.Id, awardId);

            return RedirectToAction("All");
        }

        [HttpGet]
        [Route("[controller]/[action]/{awardId:int}")]
        public async Task<IActionResult> Delete(int awardId)
        {
            var award = await this.awardService.GetAwardDetails(awardId);

            return this.View(award);
        }

        [HttpPost]
        public async Task<IActionResult> Delete (AwardDetailsInputModel model, int awardId)
        {
            var award = await this.awardService.GetAwardDetails(awardId);

            var season = await this.seasonService.GetDetailsByYearAsync(model.Year);
            await this.seasonService.AddAwardAsync(season.SeasonId, awardId);

            var player = await this.playerService.GetPlayerDetailsAsync(award.Winner);
            await this.playerService.AddAward(player.Id, awardId);

            await this.awardService.DeleteAwardAsync(awardId);

            return RedirectToAction("All");
        }
    }
}