using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class AddressList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string FaxNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string AddressTypeId { get; set; }
        public string CountryId { get; set; }
        public string CardId { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public bool CountryEC { get; set; }
        public bool IsLocalLanguage { get; set; }
        public bool InActive { get; set; }
        public string SearchFields { get; set; }
        public string StateId { get; set; }
        public string ATTN { get; set; }
    }
}
