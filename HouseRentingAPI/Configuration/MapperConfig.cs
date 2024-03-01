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
            CreateMap<User, UserRegisterDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<User, GetUserDto>().ReverseMap();
            CreateMap<User, GetUserByIdDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();
            CreateMap<User, UpdateUserPasswordDto>().ReverseMap();

            CreateMap<Landlord, LandlordRegisterDto>().ReverseMap();
            CreateMap<Landlord, LandlordLoginDto>().ReverseMap();
            CreateMap<Landlord, GetLandlordDto>().ReverseMap();
            CreateMap<Landlord,GetLandlordByIdDto>().ReverseMap();
            CreateMap<Landlord, UpdateLandlordDto>().ReverseMap();
            CreateMap<Landlord, UpdateLandlordPasswordDto>().ReverseMap();

            CreateMap<House, HouseAddDto>().ReverseMap();
            CreateMap<House, UpdateHouseDto>().ReverseMap();
            CreateMap<House, GetHouseDto>().ReverseMap();
            CreateMap<House, GetHouseByIdDto>().ReverseMap();

            CreateMap<Favorite, FavoriteDto>().ReverseMap();
            CreateMap<Comment, CommentAddDto>().ReverseMap();
        }
    }
}
