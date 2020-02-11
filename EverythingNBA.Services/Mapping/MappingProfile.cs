namespace EverythingNBA.Services.Mapping
{
    using AutoMapper;
    using System.Linq;
    using System;

    using EverythingNBA.Models;
    using EverythingNBA.Services.Models;
    using EverythingNBA.Services.Models.AllStarTeam;
    using EverythingNBA.Services.Models.Season;
    using EverythingNBA.Services.Models.Playoff;
    using EverythingNBA.Services.Models.Player;
    using EverythingNBA.Services.Models.Team;
    using EverythingNBA.Services.Models.Game;
    using EverythingNBA.Services.Models.GameStatistic;
    using EverythingNBA.Models.Enums;
    using EverythingNBA.Services.Models.Series;
    using System.Globalization;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<SeasonStatistic, GetSeasonStatisticDetailsServiceModel>().ReverseMap();

            this.CreateMap<AllStarTeam, GetAllStarTeamServiceModel>()
                .ForMember(mdl => mdl.Players, opt => opt.MapFrom(ast => ast.Players))
                .ForMember(mdl => mdl.Type, opt => opt.MapFrom(ast => ast.Type.ToString()));

            this.CreateMap<Season, GetSeasonListingServiceModel>();

            this.CreateMap<Playoff, GetPlayoffServiceModel>();

            this.CreateMap<Series, GetSeriesDetailsServiceModel>();

            this.CreateMap<Team, GetTeamDetailsServiceModel>()
            .ForMember(mdl => mdl.CurrentSeasonStatistic, opt => opt.Ignore())
            .ForMember(mdl => mdl.CurrentSeasonGames, opt => opt.Ignore())
            //.ForMember(mdl => mdl.CurrentPlayers, opt => opt.Ignore())
            .ForMember(mdl => mdl.TitlesWon, opt => opt.MapFrom(t => t.TitlesWon.Select(x => x.Year).ToList()));

            this.CreateMap<Player, PlayerOverviewServiceModel>()
                .ForMember(mdl => mdl.Name, opt => opt.MapFrom(p => p.FirstName + " " + p.LastName))
                .ForMember(mdl => mdl.Position, opt => opt.MapFrom(p => p.Position.ToString()));
            //.ForMember(mdl => mdl.PointsPerGame, opt => opt.Ignore())
            //.ForMember(mdl => mdl.AssistsPerGame, opt => opt.Ignore())
            //.ForMember(mdl => mdl.ReboundsPerGame, opt => opt.Ignore());

            //this.CreateMap<GetAllStarTeamBySeasonServiceModel, AllStarTeam>()
            //    .ForMember(at => at.Players, opt => opt.MapFrom(a => a.Players));

            this.CreateMap<Player, PlayerDetailsServiceModel>()
                .ForMember(mdl => mdl.CurrentTeam, opt => opt.MapFrom(p => p.Team.Name))
                .ForMember(mdl => mdl.Position, opt => opt.MapFrom(p => p.Position.ToString()));

            this.CreateMap<Game, GameOverviewServiceModel>();

            this.CreateMap<Game, GameDetailsServiceModel>();
        }
    }
}
