﻿
namespace EverythingNBA.Services.Implementations
{
    using EverythingNBA.Data;
    using EverythingNBA.Models;

    using System;
    using System.Threading.Tasks;
    using AutoMapper;

    public class GameStatisticService : IGameStatisticService
    {
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;

        public GameStatisticService(EverythingNBADbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<int> AddAsync(int gameId, int playerId, int minutesPlayed, int points, int assists, int rebounds, int blocks, int steals, 
            int freeThrowAttemps, int freeThrowsMade, int threeAttemps, int threeMade, int fieldGoalAttempts, int fieldGoalsMade)
        {
            var statObj = new GameStatistic
            {
                GameId = gameId,
                PlayerId = playerId,
                MinutesPlayed = minutesPlayed,
                Points = points,
                Rebounds = rebounds,
                Assists = assists,
                Blocks = blocks,
                Steals = steals,
                FreeThrowAttempts = freeThrowAttemps,
                FreeThrowsMade = freeThrowsMade,
                ThreeAttempts = threeAttemps,
                ThreeMade = threeMade,
                FieldGoalAttempts = fieldGoalAttempts,
                FieldGoalsMade = fieldGoalsMade
            };

            this.db.GameStatistics.Add(statObj);
            await this.db.SaveChangesAsync();

            return statObj.Id;
        }

        public async Task<bool> DeleteAsync(int gameStatisticId)
        {
            var statToDelete = await this.db.GameStatistics.FindAsync(gameStatisticId);

            if (statToDelete == null)
            {
                return false;
            }

            this.db.GameStatistics.Remove(statToDelete);
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task EditGameStatisticAsync(GameStatistic editedStats, int statId)
        {
            var stats = await this.db.GameStatistics.FindAsync(statId);

            stats.MinutesPlayed = editedStats.MinutesPlayed;
            stats.Points = editedStats.Points;
            stats.Rebounds = editedStats.Rebounds;
            stats.Assists = editedStats.Assists;
            stats.Steals = editedStats.Steals;
            stats.Blocks = editedStats.Blocks;
            stats.FieldGoalsMade = editedStats.FieldGoalsMade;
            stats.FieldGoalAttempts = editedStats.FieldGoalAttempts;
            stats.FreeThrowAttempts = editedStats.FreeThrowAttempts;
            stats.FreeThrowsMade = editedStats.FreeThrowsMade;
            stats.ThreeAttempts = editedStats.ThreeAttempts;
            stats.ThreeMade = editedStats.ThreeMade;

            await this.db.SaveChangesAsync();
        }

        public async Task<int> GetFieldGoalPercentage(int gameStatisticId)
        {
            var gameStatistic = await this.db.GameStatistics.FindAsync(gameStatisticId);

            var result = ((double)gameStatistic.FieldGoalsMade / (double)gameStatistic.FieldGoalAttempts)*100;

            return (int)result;
        }

        public async Task<int> GetFreeThrowPercentage(int gameStatisticId)
        {
            var gameStatistic = await this.db.GameStatistics.FindAsync(gameStatisticId);

            var result = ((double)gameStatistic.FreeThrowsMade / (double)gameStatistic.FreeThrowAttempts) * 100;

            return (int)result;
        }

        public async Task<int> GetThreePointsPercentage(int gameStatisticId)
        {
            var gameStatistic = await this.db.GameStatistics.FindAsync(gameStatisticId);

            var result = ((double)gameStatistic.ThreeMade / (double)gameStatistic.ThreeAttempts) * 100;

            return (int)result;
        }
    }
}
