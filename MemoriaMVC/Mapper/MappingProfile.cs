using AutoMapper;
using Memoria.Entities.DTOs.Incomming;
using MemoriaMVC.ViewModel.UserPageViewModel;

namespace MemoriaMVC.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserCreationViewModel, UserCreationDTO>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => ConvertToByteArray(src.Image)))
                .ForMember(dest => dest.FileFormat, opt => opt.MapFrom(src => src.Image.ContentType));
        }

        private static byte[] ConvertToByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
