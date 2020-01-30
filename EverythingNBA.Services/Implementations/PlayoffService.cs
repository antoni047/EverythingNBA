namespace EverythingNBA.Services.Implementations
{
    using System;
    using System.Threading.Tasks;

    using EverythingNBA.Data;
    using EverythingNBA.Models;
    public class PlayoffService : IPlayoffService
    {
        private readonly EverythingNBADbContext db;

        public PlayoffService(EverythingNBADbContext db)
        {
            this.db = db;
        }

        public async Task<int> AddPlayoffAsync(int? seasonId, int westernQuarterFinalFirstId, int westernQuarterFinalSecondId, int westernQuarterFinalThirdId,
            int westernQuarterFinalFourthId, int westernSemiFinalFirstId, int westernSemiFinalSecondId, int westernFinalId, int easternQuarterFinalFirstId,
            int easternQuarterFinalSecondId, int easternQuarterFinalThirdId, int easternQuarterFinalFourthId, int easternSemiFinalFirstId,
            int easternSemiFinalSecondId, int easternFinalId, int finalId)
        {
            var playoffObj = new Playoff()
            {
                SeasonId = seasonId,
                WesternQuarterFinalFirstId = westernQuarterFinalFirstId,
                WesternQuarterFinalSecondId = westernQuarterFinalSecondId,
                WesternQuarterFinalThirdId = westernQuarterFinalThirdId,
                WesternQuarterFinalFourthId = westernQuarterFinalFourthId,
                WesternSemiFinalFirstId = westernSemiFinalFirstId,
                WesternSemiFinalSecondId = westernSemiFinalSecondId,
                WesternFinalId = westernFinalId,
                EasternQuarterFinalFirstId = easternQuarterFinalFirstId,
                EasternQuarterFinalSecondId = easternQuarterFinalSecondId,
                EasternQuarterFinalThirdId = easternQuarterFinalThirdId,
                EasternQuarterFinalFourthId = easternQuarterFinalFourthId,
                EasternSemiFinalFirstId = easternSemiFinalFirstId,
                EasternSemiFinalSecondId = easternSemiFinalSecondId,
                EasternFinalId = easternFinalId,
                FinalId = finalId
            };

            this.db.Playoffs.Add(playoffObj);

            await this.db.SaveChangesAsync();

            return playoffObj.Id;
        }

        public async Task<bool> DeletePlayoffAsync(int playoffId)
        {
            var playoffToDelete = await this.db.Playoffs.FindAsync(playoffId);

            if (playoffToDelete == null)
            {
                return false;
            }

            this.db.Playoffs.Remove(playoffToDelete);
            await this.db.SaveChangesAsync();

            return true;
        }
    }
}
