namespace EverythingNBA.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using Services;
    using Services.Models;
    using Web.Models;
    using Data;

    public class AllStarTeamsController : Controller
    {
        private readonly IMapper mapper;
        private readonly EverythingNBADbContext db;
        private readonly IAllStarTeamService astService;
        private readonly ISeasonService seasonService;

        public AllStarTeamsController(IMapper mapper, EverythingNBADbContext db, IAllStarTeamService allStarTeamService, ISeasonService seasonService)
        {
            this.mapper = mapper;
            this.db = db;
            this.astService = allStarTeamService;
            this.seasonService = seasonService;
        }

        public async Task<IActionResult> SeasonAllStarTeams(int seasonId)
        {
            var year = await this.seasonService.GetYearAsync(seasonId);

            var astTeams = await this.astService.GetAllASTeamsAsync(year);

            return this.View(astTeams);
        }

        public async Task<IActionResult> AllStarTeamType(string type)
        {
            var astTeams = await this.astService.GetAllASTeamsAsync(type);

            return this.View(astTeams);
        }
    }
}