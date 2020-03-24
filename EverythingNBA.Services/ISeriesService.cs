namespace EverythingNBA.Services
{
    using EverythingNBA.Services.Models.Series;

    using System.Threading.Tasks;

    public interface ISeriesService
    {
        Task<int> AddSeriesAsync(int playoffId, string team1Name, string team2Name, int team1GameWon, int team2GamesWon, int? game1Id, int? game2Id,
             int? game3Id, int? game4Id, int? game5Id, int? game6Id, int? game7Id, string conference, string stage, int stageNumber,
             int? team1Position, int? team2Position);

        Task<bool> DeleteSeriesAsync (int seriesId);

        Task<GetSeriesDetailsServiceModel> GetSeriesAsync(int id);

        Task AddGameAsync(int seriesId, int gameId, int gameNumber);

        Task<SeriesOverviewServiceModel> GetSeriesOverview(int seriesId);

        Task SetGameWon(int seriesId, string winner);

        Task<SeriesWinnerServiceModel> GetWinnerAsync(int seriesId);
    }
}
