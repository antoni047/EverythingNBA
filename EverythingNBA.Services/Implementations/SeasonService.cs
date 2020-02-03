namespace EverythingNBA.Services.Implementations
{
    using System.Linq;
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper;

    using EverythingNBA.Data;
    using EverythingNBA.Models;
    using EverythingNBA.Services.Models;

    public class SeasonService : ISeasonService
    {
        private readonly EverythingNBADbContext db;

        public SeasonService(EverythingNBADbContext db)
        {
            this.db = db;
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

            //var topScorer = GetTopScorerAsync(season.Id)
            //var MVP = GetMVPAsync(season.Id)
            //var DPOTY = GetDPOTYAsync(season.Id)
            //var sixthMOTY = GetSixthMOTYAsync(season.Id)
            //var ROTY = GetROTYAsync(season.Id)
            //var MIP = GetMIPAsync(season.Id)

            //var firstAllNBATeam = GetFirstAllNBATeamAsync(season.Id);
            //var secondAllNBATeam = GetSecondAllNBATeamAsync(season.Id);
            //var thirdAllNBATeam = GetThirdAllNBATeamAsync(season.Id);
            //var AllDefensiveTeam = GetAllDefensiveTeamAsync(season.Id);
            //var AllrookieTeam = GetAllRookieTeamAsync(season.Id);

            var model = new GetSeasonDetailsServiceModel
            {

            };

        }
    }
}
