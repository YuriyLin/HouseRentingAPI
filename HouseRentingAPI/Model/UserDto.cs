using HouseRentingAPI.Data;

namespace HouseRentingAPI.Model
{

    public class UserRegisterDto 
    {
        public string StuId { get; set; }
        public string Name { get; set; }
        public string PhoneNum { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginDto
    {
        public string StuId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class GetUserDto
    {
        public Guid Id { get; set; }
        public string StuId { get; set; }
        public string Name { get; set; }
    }

    public class GetUserByIdDto
    {
        public string StuId { get; set; }
        public string Name { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
    }

    public class UpdateUserDto
    {
        public Guid Id { get; set; }
        public string StuId { get; set; }
        public string Name { get; set; }
        public string? PhoneNum { get; set; }
        public string? Email { get; set; }
    }

    public class UpdateUserPasswordDto
    {
        public Guid Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
