using HouseRentingAPI.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HouseRentingAPI.Model
{
    public class HouseDto
    {
        public Guid HouseID { get; set; }
        public Guid LandlordID { get; set; }
        public Guid PropertyTypeID { get; set; }
        public int Price { get; set; }
        public int Distance { get; set; }
        public string HouseName { get; set; }
        public string Address { get; set; }

        public Landlord Landlord { get; set; }

        public PropertyType PropertyType { get; set; }

        public HouseFacility HouseFacilities { get; set; }
        public HouseOtherAttribute HouseOtherAttributes { get; set; }
        public Favorite Favorites { get; set; }
        public ComparisonList ComparisonLists { get; set; }
    }

    public class HouseAddDto
    {
        public string Housename {  get; set; }
        public string Address { get; set;}
        public string? Description { get; set; }
        public int Price { get; set; }
        public int? Distance { get; set; }
        public string Landlordname { get; set; }
        public int PropertyTypeID { get; set; }
        public List<int> FacilityIDs { get; set; }
        public List<int> AttributeIDs { get; set; }
    }

    public class UpdateHouseDto
    {
        public Guid HouseID { get; set; }
        public string Housename { get; set; }
        public string Address { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public int? Distance { get; set; }
        public string Landlordname { get; set; }
        public int PropertyTypeID { get; set; }
        public List<int> FacilityIDs { get; set; }
        public List<int> AttributeIDs { get; set; }
    }

    public class GetHouseDto
    {
        public Guid HouseID { get; set; }
        public string Housename { get;set; }
        public string Address { get; set; }
        public string Landlordname { get; set; }
        public int Price { get; set; }
    }

    public class GetHouseByIdDto
    {
        public string Housename { get; set; }
        public string Address { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public int? Distance { get; set; }
        public string Landlordname { get; set; }
        public int PropertyTypeID { get; set; }
        public List<int> FacilityIDs { get; set; }
        public List<int> AttributeIDs { get; set; }
    }

}
