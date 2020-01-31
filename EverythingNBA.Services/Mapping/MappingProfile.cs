namespace EverythingNBA.Services.Mapping
{
    using AutoMapper;

    using EverythingNBA.Models;
    using EverythingNBA.Services.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<GetSeasonStatisticDetailsServiceModel, SeasonStatistic>();
            // Additional mappings here...
        }
    }
}
