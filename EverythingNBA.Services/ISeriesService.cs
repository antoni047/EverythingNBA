namespace EverythingNBA.Services
{
    using EverythingNBA.Services.Models.Series;

    using System;
    using System.Threading.Tasks;

    public interface ISeriesService
    {
        Task<int> AddSeriesAsync(int team1Id, int team2Id, int team1GameWon, int team2GamesWon, int game1Id, int game2Id, int game3Id, 
            int game4Id, int? game5Id, int? game6Id, int? game7Id);

        Task<bool> DeleteSeriesAsync (int seriesId);

        Task<GetSeriesDetailsServiceModel> GetSeriesAsync(int id);

        Task AddGameAsync(int seriesId, int gameId, int gameNumber);

        Task<string> GetWinnerAsync(int seriesId);

        Task<SeriesOverviewServiceModel> GetServiceOverview(int seriesId, string stage, string stageNumber, string conference);
    }
}
