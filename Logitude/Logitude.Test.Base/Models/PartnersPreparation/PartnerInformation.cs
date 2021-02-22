namespace Logitude.Test.Base.Models.PartnersPreparation
{
    public class PartnerInformation
    {
        public int Tenant { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string PartnerTypeId { get; set; }
        public string CarrierTypeId { get; set; }
        public string Code { get; set; }
        public bool IsCustomer { get; set; }
    }
}