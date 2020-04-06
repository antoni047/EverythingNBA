namespace EverythingNBA.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using Services;
    using Services.Models.Player;
    using Web.Models.Players;

    public class PlayersController : Controller
    {
        private readonly IMapper mapper;
        private readonly IPlayerService playerService;
        private readonly ITeamService teamService;

        public PlayersController(IPlayerService playerService, IMapper mapper, ITeamService teamService)
        {
            this.mapper = mapper;
            this.playerService = playerService;
            this.teamService = teamService;
        }

        public async Task<IActionResult> All()
        {
            var players = await this.playerService.GetAllPlayersAsync();

            return this.View(players);
        }

        [Route("[controller]/[action]/{playerId:int}")]
        public async Task<IActionResult> PlayerDetails(int playerId)
        {
            var playerDetails = await this.playerService.GetPlayerDetailsAsync(playerId);

            return this.View(playerDetails);
        }

        public async Task<IActionResult> PlayerDetails(string playerName)
        {
            var player = await this.playerService.GetPlayerDetailsAsync(playerName);

            return RedirectToAction("PlayerDetails", new { player.Id});
        }

        [Route("[controller]/[action]/{playerId:int}")]
        public async Task<IActionResult> PlayerAccomplishments(int playerId)
        {
            var player = await this.playerService.GetPlayerDetailsAsync(playerId);
            var playerAccomplishments = await this.playerService.GetPlayerAccomplishentsAsync(playerId);

            ViewBag.Name = player.FirstName + " " + player.LastName;
            return this.View(playerAccomplishments);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(PlayerInputModel model)
        {
            var year = this.GetCurrentSeasonYear();

            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var team = await this.teamService.GetTeamDetailsAsync(model.CurrentTeam, year);

            await this.playerService.AddPlayerAsync(model.FirstName, model.LastName, team.Id, model.RookieYear, model.Age, model.Height,
                model.Weight, model.Position, model.IsStarter, model.Image ,model.ShirtNumber, model.InstagramLink, model.TwitterLink);

            return RedirectToAction("All");
        }

        [HttpGet]
        [Route("[controller]/[action]/{playerId:int}")]
        public async Task<IActionResult> Edit(int playerId)
        {
            var playerDetails = await this.playerService.GetPlayerDetailsAsync(playerId);

            var inputModel = mapper.Map<PlayerInputModel>(playerDetails);

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PlayerInputModel inputModel)
        {
            var model = mapper.Map<PlayerDetailsServiceModel>(inputModel);

            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.playerService.EditPlayerAsync(model, inputModel.Id, inputModel.Image);

            return RedirectToAction("PlayerDetails", new { inputModel.Id });
        }

        [HttpGet]
        [Route("[controller]/[action]/{playerId:int}")]
        public async Task<IActionResult> Delete(int playerId)
        {
            var player = await this.playerService.GetPlayerDetailsAsync(playerId);

            return this.View(player);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PlayerDetailsServiceModel model)
        {
            await this.playerService.DeletePlayerAsync(model.Id);

            return RedirectToAction("All");
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