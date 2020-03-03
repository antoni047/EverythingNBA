﻿namespace EverythingNBA.Services
{
    using EverythingNBA.Models;
    using System;
    using System.Threading.Tasks;

    public interface IGameStatisticService
    {
        Task<int> AddAsync(int gameId, int playerId, int minutesPlayed, int points, int assists, int rebounds, int blocks, int steals,
            int freeThrowAttemps, int freeThrowsMade, int threeAttemps, int threeMade, int fieldGoalAttempts, int fieldGoalsMade);

        Task<bool> DeleteAsync(int gameStatisticId);

        Task<int> GetThreePointsPercentage(int gameStatisticId);

        Task<int> GetFieldGoalPercentage(int gameStatisticId);

        Task<int> GetFreeThrowPercentage(int gameStatisticId);

        Task EditGameStatisticAsync(GameStatistic editedStats, int statId);
    }
}
