namespace EverythingNBA.Services.Implementations
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;

    using EverythingNBA.Data;
    using EverythingNBA.Models;
    using EverythingNBA.Services.Models.Playoff;

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

        public async Task AddSeriesAsync(int playoffId, int seriesId, string conference, string stage, string seriesNumber)
        {
            var playoff = await this.db.Playoffs.FindAsync(playoffId);

            var seriesType = conference + stage + seriesNumber;

            switch (seriesType)
            {
                case "westernQuarterFirst":
                    playoff.WesternQuarterFinalFirstId = seriesId;
                    break;
                case "westernQuarterSecond":
                    playoff.WesternQuarterFinalSecondId = seriesId;
                    break;
                case "westernQuarterThird":
                    playoff.WesternQuarterFinalThirdId = seriesId;
                    break;
                case "westernQuarterFourth":
                    playoff.WesternQuarterFinalFourthId = seriesId;
                    break;
                case "easternQuarterFirst":
                    playoff.EasternQuarterFinalFirstId = seriesId;
                    break;
                case "easternQuarterSecond":
                    playoff.EasternQuarterFinalSecondId = seriesId;
                    break;
                case "easternQuarterThird":
                    playoff.EasternQuarterFinalThirdId = seriesId;
                    break;
                case "easternQuarterFourth":
                    playoff.EasternQuarterFinalFourthId = seriesId;
                    break;
                case "westernSemiFirst":
                    playoff.WesternSemiFinalFirstId = seriesId;
                    break;
                case "westernSemiSecond":
                    playoff.WesternSemiFinalSecondId = seriesId;
                    break;
                case "easternSemiFirst":
                    playoff.EasternSemiFinalFirstId = seriesId;
                    break;
                case "easternSemiSecond":
                    playoff.EasternSemiFinalSecondId = seriesId;
                    break;
                case "westernFinal":
                    playoff.WesternFinalId = seriesId;
                    break;
                case "easternFinal":
                    playoff.EasternFinalId = seriesId;
                    break;
                case "Final":
                    playoff.FinalId = seriesId;
                    break;
            } //sets seriesId

            await this.db.SaveChangesAsync();
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

        public async Task<GetPlayoffServiceModel> GetPlayoffBySeasonAsync(int seasonId)
        {
            var season = await this.db.Seasons.FindAsync(seasonId);

            if (season.PlayoffId == null)
            {
                return null;
            }

            var playoff = season.Playoff;

            var model = Mapper.Map<GetPlayoffServiceModel>(playoff);

            return model;
        }

        public async Task<GetPlayoffServiceModel> GetPlayoffDetailsAsync(int playoffId)
        {
            var playoff = await this.db.Playoffs.FindAsync();

            var model = Mapper.Map<GetPlayoffServiceModel>(playoff);

            return model;
        }
    }
}
