namespace EverythingNBA.Services.Tests
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Xunit;

    using EverythingNBA.Services.Implementations;
    using EverythingNBA.Services.Models.Game;
    using EverythingNBA.Services.Models.Series;

    public class SeriesServiceTests
    {
        [Fact]
        public async Task GetSeriesShouldReturnCorrectData()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var seriesService = new SeriesService(db, mapper);

            var team1 = Seeding.CreateTeam();
            var team2 = Seeding.CreateTeam();
            var season = Seeding.CreateFinishedSeason(team1.Id, 2019);
            var playoff = Seeding.CreatePlayoff(season.Id);
            var series = Seeding.CreateSeriesWithoutGames(playoff.Id, team1.Id, team2.Id, "QuarterFinal", 1);
            db.Seasons.Add(season);
            db.Series.Add(series);
            db.Teams.Add(team1);
            db.Teams.Add(team2);
            db.Playoffs.Add(playoff);
            await db.SaveChangesAsync();

            var games = new List<GameDetailsServiceModel>() { null, null, null, null, null, null, null };
            var fakeSeriesModel = new GetSeriesDetailsServiceModel
            {
                Id = series.Id,
                PlayoffId = series.PlayoffId,
                Team1GamesWon = 4,
                Team1Name = team1.Name,
                Team2Name = team2.Name,
                Team2GamesWon = 0,
                Conference = series.Conference,
                Stage = series.Stage,
                StageNumber = series.StageNumber,
                Games = games,
            };

            var seriesModel = await seriesService.GetSeriesAsync(series.Id);


            var obj1Str = JsonConvert.SerializeObject(fakeSeriesModel);
            var obj2Str = JsonConvert.SerializeObject(seriesModel);
            Assert.Equal(obj1Str, obj2Str);
        }

        [Fact]
        public async Task GetSeriesOverviewReturnsCorrectData()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var seriesService = new SeriesService(db, mapper);

            var team1 = Seeding.CreateTeam();
            var team2 = Seeding.CreateTeam();
            var season = Seeding.CreateFinishedSeason(team1.Id, 2019);
            var playoff = Seeding.CreatePlayoff(season.Id);
            var series = Seeding.CreateSeriesWithoutGames(playoff.Id, team1.Id, team2.Id, "QuarterFinal", 1);
            db.Seasons.Add(season);
            db.Series.Add(series);
            db.Teams.Add(team1);
            db.Teams.Add(team2);
            db.Playoffs.Add(playoff);
            await db.SaveChangesAsync();
            var fakeSeriesModel = new SeriesOverviewServiceModel
            {
                Id = series.Id,
                PlayoffId = series.PlayoffId,
                Team1GamesWon = 4,
                Team1Name = team1.Name,
                Team2Name = team2.Name,
                Team2GamesWon = 0,
                Conference = series.Conference,
                Stage = series.Stage,
                StageNumber = series.StageNumber,
            };

            var seriesModel = await seriesService.GetSeriesOverview(series.Id);


            var obj1Str = JsonConvert.SerializeObject(fakeSeriesModel);
            var obj2Str = JsonConvert.SerializeObject(seriesModel);
            Assert.Equal(obj1Str, obj2Str);
        }

        [Fact]
        public async Task GetWinnerShouldReturnCorrectTeamName()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var seriesService = new SeriesService(db, mapper);

            var team1 = Seeding.CreateTeam();
            var team2 = Seeding.CreateTeam();
            var season = Seeding.CreateFinishedSeason(team1.Id, 2019);
            var playoff = Seeding.CreatePlayoff(season.Id);
            var series = Seeding.CreateSeriesWithoutGames(playoff.Id, team1.Id, team2.Id, "QuarterFinal", 1);
            db.Seasons.Add(season);
            db.Series.Add(series);
            db.Teams.Add(team1);
            db.Teams.Add(team2);
            db.Playoffs.Add(playoff);
            await db.SaveChangesAsync();


            var winner = await seriesService.GetWinnerAsync(series.Id);


            Assert.Equal(winner.TeamName, team1.Name);
            Assert.Equal(winner.StandingsPosition, series.Team1StandingsPosition);
            Assert.Equal(winner.PlayoffId, playoff.Id);
            Assert.Equal(winner.Id, series.Id);
        }

        [Fact]
        public async Task SetGameWonShouldIncreaseGamesCorrectly() 
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var seriesService = new SeriesService(db, mapper);

            var team1 = Seeding.CreateTeam();
            var team2 = Seeding.CreateTeam();
            var season = Seeding.CreateFinishedSeason(team1.Id, 2019);
            var playoff = Seeding.CreatePlayoff(season.Id);
            var series = Seeding.CreateSeriesWithoutGames(playoff.Id, team1.Id, team2.Id, "QuarterFinal", 1);
            db.Seasons.Add(season);
            db.Series.Add(series);
            db.Teams.Add(team1);
            db.Teams.Add(team2);
            db.Playoffs.Add(playoff);
            await db.SaveChangesAsync();

            series.Team1GamesWon--;
            await seriesService.SetGameWon(series.Id, team2.Name);


            Assert.Equal(1, series.Team2GamesWon);
        }

        [Fact]
        public async Task AddSeriesShouldAddSeriesToDatabase()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var seriesService = new SeriesService(db, mapper);

            var team1 = Seeding.CreateTeam();
            var team2 = Seeding.CreateTeam();
            var season = Seeding.CreateFinishedSeason(team1.Id, 2019);
            var playoff = Seeding.CreatePlayoff(season.Id);
            db.Seasons.Add(season);
            db.Teams.Add(team1);
            db.Teams.Add(team2);
            db.Playoffs.Add(playoff);
            await db.SaveChangesAsync();


            await seriesService.AddSeriesAsync(playoff.Id, team1.Name, team2.Name, 4, 0, null, null, 
                null, null, null, null, null, "Western", "QuarterFinal", 1, 1, 8);


            Assert.True(db.Series.Count() == 1);
            Assert.True(db.Series.First().PlayoffId == playoff.Id);
        }
    }
}
