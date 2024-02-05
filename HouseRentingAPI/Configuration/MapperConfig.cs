using AutoMapper;
using HouseRentingAPI.Data;
using HouseRentingAPI.Model;
using System.Diagnostics.Metrics;

namespace HouseRentingAPI.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User, BaseUserDto>().ReverseMap();
            CreateMap<User, UserRegisterDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<User, GetUserDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();

        }
    }
}
