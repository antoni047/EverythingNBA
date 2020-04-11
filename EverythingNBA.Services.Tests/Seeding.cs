namespace EverythingNBA.Services.Tests
{
    using EverythingNBA.Models;
    using EverythingNBA.Models.Enums;
    using System;

    public class Seeding
    {
        private static int awardId;
        private static int gameId;
        private static int gameStatisticId;
        private static int playerId;
        private static int playoffId;
        private static int seasonId;
        private static int seasonStatisticId;
        private static int seriesId;
        private static int teamId;
        private static int picId;
        private const string DefaultPictureUrl = "https://www.cloudinary.com";

        public static Award CreateAward(int winnerId, string winnerTeam, int seasonId, int year)
        {
            var awardObj = new Award
            {
                Id = ++awardId,
                WinnerId = winnerId,
                WinnerTeamName = winnerTeam,
                Name = (AwardType)Enum.Parse(typeof(AwardType), "MVP"),
                Year = year,
                SeasonId = seasonId,
            };

            return awardObj;
        }

        public static Award CreateAwardWithType(int winnerId, string winnerTeam, int seasonId, int year, string type)
        {
            var awardObj = new Award
            {
                Id = ++awardId,
                WinnerId = winnerId,
                WinnerTeamName = winnerTeam,
                Name = (AwardType)Enum.Parse(typeof(AwardType), type),
                Year = year,
                SeasonId = seasonId,
            };

            return awardObj;
        }

        public static CloudinaryImage Create()
        {
            return new CloudinaryImage
            {
                Id = ++picId,
                ImageURL = DefaultPictureUrl
            };
        }

        public static CloudinaryImage CreateWithFullInfo()
        {
            return new CloudinaryImage
            {
                Id = ++picId,
                Length = long.MaxValue,
                ImageThumbnailURL = Guid.NewGuid().ToString(),
                ImagePublicId = Guid.NewGuid().ToString(),
                ImageURL = Guid.NewGuid().ToString()
            };
        }

        public static Game CreateFinishedGame(int seasonId, Team teamHost, Team team2, int teamHostPoints, int team2Points)
        {
            var random = new Random();

            var gameObj = new Game
            {
                Id = ++gameId,
                SeasonId = seasonId,
                Date = DateTime.UtcNow.AddDays(random.Next(-30, -5)),
                TeamHost = teamHost,
                Team2 = team2,
                IsFinished = true,
                TeamHostPoints = teamHostPoints,
                Team2Points = team2Points,

            };

            return gameObj;
        }

        public static Game CreateUnfinishedGame(int seasonId, Team teamHost, Team team2)
        {
            var random = new Random();

            var gameObj = new Game
            {
                Id = ++gameId,
                SeasonId = seasonId,
                Date = DateTime.UtcNow.AddDays(random.Next(5, 80)),
                TeamHost = teamHost,
                Team2 = team2,
                IsFinished = false,
                TeamHostPoints = 0,
                Team2Points = 0,
            };

            return gameObj;
        }

        public static Game CreateGameOnDate(int seasonId, Team teamHost, Team team2, DateTime date)
        {
            var random = new Random();

            var gameObj = new Game
            {
                Id = ++gameId,
                SeasonId = seasonId,
                Date = date,
                TeamHost = teamHost,
                Team2 = team2,
                IsFinished = false,
                TeamHostPoints = 0,
                Team2Points = 0,
                IsPlayoffGame = false,
            };

            return gameObj;
        }

        public static GameStatistic CreateGameStatistic(int playerId, int gameId)
        {
            var random = new Random();

            var gameStatisticObj = new GameStatistic
            {
                Id = ++gameStatisticId,
                GameId = gameId,
                PlayerId = playerId,
                Points = random.Next(0, 60),
                Assists = random.Next(0, 20),
                Rebounds = random.Next(0, 30),
                Blocks = random.Next(0, 10),
                Steals = random.Next(0, 10),
                FreeThrowAttempts = random.Next(5, 10),
                FreeThrowsMade = random.Next(0, 5),
                FieldGoalAttempts = random.Next(5, 10),
                FieldGoalsMade = random.Next(0, 5),
                ThreeAttempts = random.Next(2, 4),
                ThreeMade = random.Next(0, 2),
                MinutesPlayed = random.Next(12, 48)
            };

            return gameStatisticObj;
        }

        public static Player CreatePlayer(int teamId)
        {
            var random = new Random();

            var playerObj = new Player
            {
                Id = ++playerId,
                TeamId = teamId,
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Position = (PositionType)Enum.Parse(typeof(PositionType), "SmallForward"),
                RookieYear = DateTime.UtcNow.AddYears(-5).Year,
                Age = 23,
                Height = random.Next(185, 230),
                Weight = random.Next(90, 130),
                IsStarter = true,
                ShirtNumber = random.Next(0, 99),
            };

            return playerObj;
        }

        public static Playoff CreatePlayoff(int seasonId)
        {
            var playoffObj = new Playoff
            {
                Id = ++playoffId,
                SeasonId = seasonId,
                AreConferenceFinalsFinished = false,
                AreQuarterFinalsFinished = false,
                AreSemiFinalsFinished = false
            };

            return playoffObj;
        }

        public static Season CreateUnfinishedSeason()
        {
            var seasonObj = new Season
            {
                Id = ++seasonId,
                GamesPlayed = 80,
                SeasonEndDate = DateTime.UtcNow.AddMonths(3),
                SeasonStartDate = DateTime.UtcNow.AddMonths(-9),
                Year = DateTime.UtcNow.Year,
            };

            return seasonObj;
        }

        public static Season CreateFinishedSeason(int titleWinnerId, int year)
        {
            var seasonObj = new Season
            {
                Id = ++seasonId,
                GamesPlayed = 82,
                SeasonEndDate = DateTime.UtcNow.AddMonths(3),
                SeasonStartDate = DateTime.UtcNow.AddMonths(-9),
                TitleWinnerId = titleWinnerId,
                Year = year,
            };

            return seasonObj;
        }

        public static Season CreateFinishedSeasonWithPlayoff(int playoffId, int titleWinnerId)
        {
            var seasonObj = new Season
            {
                Id = ++seasonId,
                GamesPlayed = 82,
                PlayoffId = playoffId,
                SeasonEndDate = DateTime.UtcNow.AddMonths(3),
                SeasonStartDate = DateTime.UtcNow.AddMonths(-9),
                TitleWinnerId = titleWinnerId,
                Year = DateTime.UtcNow.AddYears(-1).Year,
            };

            return seasonObj;
        }

        public static Series CreateSeriesWithoutGames(int playoffId, int team1Id, int team2Id, string stage, int stageNumber)
        {
            var random = new Random();

            var seriesObj = new Series
            {
                Id = ++seriesId,
                PlayoffId = playoffId,
                Team1Id = team1Id,
                Team2Id = team2Id,
                Stage = stage,
                StageNumber = stageNumber,
                Conference = "Western",
                Team1GamesWon = 4,
                Team2GamesWon = 0,
                Team1StandingsPosition = random.Next(1, 8),
                Team2StandingsPosition = random.Next(1, 8),
            };

            return seriesObj;
        }

        public static Series CreateSeriesWithGames(int playoffId, int team1Id, int team2Id, string stage, int stageNumber,
            int game1Id, int game2Id, int game3Id, int game4Id)
        {
            var random = new Random();

            var seriesObj = new Series
            {
                Id = ++seriesId,
                PlayoffId = playoffId,
                Team1Id = team1Id,
                Team2Id = team2Id,
                Stage = stage,
                StageNumber = stageNumber,
                Conference = "Western",
                Team1GamesWon = 4,
                Team2GamesWon = 0,
                Team1StandingsPosition = random.Next(1, 8),
                Team2StandingsPosition = random.Next(1, 8),
                Game1Id = game1Id,
                Game2Id = game2Id,
                Game3Id = game3Id,
                Game4Id = game4Id
            };

            return seriesObj;
        }

        public static Team CreateTeam()
        {
            var teamObj = new Team
            {
                Id = ++teamId,
                Name = Guid.NewGuid().ToString(),
                AbbreviatedName = Guid.NewGuid().ToString(),
                Conference = (ConferenceType)Enum.Parse(typeof(ConferenceType), "Western"),
                Venue = Guid.NewGuid().ToString(),
            };

            return teamObj;
        }

        public static SeasonStatistic CreateSeasonStatisitc(int seasonId, int teamId)
        {
            var seasonStatisticsObj = new SeasonStatistic
            {
                Id = ++seasonStatisticId,
                Losses = 32,
                Wins = 50,
                SeasonId =  seasonId,
                TeamId = teamId
            };

            return seasonStatisticsObj;
        }
    }
}
