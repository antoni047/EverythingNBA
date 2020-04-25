using EverythingNBA.Data;
using EverythingNBA.Models;
using EverythingNBA.Models.Enums;
using EverythingNBA.Models.MappingTables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeding
{
    public static class Seeding
    {
        public static void Main()
        {
            using var db = new EverythingNBADbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var random = new Random();

            AddTeamsWithSmallAndFullImages();
            var teamsList = db.Teams.ToList();
            teamsList.Shuffle();

            AddSeasonsWithPlayoffs();
            var seasonsList = db.Seasons.ToList();

            //Adding Season Statistics for the finished seasons
            foreach (var season in seasonsList)
            {
                //The RandomList method generates 30 random numbers from 0-82
                //(each representing the wins of a team in a Win-Loss record (e.g. 60-22))
                //that add up to the total of games played by all the teams,
                //therefore keeping the random numbers evenly distributed
                var westernWinsRandomList = RandomList(0, 82, 2460, 30);

                for (int i = 0; i < teamsList.Count; i++)
                {
                    var seasonStatistic = new SeasonStatistic
                    {
                        Season = season,
                        Team = teamsList[i],
                        Wins = westernWinsRandomList[i],
                        Losses = 82 - westernWinsRandomList[i],
                    };

                    db.SeasonStatistics.Add(seasonStatistic);
                };
            }

            db.SaveChanges();

            //Adding Players
            var playersNamesList = new List<Player>()
            {
                new Player { FirstName = "LeBron", LastName = "James", },
                new Player { FirstName = "Stephen", LastName = "Curry", },
                new Player { FirstName = "Giannis", LastName = "Antetokounmpo", },
                new Player { FirstName = "James" ,LastName = "Harden", },
                new Player { FirstName = "Kawhi", LastName = "Leonard", },
                new Player { FirstName = "Zion", LastName = "Williamson", },
                new Player { FirstName = "Luka", LastName = "Doncic",},
                new Player { FirstName = "Kyrie", LastName = "Irving", },
                new Player { FirstName = "Anthony", LastName = "Davis", },
                new Player { FirstName = "Russel", LastName = "Westbrook", },
                new Player { FirstName = "Trae", LastName = "Young", },
                new Player { FirstName = "Derrick",LastName = "Rose", },
                new Player { FirstName = "Rudy", LastName = "Gobert", },
                new Player { FirstName = "Carmelo",LastName = "Anthony",},
                new Player { FirstName = "Chris", LastName = "Paul", },
                new Player { FirstName = "Ja", LastName = "Morant", },
                new Player { FirstName = "RJ", LastName = "Barret", },
                new Player { FirstName = "Damian", LastName = "Lillard", },
                new Player { FirstName = "Paul", LastName = "George",},
                new Player { FirstName = "Lou", LastName = "Williams", },
                new Player { FirstName = "Ben", LastName = "Simmons", },
                new Player { FirstName = "Devin", LastName = "Booker", },
                new Player { FirstName = "Vince", LastName = "Carter", },
                new Player { FirstName = "Tacko", LastName = "Fall", },
                new Player { FirstName = "Lonzo", LastName = "Ball", },
                new Player { FirstName = "Donovan", LastName = "Mitchell", },
                new Player { FirstName = "Josh", LastName = "Hart",   },
                new Player { FirstName = "Ben", LastName = "McLemore",  },
                new Player { FirstName = "Klay", LastName = "Thompson",   },
                new Player { FirstName = "Draymond",LastName = "Green", },
                new Player { FirstName = "D'Angelo", LastName = "Russell",  },
                new Player { FirstName = "Andrew", LastName = "Wiggins",},
                new Player { FirstName = "Jayson", LastName = "Tatum", },
                new Player { FirstName = "Kemba", LastName = "Walker", },
                new Player { FirstName = "Joel",LastName = "Embiid", },
                new Player { FirstName = "Kristaps", LastName = "Porzingis", },
                new Player { FirstName = "Nikola", LastName = "Jokic",  },
                new Player { FirstName = "Brandon", LastName = "Ingram", },
                new Player { FirstName = "Dwight", LastName = "Howard", },
                new Player { FirstName = "Bradley", LastName = "Beal", },
                new Player { FirstName = "Jordan", LastName = "Clarkson", },
                new Player { FirstName = "Jimmy", LastName = "Butler",},
                new Player { FirstName = "Andre", LastName = "Drummond" },
                new Player { FirstName = "Andre", LastName = "Igoudala", },
                new Player { FirstName = "Zach", LastName = "LaVine", },
                new Player { FirstName = "Markelle", LastName = "Flutz", },
                new Player { FirstName = "Coby", LastName = "White", },
                new Player { FirstName = "Khris", LastName = "Middleton", },
                new Player { FirstName = "Brook", LastName = "Lopez",  },
                new Player { FirstName = "Eric", LastName = "Bledsoe", },
                new Player { FirstName = "Rui", LastName = "Hachimura", },
                new Player { FirstName = "Karl-Anthony", LastName = "Towns", },
                new Player { FirstName = "Robert", LastName = "Covington", },
                new Player { FirstName = "Michael", LastName = "Porter", },
                new Player { FirstName = "Kyle", LastName = "Lowry", },
                new Player { FirstName = "Pascal", LastName = "Siakam", },
                new Player { FirstName = "Kevin", LastName = "Love", },
                new Player { FirstName = "Tristan", LastName = "Thompson", },
                new Player { FirstName = "Clint", LastName = "Capela", },
                new Player { FirstName = "DeMar", LastName = "DeRozan", },
                new Player { FirstName = "LaMarcus", LastName = "Altridge", },
                new Player { FirstName = "Myles", LastName = "Turner", },
                new Player { FirstName = "Damontas", LastName = "Sabonis", },
                new Player { FirstName = "Jaylen", LastName = "Brown", },
                new Player { FirstName = "Marcus", LastName = "Smart", },
                new Player { FirstName = "Gordon", LastName = "Hayward", },
                new Player { FirstName = "Tobias", LastName = "Harris", },
                new Player { FirstName = "JJ", LastName = "Reddick", },
                new Player { FirstName = "Spencer", LastName = "Dinwiddie", },
                new Player { FirstName = "CJ", LastName = "McCollum", },
                new Player { FirstName = "Seth", LastName = "Curry", },
                new Player { FirstName = "Steven", LastName = "Adams", },
                new Player { FirstName = "Marc", LastName = "Gasol", },
                new Player { FirstName = "Danny", LastName = "Green", },
                new Player { FirstName = "Serge", LastName = "Ibaka", },
                new Player { FirstName = "DeMarcus", LastName = "Cousins", },
                new Player { FirstName = "PJ", LastName = "Tucker", },
                new Player { FirstName = "Jae", LastName = "Crowder", },
                new Player { FirstName = "Ricky", LastName = "Rubio", },
                new Player { FirstName = "DeAnder", LastName = "Hunter", },
                new Player { FirstName = "Patrick", LastName = "Beverly", },
                new Player { FirstName = "Shai", LastName = "Gilgeous-Alexander", },
                new Player { FirstName = "Aaron", LastName = "Gordon", },
                new Player { FirstName = "Nikola", LastName = "Vucevic", },
                new Player { FirstName = "Mo", LastName = "Bamba", },
                new Player { FirstName = "Terrance", LastName = "Ross", },
                new Player { FirstName = "Blake", LastName = "Griffin", },
                new Player { FirstName = "Stirling", LastName = "Brown", },
                new Player { FirstName = "Jrue", LastName = "Holiday", },
                new Player { FirstName = "Danuel", LastName = "House", },
                new Player { FirstName = "Derrick", LastName = "Favors", },
                new Player { FirstName = "De'Aron", LastName = "Fox", },
            };
            var playersArray = new Player[90];

            for (int i = 0; i < playersArray.Length; i++)
            {
                var playerAge = random.Next(18, 38);
                playersArray[i] = new Player
                {
                    FirstName = playersNamesList[i].FirstName,
                    LastName = playersNamesList[i].LastName,
                    Age = playerAge,
                    RookieYear = DateTime.UtcNow.Year - (playerAge - 18),
                    Position = (PositionType)random.Next(1, 5),
                    CloudinaryImage = null,
                    InstagramLink = "",
                    TwitterLink = "",
                    Height = random.Next(188, 225),
                    Weight = random.Next(80, 130),
                    IsStarter = true,
                    ShirtNumber = random.Next(0, 99),
                };

                if (i <= 29)
                {
                    playersArray[i].Team = teamsList[i];
                }
                else if (i <= 59)
                {
                    playersArray[i].Team = teamsList[i - 30];
                }
                else if (i <= 89)
                {
                    playersArray[i].Team = teamsList[i - 60];
                }
            }

            db.Players.AddRange(playersArray);
            db.SaveChanges();

            var playersList = db.Players.Include(p => p.Team).ToList();
            playersList.Shuffle();




            //Adding awards and all star teams to the finished seasons
            foreach (var season in seasonsList)
            {
                var playersAvailable = playersList.Where(p => p.RookieYear <= season.Year).ToList();

                var mvp = playersAvailable[random.Next(0, (playersAvailable.Count - 1))];
                var awardMVP = new Award
                {
                    Name = (AwardType)1,
                    Season = season,
                    Winner = mvp,
                    WinnerTeamName = mvp.Team.Name,
                    Year = season.Year,
                };

                var finalsMvp = playersAvailable[random.Next(0, (playersAvailable.Count - 1))];
                var awardFinalsMvp = new Award
                {
                    Name = (AwardType)2,
                    Season = season,
                    Winner = finalsMvp,
                    WinnerTeamName = finalsMvp.Team.Name,
                    Year = season.Year,
                };

                var roty = playersAvailable[random.Next(0, (playersAvailable.Count - 1))];
                var awardRoty = new Award
                {
                    Name = (AwardType)3,
                    Season = season,
                    Winner = roty,
                    WinnerTeamName = roty.Team.Name,
                    Year = season.Year,
                };

                var topScorer = playersAvailable[random.Next(0, (playersAvailable.Count - 1))];
                var awardTopScorer = new Award
                {
                    Name = (AwardType)4,
                    Season = season,
                    Winner = topScorer,
                    WinnerTeamName = topScorer.Team.Name,
                    Year = season.Year,
                };

                var dpoty = playersAvailable[random.Next(0, (playersAvailable.Count - 1))];
                var awardDpoty = new Award
                {
                    Name = (AwardType)5,
                    Season = season,
                    Winner = dpoty,
                    WinnerTeamName = dpoty.Team.Name,
                    Year = season.Year,
                };

                var smoty = playersAvailable[random.Next(0, (playersAvailable.Count - 1))];
                var awardSmoty = new Award
                {
                    Name = (AwardType)6,
                    Season = season,
                    Winner = smoty,
                    WinnerTeamName = smoty.Team.Name,
                    Year = season.Year,
                };

                var mip = playersAvailable[random.Next(0, (playersAvailable.Count - 1))];
                var awardMip = new Award
                {
                    Name = (AwardType)7,
                    Season = season,
                    Winner = mip,
                    WinnerTeamName = mip.Team.Name,
                    Year = season.Year,
                };

                var awards = new List<Award>() { awardMVP, awardFinalsMvp, awardRoty, awardTopScorer, awardDpoty, awardSmoty, awardMip };

                db.Awards.AddRange(awards);
            }

            db.SaveChanges();


            foreach (var season in seasonsList)
            {
                var playersAvailable = playersList.Where(p => p.RookieYear <= season.Year).ToList();
                var firstAllNBA = new AllStarTeam { Type = (AllStarTeamType)1, Year = season.Year, };
                var secondAllNBA = new AllStarTeam { Type = (AllStarTeamType)2, Year = season.Year, };
                var thirdAllNBA = new AllStarTeam { Type = (AllStarTeamType)3, Year = season.Year, };
                var allDefensive = new AllStarTeam { Type = (AllStarTeamType)4, Year = season.Year, };
                var allRookie = new AllStarTeam { Type = (AllStarTeamType)5, Year = season.Year, };

                firstAllNBA.Players = new List<AllStarTeamsPlayers>
                {
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = firstAllNBA },
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = firstAllNBA },
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = firstAllNBA },
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = firstAllNBA },
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = firstAllNBA },
                };
                secondAllNBA.Players = new List<AllStarTeamsPlayers>
                {
                    new AllStarTeamsPlayers{  Player =playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = secondAllNBA },
                    new AllStarTeamsPlayers{  Player =playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = secondAllNBA },
                    new AllStarTeamsPlayers{  Player =playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = secondAllNBA },
                    new AllStarTeamsPlayers{  Player =playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = secondAllNBA },
                    new AllStarTeamsPlayers{  Player =playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = secondAllNBA },
                };
                thirdAllNBA.Players = new List<AllStarTeamsPlayers>
                {
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = thirdAllNBA },
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = thirdAllNBA },
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = thirdAllNBA },
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = thirdAllNBA },
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = thirdAllNBA },
                };
                allDefensive.Players = new List<AllStarTeamsPlayers>
                {
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = allDefensive },
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = allDefensive },
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = allDefensive },
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = allDefensive },
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = allDefensive },
                };
                allRookie.Players = new List<AllStarTeamsPlayers>
                {
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = allRookie },
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = allRookie },
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = allRookie },
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = allRookie },
                    new AllStarTeamsPlayers{  Player = playersAvailable[random.Next(0, (playersAvailable.Count - 1))], AllStarTeam = allRookie },
                };

                var allStarTeams = new List<AllStarTeam>() { firstAllNBA, secondAllNBA, thirdAllNBA, allDefensive, allRookie };

                db.AllStarTeams.AddRange(allStarTeams);
            }

            db.SaveChanges();





            //Adding Games
            var seasonObj = new Season
            {
                Year = 2020,
                SeasonStartDate = new DateTime(2020, 02, 01),
                SeasonEndDate = new DateTime(2020, 06, 30),
                GamesPlayed = 30,
            };

            var playoff = new Playoff
            {
                AreConferenceFinalsFinished = false,
                AreQuarterFinalsFinished = false,
                AreSemiFinalsFinished = false,
                Season = seasonObj,
            };

            db.Seasons.Add(seasonObj);
            db.SaveChanges();

            var season2020 = db.Seasons.Where(s => s.Year == 2020).FirstOrDefault();

            //Creating empty season statistics for the new season
            var season2020Stats = new List<SeasonStatistic>();
            foreach (var team in teamsList)
            {
                var seasonStatistic = new SeasonStatistic
                {
                    SeasonId = season2020.Id,
                    Team = team,
                    Wins = 0,
                    Losses = 0,
                };

                season2020Stats.Add(seasonStatistic);
            }

            db.SeasonStatistics.AddRange(season2020Stats);
            db.SaveChanges();


            var finishedGamesList = new List<Game>();

            var finishedGamesPerTeamCount = 30;
            var timePeriod = 90; //how many days back from now will the games take place (number must be divisible by the finishedGamesPerTeamCount)
            var daysFromSeasonStartList = new List<int>();
            for (int i = 0; i < timePeriod; i++)
            {
                daysFromSeasonStartList.Add(i);
            }

            daysFromSeasonStartList.Shuffle();


            //Adding 30 finshed games per team
            for (int i = 0; i < finishedGamesPerTeamCount; i++)
            {
                //Creating 2 stacks of teams from which the oponents in each game will be gotten
                var firstHalfOfTeams = new Stack<Team>();
                var secondHalfOfTeams = new Stack<Team>();
                for (int k = 0; k < 15; k++)
                {
                    firstHalfOfTeams.Push(teamsList[k]);
                    secondHalfOfTeams.Push(teamsList[k + 14]);
                }

                while (firstHalfOfTeams.Any())
                {
                    //Getting 2 different random numbers for points
                    var teamHostPoints = random.Next(80, 130);
                    var team2Points = teamHostPoints;
                    while (team2Points == teamHostPoints) { team2Points = random.Next(80, 130); }

                    var game = new Game
                    {
                        TeamHost = firstHalfOfTeams.Pop(),
                        Team2 = secondHalfOfTeams.Pop(),
                        Season = season2020,
                        TeamHostPoints = teamHostPoints,
                        Team2Points = team2Points,
                        IsFinished = true,
                        IsPlayoffGame = false,
                    };

                    finishedGamesList.Add(game);
                }

                //Shuffling teams to get different matchups next iteration
                teamsList.Shuffle();
            }

            var daysStack = new Stack<int>();
            foreach (var game in finishedGamesList)
            {
                if (!daysStack.Any())
                {
                    for (int i = 0; i < timePeriod; i++)
                    {
                        daysStack.Push(daysFromSeasonStartList[i]);
                    }
                }

                game.Date = DateTime.Now.Date.AddDays(-daysStack.Pop());
            }

            db.Games.AddRange(finishedGamesList);
            db.SaveChanges();


            var gamesList = db.Games.Where(g => g.SeasonId == season2020.Id).ToList();
            var seasonStats2020 = db.SeasonStatistics.Where(g => g.SeasonId == season2020.Id).ToList();
            //Editing seasonStatistics for each team to match the games played
            foreach (var game in gamesList)
            {
                var seasonStatisticForTeamHost = seasonStats2020.Where(ss => ss.Team == game.TeamHost).FirstOrDefault();
                var seasonStatisticForTeam2 = seasonStats2020.Where(ss => ss.Team == game.Team2).FirstOrDefault();

                if (game.TeamHostPoints > game.Team2Points)
                {
                    seasonStatisticForTeamHost.Wins++;
                    seasonStatisticForTeam2.Losses++;
                }
                else
                {
                    seasonStatisticForTeamHost.Losses++;
                    seasonStatisticForTeam2.Wins++;
                }
            }

            db.SaveChanges();



            //Adding Unfinished Games
            //Change gamesPerTeamCount to change the ammount of games added per team
            var gamesPerTeamCount = 3;
            //Change daysDuration to change over how many days the created games will be played
            var daysDuration = 15;

            var unfinishedGamesList = new List<Game>();
            for (int i = 0; i < gamesPerTeamCount; i++)
            {
                //Creating 2 stacks of teams from which the oponents in each game will be gotten
                var firstHalfOfTeams = new Stack<Team>();
                var secondHalfOfTeams = new Stack<Team>();
                for (int k = 0; k < 15; k++)
                {
                    firstHalfOfTeams.Push(teamsList[k]);
                    secondHalfOfTeams.Push(teamsList[k + 15]);
                }

                while (firstHalfOfTeams.Any())
                {
                    var game = new Game
                    {
                        TeamHost = firstHalfOfTeams.Pop(),
                        Team2 = secondHalfOfTeams.Pop(),
                        Date = DateTime.UtcNow.AddDays(random.Next(0, daysDuration)),
                        SeasonId = season2020.Id,
                        TeamHostPoints = 0,
                        Team2Points = 0,
                        IsFinished = false,
                        IsPlayoffGame = false,
                    };

                    unfinishedGamesList.Add(game);
                }

                //Shuffling teams to get different matchups next iteration
                teamsList.Shuffle();
            }

            db.Games.AddRange(unfinishedGamesList);
            db.SaveChanges();


            AddGameStatisticsToFinishedGames(season2020.Id);
        }

        public static List<int> RandomList(int minNumber, int maxNumber,
                                   int total, int numbersCount)
        {
            List<int> numbersList = new List<int>(numbersCount);

            Random random = new Random();
            int localMin, localMax, nextDigit;
            int remainingSum = total;

            for (int i = 1; i <= numbersCount; i++)
            {
                localMax = remainingSum - ((numbersCount - i) * minNumber);
                if (localMax > maxNumber)
                    localMax = maxNumber;

                localMin = remainingSum - ((numbersCount - i) * maxNumber);
                if (localMin > minNumber)
                    localMin = minNumber;

                nextDigit = random.Next(localMin, localMax);
                numbersList.Add(nextDigit);
                remainingSum -= nextDigit;
            }

            return numbersList;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var rng = new Random();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static List<GameStatistic> AddGameStatisticsToFinishedGames(int seasonId)
        {
            var random = new Random();
            using var db = new EverythingNBADbContext();

            var games = db.Games
               .Include(g => g.TeamHost)
                   .ThenInclude(t => t.Players)
               .Include(g => g.Team2)
                   .ThenInclude(t => t.Players)
               .Where(g => g.SeasonId == seasonId && g.IsFinished == true)
               .ToList();

            var gameStatistics = new List<GameStatistic>();

            foreach (var game in games)
            {
                foreach (var player in game.TeamHost.Players)
                {
                    var playerCount = game.TeamHost.Players.Count();
                    var pointsGottenByPlayers = game.TeamHostPoints / 1.75;
                    var points = random.Next(0, (int)(pointsGottenByPlayers / playerCount));
                    var fieldGoalsAttempted = random.Next(5, 30);
                    var threesAttempted = random.Next(0, 20);
                    var freeThrowsAttempted = random.Next(0, 20);

                    var gameStatistic = new GameStatistic
                    {
                        Game = game,
                        Player = player,
                        MinutesPlayed = random.Next(25, 45),
                        Points = points,
                        Assists = random.Next(0, 12),
                        Rebounds = random.Next(0, 15),
                        Steals = random.Next(0, 5),
                        Blocks = random.Next(0, 5),
                        FieldGoalAttempts = fieldGoalsAttempted,
                        FieldGoalsMade = random.Next(0, fieldGoalsAttempted),
                        ThreeAttempts = threesAttempted,
                        ThreeMade = random.Next(0, threesAttempted),
                        FreeThrowAttempts = freeThrowsAttempted,
                        FreeThrowsMade = random.Next(0, freeThrowsAttempted),
                    };

                    gameStatistics.Add(gameStatistic);
                }

                foreach (var player in game.Team2.Players)
                {
                    var playerCount = game.Team2.Players.Count();
                    var pointsGottenByPlayers = game.Team2Points / 1.75;
                    var points = random.Next(0, (int)(pointsGottenByPlayers / playerCount));
                    var fieldGoalsAttempted = random.Next(5, 30);
                    var threesAttempted = random.Next(0, 20);
                    var freeThrowsAttempted = random.Next(0, 20);

                    var gameStatistic = new GameStatistic
                    {
                        Game = game,
                        Player = player,
                        MinutesPlayed = random.Next(25, 45),
                        Points = points,
                        Assists = random.Next(0, 12),
                        Rebounds = random.Next(0, 15),
                        Steals = random.Next(0, 5),
                        Blocks = random.Next(0, 5),
                        FieldGoalAttempts = fieldGoalsAttempted,
                        FieldGoalsMade = random.Next(0, fieldGoalsAttempted),
                        ThreeAttempts = threesAttempted,
                        ThreeMade = random.Next(0, threesAttempted),
                        FreeThrowAttempts = freeThrowsAttempted,
                        FreeThrowsMade = random.Next(0, freeThrowsAttempted),
                    };

                    gameStatistics.Add(gameStatistic);
                }
            }

            db.GameStatistics.AddRange(gameStatistics);
            db.SaveChanges();
            return gameStatistics;
        }

        public static void AddSeasonsWithPlayoffs()
        {
            using var db = new EverythingNBADbContext();

            var season2019 = new Season
            {
                TitleWinner = db.Teams.Where(t => t.Name == "Toronto Raptors").FirstOrDefault(),
                SeasonStartDate = new DateTime(2018, 10, 17),
                SeasonEndDate = new DateTime(2019, 04, 11),
                Year = 2019,
                GamesPlayed = 82,
            };
            var season2018 = new Season
            {
                TitleWinner = db.Teams.Where(t => t.Name == "Golden State Warriors").FirstOrDefault(),
                SeasonStartDate = new DateTime(2017, 10, 17),
                SeasonEndDate = new DateTime(2018, 04, 11),
                Year = 2018,
                GamesPlayed = 82,
            };
            var season2017 = new Season
            {
                TitleWinner = db.Teams.Where(t => t.Name == "Golden State Warriors").FirstOrDefault(),
                SeasonStartDate = new DateTime(2016, 10, 17),
                SeasonEndDate = new DateTime(2017, 04, 11),
                Year = 2017,
                GamesPlayed = 82,
            };
            var season2016 = new Season
            {
                TitleWinner = db.Teams.Where(t => t.Name == "Cleveland Cavaliers").FirstOrDefault(),
                SeasonStartDate = new DateTime(2015, 10, 17),
                SeasonEndDate = new DateTime(2016, 04, 11),
                Year = 2016,
                GamesPlayed = 82,
            };
            var season2015 = new Season
            {
                TitleWinner = db.Teams.Where(t => t.Name == "Golden State Warriors").FirstOrDefault(),
                SeasonStartDate = new DateTime(2014, 10, 17),
                SeasonEndDate = new DateTime(2015, 04, 11),
                Year = 2015,
                GamesPlayed = 82,
            };


            var playoff2019 = new Playoff
            {
                Season = season2019,
                AreQuarterFinalsFinished = false,
                AreSemiFinalsFinished = false,
                AreConferenceFinalsFinished = false,
            };
            var playoff2018 = new Playoff
            {
                Season = season2018,
                AreQuarterFinalsFinished = false,
                AreSemiFinalsFinished = false,
                AreConferenceFinalsFinished = false,
            };
            var playoff2017 = new Playoff
            {
                Season = season2017,
                AreQuarterFinalsFinished = false,
                AreSemiFinalsFinished = false,
                AreConferenceFinalsFinished = false,
            };
            var playoff2016 = new Playoff
            {
                Season = season2016,
                AreQuarterFinalsFinished = false,
                AreSemiFinalsFinished = false,
                AreConferenceFinalsFinished = false,
            };
            var playoff2015 = new Playoff
            {
                Season = season2015,
                AreQuarterFinalsFinished = false,
                AreSemiFinalsFinished = false,
                AreConferenceFinalsFinished = false,
            };


            db.Seasons.AddRange(new List<Season>() { season2019, season2018, season2017, season2016, season2015 });
            db.Playoffs.AddRange(new List<Playoff>() { playoff2019, playoff2018, playoff2017, playoff2016, playoff2015 });
            db.SaveChanges();
        }

        private static void AddTeamsWithSmallAndFullImages()
        {
            var random = new Random();
            using var db = new EverythingNBADbContext();
            var easternConference = (ConferenceType)Enum.Parse(typeof(ConferenceType), "Eastern");
            var westernConference = (ConferenceType)Enum.Parse(typeof(ConferenceType), "Western");

            //Adding small and full images for every team with existing URLs from my cloud
            var smallImage1 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007067/nba570956AtlantaHawksThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage2 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007070/nba381679BrooklynNetsThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage3 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007069/nba672740BostonCelticsThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage4 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007071/nba429847CharlotteHornetsThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage5 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007072/nba700222ChicagoBullsThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage7 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007073/nba872177ClevelandCavaliersThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage8 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007074/nba353339DallasMavericksThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage9 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007075/nba629805DenverNuggetsThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage10 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007076/nba545872DetroitPistonsThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage11 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007077/nba823179GoldesnStateWarriorsThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage12 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007077/nba418549HoustonRocketsThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage13 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007078/nba007643IndianaPacersThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage14 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007079/nba604042LAClippersThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage15 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007080/nba068320LALakersThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage16 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007081/nba690626MemphisGrizzliesThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage17 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007082/nba206400MiamiHeatThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage18 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007082/nba556492MilwaukeeBucksThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage19 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007083/nba672363MinnesottaTimberwolvesThubm.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage20 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007084/nba438249NewOrleansPelicansThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage21 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007085/nba114206NewYorkKnicksThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage22 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007086/nba791705OklahomaCityThunderThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage23 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007086/nba982438OrlandoMagicThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage24 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007087/nba322413Philadelphia76ersThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage25 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007088/nba354756PhoenixSunsThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage26 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007089/nba349660PortlandTrailsBlazersThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage27 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007090/nba887868SacramentoKingsThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage28 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007091/nba812009SanAntonioSpursThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage29 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007092/nba210542TorontoRaptorsThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage30 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007093/nba407169UtahJazzThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var smallImage31 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584007094/nba636185WashingtonWizardsThumb.png",
                ImagePublicId = $"nba{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };

            db.CloudinaryImages.AddRange(new List<CloudinaryImage>() { smallImage1, smallImage2, smallImage3 , smallImage4, smallImage5, smallImage7, smallImage8, smallImage9, smallImage10,
                smallImage11, smallImage12, smallImage13, smallImage14, smallImage15, smallImage16, smallImage17, smallImage18, smallImage19, smallImage20, smallImage21, smallImage22, smallImage23,
                smallImage24, smallImage25, smallImage26, smallImage27, smallImage28, smallImage29, smallImage30, smallImage31});



            var fullImage1 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439111/nba406587AtlantaHawks.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage2 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439115/nba561407BrookylnNets.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage3 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439113/nba600577BotonCeltics.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage4 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439116/nba329107CharlotteHornets.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage5 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439117/nba613356ChicagoBulls.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage7 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439118/nba501734ClevelandCavaliers.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage8 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439123/nba685705DallasMavericks.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage9 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439125/nba744013DenverNuggets.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage10 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439126/nba306996DetroitPistons.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage11 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439128/nba195420GoldenStateWarriors.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage12 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439130/nba120925HoustonRockets.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage13 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439131/nba590839IndianaPacers.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage14 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439133/nba216566LosAngelesClippers.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage15 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439135/nba548414LosAngelesLakers.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage16 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439136/nba492757MemphisGrizzlies.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage17 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439138/nba878134MiamiHeat.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage18 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439139/nba328540MilwaukeeBucks.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage19 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439142/nba781904MinnesottaTimberwolves.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage20 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439144/nba564048NewOrleansPelicans.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage21 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439146/nba038177NewYorkKnicks.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage22 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439147/nba380091OklahomaCityThunder.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage23 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439148/nba376262OrlandoMagic.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage24 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439150/nba680179Philadelphia76ers.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage25 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439151/nba100155PhoenixSuns.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage26 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584440468/nba380399por.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage27 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439152/nba042182SacramentoKings.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage28 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439154/nba339272SanAntonioSpurs.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage29 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439155/nba653533TorontoRaptors.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage30 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439157/nba128665UtahJazz.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };
            var fullImage31 = new CloudinaryImage
            {
                ImageURL = "https://res.cloudinary.com/bxgmxncloud/image/upload/v1584439158/nba139740WashingtonWizards.png",
                ImagePublicId = $"nbaFull{Guid.NewGuid()}",
                Length = random.Next(0, 99999999),
            };


            db.CloudinaryImages.AddRange(new List<CloudinaryImage>() { fullImage1, fullImage2, fullImage3 , fullImage4, fullImage5, fullImage7, fullImage8, fullImage9, fullImage10,
                fullImage11, fullImage12, fullImage13, fullImage14, fullImage15, fullImage16, fullImage17, fullImage18, fullImage19, fullImage20, fullImage21, fullImage22, fullImage23,
                fullImage24, fullImage25, fullImage26, fullImage27, fullImage28, fullImage29, fullImage30, fullImage31});



            //Creating teams
            var team1 = new Team
            {
                Name = "Atlanta Hawks",
                AbbreviatedName = "ATL",
                Conference = easternConference,
                Venue = "State Farm Arena",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage1,
                FullImage = fullImage1,
                PrimaryColorHex = "#ff5151",
                SecondaryColorHex = "#bdcc00",
                Coach = "Lloyd Pierce",
            };
            var team2 = new Team
            {
                Name = "Brooklyn Nets",
                AbbreviatedName = "BKN",
                Conference = easternConference,
                Venue = "Barclays Center",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage2,
                FullImage = fullImage2,
                PrimaryColorHex = "#000000",
                SecondaryColorHex = "#707271",
                Coach = "Brad Stevens",
            };
            var team3 = new Team
            {
                Name = "Boston Celtics",
                AbbreviatedName = "BOS",
                Conference = easternConference,
                Venue = "TD Garden",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage3,
                FullImage = fullImage3,
                PrimaryColorHex = "#008348",
                SecondaryColorHex = "#bb9753",
                Coach = "Jacque Vaughn",
            };
            var team4 = new Team
            {
                Name = "Charlotte Hornets",
                AbbreviatedName = "CHA",
                Conference = easternConference,
                Venue = "Spectrum Center",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage4,
                FullImage = fullImage4,
                PrimaryColorHex = "#1d1160",
                SecondaryColorHex = "#00788c",
                Coach = "James Borrego",
            };
            var team5 = new Team
            {
                Name = "Chicago Bulls",
                AbbreviatedName = "CHI",
                Conference = easternConference,
                Venue = "United Center ",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage5,
                FullImage = fullImage5,
                PrimaryColorHex = "#ce1141",
                SecondaryColorHex = "#000000",
                Coach = "Jim Boylen",
            };
            var team7 = new Team
            {
                Name = "Cleveland Cavaliers",
                AbbreviatedName = "CLE",
                Conference = easternConference,
                Venue = "State Farm Arena",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage7,
                FullImage = fullImage7,
                PrimaryColorHex = "#6f263d",
                SecondaryColorHex = "#ffb81c",
                Coach = "J. B. Bickerstaff",
            };
            var team8 = new Team
            {
                Name = "Dallas Mavericks",
                AbbreviatedName = "DAL",
                Conference = westernConference,
                Venue = "American Airlines Center",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage8,
                FullImage = fullImage8,
                PrimaryColorHex = "#0064b1",
                SecondaryColorHex = "#bbc4ca",
                Coach = "Rick Carlisle",
            };
            var team9 = new Team
            {
                Name = "Denver Nuggets",
                AbbreviatedName = "DEN",
                Conference = westernConference,
                Venue = "Pepsi Center",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage9,
                FullImage = fullImage9,
                PrimaryColorHex = "#0e2240",
                SecondaryColorHex = "#fec524",
                Coach = "Michael Malone",
            };
            var team10 = new Team
            {
                Name = "Detroit Pistons",
                AbbreviatedName = "DEN",
                Conference = easternConference,
                Venue = "Little Caesars Arena",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage10,
                FullImage = fullImage10,
                PrimaryColorHex = "#1d428a",
                SecondaryColorHex = "#c8102e",
                Coach = "Dwane Casey",
            };
            var team11 = new Team
            {
                Name = "Golden State Warriors",
                AbbreviatedName = "GSW",
                Conference = westernConference,
                Venue = "Chase Center",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage11,
                FullImage = fullImage11,
                PrimaryColorHex = "#1d428a",
                SecondaryColorHex = "#fdb927",
                Coach = "Steve Kerr",
            };
            var team12 = new Team
            {
                Name = "Houston Rockets",
                AbbreviatedName = "HOU",
                Conference = westernConference,
                Venue = "Toyota Center",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage12,
                FullImage = fullImage12,
                PrimaryColorHex = "#ce1141",
                SecondaryColorHex = "#000000",
                Coach = "Mike D'Antoni",
            };
            var team13 = new Team
            {
                Name = "Indiana Pacers",
                AbbreviatedName = "IND",
                Conference = easternConference,
                Venue = "Bankers Life Fieldhouse",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage13,
                FullImage = fullImage13,
                PrimaryColorHex = "#002d62",
                SecondaryColorHex = "#fdbb30",
                Coach = "Nate McMillan",
            };
            var team14 = new Team
            {
                Name = "Los Angeles Clippers",
                AbbreviatedName = "LAC",
                Conference = westernConference,
                Venue = "Staples Center",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage14,
                FullImage = fullImage14,
                PrimaryColorHex = "#1d428a",
                SecondaryColorHex = "#ffffff",
                Coach = "Doc Rivers",
            };
            var team15 = new Team
            {
                Name = "Los Angeles Lakers",
                AbbreviatedName = "LAL",
                Conference = westernConference,
                Venue = "Staples Center",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage15,
                FullImage = fullImage15,
                PrimaryColorHex = "#552583",
                SecondaryColorHex = "#fdb927",
                Coach = "Frank Vogel",
            };
            var team16 = new Team
            {
                Name = "Memphis Grizzlies",
                AbbreviatedName = "MEM",
                Conference = westernConference,
                Venue = "FedExForum",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage16,
                FullImage = fullImage16,
                PrimaryColorHex = "#0c2340",
                SecondaryColorHex = "#7d9bc1",
                Coach = "Taylor Jenkins",
            };
            var team17 = new Team
            {
                Name = "Miami Heat",
                AbbreviatedName = "MIA",
                Conference = easternConference,
                Venue = "American Airlines Arena",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage17,
                FullImage = fullImage17,
                PrimaryColorHex = "#000000",
                SecondaryColorHex = "#98002e",
                Coach = "Erik Spoelstra",
            };
            var team18 = new Team
            {
                Name = "Milwaukee Bucks",
                AbbreviatedName = "MIL",
                Conference = easternConference,
                Venue = "Fiserv Forum",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage18,
                FullImage = fullImage18,
                PrimaryColorHex = "#eee1c6",
                SecondaryColorHex = "#00471b",
                Coach = "Mike Budenholzer",
            };
            var team19 = new Team
            {
                Name = "Minnesota Timberwolves",
                AbbreviatedName = "MIN",
                Conference = westernConference,
                Venue = "Target Center",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage19,
                FullImage = fullImage19,
                PrimaryColorHex = "#0c2340",
                SecondaryColorHex = "#78be20",
                Coach = "Ryan Saunders",
            };
            var team20 = new Team
            {
                Name = "New Orleans Pelicans",
                AbbreviatedName = "NOP",
                Conference = westernConference,
                Venue = "Smoothie King Center",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage20,
                FullImage = fullImage20,
                PrimaryColorHex = "#002b5c",
                SecondaryColorHex = "#e31837",
                Coach = "Alvin Gentry",
            };
            var team21 = new Team
            {
                Name = "New York Knicks",
                AbbreviatedName = "NYK",
                Conference = easternConference,
                Venue = "Madison Square Garden",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage21,
                FullImage = fullImage21,
                PrimaryColorHex = "#006bb6",
                SecondaryColorHex = "#f58426",
                Coach = "Mike Miller",
            };
            var team22 = new Team
            {
                Name = "Oklahoma City Thunder",
                AbbreviatedName = "OKC",
                Conference = westernConference,
                Venue = "Smoothie King Center",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage22,
                FullImage = fullImage22,
                PrimaryColorHex = "#007ac1",
                SecondaryColorHex = "#ef3b24",
                Coach = "Billy Donovan",
            };
            var team23 = new Team
            {
                Name = "Orlando Magic",
                AbbreviatedName = "ORL",
                Conference = easternConference,
                Venue = "Amway Center",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage23,
                FullImage = fullImage23,
                PrimaryColorHex = "#0077c0",
                SecondaryColorHex = "#000000",
                Coach = "Steve Clifford",
            };
            var team24 = new Team
            {
                Name = "Philadelphia 76ers",
                AbbreviatedName = "PHI",
                Conference = easternConference,
                Venue = "Wells Fargo Center",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage24,
                FullImage = fullImage24,
                PrimaryColorHex = "#006bb6",
                SecondaryColorHex = "#ed174c",
                Coach = "Brett Brown",
            };
            var team25 = new Team
            {
                Name = "Phoenix Suns",
                AbbreviatedName = "PHX",
                Conference = westernConference,
                Venue = "Talking Stick Resort Arena",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage25,
                FullImage = fullImage25,
                PrimaryColorHex = "#1d1160",
                SecondaryColorHex = "#e56020",
                Coach = "Monty Williams ",
            };
            var team26 = new Team
            {
                Name = "Portland Trail Blazers",
                AbbreviatedName = "POR",
                Conference = westernConference,
                Venue = "Moda Center",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage26,
                FullImage = fullImage26,
                PrimaryColorHex = "#e03a3e",
                SecondaryColorHex = "#000000",
                Coach = "Terry Stotts",
            };
            var team27 = new Team
            {
                Name = "Sacramento Kings",
                AbbreviatedName = "SAC",
                Conference = westernConference,
                Venue = "Golden 1 Center",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage27,
                FullImage = fullImage27,
                PrimaryColorHex = "#5a2b81",
                SecondaryColorHex = "#63727a",
                Coach = "Luke Walton",
            };
            var team28 = new Team
            {
                Name = "San Antonio Spurs",
                AbbreviatedName = "SAS",
                Conference = westernConference,
                Venue = "Smoothie King Center",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage28,
                FullImage = fullImage28,
                PrimaryColorHex = "#c4ced4",
                SecondaryColorHex = "#000000",
                Coach = "Gregg Popovich",
            };
            var team29 = new Team
            {
                Name = "Toronto Raptors",
                AbbreviatedName = "TOR",
                Conference = easternConference,
                Venue = "Scotiabank Arena",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage29,
                FullImage = fullImage29,
                PrimaryColorHex = "#ce1141",
                SecondaryColorHex = "#000000",
                Coach = "Nick Nurse",
            };
            var team30 = new Team
            {
                Name = "Utah Jazz",
                AbbreviatedName = "NOP",
                Conference = westernConference,
                Venue = "Vivint Smart Home Arena",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage30,
                FullImage = fullImage30,
                PrimaryColorHex = "#002b5c",
                SecondaryColorHex = "#f9a01b",
                Coach = "Quin Snyder",
            };
            var team31 = new Team
            {
                Name = "Washington Wizards",
                AbbreviatedName = "WAS",
                Conference = easternConference,
                Venue = "Capital One Arena",
                Instagram = "",
                Twitter = "",
                SmallImage = smallImage31,
                FullImage = fullImage31,
                PrimaryColorHex = "#e31837",
                SecondaryColorHex = "#002b5c",
                Coach = "Scott Brooks"
            };

            db.Teams.AddRange(new List<Team>() { team1, team2, team3, team4, team5, team7, team8, team9, team10, team11, team12, team13, team14, team15,
                 team16, team17, team18, team19, team20, team21, team22, team23, team24, team25, team26, team27, team28, team29, team30, team31});
            db.SaveChanges();
        }
    }
}
