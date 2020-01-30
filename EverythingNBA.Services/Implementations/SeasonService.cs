namespace EverythingNBA.Services.Implementations
{
    using System;
    using System.Threading.Tasks;

    using EverythingNBA.Data;
    using EverythingNBA.Models;

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
    }
}
