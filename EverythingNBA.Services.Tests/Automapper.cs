namespace EverythingNBA.Services.Tests
{
    using AutoMapper;

    using EverythingNBA.Web.Mapping;

    public class AutomapperSingleton
    {
        private static IMapper mapper;
        public static IMapper Mapper
        {
            get
            {
                if (mapper == null)
                {
                    // Auto Mapper Configurations
                    var mappingConfig = new MapperConfiguration(mc =>
                    {
                        mc.AddProfile(new MappingProfile());
                    });
                    IMapper mapper = mappingConfig.CreateMapper();
                    AutomapperSingleton.mapper = mapper;
                }
                return mapper;
            }
        }
    }
}
