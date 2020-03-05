namespace EverythingNBA.Services.Implementations
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;

    using EverythingNBA.Data;
    using EverythingNBA.Models;
    using EverythingNBA.Services.Models.Playoff;
    using Microsoft.EntityFrameworkCore;

    public class PlayoffService : IPlayoffService
    {
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;
        private readonly ISeriesService seriesService;

        public PlayoffService(EverythingNBADbContext db, IMapper mapper, ISeriesService seriesService)
        {
            this.db = db;
            this.mapper = mapper;
            this.seriesService = seriesService;
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

        public async Task<GetPlayoffServiceModel> GetDetailsBySeasonAsync(int seasonId)
        {
            var season = await this.db.Seasons.Include(s => s.Playoff).Where(s => s.Id == seasonId).FirstOrDefaultAsync();

            if (season.Playoff == null)
            {
                return null;
            }

            var playoff = season.Playoff;

            var model = mapper.Map<GetPlayoffServiceModel>(playoff);

            return model;
        }

        public async Task<GetPlayoffServiceModel> GetDetailsAsync(int playoffId)
        {
            var playoff = await this.db.Playoffs
                .Where(p => p.Id == playoffId)
                .Include(p => p.WesternQuarterFinalFirst)
                .Include(p => p.WesternQuarterFinalSecond)
                .Include(p => p.WesternQuarterFinalThird)
                .Include(p => p.WesternQuarterFinalFourth)
                .Include(p => p.EasternQuarterFinalFirst)
                .Include(p => p.EasternQuarterFinalSecond)
                .Include(p => p.EasternQuarterFinalThird)
                .Include(p => p.EasternQuarterFinalFourth)
                .Include(p => p.WesternSemiFinalFirst)
                .Include(p => p.WesternSemiFinalSecond)
                .Include(p => p.EasternSemiFinalFirst)
                .Include(p => p.EasternSemiFinalSecond)
                .Include(p => p.WesternFinal)
                .Include(p => p.EasternFinal)
                .Include(p => p.Final)
                .FirstOrDefaultAsync();

            var WesternQuarterFinalFirst = await this.seriesService.GetServiceOverview(playoff.WesternQuarterFinalFirst.Id, "QuarterFinal", "First", "Western");
            var WesternQuarterFinalSecond = await this.seriesService.GetServiceOverview(playoff.WesternQuarterFinalFirst.Id, "QuarterFinal", "Second", "Western");
            var WesternQuarterFinalThird = await this.seriesService.GetServiceOverview(playoff.WesternQuarterFinalFirst.Id, "QuarterFinal", "Third", "Western");
            var WesternQuarterFinalFourth = await this.seriesService.GetServiceOverview(playoff.WesternQuarterFinalFirst.Id, "QuarterFinal", "Fourth", "Western");
            var EasternQuarterFinalFirst = await this.seriesService.GetServiceOverview(playoff.WesternQuarterFinalFirst.Id, "QuarterFinal", "First", "Eastern");
            var EasternQuarterFinalSecond = await this.seriesService.GetServiceOverview(playoff.WesternQuarterFinalFirst.Id, "QuarterFinal", "Second", "Eastern");
            var EasternQuarterFinalThird = await this.seriesService.GetServiceOverview(playoff.WesternQuarterFinalFirst.Id, "QuarterFinal", "Third", "Eastern");
            var EasternQuarterFinalFourth = await this.seriesService.GetServiceOverview(playoff.WesternQuarterFinalFirst.Id, "QuarterFinal", "Fourth", "Eastern");
            var WesternSemiFinalFirst = await this.seriesService.GetServiceOverview(playoff.WesternQuarterFinalFirst.Id, "SemiFinal", "First", "Western");
            var WesternSemiFinalSecond = await this.seriesService.GetServiceOverview(playoff.WesternQuarterFinalFirst.Id, "SemiFinal", "Second", "Western");
            var EasternSemiFinalFirst = await this.seriesService.GetServiceOverview(playoff.WesternQuarterFinalFirst.Id, "SemiFinal", "First", "Eastern");
            var EasternSemiFinalSecond = await this.seriesService.GetServiceOverview(playoff.WesternQuarterFinalFirst.Id, "SemiFinal", "Second", "Eastern");
            var WesternFinal = await this.seriesService.GetServiceOverview(playoff.WesternQuarterFinalFirst.Id, "ConferenceFinal", "First", "Western");
            var EasternFinal = await this.seriesService.GetServiceOverview(playoff.WesternQuarterFinalFirst.Id, "ConferenceFinal", "First", "Western");
            var Final = await this.seriesService.GetServiceOverview(playoff.WesternQuarterFinalFirst.Id, "Final", "First", "Western");

            var winnerName = await this.seriesService.GetWinnerAsync(Final.Id);

            var model = new GetPlayoffServiceModel
            {
                WesternQuarterFinalFirst = WesternQuarterFinalFirst,
                WesternQuarterFinalSecond = WesternQuarterFinalSecond,
                WesternQuarterFinalThird = WesternQuarterFinalThird,
                WesternQuarterFinalFourth = WesternQuarterFinalFourth,
                EasternQuarterFinalFirst = EasternQuarterFinalFirst,
                EasternQuarterFinalSecond = EasternQuarterFinalSecond,
                EasternQuarterFinalThird = EasternQuarterFinalThird,
                EasternQuarterFinalFourth = EasternQuarterFinalFourth,
                WesternSemiFinalFirst = WesternSemiFinalFirst,
                WesternSemiFinalSecond = WesternSemiFinalSecond,
                EasternSemiFinalFirst = EasternSemiFinalFirst,
                EasternSemiFinalSecond = EasternSemiFinalSecond,
                WesternFinal = WesternFinal,
                EasternFinal = EasternFinal,
                Final = Final,
                WinnerName = winnerName
            };
        }
    }
}
