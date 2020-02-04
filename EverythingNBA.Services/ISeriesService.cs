namespace EverythingNBA.Services
{
    using System;
    using System.Threading.Tasks;

    public interface ISeriesService
    {
        Task<int> AddSeriesAsync(int team1Id, int team2Id, int team1GameWon, int team2GamesWon, int game1Id, int game2Id, int game3Id, 
            int game4Id, int? game5Id, int? game6Id, int? game7Id);

        Task<bool> DeleteSeriesAsync (int seriesId);
    }
}
