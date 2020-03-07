﻿namespace EverythingNBA.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using EverythingNBA.Services.Models.Game;
    using EverythingNBA.Services.Models.GameStatistic;

    public interface IGameService
    {
        Task<int> AddGameAsync(int seasonId, int teamHostId, int team2Id, int teamHostPoints, int team2Points, string date, bool isFinished);

        Task<bool> DeleteGameAsync(int gameId);

        Task<ICollection<GameDetailsServiceModel>> GetGamesOnDateAsync(string date);

        Task<GameDetailsServiceModel> GetGameAsync(int gameId);

        Task<ICollection<GameOverviewServiceModel>> GetSeasonGamesAsync(int seasonId);

        Task<bool> SetScoreAsync(int gameId, int teamHostScore, int team2Score);

        Task<string> GetWinnerAsync(int gameId);

        Task<PlayerGameStatisticServiceModel> GetTopPointsAsync(int gameId);

        Task<PlayerGameStatisticServiceModel> GetTopAssistsAsync(int gameId);

        Task<PlayerGameStatisticServiceModel> GetTopReboundsAsync(int gameId);

        Task<ICollection<GameOverviewServiceModel>> GetAllGamesBetweenTeamsAsync(string team1Name, string team2Name);

        Task<ICollection<GameOverviewServiceModel>> GetAllGamesBetweenTeamsBySeasonAsync(string team1Name, string team2Name, int seasonId);

        Task<GameOverviewServiceModel> GetGameOverview(int gameId);

        Task EditGameAsync(GameDetailsServiceModel model, int gameId);
    }
}
