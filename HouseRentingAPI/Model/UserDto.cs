using HouseRentingAPI.Data;
using System.ComponentModel.DataAnnotations;

namespace HouseRentingAPI.Model
{

    public class UserRegisterDto 
    {
        [Required]
        public string StuId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNum { get; set; }
        public string? Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public IFormFile StudentIdCard { get; set; }
        public string? StudentIdCardPath { get; set; }
    }

    public class UserLoginDto
    {
        public string StuId { get; set; }
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
        public string? StuId { get; set; }
        public string? Name { get; set; }
        public string? PhoneNum { get; set; }
        public string? Email { get; set; }
    }

    public class UpdateUserPasswordDto
    {
        public Guid Id { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmNewPassword { get; set; }
    }
}
