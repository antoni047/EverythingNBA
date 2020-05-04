namespace EverythingNBA.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;
    using System.Linq;

    using Services;
    using Services.Models.Player;
    using Web.Models.Players;
    using Microsoft.AspNetCore.Authorization;

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

        public async Task<IActionResult> All(int page = 1)
        {
            var players = await this.playerService.GetAllPlayersAsync(page);

            var totalPlayers = await this.playerService.TotalPlayers();

            var model = new AllPlayersViewModel
            {
                PlayerNames = players,
                Total = totalPlayers,
                CurrentPage = page,
            };

            return this.View(model);
        }

        [Route("[controller]/[action]/{playerId:int}")]
        public async Task<IActionResult> PlayerDetails(int playerId)
        {
            var playerDetails = await this.playerService.GetPlayerDetailsAsync(playerId);

            return this.View(playerDetails);
        }

        [Route("[controller]/[action]/{playerName}")]
        public async Task<IActionResult> PlayerDetails(string playerName)
        {
            var player = await this.playerService.GetPlayerDetailsAsync(playerName);

            return this.View(player);
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
        [Authorize]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(PlayerInputModel model)
        {
            var year = this.GetCurrentSeasonYear();

            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var team = await this.teamService.GetTeamDetailsAsync(model.CurrentTeam, year);

            var player = await this.playerService.AddPlayerAsync(model.FirstName, model.LastName, team.Id, model.RookieYear, model.Age, model.Height,
                model.Weight, model.Position, model.IsStarter, model.Image ,model.ShirtNumber, model.InstagramLink, model.TwitterLink);

            //Success notification data 
            if (player.Split(" ")[0] == "Success")
            {
                TempData["Message"] = "Player added successfully";
                TempData["Type"] = "Success";
            }
            else
            {
                TempData["Message"] = "Player not added";
                TempData["Type"] = "Success";
            }

            return RedirectToAction("All");
        }

        [HttpGet]
        [Authorize]
        [Route("[controller]/[action]/{playerId:int}")]
        public async Task<IActionResult> Edit(int playerId)
        {
            var playerDetails = await this.playerService.GetPlayerDetailsAsync(playerId);

            var inputModel = mapper.Map<PlayerInputModel>(playerDetails);

            return this.View(inputModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(PlayerInputModel inputModel)
        {
            var model = mapper.Map<PlayerDetailsServiceModel>(inputModel);

            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.playerService.EditPlayerAsync(model, inputModel.Id, inputModel.Image);

            TempData["Message"] = "Player edited successfully";
            TempData["Type"] = "Success";

            return RedirectToAction("PlayerDetails", new { inputModel.Id });
        }

        [HttpGet]
        [Authorize]
        [Route("[controller]/[action]/{playerId:int}")]
        public async Task<IActionResult> Delete(int playerId)
        {
            var player = await this.playerService.GetPlayerDetailsAsync(playerId);

            return this.View(player);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(PlayerDetailsServiceModel model)
        {
            var deleted = await this.playerService.DeletePlayerAsync(model.Id);

            if (deleted == true)
            {
                TempData["Message"] = "Player deleted successfully";
                TempData["Type"] = "Success";
            }
            else
            {
                TempData["Message"] = "Player not deleted";
                TempData["Type"] = "Error";
            }
            

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