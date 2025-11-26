using AutoMapper;
using TrabajoPracticoApp.Dtos;
using TrabajoPracticoApp.Models;

namespace TrabajoPracticoApp.MappingProfiles
{
    public class UserProfile : Profile
    {


        public UserProfile()
        {
            CreateMap<User, LoginResponseDto>()
            .ForMember(dest => dest.AccesToken, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
            .ForMember(dest => dest.ExpireAt, opt => opt.Ignore());

            CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.EmailConfirmationToken, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshTokenExpiryTime, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordResetToken, opt => opt.Ignore())
            .ForMember(dest => dest.ResetTokenExpiryTime, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username));
        }

       
    }
}
