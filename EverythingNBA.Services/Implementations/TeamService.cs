﻿namespace EverythingNBA.Services.Implementations
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Text;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;

    using EverythingNBA.Models;
    using EverythingNBA.Models.Enums;
    using Data;
    using Services.Models;
    using Services.Models.Team;
    using Services.Models.SeasonStatistic;
    using Services.Models.Game;

    public class TeamService : ITeamService
    {
        private readonly EverythingNBADbContext db;
        private readonly IImageService imageService;
        private readonly ISeasonStatisticService statisticService;
        private readonly IGameService gameService;
        private readonly IMapper mapper;
        private readonly IPlayerService playerService;

        public TeamService(EverythingNBADbContext db, IImageService imageService, ISeasonStatisticService statisticService,
            IMapper mapper, IGameService gameService, IPlayerService playerService)
        {
            this.db = db;
            this.imageService = imageService;
            this.statisticService = statisticService;
            this.mapper = mapper;
            this.gameService = gameService;
            this.playerService = playerService;
        }

        public async Task AddPlayerAsync(int playerId, int teamId)
        {
            var player = await this.db.Players.FindAsync(playerId);
            var team = await this.db.Teams.FindAsync(teamId);

            team.Players.Add(player);
            player.Team = team;

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> RemovePlayerAsync(int playedId, int teamId)
        {
            var team = await this.db.Teams.Where(t => t.Id == teamId).FirstOrDefaultAsync();
            var player = team.Players.Where(p => p.Id == playedId).FirstOrDefault();

            if (player == null)
            {
                return false;
            }

            team.Players.Remove(player);
            player.Team = null;

            await this.db.SaveChangesAsync();

            return true;

        }

        public async Task<int> AddTeamAsync(string name, IFormFile FullImageFile, IFormFile smallImageFile, string conference,
            string venue, string instagram, string twitter)
        {
            var fullImageId = await this.imageService.UploadImageAsync(FullImageFile);
            var smallImageId = await this.imageService.UploadImageAsync(smallImageFile);

            var teamObj = new Team
            {
                Name = name,
                FullImageId = fullImageId,
                SmallImageId = smallImageId,
                Conference = (ConferenceType)Enum.Parse(typeof(ConferenceType), conference),
                Venue = venue,
                Twitter = twitter,
                Instagram = instagram
            };

            this.db.Teams.Add(teamObj);
            await this.db.SaveChangesAsync();

            return teamObj.Id;
        }

        public async Task<bool> DeleteTeamAsync(int teamId)
        {
            var teamToDelete = await this.db.Teams.FindAsync(teamId);

            if (teamToDelete == null)
            {
                return false;
            }

            this.db.Teams.Remove(teamToDelete);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<TeamStandingsListingServiceModel> GetStandingsAsync(int seasonId)
        {
            var easternStandingsList = new List<TeamSeasonStatisticServiceModel>();
            var westernStandingsList = new List<TeamSeasonStatisticServiceModel>();

            var teams = this.db.Teams
                .Include(t => t.SeasonsStatistics)
                .Include(t => t.GamesWon)
                .Include(t => t.HomeGames)
                .Include(t => t.AwayGames)
                .Include(t => t.SmallImage)
                .ToList();

            foreach (var team in teams)
            {
                var gamesPlayed = await this.GetGamesPlayed(team.Id, seasonId);
                var lastTenGames = await this.GetLastTenGames(team.Id, seasonId);
                var seasonStatisicModel = await statisticService.GetDetailsAsync(seasonId, team.Id);

                var wins = 0;
                var losses = 0;
                var winPercentage = string.Empty;

                if (seasonStatisicModel != null)
                {
                    var seasonStatistic = mapper.Map<SeasonStatistic>(seasonStatisicModel);
                    winPercentage = await statisticService.GetWinPercentageAsync(seasonStatistic.Id);
                    wins = seasonStatistic.Wins;
                    losses = seasonStatistic.Losses;
                }

                var teamStatModel = new TeamSeasonStatisticServiceModel()
                {
                    Name = team.Name,
                    ImageURL = team.SmallImage.ImageURL,
                    Conference = team.Conference.ToString(),
                    Wins = wins,
                    Losses = losses,
                    WinPercentage = winPercentage.ToString(),
                    GamesPlayed = gamesPlayed,
                    LastTenGames = lastTenGames
                };

                if (teamStatModel.Conference == "Western")
                {
                    westernStandingsList.Add(teamStatModel);
                }

                else
                {
                    easternStandingsList.Add(teamStatModel);
                }
            }

            var standingsModel = new TeamStandingsListingServiceModel()
            {
                WesternStandings = westernStandingsList.OrderByDescending(x => x.WinPercentage).ToList(),
                EasternStandings = easternStandingsList.OrderByDescending(x => x.WinPercentage).ToList()
            };

            return standingsModel;
        }

        public async Task<GetTeamDetailsServiceModel> GetTeamDetailsAsync(int teamId, int year)
        {
            var seasonId = await this.db.Seasons.Where(s => s.Year == year).Select(s => s.Id).FirstOrDefaultAsync();

            var team = await this.db.Teams
                .Include(t => t.SeasonsStatistics)
                .Include(t => t.TitlesWon)
                .Include(t => t.HomeGames)
                .Include(t => t.AwayGames)
                .Include(t => t.GamesWon)
                .Include(t => t.SmallImage)
                .Include(t => t.FullImage)
                .Where(t => t.Id == teamId)
                .FirstOrDefaultAsync();

            var teamDetailsModel = mapper.Map<GetTeamDetailsServiceModel>(team);
            teamDetailsModel.FullImageURL = team.FullImage.ImageURL;

            var players = await this.playerService.GetAllPlayersFromTeam(teamId);
            teamDetailsModel.Players = players;

            var titlesString = new StringBuilder();
            foreach (var title in team.TitlesWon)
            {
                titlesString.Append(title.Year.ToString() + ", ");
            }
            teamDetailsModel.TitlesWon = titlesString.ToString();

            var teamStandings = new List<SeasonStatisticOverviewServiceModel>();
            var allStandings = await this.GetStandingsAsync(seasonId);

            if (teamDetailsModel.Conference == "Western")
            {
                var listCount = allStandings.WesternStandings.OrderByDescending(x => x.WinPercentage).ToList().Count();

                for (int i = 0; i < listCount; i++)
                {
                    var standings = allStandings.WesternStandings.OrderByDescending(x => x.WinPercentage).ToList();

                    if (standings[i].Name == team.Name)
                    {
                        if (i == 14)
                        {
                            var teamInfront = mapper.Map<SeasonStatisticOverviewServiceModel>(standings[i - 1]);
                            teamInfront.Position = i;
                            teamStandings.Add(teamInfront);

                            var currentTeam = mapper.Map<SeasonStatisticOverviewServiceModel>(standings[i]);
                            currentTeam.Position = i + 1;
                            teamStandings.Add(currentTeam);
                        }
                        else if (i == 0)
                        {
                            var currentTeam = mapper.Map<SeasonStatisticOverviewServiceModel>(standings[i]);
                            currentTeam.Position = i + 1;
                            teamStandings.Add(currentTeam);

                            var teamBehind = mapper.Map<SeasonStatisticOverviewServiceModel>(standings[i + 1]);
                            teamBehind.Position = i + 2;
                            teamStandings.Add(teamBehind);
                        }
                        else
                        {
                            var teamInfront = mapper.Map<SeasonStatisticOverviewServiceModel>(standings[i - 1]);
                            teamInfront.Position = i;
                            teamStandings.Add(teamInfront);

                            var currentTeam = mapper.Map<SeasonStatisticOverviewServiceModel>(standings[i]);
                            currentTeam.Position = i + 1;
                            teamStandings.Add(currentTeam);

                            var teamBehind = mapper.Map<SeasonStatisticOverviewServiceModel>(standings[i + 1]);
                            teamBehind.Position = i + 2;
                            teamStandings.Add(teamBehind);
                        }
                    }
                }
            }
            else
            {
                var listCount = allStandings.EasternStandings.OrderByDescending(x => x.WinPercentage).ToList().Count();

                for (int i = 0; i < listCount; i++)
                {
                    var standings = allStandings.EasternStandings.OrderByDescending(x => x.WinPercentage).ToList();

                    if (standings[i].Name == team.Name)
                    {
                        if (i == 15)
                        {
                            var teamInfront = mapper.Map<SeasonStatisticOverviewServiceModel>(standings[i - 1]);
                            teamInfront.Position = i;
                            teamStandings.Add(teamInfront);

                            var currentTeam = mapper.Map<SeasonStatisticOverviewServiceModel>(standings[i]);
                            currentTeam.Position = i + 1;
                            teamStandings.Add(currentTeam);
                        }
                        if (i == 0)
                        {
                            var currentTeam = mapper.Map<SeasonStatisticOverviewServiceModel>(standings[i]);
                            currentTeam.Position = i + 1;
                            teamStandings.Add(currentTeam);

                            var teamBehind = mapper.Map<SeasonStatisticOverviewServiceModel>(standings[i + 1]);
                            teamBehind.Position = i + 2;
                            teamStandings.Add(teamBehind);
                        }
                        else
                        {
                            var teamInfront = mapper.Map<SeasonStatisticOverviewServiceModel>(standings[i - 1]);
                            teamInfront.Position = i;
                            teamStandings.Add(teamInfront);

                            var currentTeam = mapper.Map<SeasonStatisticOverviewServiceModel>(standings[i]);
                            currentTeam.Position = i + 1;
                            teamStandings.Add(currentTeam);

                            var teamBehind = mapper.Map<SeasonStatisticOverviewServiceModel>(standings[i + 1]);
                            teamBehind.Position = i + 2;
                            teamStandings.Add(teamBehind);
                        }
                    }
                }
            }

            teamDetailsModel.CurrentSeasonStatistic = teamStandings;

            var games = this.GetLastNineGames(team);
            teamDetailsModel.Last9Games = games;

            return teamDetailsModel;
        }

        public async Task<GetTeamDetailsServiceModel> GetTeamDetailsAsync(string name, int year)
        {
            var teamId = await this.db.Teams
                .Where(t => t.Name.ToLower() == name.ToLower()).Select(t => t.Id).FirstOrDefaultAsync();

            return await this.GetTeamDetailsAsync(teamId, year);
        }

        public async Task<TeamOverviewServiceModel> GetTeamAsync(int teamId)
        {
            var team = await this.db.Teams.FindAsync(teamId);

            return mapper.Map<TeamOverviewServiceModel>(team);
        }

        public async Task<TeamOverviewServiceModel> GetTeamAsync(string name)
        {
            var teamId = await this.db.Teams.Where(t => t.Name == name).Select(t => t.Id).FirstOrDefaultAsync();

            return await this.GetTeamAsync(teamId);
        }

        public async Task<bool> AddGameAsync(int gameId, int teamId)
        {
            var team = await this.db.Teams
                .Include(t => t.GamesWon)
                .Include(t => t.HomeGames)
                .Include(t => t.AwayGames)
                .Where(t => t.Id == teamId)
                .FirstOrDefaultAsync();

            var game = await this.db.Games.FindAsync(gameId);
            bool teamIsHost;
            bool isGameWon = false;

            if (game.TeamHostId == teamId)
            {
                team.HomeGames.Add(game);
                teamIsHost = true;
            }
            else if (game.Team2Id == teamId)
            {
                team.AwayGames.Add(game);
                teamIsHost = false;
            }
            else
            {
                return false;
            }

            if (game.IsFinished)
            {
                if (game.TeamHostPoints > game.Team2Points && teamIsHost)
                {
                    team.GamesWon.Add(game);
                    isGameWon = true;
                }
                else
                {
                    if (teamIsHost == false)
                    {
                        team.GamesWon.Add(game);
                        isGameWon = true;
                    }
                }
            }

            var gameSeasonYear = this.GetSeasonYear(game.Date);
            var currentYear = this.GetCurrentSeasonYear();

            if (gameSeasonYear == currentYear && team.SeasonsStatistics.Count < 82)
            {
                await this.statisticService.AddGameAsync(teamId, isGameWon);
            }

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveGameAsync(int gameId, int teamId)
        {
            var team = await this.db.Teams.FindAsync(teamId);

            var game = await this.db.Games.FindAsync(gameId);

            if (game == null || team == null)
            {
                return false;
            }

            if (team.HomeGames.Contains(game))
            {
                team.HomeGames.Remove(game);

                await this.db.SaveChangesAsync();
                return true;
            }

            else if (team.AwayGames.Contains(game))
            {
                team.AwayGames.Remove(game);

                await this.db.SaveChangesAsync();
                return true;
            }

            else
            {
                return false;
            }
        }

        public async Task<bool> AddTitleAsync(int teamId, int year)
        {
            var team = await this.db.Teams
                .Include(t => t.TitlesWon)
                .Where(t => t.Id == teamId)
                .FirstOrDefaultAsync();

            var season = await this.db.Seasons
                .Include(s => s.TitleWinner)
                .Where(s => s.Year == year)
                .FirstOrDefaultAsync();

            if (team == null || season == null)
            {
                return false;
            }

            if (season.TitleWinner == null)
            {
                season.TitleWinner = team;
            }

            team.TitlesWon.Add(season);

            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveTitleAsync(int teamId, int seasonId)
        {
            var team = await this.db.Teams.FindAsync(teamId);
            var season = await this.db.Seasons.FindAsync(seasonId);

            if (team == null || season == null)
            {
                return false;
            }

            season.TitleWinner = null;
            team.TitlesWon.Remove(season);

            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddSeasonStatistic(int teamId, int seasonStatisticId)
        {
            var team = await this.db.Teams
                .Include(t => t.SeasonsStatistics)
                .Where(t => t.Id == teamId)
                .FirstOrDefaultAsync();

            var seasonStatistic = await this.db.SeasonStatistics.FindAsync(seasonStatisticId);

            if (team == null || seasonStatistic == null)
            {
                return false;
            }

            team.SeasonsStatistics.Add(seasonStatistic);

            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveSeasonStatistic(int teamId, int seasonStatisticId)
        {
            var team = await this.db.Teams.FindAsync(teamId);
            var seasonStatistic = await this.db.SeasonStatistics.FindAsync(seasonStatisticId);

            if (team == null || seasonStatistic == null)
            {
                return false;
            }

            team.SeasonsStatistics.Remove(seasonStatistic);

            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task EditTeamAsync(GetTeamDetailsServiceModel model, int id, IFormFile fullImage, IFormFile smallImage)
        {
            var team = await this.db.Teams.FindAsync(id);

            if (fullImage != null)
            {
                var imageId = await this.imageService.UploadImageAsync(fullImage);
                team.FullImageId = imageId;
            }
            if (smallImage != null)
            {
                var smallImageId = await this.imageService.UploadImageAsync(smallImage);
                team.SmallImageId = smallImageId;
            }

            team.Name = model.Name;
            team.AbbreviatedName = model.AbbreviatedName;
            team.Conference = (ConferenceType)Enum.Parse(typeof(ConferenceType), model.Conference);
            team.Venue = model.Venue;
            team.Instagram = model.Instagram;
            team.Twitter = model.Twitter;
        }

        public async Task<ICollection<TeamListingSerivceModel>> GetAllTeamsAsync()
        {
            var teams = await this.db.Teams.OrderBy(t => t.Name).ToListAsync();

            var models = new List<TeamListingSerivceModel>();
            foreach (var team in teams)
            {
                models.Add(mapper.Map<TeamListingSerivceModel>(team));
            }

            return models;
        }

        private async Task<int> GetGamesPlayed(int teamId, int seasonId)
        {
            var team = await this.db.Teams
                .Include(t => t.HomeGames)
                .Include(t => t.AwayGames)
                .Where(t => t.Id == teamId)
                .FirstOrDefaultAsync();

            var homeGames = team.HomeGames.Where(g => g.SeasonId == seasonId && g.IsFinished == true && g.IsPlayoffGame == false).ToList();
            var awayGames = team.AwayGames.Where(g => g.SeasonId == seasonId && g.IsFinished == true && g.IsPlayoffGame == false).ToList();

            return homeGames.Count() + awayGames.Count();
        }

        private async Task<string> GetLastTenGames(int teamId, int seasonId)
        {
            var team = await this.db.Teams
                .Include(t => t.HomeGames)
                .Include(t => t.AwayGames)
                .Where(t => t.Id == teamId)
                .FirstOrDefaultAsync();

            var gamesWon = 0;
            var gamesLost = 0;

            var gamesPlayed = new List<Game>();

            gamesPlayed.AddRange(team.HomeGames.Where(g => g.SeasonId == seasonId && g.IsFinished == true && g.IsPlayoffGame == false));
            gamesPlayed.AddRange(team.AwayGames.Where(g => g.SeasonId == seasonId && g.IsFinished == true && g.IsPlayoffGame == false));

            var lastTenGames = gamesPlayed.OrderByDescending(g => g.Date).Take(10).ToList();

            foreach (var game in lastTenGames)
            {
                var winnerName = await this.gameService.GetWinnerAsync(game.Id);

                if (winnerName == team.Name)
                {
                    gamesWon++;
                }
                else
                {
                    gamesLost++;
                }
            }

            return $"{gamesWon}-{gamesLost}";
        }

        private ICollection<TeamGameOverviewServiceModel> GetLastNineGames(Team team)
        {
            var teamHomeGames = team.HomeGames;
            var teamAwayGames = team.AwayGames;

            var teamGames = new List<Game>();

            teamGames.AddRange(teamHomeGames);
            teamGames.AddRange(teamAwayGames);

            var last9 = teamGames.Where(g => g.IsFinished == true).OrderByDescending(g => g.Date).ToList().Take(9).ToList();

            var lastNineGames = new List<TeamGameOverviewServiceModel>();

            foreach (var game in last9)
            {
                var model = mapper.Map<TeamGameOverviewServiceModel>(game);
                model.TeamHostName = game.TeamHost.AbbreviatedName;
                model.Team2Name = game.Team2.AbbreviatedName;

                if (model.TeamHostName == team.AbbreviatedName)
                {
                    model.IsHomeGame = true;
                }
                else
                {
                    model.IsHomeGame = false;
                }

                lastNineGames.Add(model);
            }

            return lastNineGames.OrderByDescending(g => DateTime.ParseExact(g.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
        }

        private int GetSeasonYear(DateTime date)
        {
            var seasonYear = 0;

            if (date.Month >= 9)
            {
                seasonYear = date.Year + 1;
            }
            else if (date.Month < 9)
            {
                seasonYear = date.Year;
            }

            return seasonYear;
        }

        private int GetCurrentSeasonYear()
        {
            var currentYear = 0;

            if (DateTime.Now.Month >= 9)
            {
                currentYear = DateTime.Now.Year + 1;
            }
            else if (DateTime.Now.Month < 9)
            {
                currentYear = DateTime.Now.Year;
            }

            return currentYear;
        }
    }
}
