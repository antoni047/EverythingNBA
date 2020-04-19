namespace EverythingNBA.Services.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Xunit;

    using EverythingNBA.Services.Implementations;
    using EverythingNBA.Services.Models.SeasonStatistic;

    public class SeasonStatisticServiceTests
    {
        [Fact]
        public async Task GetDetailsShouldReturnCorrectData()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var seasonStatService = new SeasonStatisticService(db, mapper);

            var team = Seeding.CreateTeam();
            var season = Seeding.CreateUnfinishedSeason();
            var seasonStat = Seeding.CreateSeasonStatisitc(season.Id, team.Id);
            db.Seasons.Add(season);
            db.Teams.Add(team);
            db.SeasonStatistics.Add(seasonStat);
            await db.SaveChangesAsync();
            var fakeStatModel = new GetSeasonStatisticDetailsServiceModel
            {
                Id = seasonStat.Id,
                Wins = seasonStat.Wins,
                Losses = seasonStat.Losses,
                SeasonId = seasonStat.SeasonId,
                TeamId = seasonStat.TeamId,
            };


            var detailsModel = await seasonStatService.GetDetailsAsync(seasonStat.Id);

            var obj1Str = JsonConvert.SerializeObject(fakeStatModel);
            var obj2Str = JsonConvert.SerializeObject(detailsModel);
            Assert.Equal(obj1Str, obj2Str);
        }

        [Fact]
        public async Task GetDetailsShouldRedirectAndReturnCorrectData()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var seasonStatService = new SeasonStatisticService(db, mapper);

            var team = Seeding.CreateTeam();
            var season = Seeding.CreateUnfinishedSeason();
            var seasonStat = Seeding.CreateSeasonStatisitc(season.Id, team.Id);
            db.Seasons.Add(season);
            db.Teams.Add(team);
            db.SeasonStatistics.Add(seasonStat);
            await db.SaveChangesAsync();
            var fakeStatModel = new GetSeasonStatisticDetailsServiceModel
            {
                Id = seasonStat.Id,
                Wins = seasonStat.Wins,
                Losses = seasonStat.Losses,
                SeasonId = seasonStat.SeasonId,
                TeamId = seasonStat.TeamId,
            };


            var detailsModel = await seasonStatService.GetDetailsAsync(season.Id, team.Id);

            var obj1Str = JsonConvert.SerializeObject(fakeStatModel);
            var obj2Str = JsonConvert.SerializeObject(detailsModel);
            Assert.Equal(obj1Str, obj2Str);
        }

        [Fact]
        public async Task GetWinPercentageShouldReturnCorrectResult()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var seasonStatService = new SeasonStatisticService(db, mapper);

            var team = Seeding.CreateTeam();
            var season = Seeding.CreateFinishedSeason(team.Id, 2019);
            var seasonStat = Seeding.CreateSeasonStatisitc(season.Id, team.Id);
            db.Seasons.Add(season);
            db.Teams.Add(team);
            db.SeasonStatistics.Add(seasonStat);
            await db.SaveChangesAsync();

            var fakeWinPercentage = 0.610.ToString(".000");
            var winPercentage = await seasonStatService.GetWinPercentageAsync(seasonStat.Id);

            Assert.True(winPercentage == fakeWinPercentage);
        }

        [Fact]
        public async Task AddSeasonStatisticShouldAddStatisticToDatabase()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var seasonStatService = new SeasonStatisticService(db, mapper);

            var team = Seeding.CreateTeam();
            var season = Seeding.CreateFinishedSeason(team.Id, 2019);
            db.Seasons.Add(season);
            db.Teams.Add(team);
            await db.SaveChangesAsync();


            await seasonStatService.AddAsync(season.Id, team.Id, 50, 32);


            Assert.True(db.SeasonStatistics.Count() == 1);
            Assert.True(db.SeasonStatistics.First().SeasonId == season.Id);
        }
    }
}
