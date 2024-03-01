using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HouseRentingAPI.Data
{
    public class House
    {
        [Key]
        public Guid HouseID { get; set; }
        public Guid LandlordID { get; set; }
        public int PropertyTypeID { get; set; }
        public int Price { get; set; }
        public int Distance { get; set; }
        public string Description { get; set; }
        [Required]
        public string HouseName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string SquareFeet { get; set; }

        [ForeignKey("LandlordID")]
        public Landlord Landlord { get; set; }

        [ForeignKey("PropertyTypeID")]
        public PropertyType PropertyType { get; set; }

        public ICollection<HouseFacility> HouseFacilities { get; set; } = new List<HouseFacility>();
        public ICollection<HouseOtherAttribute> HouseOtherAttributes { get; set; } = new List<HouseOtherAttribute>();
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<ComparisonList> ComparisonLists { get; set; }
    }

    public class HouseFacility
    {
        [Key]
        public Guid HouseID { get; set; }
        [Key]
        public int FacilityID { get; set; }

        public House House { get; set; }
        public Facility Facility { get; set; }
    }

    public class HouseOtherAttribute
    {
        [Key]
        public Guid HouseID { get; set; }
        [Key]
        public int AttributeID { get; set; }

        public House House { get; set; }
        public OtherAttribute OtherAttribute { get; set; }
    }

    public class PropertyType
    {
        [Key]
        public int PropertyTypeID { get; set; }
        [Required]
        public string TypeName { get; set; }
        public ICollection<House> Houses { get; set; }
    }

    public class Facility
    {
        [Key]
        public int FacilityID { get; set; }
        [Required]
        public string FacilityName { get; set; }
        public ICollection<HouseFacility> HouseFacilities { get; set; }
    }

    public class OtherAttribute
    {
        [Key]
        public int AttributeID { get; set; }
        [Required]
        public string AttributeName { get; set; }
        public ICollection<HouseOtherAttribute> HouseOtherAttributes { get; set; }
    }
}
