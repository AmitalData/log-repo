namespace Logitude.Test.Base.Models
{
    public class Partner
    {
        public int Tenant { get; set; }
        public string PartnerId { get; set; }
        public string PartnerTypeId { get; set; }
        //public Address Address { get; set; }
        //public Contact Contact { get; set; }
        public PartnerInformation Agent { get; set; }
        public PartnerInformation Customer { get; set; }
        public PartnerInformation CustomAgent { get; set; }
        public PartnerInformation ShippingAgent { get; set; }
        public PartnerInformation Vendor { get; set; }
        public PartnerInformation Warehouse { get; set; }
        //public Airline Airline { get; set; }
        //public ShippingLine ShippingLine { get; set; }
        public PartnerInformation Trucker { get; set; }
        //public AccountingPartner AccountingPartner { get; set; }
    }
}