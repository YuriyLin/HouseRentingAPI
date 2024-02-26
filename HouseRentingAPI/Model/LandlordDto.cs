using HouseRentingAPI.Data;
using System.ComponentModel.DataAnnotations;

namespace HouseRentingAPI.Model
{
    public class LandlordLoginDto
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LandlordRegisterDto
    {
        [Required]
        public string Landlordname {  get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string? LineId {  get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class GetLandlordDto
    {
        public Guid LandlordID { get; set; }
        public string Landlordname { get; set; }
        public string Phone { get; set; }
        public string? LineID { get; set; }
    }
    public class GetLandlordByIdDto
    {
        public string Landlordname { get; set; }
        public string Phone { get; set; }
        public string? LineID { get; set; }
        public House house { get; set; }
    }

    public class UpdateLandlordDto
    {
        public Guid LandlordID { get; set; }
        public string? Landlordname { get; set; }
        public string? Phone { get; set; }
        public string? LineId { get; set; }
        public string? Password { get; set; }
    }

    public class UpdateLandlordPasswordDto
    {
        public Guid LandlordID { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
