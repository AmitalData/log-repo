namespace Logitude.Test.Base.Models.PartnersPreparation
{
    public class Address
    {
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AddressTypeId { get; set; }
        public bool IsCreatedWithPartner { get; set; }
    }
}