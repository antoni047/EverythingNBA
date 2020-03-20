﻿namespace EverythingNBA.Web.Controllers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    using Services;
    using Web.Models.Awards;

    public class AwardsController : Controller
    {
        private readonly IAwardService awardService;
        private readonly ISeasonService seasonService;
        private readonly IPlayerService playerService;
        private readonly IMapper mapper;

        public AwardsController(IAwardService awardService, ISeasonService seasonService, IPlayerService playerService, IMapper mapper)
        {
            this.awardService = awardService;
            this.seasonService = seasonService;
            this.playerService = playerService;
            this.mapper = mapper;
        }

        [Route("[controller]/[action]/{year:int}")]
        public async Task<IActionResult> SeasonAwards(int year)
        {
            var awards = await this.awardService.GetSeasonAwardsAsync(year);

            return this.View(awards);
        }

        public async Task<IActionResult> All()
        {
            var awards = await this.awardService.GetAllAwardsAsync();

            return this.View(awards);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AwardDetailsInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var awardId = await this.awardService.AddAwardAsync(model.Type, model.Year, model.Winner, model.WinnerTeam);

            var season = await this.seasonService.GetDetailsByYearAsync(model.Year);
            await this.seasonService.AddAwardAsync(season.SeasonId, awardId);

            var player = await this.playerService.GetPlayerDetailsAsync(model.Winner);
            await this.playerService.AddAward(player.Id, awardId);

            return RedirectToAction("All");
        }

        [HttpGet]
        [Route("[controller]/[action]/{awardId:int}")]
        public async Task<IActionResult> Edit(int awardId)
        {
            var award = await this.awardService.GetAwardDetails(awardId);

            var model = mapper.Map<AwardDetailsInputModel>(award);
            ViewBag.Id = awardId;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AwardDetailsInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var oldAwardWinner = await this.playerService.GetPlayerDetailsAsync(model.Winner);
            await this.playerService.RemoveAward(oldAwardWinner.Id, model.Id);

            await this.awardService.EditAwardWinnerAsync(model.Winner, model.Id);
            var award = await this.awardService.GetAwardDetails(model.Id);

            var newAwardWinner = await this.playerService.GetPlayerDetailsAsync(award.Winner);
            await this.playerService.AddAward(newAwardWinner.Id, model.Id);

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