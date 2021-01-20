namespace Logitude.CommonDataTests.Models
{
    public class AddressPM
    {
        public string Id { get; set; }
        public string AddressId { get; set; }

        public string Description { get; set; }
        public string AddressTypeId { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CountryId { get; set; }
        public string Name { get; set; }
        public string AgentId { get; set; }
        public string CurrencyId { get; set; }
        public string StateId { get; set; }
        public string ZipCode { get; set; }
        public string FaxNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string CardId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CountryEnglishName { get; set; }
        public string StateEnglishName { get; set; }
        public string StateCode { get; set; }
        public string SearchFields { get; set; }
        public string VatNumber { get; set; }
        public string CardCode { get; set; }
        public string CardEnglishName { get; set; }
        public bool HasStates { get; set; }
        public bool IsStateRequired { get; set; }
        public int Tenant { get; set; }
    }
}