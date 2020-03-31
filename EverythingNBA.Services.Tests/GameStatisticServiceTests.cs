﻿namespace EverythingNBA.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;
    using EverythingNBA.Services.Implementations;
    using EverythingNBA.Services.Models.GameStatistic;
    using EverythingNBA.Models;

    public class GameStatisticServiceTests
    {
        [Fact]
        public async Task EditGameStatisticShouldSetProperData()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var gameStatisticService = new GameStatisticService(db, mapper);

            var season = Seeding.CreateUnfinishedSeason();
            db.Seasons.Add(season);
            var teamHost = Seeding.CreateTeam();
            var team2 = Seeding.CreateTeam();
            db.Teams.Add(teamHost);
            db.Teams.Add(team2);
            var game = Seeding.CreateFinishedGame(season.Id, teamHost, team2, 120, 100);
            db.Games.Add(game);
            var player = Seeding.CreatePlayer(teamHost.Id);
            db.Players.Add(player);

            var gameStatistic = Seeding.CreateGameStatistic(player.Id, game.Id);
            db.GameStatistics.Add(gameStatistic);
            await db.SaveChangesAsync();

            var model = new PlayerGameStatisticServiceModel
            {
                Id = gameStatistic.Id,
                Points = 5,
                Assists = 5,
                Rebounds = 5,
                Blocks = 5,
                Steals = 5,
                FreeThrowAttempts = 5,
                FreeThrowsMade = 5,
                FieldGoalAttempts = 5,
                FieldGoalsMade = 5,
                ThreeAttempts = 5,
                ThreeMade = 5,
                MinutesPlayed = 5,
                FieldGoalPercentage = 50,
                FreeThrowPercentage = 50,
                IsPlayerStarter = true,
                PlayerName = player.FirstName + " " + player.LastName,
                ThreePercentage = 10,
            };


            await gameStatisticService.EditGameStatisticAsync(model, gameStatistic.Id);


            Assert.Equal(5, gameStatistic.Points);
            Assert.Equal(5, gameStatistic.Assists);
            Assert.Equal(5, gameStatistic.Rebounds);
            Assert.Equal(5, gameStatistic.Steals);
            Assert.Equal(5, gameStatistic.FreeThrowAttempts);
            Assert.Equal(5, gameStatistic.FreeThrowsMade);
            Assert.Equal(5, gameStatistic.FieldGoalAttempts);
            Assert.Equal(5, gameStatistic.FieldGoalsMade);
            Assert.Equal(5, gameStatistic.ThreeAttempts);
            Assert.Equal(5, gameStatistic.ThreeMade);
            Assert.Equal(5, gameStatistic.MinutesPlayed);
        }

        [Fact]
        public async Task GetGameStatisticShouldReturnCorrectGameStat()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var gameStatisticService = new GameStatisticService(db, mapper);

            var season = Seeding.CreateUnfinishedSeason();
            db.Seasons.Add(season);
            var teamHost = Seeding.CreateTeam();
            var team2 = Seeding.CreateTeam();
            db.Teams.Add(teamHost);
            db.Teams.Add(team2);
            var game = Seeding.CreateFinishedGame(season.Id, teamHost, team2, 120, 100);
            db.Games.Add(game);
            var player = Seeding.CreatePlayer(teamHost.Id);
            db.Players.Add(player);

            var gameStatistic = Seeding.CreateGameStatistic(player.Id, game.Id);
            db.GameStatistics.Add(gameStatistic);
            await db.SaveChangesAsync();


            var gameStatDetails = await gameStatisticService.GetGameStatisticsAsync(game.Id, player.FirstName + " " + player.LastName);


            Assert.Equal(gameStatistic.Points, gameStatDetails.Points);
            Assert.Equal(gameStatistic.Assists, gameStatDetails.Assists);
            Assert.Equal(gameStatistic.Rebounds, gameStatDetails.Rebounds);
            Assert.Equal(gameStatistic.Steals, gameStatDetails.Steals);
            Assert.Equal(gameStatistic.Blocks, gameStatDetails.Blocks);
            Assert.Equal(gameStatistic.MinutesPlayed, gameStatDetails.MinutesPlayed);
        }


    }
}