namespace EverythingNBA.Services.Tests
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Xunit;

    using EverythingNBA.Services.Implementations;
    using EverythingNBA.Services.Models.Game;

    public class GameServiceTests
    {
        [Fact]
        public async Task GetWinnerShouldGetTeamWithMostPoints()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var gameService = new GameService(db, mapper);

            var season = Seeding.CreateUnfinishedSeason();
            db.Seasons.Add(season);
            var teamHost = Seeding.CreateTeam();
            var team2 = Seeding.CreateTeam();
            db.Teams.Add(teamHost);
            db.Teams.Add(team2);
            var game = Seeding.CreateFinishedGame(season.Id, teamHost, team2, 120, 100);
            db.Games.Add(game);
            await db.SaveChangesAsync();

            var winner = await gameService.GetWinnerAsync(game.Id);


            Assert.Equal(teamHost.Name, winner.ToString());
        }

        [Fact]
        public async Task GetGamesBetweenTeamsShouldReturnCorrectCountOfGames()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var gameService = new GameService(db, mapper);

            var season = Seeding.CreateUnfinishedSeason();
            db.Seasons.Add(season);
            var teamHost = Seeding.CreateTeam();
            var team2 = Seeding.CreateTeam();
            db.Teams.Add(teamHost);
            db.Teams.Add(team2);
            var game = Seeding.CreateFinishedGame(season.Id, teamHost, team2, 120, 100);
            var game2 = Seeding.CreateFinishedGame(season.Id, teamHost, team2, 120, 100);
            db.Games.Add(game);
            db.Games.Add(game2);
            await db.SaveChangesAsync();

            var fakeGame1 = new GameDetailsServiceModel
            {
                Id = game.Id,
                SeasonYear = game.Date.Year,
                IsFinished = true,
                Team2Points = 100,
                TeamHostPoints = 120,
                Team2ShortName = team2.AbbreviatedName,
                TeamHostShortName = teamHost.AbbreviatedName,
                Date = game.Date.ToString("dd/MM/yyyy"),
                Venue = teamHost.Venue,
            };
            var fakeGame2 = new GameDetailsServiceModel
            {
                Id = game2.Id,
                SeasonYear = game2.Date.Year,
                IsFinished = true,
                Team2Points = 100,
                TeamHostPoints = 120,
                Team2ShortName = team2.AbbreviatedName,
                TeamHostShortName = teamHost.AbbreviatedName,
                Date = game2.Date.ToString("dd/MM/yyyy"),
                Venue = teamHost.Venue
            };
            var fakeGames = new List<GameDetailsServiceModel>() { fakeGame1, fakeGame2 };



            var games = await gameService.GetAllGamesBetweenTeamsAsync(teamHost.Name, team2.Name);



            Assert.True(games.Count == 2);
            Assert.IsType<List<GameDetailsServiceModel>>(games);
        }

        [Fact]
        public async Task GetSeasonGamesShouldGetAllGamesFromSeason()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var gameService = new GameService(db, mapper);

            var season = Seeding.CreateUnfinishedSeason();
            db.Seasons.Add(season);
            var teamHost = Seeding.CreateTeam();
            var team2 = Seeding.CreateTeam();
            db.Teams.Add(teamHost);
            db.Teams.Add(team2);
            var game = Seeding.CreateFinishedGame(season.Id, teamHost, team2, 120, 100);
            var game2 = Seeding.CreateFinishedGame(season.Id, teamHost, team2, 120, 100);
            var game3 = Seeding.CreateFinishedGame(season.Id, teamHost, team2, 120, 100);
            db.Games.Add(game);
            db.Games.Add(game2);
            db.Games.Add(game3);
            await db.SaveChangesAsync();


            var games = await gameService.GetSeasonGamesAsync(season.Id);


            Assert.True(games.Count == 3);
            Assert.IsType<List<GameDetailsServiceModel>>(games);
            foreach (var item in games)
            {
                Assert.Equal(teamHost.AbbreviatedName, item.TeamHostShortName);
                Assert.Equal(team2.AbbreviatedName, item.Team2ShortName);
            }
        }

        [Fact]
        public async Task GetGameShouldReturnCorrectGameAndData()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var gameService = new GameService(db, mapper);

            var season = Seeding.CreateUnfinishedSeason();
            db.Seasons.Add(season);
            var teamHost = Seeding.CreateTeam();
            var team2 = Seeding.CreateTeam();
            db.Teams.Add(teamHost);
            db.Teams.Add(team2);
            var game = Seeding.CreateFinishedGame(season.Id, teamHost, team2, 120, 100);
            db.Games.Add(game);
            await db.SaveChangesAsync();
            var fakeGameModel = new GameDetailsServiceModel
            {
                Id = game.Id,
                SeasonYear = season.Year,
                Date = game.Date.ToString("dd/MM/yyyy"),
                TeamHostShortName = teamHost.AbbreviatedName,
                Team2ShortName = team2.AbbreviatedName,
                IsFinished = true,
                TeamHostPoints = game.TeamHostPoints,
                Team2Points = game.Team2Points,
                Venue = game.TeamHost.Venue
            };


            var gameDetails = await gameService.GetGameAsync(game.Id);

            var obj1Str = JsonConvert.SerializeObject(fakeGameModel);
            var obj2Str = JsonConvert.SerializeObject(gameDetails);
            Assert.Equal(obj1Str, obj2Str);
        }

        [Fact]
        public async Task EditGameShouldSetCorrectlyNewValues()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var gameService = new GameService(db, mapper);

            var season = Seeding.CreateUnfinishedSeason();
            db.Seasons.Add(season);
            var teamHost = Seeding.CreateTeam();
            var team2 = Seeding.CreateTeam();
            db.Teams.Add(teamHost);
            db.Teams.Add(team2);
            var game = Seeding.CreateFinishedGame(season.Id, teamHost, team2, 120, 100);
            db.Games.Add(game);
            await db.SaveChangesAsync();
            var gameModel = new GameDetailsServiceModel
            {
                Id = game.Id,
                SeasonYear = season.Year,
                Date = game.Date.ToString("yyyy-MM-dd"),
                TeamHostShortName = teamHost.AbbreviatedName,
                Team2ShortName = team2.AbbreviatedName,
                IsFinished = true,
                TeamHostPoints = 121,
                Team2Points = 111,
                Venue = game.TeamHost.Venue
            };


            await gameService.EditGameAsync(gameModel, game.Id);


            Assert.Equal(121, game.TeamHostPoints);
            Assert.Equal(111, game.Team2Points);
        }

        [Fact]
        public async Task GetGamesOnDateShouldReturnCorrectGames()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var gameService = new GameService(db, mapper);

            var season = Seeding.CreateUnfinishedSeason();
            db.Seasons.Add(season);
            var teamHost = Seeding.CreateTeam();
            var team2 = Seeding.CreateTeam();
            db.Teams.Add(teamHost);
            db.Teams.Add(team2);
            var game = Seeding.CreateGameOnDate(season.Id, teamHost, team2, new DateTime(2020, 04, 04));
            var game2 = Seeding.CreateGameOnDate(season.Id, teamHost, team2, new DateTime(2020, 04, 05));
            db.Games.Add(game);
            db.Games.Add(game2);
            await db.SaveChangesAsync();


            var games = await gameService.GetGamesOnDateAsync(new DateTime(2020, 04, 04));


            Assert.Equal(1, games.Count);
            Assert.Equal("04/04/2020", games.First().Date);
        }

        [Fact]
        public async Task AddGameShouldAddGameToDatabase()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var gameService = new GameService(db, mapper);

            var season = Seeding.CreateUnfinishedSeason();
            db.Seasons.Add(season);
            var teamHost = Seeding.CreateTeam();
            var team2 = Seeding.CreateTeam();
            db.Teams.Add(teamHost);
            db.Teams.Add(team2);


            await gameService.AddGameAsync(season.Id, teamHost.Id, team2.Id, 120, 100, "2020-04-04", true, false);


            Assert.True(db.Games.Count() == 1);
        }

        [Fact]
        public async Task DeleteGameShouldRemoveGameFromDatabase()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var gameService = new GameService(db, mapper);

            var season = Seeding.CreateUnfinishedSeason();
            db.Seasons.Add(season);
            var teamHost = Seeding.CreateTeam();
            var team2 = Seeding.CreateTeam();
            db.Teams.Add(teamHost);
            db.Teams.Add(team2);
            var game = Seeding.CreateGameOnDate(season.Id, teamHost, team2, new DateTime(2020, 04, 04));
            db.Games.Add(game);
            await db.SaveChangesAsync();


            await gameService.DeleteGameAsync(game.Id);


            Assert.True(!db.Games.Any());
        }

        [Fact]
        public async Task GetFixturesShouldReturnOnlyNotFinishedUpcomingGames()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var gameService = new GameService(db, mapper);

            var season = Seeding.CreateUnfinishedSeason();
            db.Seasons.Add(season);
            var teamHost = Seeding.CreateTeam();
            var team2 = Seeding.CreateTeam();
            db.Teams.Add(teamHost);
            db.Teams.Add(team2);
            var game = Seeding.CreateGameOnDate(season.Id, teamHost, team2, new DateTime(2020, 04, 12));
            db.Games.Add(game);
            var game2 = Seeding.CreateGameOnDate(season.Id, teamHost, team2, new DateTime(2020, 04, 13));
            db.Games.Add(game2);
            var game4 = Seeding.CreateFinishedGame(season.Id, teamHost, team2, 120, 100);
            db.Games.Add(game4);
            var game3 = Seeding.CreateGameOnDate(season.Id, teamHost, team2, new DateTime(2020, 04, 25));
            db.Games.Add(game3);
            await db.SaveChangesAsync();


            var fixtures = await gameService.GetFixturesAsync(season.Id);


            Assert.True(fixtures.Games.Count == 2);
            foreach (var gameFixture in fixtures.Games)
            {
                Assert.True(gameFixture.IsFinished == false);
            }
        }

        [Fact]
        public async Task GetResultsShouldReturnOnlyFinishedGames()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var gameService = new GameService(db, mapper);

            var season = Seeding.CreateUnfinishedSeason();
            db.Seasons.Add(season);
            var teamHost = Seeding.CreateTeam();
            var team2 = Seeding.CreateTeam();
            db.Teams.Add(teamHost);
            db.Teams.Add(team2);
            var game = Seeding.CreateGameOnDate(season.Id, teamHost, team2, new DateTime(2020, 04, 12));
            var game2 = Seeding.CreateGameOnDate(season.Id, teamHost, team2, new DateTime(2020, 04, 10));
            var game3 = Seeding.CreateGameOnDate(season.Id, teamHost, team2, new DateTime(2020, 04, 09));
            var game4 = Seeding.CreateGameOnDate(season.Id, teamHost, team2, new DateTime(2020, 04, 08));

            db.Games.Add(game);
            db.Games.Add(game2);
            db.Games.Add(game4);
            db.Games.Add(game3);

            game2.IsFinished = true;
            game3.IsFinished = true;
            game4.IsFinished = true;
            await db.SaveChangesAsync();


            var results = await gameService.GetResultsAsync(season.Id);


            Assert.True(results.Games.Count == 3);
            foreach (var gameresult in results.Games)
            {
                Assert.True(gameresult.IsFinished = true);
            }
        }
    }
}
