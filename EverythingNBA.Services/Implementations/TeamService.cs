namespace EverythingNBA.Services.Implementations
{
    using EverythingNBA.Data;
    using EverythingNBA.Models;
    using EverythingNBA.Models.Enums;
    using EverythingNBA.Services.Models;
    using EverythingNBA.Services.Models.Team;

    using Microsoft.AspNetCore.Http;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using EverythingNBA.Services.Models.Player;

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
            player.TeamId = teamId;
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
            player.TeamId = null;
            player.Team = null;

            await this.db.SaveChangesAsync();

            return true;

        }

        public async Task<int> AddTeamAsync(string name, /*IFormFile imageFile,*/ string conference, string venue, string instagram, string twitter)
        {
            //var imageId = await this.imageService.UploadImageAsync(imageFile);

            var teamObj = new Team
            {
                Name = name,
                //CloudinaryImageId = imageId,
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
                .ToList();

            foreach (var team in teams)
            {
                var seasonStatisicmodel = await statisticService.GetDetailsAsync(seasonId, team.Id);
                var seasonStatistic = mapper.Map<SeasonStatistic>(seasonStatisicmodel);
                var winPercentage = await statisticService.GetWinPercentageAsync(seasonStatistic.Id);
                var gamesPlayed = this.GetGamesPlayed(team, seasonId);
                var lastTenGames = await this.GetLastTenGames(team, seasonId);

                var teamStatModel = new TeamSeasonStatisticServiceModel
                {
                    Name = team.Name,
                    //TeamLogoImageURL = team.CloudinaryImage.ImageURL,
                    Conference = team.Conference.ToString(),
                    Wins = seasonStatistic.Wins,
                    Losses = seasonStatistic.Losses,
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


        public async Task<GetTeamDetailsServiceModel> GetTeamDetailsAsync(int teamId)
        {
            var team = await this.db.Teams
                .Include(t => t.SeasonsStatistics)
                .Include(t => t.TitlesWon)
                .Include(t => t.HomeGames)
                .Include(t => t.AwayGames)
                .Include(t => t.GamesWon)
                .Where(t => t.Id == teamId)
                .FirstOrDefaultAsync();

            var seasonId = await this.db.Seasons.Where(s => s.Year == DateTime.Now.Year).Select(s => s.Id).FirstOrDefaultAsync();

            var teamDetailsModel = mapper.Map<GetTeamDetailsServiceModel>(team);

            var players = team.Players.ToList();
            var playerModels = new List<PlayerOverviewServiceModel>();

            foreach (var player in players)
            {
                var playerOverviewModel = mapper.Map<PlayerOverviewServiceModel>(player);

                //var pointsPerGame = player.SeasonStatistics.Where(ss => ss.SeasonId == seasonId).Select(ss => ss.Points).FirstOrDefault();
                //var assistsPerGame = player.SeasonStatistics.Where(ss => ss.SeasonId == seasonId).Select(ss => ss.Assists).FirstOrDefault();
                //var reboundsPerGame = player.SeasonStatistics.Where(ss => ss.SeasonId == seasonId).Select(ss => ss.AverageRebounds).FirstOrDefault();
                var seasonStatsModel = await this.playerService.GetSeasonStatistics(player.Id, seasonId);

                var pointsPerGame = seasonStatsModel.AveragePoints;
                var assistsPerGame = seasonStatsModel.AverageAssists;
                var reboundsPerGame = seasonStatsModel.AverageRebounds;
                var stealsPerGame = seasonStatsModel.AverageSteals;
                var blocksPerGame = seasonStatsModel.AverageBlocks;
                
                playerOverviewModel.PointsPerGame = pointsPerGame;
                playerOverviewModel.AssistsPerGame = assistsPerGame;
                playerOverviewModel.ReboundsPerGame = reboundsPerGame;

                playerModels.Add(playerOverviewModel);
            }

            teamDetailsModel.Players = playerModels;

            var games = new List<Game>();
            games.AddRange(team.HomeGames);
            games.AddRange(team.AwayGames);
            teamDetailsModel.CurrentSeasonGames = games.OrderByDescending(g => g.Date).ToList();
         
            teamDetailsModel.CurrentSeasonStatistic = team.SeasonsStatistics.Where(ss => ss.SeasonId == seasonId && ss.TeamId == teamId).FirstOrDefault();

            teamDetailsModel.TitlesWon = team.TitlesWon.Select(t => t.Year).ToList();

            return teamDetailsModel;
        }

        public async Task<GetTeamDetailsServiceModel> GetTeamDetailsAsync(string name)
        {
            var teamId = await this.db.Teams
                .Where(t => t.Name.ToLower() == name.ToLower()).Select(t => t.Id).FirstOrDefaultAsync();

            return await this.GetTeamDetailsAsync(teamId);
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

            if (game.TeamHostId == teamId)
            {
                team.HomeGames.Add(game);
                teamIsHost = true;
            }
            else if(game.Team2Id == teamId)
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
                }
                else
                {
                    if (teamIsHost == false)
                    {
                        team.GamesWon.Add(game);
                    }
                }
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

        public async Task<bool> AddTitleAsync(int teamId, int seasonId)
        {
            var team = await this.db.Teams
                .Include(t => t.TitlesWon)
                .Where(t => t.Id == teamId)
                .FirstOrDefaultAsync();

            var season = await this.db.Seasons
                .Include(s => s.TitleWinner)
                .Where(s => s.Id == seasonId)
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

        private int GetGamesPlayed(Team team, int seasonId)
        {
            var homeGames = team.HomeGames.Where(g => g.SeasonId == seasonId).ToList();
            var awayGames = team.AwayGames.Where(g => g.SeasonId == seasonId).ToList();

            return homeGames.Count() + awayGames.Count();
        }

        private async Task<string> GetLastTenGames(Team team, int seasonId)
        {
            var gamesWon = 0;
            var gamesLost = 0;

            var gamesPlayed = new List<Game>();

            gamesPlayed.AddRange(team.HomeGames);
            gamesPlayed.AddRange(team.AwayGames);

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

        public async Task EditTeamAsync(GetTeamDetailsServiceModel model, int id)
        {
            var team = await this.db.Teams.FindAsync(id);

            team.Name = model.Name;
            team.AbbreviatedName = model.AbbreviatedName;
            team.CloudinaryImageId = model.CloudinaryImageId;
            team.Conference = (ConferenceType)Enum.Parse(typeof(ConferenceType), model.Conference);
            team.Venue = model.Venue;
            team.Instagram = model.Instagram;
            team.Twitter = model.Twitter;
        }
    }
}
