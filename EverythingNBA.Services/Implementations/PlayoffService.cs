namespace EverythingNBA.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Services.Models.Playoff;
    using Services.Models.Series;
    using EverythingNBA.Models;

    public class PlayoffService : IPlayoffService
    {
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;
        private readonly ISeriesService seriesService;
        private readonly ITeamService teamService;

        public PlayoffService(EverythingNBADbContext db, IMapper mapper, ISeriesService seriesService, ITeamService teamService)
        {
            this.db = db;
            this.mapper = mapper;
            this.seriesService = seriesService;
            this.teamService = teamService;
        }

        public async Task<int> AddPlayoffAsync(int? seasonId)
        {
            var playoffObj = new Playoff()
            {
                SeasonId = seasonId
            };

            this.db.Playoffs.Add(playoffObj);

            await this.db.SaveChangesAsync();

            return playoffObj.Id;
        }

        public async Task AddSeriesAsync(int playoffId, int seriesId)
        {
            var playoff = await this.db.Playoffs.FindAsync(playoffId);
            var series = await this.db.Series.FindAsync(seriesId);

            if (series != null)
            {
                playoff.Series.Add(series);
                await this.db.SaveChangesAsync();
            }
        }

        public async Task RemoveSeriesAsync(int playoffId, int seriesId)
        {
            var playoff = await this.db.Playoffs.FindAsync(playoffId);
            var series = await this.db.Series.FindAsync(seriesId);

            if (series != null)
            {
                playoff.Series.Remove(series);
                await this.db.SaveChangesAsync();
            }
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

        public async Task<GetPlayoffServiceModel> GetDetailsBySeasonAsync(int year)
        {
            var playoffId = await this.db.Seasons.Where(s => s.Year == year).Select(s => s.PlayoffId).FirstOrDefaultAsync();

            if (playoffId == null)
            {
                return null;
            }

            return await this.GetDetailsAsync((int)playoffId);
        }

        public async Task<GetPlayoffServiceModel> GetDetailsAsync(int playoffId)
        {
            var playoff = await this.db.Playoffs
                .Where(p => p.Id == playoffId)
                .Include(p => p.Series)
                    .ThenInclude(s => s.Team1)
                 .Include(p => p.Series)
                    .ThenInclude(s => s.Team2)
                .FirstOrDefaultAsync();

            if (playoff == null)
            {
                return null;
            }

            var finalSeries = playoff.Series.Where(s => s.Stage == "Final").FirstOrDefault();
            var winnerName = finalSeries != null ? await this.seriesService.GetWinnerAsync(finalSeries.Id) : "";

            var seriesModels = new List<SeriesOverviewServiceModel>();

            foreach (var serie in playoff.Series)
            {
                var serieModel = mapper.Map<SeriesOverviewServiceModel>(serie);

                if (serie.Team1.Name.Split(" ").Length == 3)
                {
                    serieModel.Team1Name = serie.Team1.Name.Split(" ")[0] + " " + serie.Team1.Name.Split(" ")[1];
                    if (serieModel.Team1Name == "Los Angeles")
                    {
                        serieModel.Team1Name = "LA" + " " + serie.Team1.Name.Split(" ")[2];
                    }
                }
                else
                {
                    serieModel.Team1Name = serie.Team1.Name.Split(" ")[0];
                }

                if (serie.Team2.Name.Split(" ").Length == 3)
                {
                    serieModel.Team2Name = serie.Team2.Name.Split(" ")[0] + " " + serie.Team2.Name.Split(" ")[1];
                    if (serieModel.Team2Name == "Los Angeles")
                    {
                        serieModel.Team2Name = "LA" + " " + serie.Team2.Name.Split(" ")[2];
                    }
                }
                else
                {
                    serieModel.Team2Name = serie.Team2.Name.Split(" ")[0];
                }

                seriesModels.Add(serieModel);
            }

            var model = new GetPlayoffServiceModel
            {
                Id = playoff.Id,
                SeasonId = playoff.SeasonId,
                Series = seriesModels,
                WinnerName = winnerName,
                AreConferenceFinalsFinished = playoff.AreConferenceFinalsFinished,
                AreQuarterFinalsFinished = playoff.AreQuarterFinalsFinished,
                AreSemiFinalsFinished = playoff.AreSemiFinalsFinished
            };

            return model;
        }

        public async Task SetStartingSeries(int playoffId)
        {
            var playoff = await this.db.Playoffs.Include(p => p.Series).Where(p => p.Id == playoffId).FirstOrDefaultAsync();
            var playoffModel = await this.GetDetailsAsync(playoffId);

            var seasonId = (int)playoff.SeasonId;

            var standings = await this.teamService.GetStandingsAsync(seasonId);
            var easternTop8 = standings.EasternStandings.Take(8).ToList();
            var westernTop8 = standings.WesternStandings.Take(8).ToList();

            var eQuarterFirstId = await this.seriesService.AddSeriesAsync(playoffId, easternTop8[0].Name, easternTop8[7].Name, 0, 0,
                null, null, null, null, null, null, null, "Eastern", "QuarterFinal", 1);
            var eQuarterSecondId = await this.seriesService.AddSeriesAsync(playoffId, easternTop8[3].Name, easternTop8[4].Name, 0, 0,
                null, null, null, null, null, null, null, "Eastern", "QuarterFinal", 2);
            var eQuarterThirdId = await this.seriesService.AddSeriesAsync(playoffId, easternTop8[2].Name, easternTop8[5].Name, 0, 0,
                null, null, null, null, null, null, null, "Eastern", "QuarterFinal", 3);
            var eQuarterFourthId = await this.seriesService.AddSeriesAsync(playoffId, easternTop8[1].Name, easternTop8[6].Name, 0, 0,
                null, null, null, null, null, null, null, "Eastern", "QuarterFinal", 4);

            var wQuarterFirstId = await this.seriesService.AddSeriesAsync(playoffId, westernTop8[0].Name, westernTop8[7].Name, 0, 0,
                null, null, null, null, null, null, null, "Western", "QuarterFinal", 1);
            var wQuarterSecondId = await this.seriesService.AddSeriesAsync(playoffId, westernTop8[3].Name, westernTop8[4].Name, 0, 0,
                null, null, null, null, null, null, null, "Western", "QuarterFinal", 2);
            var wQuarterThirdId = await this.seriesService.AddSeriesAsync(playoffId, westernTop8[2].Name, westernTop8[5].Name, 0, 0,
                null, null, null, null, null, null, null, "Western", "QuarterFinal", 3);
            var wQuarterFourthId = await this.seriesService.AddSeriesAsync(playoffId, westernTop8[1].Name, westernTop8[6].Name, 0, 0,
                null, null, null, null, null, null, null, "Western", "QuarterFinal", 4);

            await this.AddSeriesAsync(playoffId, eQuarterFirstId);
            await this.AddSeriesAsync(playoffId, eQuarterSecondId);
            await this.AddSeriesAsync(playoffId, eQuarterThirdId);
            await this.AddSeriesAsync(playoffId, eQuarterFourthId);
            await this.AddSeriesAsync(playoffId, wQuarterFirstId);
            await this.AddSeriesAsync(playoffId, wQuarterSecondId);
            await this.AddSeriesAsync(playoffId, wQuarterThirdId);
            await this.AddSeriesAsync(playoffId, wQuarterFourthId);


        }

        public async Task FinishQuarterFinals(int playoffId)
        {
            var playoffFromDB = await this.db.Playoffs.FindAsync(playoffId);
            playoffFromDB.AreQuarterFinalsFinished = true;
            await this.db.SaveChangesAsync();

            var playoff = await this.GetDetailsAsync(playoffId);

            var easternSemiFinalFirstTeam1 = await this.seriesService.GetWinnerAsync(playoff.Series[0].Id);
            var easternSemiFinalFirstTeam2 = await this.seriesService.GetWinnerAsync(playoff.Series[1].Id);

            var easternSemiFinalFirst = await this.seriesService.AddSeriesAsync(playoffId, easternSemiFinalFirstTeam1, 
                easternSemiFinalFirstTeam2, 0, 0, null, null, null, null, null, null, null, "Eastern", "SemiFinal", 1);
            await this.AddSeriesAsync(playoffId, easternSemiFinalFirst);

            var easternSemiFinalSecondTeam1 = await this.seriesService.GetWinnerAsync(playoff.Series[2].Id);
            var easternSemiFinalSecondTeam2 = await this.seriesService.GetWinnerAsync(playoff.Series[3].Id);

            var easternSemiFinalSecond = await this.seriesService.AddSeriesAsync(playoffId, easternSemiFinalSecondTeam1,
                easternSemiFinalSecondTeam2, 0, 0, null, null, null, null, null, null, null, "Eastern", "SemiFinal", 2);
            await this.AddSeriesAsync(playoffId, easternSemiFinalSecond);

            var westernSemiFinalFirstTeam1 = await this.seriesService.GetWinnerAsync(playoff.Series[4].Id);
            var westernSemiFinalFirstTeam2 = await this.seriesService.GetWinnerAsync(playoff.Series[5].Id);

            var westerSemiFinalFirst = await this.seriesService.AddSeriesAsync(playoffId, westernSemiFinalFirstTeam1,
                westernSemiFinalFirstTeam2, 0, 0, null, null, null, null, null, null, null, "Western", "SemiFinal", 1);
            await this.AddSeriesAsync(playoffId, westerSemiFinalFirst);

            var westernSemiFinalSecondTeam1 = await this.seriesService.GetWinnerAsync(playoff.Series[6].Id);
            var westernSemiFinalSecondTeam2 = await this.seriesService.GetWinnerAsync(playoff.Series[7].Id);
                
            var westernSemiFinalSecond = await this.seriesService.AddSeriesAsync(playoffId, westernSemiFinalSecondTeam1,
                westernSemiFinalSecondTeam2, 0, 0, null, null, null, null, null, null, null, "Western", "SemiFinal", 2);
            await this.AddSeriesAsync(playoffId, westernSemiFinalSecond);
        }

        public async Task FinishSemiFinals(int playoffId)
        {
            var playoffFromDB = await this.db.Playoffs.FindAsync(playoffId);
            playoffFromDB.AreSemiFinalsFinished = true;
            await this.db.SaveChangesAsync();

            var playoff = await this.GetDetailsAsync(playoffId);
            

            var westernFinalFirstTeam1 = await this.seriesService.GetWinnerAsync(playoff.Series[8].Id);
            var westernFinalFirstTeam2 = await this.seriesService.GetWinnerAsync(playoff.Series[9].Id);

            var westernFinalFirst = await this.seriesService.AddSeriesAsync(playoffId, westernFinalFirstTeam1,
                westernFinalFirstTeam2, 0, 0, null, null, null, null, null, null, null, "Western", "ConferenceFinal", 1);
            await this.AddSeriesAsync(playoffId, westernFinalFirst);

            var easternFinalFirstTeam1 = await this.seriesService.GetWinnerAsync(playoff.Series[10].Id);
            var easternFinalFirstTeam2 = await this.seriesService.GetWinnerAsync(playoff.Series[11].Id);
                
            var easternFinalFirst = await this.seriesService.AddSeriesAsync(playoffId, easternFinalFirstTeam1,
                easternFinalFirstTeam2, 0, 0, null, null, null, null, null, null, null, "Western", "ConferenceFinal", 1);
            await this.AddSeriesAsync(playoffId, easternFinalFirst);
        }

        public async Task FinishConferenceFinals(int playoffId)
        {
            var playoffFromDB = await this.db.Playoffs.FindAsync(playoffId);
            playoffFromDB.AreConferenceFinalsFinished = true;
            await this.db.SaveChangesAsync();

            var playoff = await this.GetDetailsAsync(playoffId);
            

            var finalTeam1 = await this.seriesService.GetWinnerAsync(playoff.Series[12].Id);
            var firstTeam2 = await this.seriesService.GetWinnerAsync(playoff.Series[13].Id);

            var westernFinalFirst = await this.seriesService.AddSeriesAsync(playoffId, finalTeam1,firstTeam2, 0, 0, null, null, 
                null, null, null, null, null, "Western", "Final", 1);
            await this.AddSeriesAsync(playoffId, westernFinalFirst);
        }
    }
}
