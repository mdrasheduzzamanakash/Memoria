using AutoMapper;
using Memoria.Entities.DbSet;
using Memoria.Entities.DTOs.Incomming;
using Memoria.Entities.DTOs.Outgoing;

namespace Memoria.DataService.Mapper
{
    public static class AutoMapperConfig
    {
        public static IMapper Configure()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserSingleInDTO, User>();
                cfg.CreateMap<User, UserSingleOutDTO>();
                cfg.CreateMap<Label, LabelSingleOutDTO>();
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            return mapper;
        }
    }
}

