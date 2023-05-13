using AutoMapper;
using Memoria.Entities.DbSet;
using Memoria.Entities.DTOs.Incomming;

namespace Memoria.DataService.Mapper
{
    public static class AutoMapperConfig
    {
        public static IMapper Configure()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserCreationDTO, User>();
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            return mapper;
        }
    }
}

