﻿namespace EverythingNBA.Services.Mapping
{
    using AutoMapper;
    using System.Linq;
    using System;

    using EverythingNBA.Models;
    using EverythingNBA.Services.Models;
    using EverythingNBA.Services.Models.AllStarTeam;
    using EverythingNBA.Services.Models.Season;
    using EverythingNBA.Services.Models.Playoff;
    using EverythingNBA.Models.Enums;
    using EverythingNBA.Services.Models.Series;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<SeasonStatistic, GetSeasonStatisticDetailsServiceModel>();
            this.CreateMap<AllStarTeam, GetAllStarTeamServiceModel>()
                .ForMember(mdl => mdl.Players, opt => opt.MapFrom(ast => ast.Players))
                .ForMember(mdl => mdl.Type, opt => opt.MapFrom(ast => ast.Type.ToString()));
            this.CreateMap<Season, GetSeasonListingServiceModel>();
            this.CreateMap<Playoff, GetPlayoffServiceModel>();
            this.CreateMap<Series, GetSeriesDetailsServiceModel>();

            //this.CreateMap<GetAllStarTeamBySeasonServiceModel, AllStarTeam>()
            //    .ForMember(at => at.Players, opt => opt.MapFrom(a => a.Players));

        }
    }
}
