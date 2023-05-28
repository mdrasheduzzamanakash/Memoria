using Authentication.Models.DTO.Incomming;
using AutoMapper;
using Memoria.Entities.DTOs.Incomming;
using Memoria.Entities.DTOs.Outgoing;
using MemoriaMVC.ViewModel.Attachment;
using MemoriaMVC.ViewModel.HomePageViewModel;
using MemoriaMVC.ViewModel.UserPageViewModel;
using Microsoft.AspNetCore.Identity;

namespace MemoriaMVC.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserCreationViewModel, UserSingleInDTO>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => ConvertToByteArray(src.Image)))
                .ForMember(dest => dest.FileFormat, opt => opt.MapFrom(src => src.Image.ContentType));

            CreateMap<UserEditViewModel, UserSingleInDTO>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => ConvertToByteArray(src.UpdatedImage)))
                .ForMember(dest => dest.FileFormat, opt => opt.MapFrom(src => src.UpdatedImage.ContentType));

            CreateMap<UserSingleOutDTO, UserDetailsViewModel>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => ConvertToString(src.Image)));

            CreateMap<UserSingleOutDTO, HomeIndexViewModel>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => ConvertToString(src.Image)));


            CreateMap<UserSingleOutDTO, UserDeletetionViewMode>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => ConvertToString(src.Image)));

            CreateMap<UserSingleOutDTO, UserEditViewModel>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => ConvertToString(src.Image)));

            CreateMap<AttachmentViewModel, AttachmentSingleInDTO>();


        }

        private static string ConvertToString(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
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
