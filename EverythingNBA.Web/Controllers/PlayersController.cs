﻿namespace EverythingNBA.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using Services;
    using Services.Models.Player;
    using Web.Models.Players;
    using Data;

    public class PlayersController : Controller
    {
        private readonly IMapper mapper;
        private readonly EverythingNBADbContext db;
        private readonly IPlayerService playerService;
        private readonly ITeamService teamService;

        public PlayersController(IPlayerService playerService, IMapper mapper, EverythingNBADbContext db, ITeamService teamService)
        {
            this.db = db;
            this.mapper = mapper;
            this.playerService = playerService;
            this.teamService = teamService;
        }

        public async Task<IActionResult> All()
        {
            var players = await this.playerService.GetAllPlayersAsync();

            return this.View(players);
        }

        public async Task<IActionResult> PlayerDetails(int playerId)
        {
            var playerDetails = await this.playerService.GetPlayerDetailsAsync(playerId);

            return this.View(playerDetails);
        }

        public async Task<IActionResult> PlayerAccomplishments(int playerId)
        {
            var playerAccomplishments = await this.playerService.GetPlayerAccomplishentsAsync(playerId);

            return this.View(playerAccomplishments);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(PlayerInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var team = await this.teamService.GetTeamDetailsAsync(model.Team);

            await this.playerService.AddPlayerAsync(model.FirstName, model.LastName, team.Id, model.RookieYear, model.Age, model.Height, model.Weight,
                model.Position, model.IsStarter, model.ShirtNumber, model.InstagramLink, model.TwitterLink);

            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int playerId)
        {
            var playerDetails = await this.playerService.GetPlayerDetailsAsync(playerId);



            return this.View(playerDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PlayerInputModel inputModel, int playerId)
        {
            var model = mapper.Map<PlayerDetailsServiceModel>(inputModel);

            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.playerService.EditPlayerAsync(model, playerId);

            return RedirectToAction("PlayerDetails");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int playerId)
        {
            var player = await this.playerService.GetPlayerDetailsAsync(playerId);

            return this.View(player);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PlayerDetailsServiceModel model, int playerId)
        {
            await this.playerService.DeletePlayerAsync(playerId);

            return RedirectToAction("All");
        }
    }
}