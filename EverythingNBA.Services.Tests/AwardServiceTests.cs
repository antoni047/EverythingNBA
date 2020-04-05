namespace EverythingNBA.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;
    using EverythingNBA.Services.Implementations;
    using EverythingNBA.Services.Models.Award;
    using System.Linq;

    public class AwardServiceTests
    {
        [Fact]
        public async Task EditAwardWinnerShouldSetCorrectWinner()
        {
            var db = InMemoryDatabase.Get();

            var team = Seeding.CreateTeam();
            var season = Seeding.CreateFinishedSeason(team.Id, 2020);
            var player = Seeding.CreatePlayer(team.Id);
            var player2 = Seeding.CreatePlayer(team.Id);
            team.Players.Add(player);
            team.Players.Add(player2);
            var award = Seeding.CreateAward(player.Id, team.Name, season.Id, 2020);
            player.Awards.Add(award);
            db.Players.Add(player);
            db.Players.Add(player2);
            db.Teams.Add(team);
            db.Awards.Add(award);
            await db.SaveChangesAsync();

            var awardService = new AwardService(db);


            await awardService.EditAwardWinnerAsync(player2.FirstName + " " + player2.LastName, award.Id);

            var newAwardWinnerName = award.Winner.FirstName + " " + award.Winner.LastName;
            var expectedAwardWinnerName = player2.FirstName + " " + player2.LastName;

            Assert.Equal(expectedAwardWinnerName, newAwardWinnerName);
        }

        [Fact]
        public async Task GetSeasonAwardsShouldReturnAwardsFromGivenSeason()
        {
            var db = InMemoryDatabase.Get();

            var team = Seeding.CreateTeam();
            var season = Seeding.CreateFinishedSeason(team.Id, 2020);
            var season2 = Seeding.CreateFinishedSeason(team.Id, 2019);
            db.Seasons.Add(season);
            db.Seasons.Add(season2);
            var player = Seeding.CreatePlayer(team.Id);
            var player2 = Seeding.CreatePlayer(team.Id);
            team.Players.Add(player);
            team.Players.Add(player2);
            var award = Seeding.CreateAward(player.Id, team.Name, season.Id, 2020);
            var award2 = Seeding.CreateAward(player2.Id, team.Name, season.Id, 2020);
            var award3 = Seeding.CreateAward(player.Id, team.Name, season2.Id, 2019);
            player.Awards.Add(award);
            player.Awards.Add(award3);
            player2.Awards.Add(award2);
            db.Players.Add(player);
            db.Players.Add(player2);
            db.Teams.Add(team);
            db.Awards.Add(award);
            db.Awards.Add(award2);
            db.Awards.Add(award3);
            await db.SaveChangesAsync();

            var awardService = new AwardService(db);


            var awards2020 = await awardService.GetSeasonAwardsAsync(2020);


            Assert.True(awards2020.Count == 2);
            foreach (var awardItem in awards2020)
            {
                Assert.Equal(2020, awardItem.Year);
            }
        }

        [Fact]
        public async Task GetAllAwardsShouldReturnAllAwards()
        {
            var db = InMemoryDatabase.Get();

            var team = Seeding.CreateTeam();
            var season = Seeding.CreateFinishedSeason(team.Id, 2020);
            var season2 = Seeding.CreateFinishedSeason(team.Id, 2019);
            db.Seasons.Add(season);
            db.Seasons.Add(season2);
            var player = Seeding.CreatePlayer(team.Id);
            var player2 = Seeding.CreatePlayer(team.Id);
            var player3 = Seeding.CreatePlayer(team.Id);
            team.Players.Add(player);
            team.Players.Add(player2);
            team.Players.Add(player3);
            var award = Seeding.CreateAwardWithType(player.Id, team.Name, season.Id, 2019, "MVP");
            var award2 = Seeding.CreateAwardWithType(player2.Id, team.Name, season.Id, 2019, "DPOTY");
            var award3 = Seeding.CreateAwardWithType(player3.Id, team.Name, season.Id, 2019, "ROTY");
            player.Awards.Add(award);
            player2.Awards.Add(award3);
            player3.Awards.Add(award2);
            db.Players.Add(player);
            db.Players.Add(player2);
            db.Players.Add(player3);
            db.Teams.Add(team);
            db.Awards.Add(award);
            db.Awards.Add(award2);
            db.Awards.Add(award3);
            await db.SaveChangesAsync();

            var awardService = new AwardService(db);


            var allAwards = await awardService.GetAllAwardsAsync();


            Assert.True(allAwards.Count == 1);
            foreach (var item in allAwards)
            {
                Assert.True(item.MVP == player.FirstName + " " + player.LastName);
                Assert.True(item.DPOTY == player3.FirstName + " " + player3.LastName);
                Assert.True(item.ROTY == player2.FirstName + " " + player2.LastName);
            }
        }

        [Fact]
        public async Task GetAwardDetailsShouldReturnCorrectData()
        {
            var db = InMemoryDatabase.Get();

            var team = Seeding.CreateTeam();
            var season = Seeding.CreateFinishedSeason(team.Id, 2019);
            db.Seasons.Add(season);
            var player = Seeding.CreatePlayer(team.Id);
            team.Players.Add(player);
            var award = Seeding.CreateAwardWithType(player.Id, team.Name, season.Id, 2019, "MVP");
            player.Awards.Add(award);
            db.Players.Add(player);
            db.Teams.Add(team);
            db.Awards.Add(award);
            await db.SaveChangesAsync();

            var awardService = new AwardService(db);


            var awardModel = await awardService.GetAwardDetails(award.Id);


            Assert.Equal(2019, awardModel.Year);
            Assert.Equal("MVP", awardModel.Type);
            Assert.Equal(player.FirstName + " " + player.LastName, awardModel.Winner);
            Assert.Equal(team.Name, awardModel.WinnerTeam);
        }

        [Fact]
        public async Task AddAwardShouldAddAwardToDatabase()
        {
            var db = InMemoryDatabase.Get();

            var team = Seeding.CreateTeam();
            var season = Seeding.CreateFinishedSeason(team.Id, 2020);
            var player = Seeding.CreatePlayer(team.Id);
            var player2 = Seeding.CreatePlayer(team.Id);
            team.Players.Add(player);
            team.Players.Add(player2);
            var award = Seeding.CreateAward(player.Id, team.Name, season.Id, 2020);
            //player.Awards.Add(award);
            db.Players.Add(player);
            db.Players.Add(player2);
            db.Teams.Add(team);
            db.Seasons.Add(season);
            await db.SaveChangesAsync();

            var awardService = new AwardService(db);



            await awardService.AddAwardAsync("MVP", 2020, player.FirstName + " " + player.LastName, team.Name);


            
            Assert.True(db.Awards.Count() == 1);
            Assert.True(db.Awards.First().Name.ToString() == "MVP");
        }
        
    }
}
