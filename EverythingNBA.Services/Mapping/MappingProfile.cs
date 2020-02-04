namespace EverythingNBA.Services.Mapping
{
    using AutoMapper;
    using System.Linq;
    using System;

    using EverythingNBA.Models;
    using EverythingNBA.Services.Models;
    using EverythingNBA.Services.Models.AllStarTeam;
    using EverythingNBA.Services.Models.Season;
    using EverythingNBA.Models.Enums;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<GetSeasonStatisticDetailsServiceModel, SeasonStatistic>();
            this.CreateMap<AllStarTeam, GetAllStarTeamServiceModel>()
                .ForMember(mdl => mdl.Players, opt => opt.MapFrom(ast => ast.Players))
                .ForMember(mdl => mdl.Type, opt => opt.MapFrom(ast => ast.Type.ToString()));
            this.CreateMap<Season, GetSeasonListingServiceModel>();

            //this.CreateMap<GetAllStarTeamBySeasonServiceModel, AllStarTeam>()
            //    .ForMember(at => at.Players, opt => opt.MapFrom(a => a.Players));

        }
    }
}
