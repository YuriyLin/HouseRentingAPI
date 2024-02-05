namespace HouseRentingAPI.Model
{
    public class BaseUserDto
    {
        public string StuId { get; set; }
        public string Name { get; set; }
    }

    public class UserRegisterDto : BaseUserDto
    {
        public string PhoneNum { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginDto : BaseUserDto
    {
        public string Password { get; set; }
    }

    public class GetUserDto : BaseUserDto
    {
        public Guid Id { get; set; }
    }

    public class UpdateUserDto : BaseUserDto
    {
        public Guid Id { get; set; }
        public string? PhoneNum { get; set; }
        public string? Email { get; set; }
    }
}
