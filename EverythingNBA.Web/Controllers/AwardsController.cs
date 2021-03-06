﻿namespace EverythingNBA.Web.Controllers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
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

            ViewBag.Year = year;
            return this.View(awards);
        }

        public async Task<IActionResult> All()
        {
            var awards = await this.awardService.GetAllAwardsAsync();

            return this.View(awards);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AwardDetailsInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var award = await this.awardService.AddAwardAsync(model.Type, model.Year, model.Winner, model.WinnerTeam);

            //Success notification data 
            if (award.Split(" ")[0] == "Success")
            {
                TempData["Message"] = "Award added successfully";
                TempData["Type"] = "Success";
            }
            else
            {
                TempData["Message"] = "Award already exists";
                TempData["Type"] = "Error";
            }

            return RedirectToAction("All");
        }

        [HttpGet]
        [Route("[controller]/[action]/{awardId:int}")]
        [Authorize]
        public async Task<IActionResult> Edit(int awardId)
        {
            var award = await this.awardService.GetAwardDetails(awardId);

            var model = mapper.Map<AwardDetailsInputModel>(award);
            ViewBag.Id = awardId;

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(AwardDetailsInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            //Success notification data 
            TempData["Message"] = "Award added successfully";
            TempData["Type"] = "Success";

            await this.awardService.EditAwardWinnerAsync(model.Winner, model.Id);
            return RedirectToAction("All");
        }

        [HttpGet]
        [Route("[controller]/[action]/{awardId:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int awardId)
        {
            var award = await this.awardService.GetAwardDetails(awardId);

            var model = mapper.Map<AwardDetailsInputModel>(award);
            model.Id = awardId;

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete (AwardDetailsInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.awardService.DeleteAwardAsync(model.Id);

            return RedirectToAction("All");
        }
    }
}