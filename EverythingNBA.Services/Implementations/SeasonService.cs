namespace EverythingNBA.Services.Implementations
{
    using System.Linq;
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper;

    using EverythingNBA.Data;
    using EverythingNBA.Models;
    using EverythingNBA.Services.Models.AllStarTeam;
    using EverythingNBA.Services.Models;

    public class SeasonService : ISeasonService
    {
        private readonly EverythingNBADbContext db;
        private readonly IAwardService awardService;
        private readonly IAllStarTeamService ASTeamService;

        public SeasonService(EverythingNBADbContext db, IAwardService awardService, IAllStarTeamService ASTeamService)
        {
            this.db = db;
            this.awardService = awardService;
            this.ASTeamService = ASTeamService;
        }

        public async Task<int> AddAsync(int year, int? titleWinnerId, int? playoffId)
        {
            if (playoffId != null)
            {
                var playoff = await this.db.Playoffs.FindAsync(playoffId);
            }

            if (titleWinnerId != null)
            {
                var titleWinnerTeam = await this.db.Teams.FindAsync(titleWinnerId);
            }

            var seasonObj = new Season
            {
                Year = year,
                TitleWinnerId = titleWinnerId,
                PlayoffId = playoffId
            };

            this.db.Seasons.Add(seasonObj);

            await this.db.SaveChangesAsync();

            return seasonObj.Id;
        }

        public async Task<bool> DeleteAsync(int seasonId)
        {
            var seasonToDelete = await this.db.Seasons.FindAsync(seasonId);

            if (seasonToDelete == null)
            {
                return false;
            }

            this.db.Seasons.Remove(seasonToDelete);
            await this.db.SaveChangesAsync();

            return true;
        }

        public Task<GetSeasonDetailsServiceModel> GetSeasonByIdAsync(int seasonId)
        {
            throw new NotImplementedException();
        }

        public async Task<GetSeasonDetailsServiceModel> GetSeasonByYearAsync(int Year)
        {
            var season = await this.db.Seasons.Where(s => s.Year == Year).FirstOrDefaultAsync();

            var bestSeed = season.SingleSeasonStatistics.OrderByDescending(s => s.Wins).Select(s => s.Team.Name).FirstOrDefault();
            var worstSeed = season.SingleSeasonStatistics.OrderBy(s => s.Wins).Select(s => s.Team.Name).FirstOrDefault();

            var topScorer = await awardService.GetAwardWinnerAsync(season.Id, "Top Scorer");
            var MVP = await awardService.GetAwardWinnerAsync(season.Id, "MVP");
            var DPOTY = await awardService.GetAwardWinnerAsync(season.Id, "DPOTY");
            var sixthMOTY = await awardService.GetAwardWinnerAsync(season.Id, "SixthMOTY");
            var ROTY = await awardService.GetAwardWinnerAsync(season.Id, "ROTY");
            var MIP = await awardService.GetAwardWinnerAsync(season.Id, "MIP");

            var firstAllNBATeam = await ASTeamService.GetAllStarTeamAsync("FirstAllNBA", season.Id);
            var secondAllNBATeam = await ASTeamService.GetAllStarTeamAsync("SecondAllNBA", season.Id);
            var thirdAllNBATeam = await ASTeamService.GetAllStarTeamAsync("ThirdAllNBA", season.Id);
            var allDefensiveTeam = await ASTeamService.GetAllStarTeamAsync("AllDefensive", season.Id);
            var allRookieTeam = await ASTeamService.GetAllStarTeamAsync("AllRookie", season.Id);

            var model = new GetSeasonDetailsServiceModel
            {
                Year = season.Year,
                FirstAllNBATeamId = firstAllNBATeam.Id,
            };

        }
    }
}
