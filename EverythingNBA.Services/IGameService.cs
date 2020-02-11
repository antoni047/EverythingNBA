namespace EverythingNBA.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using EverythingNBA.Services.Models.Game;
    using EverythingNBA.Services.Models.GameStatistic;

    public interface IGameService
    {
        Task<int> AddGamesAsync(int seasonId, int teamHostId, int team2Id, int winnerPoints, int loserPoints, DateTime date, bool isFinished);

        Task<bool> DeletePlayerAsync(int gameId);

        Task<GameDetailsServiceModel> GetGameAsync(DateTime date);

        Task<GameDetailsServiceModel> GetGameAsync(int gameId);

        Task<ICollection<GameOverviewServiceModel>> GetCurrentSeasonGamesAsync(int seasonId);

        Task<bool> SetScoreAsync(int gameId);

        Task<string> GetWinnerAsync(int gameId);

        Task<PlayerGameStatisticServiceModel> GetTopPointsAsync(int gameId);

        Task<PlayerGameStatisticServiceModel> GetTopAssistsAsync(int gameId);

        Task<PlayerGameStatisticServiceModel> GetTopReboundsAsync(int gameId);

        Task<ICollection<GameOverviewServiceModel>> GetAllGamesBetweenTeamsAsync(int team1Id, int team2Id);

        Task<ICollection<GameOverviewServiceModel>> GetAllGamesBetweenTeamsBySeasonAsync(int team1Id, int team2Id, int seasonId);
    }
}
