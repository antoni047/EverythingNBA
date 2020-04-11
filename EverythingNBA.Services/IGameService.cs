﻿namespace EverythingNBA.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using EverythingNBA.Services.Models.Game;
    using EverythingNBA.Services.Models.GameStatistic;

    public interface IGameService
    {
        Task<string> AddGameAsync(int seasonId, int teamHostId, int team2Id, int teamHostPoints, int team2Points, string date, bool isFinished, bool isPlayoffGame);

        Task<bool> DeleteGameAsync(int gameId);

        Task<ICollection<GameDetailsServiceModel>> GetGamesOnDateAsync(string date);

        Task<GameDetailsServiceModel> GetGameAsync(int gameId);

        Task<ICollection<GameDetailsServiceModel>> GetSeasonGamesAsync(int seasonId);

        Task<bool> SetScoreAsync(int gameId, int teamHostScore, int team2Score);

        Task<string> GetWinnerAsync(int gameId);

        Task<PlayerTopStatisticServiceModel> GetTopPointsAsync(int gameId);

        Task<PlayerTopStatisticServiceModel> GetTopAssistsAsync(int gameId);

        Task<PlayerTopStatisticServiceModel> GetTopReboundsAsync(int gameId);

        Task<ICollection<GameDetailsServiceModel>> GetAllGamesBetweenTeamsAsync(string team1Name, string team2Name);

        Task<ICollection<GameDetailsServiceModel>> GetAllGamesBetweenTeamsBySeasonAsync(string team1Name, string team2Name, int seasonId);

        Task<GameDetailsServiceModel> GetGameOverview(int gameId);

        Task EditGameAsync(GameDetailsServiceModel model, int gameId);

        Task<GameListingServiceModel> GetFixturesAsync(int seasonId, int page = 1);

        Task<GameListingServiceModel> GetResultsAsync(int seasonId, int page = 1);
    }
}
