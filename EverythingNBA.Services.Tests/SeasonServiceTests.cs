namespace EverythingNBA.Services.Tests
{
    using EverythingNBA.Models;
    using EverythingNBA.Services.Implementations;
    using EverythingNBA.Services.Models.Season;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class SeasonServiceTests
    {
        [Fact]
        public async Task GetDetailsShouldRetrunCorrectData()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var seasonService = new SeasonService(db, mapper);

            var season = Seeding.CreateUnfinishedSeason();
            db.Seasons.Add(season);
            await db.SaveChangesAsync();


            var seasonDetails = await seasonService.GetDetailsAsync(season.Id);


            Assert.Equal(season.Year, seasonDetails.Year);
            Assert.Equal(season.Id, seasonDetails.SeasonId);
        }

        [Fact]
        public async Task GetDetailsByYearShouldRedirectAndReturnCorrectData()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var seasonService = new SeasonService(db, mapper);

            var season = Seeding.CreateUnfinishedSeason();
            db.Seasons.Add(season);
            await db.SaveChangesAsync();


            var seasonDetails = await seasonService.GetDetailsByYearAsync(season.Year);


            Assert.Equal(season.Year, seasonDetails.Year);
            Assert.Equal(season.Id, seasonDetails.SeasonId);
        }

        [Fact]

        public async Task GetAllSeasonsShouldReturnAllSeasons()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var seasonService = new SeasonService(db, mapper);

            var team = Seeding.CreateTeam();
            var season = Seeding.CreateFinishedSeason(team.Id, 2019);
            var season2 = Seeding.CreateFinishedSeason(team.Id, 2018);
            db.Seasons.Add(season);
            db.Seasons.Add(season2);
            await db.SaveChangesAsync();


            var allSeasons = await seasonService.GetAllSeasonsAsync();


            Assert.True(allSeasons.Count == 2);
        }

        [Fact]
        public async Task EditSeasonShouldSetCorrectData()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var seasonService = new SeasonService(db, mapper);

            var team = Seeding.CreateTeam();
            var season = Seeding.CreateFinishedSeason(team.Id, 2019);
            db.Seasons.Add(season);
            await db.SaveChangesAsync();

            var editedSeason = new GetSeasonDetailsServiceModel
            {
                TitleWinnerId = team.Id,
                Year = 2018,
                SeasonId = season.Id
            };


            await seasonService.EditSeasonAsync(editedSeason ,season.Id);


            Assert.Equal(2018, season.Year);
            Assert.Equal(team.Id, editedSeason.TitleWinnerId);
            Assert.Equal(season.Id, editedSeason.SeasonId);
        }

        [Fact]
        public async Task GetYearShouldReturnCorrectYear()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var seasonService = new SeasonService(db, mapper);

            var team = Seeding.CreateTeam();
            var season = Seeding.CreateFinishedSeason(team.Id, 2019);
            db.Seasons.Add(season);
            await db.SaveChangesAsync();


            var year = await seasonService.GetYearAsync(season.Id);


            Assert.True(year == 2019);
        }

        [Fact]

        public async Task GetSeasonStartDateShouldReturnCorrectDate()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var seasonService = new SeasonService(db, mapper);

            var season = new Season
            {
                Id = 1,
                GamesPlayed = 82,
                SeasonEndDate = new DateTime(25/04/2020),
                SeasonStartDate = new DateTime(25/10/2020),
                TitleWinnerId = 1,
                Year = 2020,
            };

            db.Seasons.Add(season);
            await db.SaveChangesAsync();


            var startDate = await seasonService.GetSeasonStartDateAsync(1);


            var date = new DateTime(25/04/2020);
            Assert.True(DateTime.Compare(date, startDate) == 0);
        }

        [Fact]

        public async Task GetSeasonEndDateShouldReturnCorrectDate()
        {
            var db = InMemoryDatabase.Get();
            var mapper = AutomapperSingleton.Mapper;

            var seasonService = new SeasonService(db, mapper);

            var season = new Season
            {
                Id = 1,
                GamesPlayed = 82,
                SeasonEndDate = new DateTime(25 / 04 / 2020),
                SeasonStartDate = new DateTime(25 / 10 / 2019),
                TitleWinnerId = 1,
                Year = 2020,
            };

            db.Seasons.Add(season);
            await db.SaveChangesAsync();


            var startDate = await seasonService.GetSeasonEndDateAsync(1);


            var date = new DateTime(25 / 10 / 2019);
            Assert.True(DateTime.Compare(date, startDate) == 0);
        }
    }
}
