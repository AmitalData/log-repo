namespace Logitude.FullAccounting.Test.Models
{
    public class AddressPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string AddressId { get; set; }
        public string Description { get; set; }
        public string AddressTypeId { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CountryId { get; set; } //p
        public string Name { get; set; }
        public string AgentId { get; set; } //p
        public string CurrencyId { get; set; }
        public string StateId { get; set; } //L
        public string ZipCode { get; set; }
        public string FaxNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string CardId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }  //us
        public string CountryEnglishName { get; set; }
        public string StateEnglishName { get; set; }
        public string StateCode { get; set; } //L AK
        public string VatNumber { get; set; }
        public string CardCode { get; set; }
        public string CardEnglishName { get; set; }
        public bool HasStates { get; set; }
        public bool IsStateRequired { get; set; }
       
    }
}