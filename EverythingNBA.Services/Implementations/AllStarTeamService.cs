

namespace EverythingNBA.Services.Implementations
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper;

    using EverythingNBA.Models;
    using EverythingNBA.Data;
    using EverythingNBA.Models.Enums;
    using EverythingNBA.Services.Models.AllStarTeam;
    using EverythingNBA.Models.MappingTables;
    using EverythingNBA.Services.Models.Player;

    public class AllStarTeamService : IAllStarTeamService
    {
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;

        public AllStarTeamService(EverythingNBADbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<int?> AddAllStarTeamAsync(int year, string type, ICollection<string> playerNames)
        {
            var allStarTeamObj = new AllStarTeam
            {
                Year = year,
                Type = (AllStarTeamType)Enum.Parse(typeof(AllStarTeamType), type)
            };

            var allStarTeamsPlayersList = new List<AllStarTeamsPlayers>();

            foreach (var name in playerNames)
            {
                var player = await this.db.Players.Where(p => p.FirstName + " " + p.LastName == name).FirstOrDefaultAsync();

                if (player == null)
                {
                    return null;
                }

                var obj = new AllStarTeamsPlayers
                {
                    AllStarTeam = allStarTeamObj,
                    Player = player
                };

                allStarTeamsPlayersList.Add(obj);
            }

            allStarTeamObj.Players = allStarTeamsPlayersList;

            this.db.AllStarTeams.Add(allStarTeamObj);
            await this.db.SaveChangesAsync();

            return allStarTeamObj.Id;
        }

        public async Task<bool> RemovePlayerAsync(int allStarTeamId, string playerName)
        {
            var allStarTeam = await this.db.AllStarTeams
                .Include(ast => ast.Players)
                .Where(ast => ast.Id == allStarTeamId)
                .FirstOrDefaultAsync();

            if (allStarTeam == null)
            {
                return false;
            }

            var obj = await this.db.AllStarTeamsPlayers
                .Where(x => x.Player.FirstName + " " + x.Player.LastName == playerName && x.AllStarTeamId == allStarTeamId)
                .FirstOrDefaultAsync();

            var player = await this.db.Players
                .Include(p => p.AllStarTeams)
                .Where(p => p.FirstName + " " + p.LastName == playerName)
                .FirstOrDefaultAsync();

            player.AllStarTeams.Remove(obj);

            var IsSucessfulllyRemoved = allStarTeam.Players.Remove(obj);

            if (!IsSucessfulllyRemoved)
            {
                return false;
            }

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAllStarTeamAsync(int allStarTeamId)
        {
            var teamToDelete = await this.db.AllStarTeams.FindAsync(allStarTeamId);

            if (teamToDelete == null)
            {
                return false;
            }

            var season = await this.db.Seasons
                .Include(s => s.AllStarTeams)
                .Where(s => s.Year == teamToDelete.Year)
                .FirstOrDefaultAsync();

            season.AllStarTeams.Remove(teamToDelete);

            var player = await this.db.Players
                .Include(p => p.AllStarTeams)
                .Where(p => p.AllStarTeams.Where(ast => ast.AllStarTeam == teamToDelete)
                                          .Select(ast => ast.AllStarTeam)
                                          .FirstOrDefault() == teamToDelete)
                .FirstOrDefaultAsync();

            var obj = await this.db.AllStarTeamsPlayers
                .Where(x => x.Player == player && x.AllStarTeam == teamToDelete)
                .FirstOrDefaultAsync();

            player.AllStarTeams.Remove(obj);

            this.db.AllStarTeams.Remove(teamToDelete);
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<GetAllStarTeamServiceModel>> GetAllASTeamsAsync(string type)
        {
            var enumType = (AllStarTeamType)Enum.Parse(typeof(AllStarTeamType), type);
            var allStarTeams = await this.db.AllStarTeams
                .Include(ast => ast.Players)
                    .ThenInclude(x => x.Player)
                .Where(ast => ast.Type == enumType)
                .ToListAsync();

            var allStarTeamServiceModels = new List<GetAllStarTeamServiceModel>();

            foreach (var (team, players, allStarTeamsPlayers) in from team in allStarTeams
                                                                 let players = new List<Player>()
                                                                 let allStarTeamsPlayers = team.Players.ToList()
                                                                 select (team, players, allStarTeamsPlayers))
            {
                allStarTeamsPlayers.ForEach(x => players.Add(x.Player));
                var model = new GetAllStarTeamServiceModel
                {
                    Type = team.Type.ToString(),
                    Year = team.Year,
                    Players = players
                };
                allStarTeamServiceModels.Add(model);
            }

            return allStarTeamServiceModels;
        }

        public async Task<ICollection<GetAllStarTeamServiceModel>> GetAllASTeamsAsync(int Year)
        {
            var allStarTeam = await this.db.AllStarTeams.Where(ast => ast.Year == Year).FirstOrDefaultAsync();

            var result = await this.GetAllASTeamsAsync(allStarTeam.Type.ToString());

            return result;
        }

        public async Task<GetAllStarTeamServiceModel> GetAllStarTeamAsync(int id)
        {
            var allStarTeam = await this.db.AllStarTeams
                .Include(ast => ast.Players)
                    .ThenInclude(x => x.Player)
                .Where(ast => ast.Id == id)
                .FirstOrDefaultAsync();

            var playersList = new List<Player>();

            foreach (var player in allStarTeam.Players)
            {
                playersList.Add(player.Player);
            }

            var model = new GetAllStarTeamServiceModel
            {
                Type = allStarTeam.Type.ToString(),
                Year = allStarTeam.Year,
                Players = playersList
            };

            return model;
        }

        public async Task<GetAllStarTeamServiceModel> GetAllStarTeamByTypeAndYearAsync(string type, int Year)
        {
            var enumType = (AllStarTeamType)Enum.Parse(typeof(AllStarTeamType), type);
            var allStarTeam = await this.db.AllStarTeams.Where(ast => ast.Type == enumType && ast.Year == Year).FirstOrDefaultAsync();

            return await this.GetAllStarTeamAsync(allStarTeam.Id);
        }

        public async Task EditAllStarTeam(int allStarTeamId, List<string> playerNames)
        {
            var team = await this.db.AllStarTeams
                .Include(ast => ast.Players)
                .Where(ast => ast.Id == allStarTeamId)
                .FirstOrDefaultAsync();

            var player1 = await this.db.Players.Where(p => p.FirstName + " " + p.LastName == playerNames[0]).FirstOrDefaultAsync();
            var player2 = await this.db.Players.Where(p => p.FirstName + " " + p.LastName == playerNames[1]).FirstOrDefaultAsync();
            var player3 = await this.db.Players.Where(p => p.FirstName + " " + p.LastName == playerNames[2]).FirstOrDefaultAsync();
            var player4 = await this.db.Players.Where(p => p.FirstName + " " + p.LastName == playerNames[3]).FirstOrDefaultAsync();
            var player5 = await this.db.Players.Where(p => p.FirstName + " " + p.LastName == playerNames[4]).FirstOrDefaultAsync();

            var players = new List<Player> { player1, player2, player3, player4, player5 };

            var astPlayerObjects = await this.db.AllStarTeamsPlayers
                    .Include(x => x.Player)
                    .Where(x => x.AllStarTeamId == allStarTeamId)
                    .ToListAsync();

            for (int i = 0; i < astPlayerObjects.Count(); i++)
            {
                astPlayerObjects[i].Player = players[i];
            }
        }
    }
}
