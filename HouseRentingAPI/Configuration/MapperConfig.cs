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
            CreateMap<HouseAddDto, House>()
                .ForMember(dest => dest.HousePhotos, opt => opt.Ignore());
            CreateMap<House, UpdateHouseDto>().ReverseMap();
            CreateMap<House, GetHouseDto>().ReverseMap();
            CreateMap<House, GetHouseByIdDto>()
                .ForMember(dest => dest.FacilityIDs, opt => opt.MapFrom(src => src.HouseFacilities.Select(f => f.FacilityID).ToList()))
                .ForMember(dest => dest.AttributeIDs, opt => opt.MapFrom(src => src.HouseOtherAttributes.Select(a => a.AttributeID).ToList()))
                .ForMember(dest => dest.Landlordname, opt => opt.MapFrom(src => src.Landlord.Landlordname));
            CreateMap<IFormFile, Photo>()
                .ForMember(dest => dest.PhotoURL, opt => opt.MapFrom(src => src.FileName));

            CreateMap<Favorite, FavoriteDto>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Comment, CommentUpdateDto>().ReverseMap();
        }
    }
}
