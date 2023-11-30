namespace Logitude.Base.Models.Partners
{
    public class Address
    {
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AddressTypeId { get; set; }
        public string Address1 { get; set; }
        public string CountryId { get; set; }
        public string StateId { get; set; }
        public bool IsCreatedWithPartner { get; set; }
    }
}