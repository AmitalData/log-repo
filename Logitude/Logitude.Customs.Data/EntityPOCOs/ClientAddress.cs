using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.Customs.Data.EntityPOCOs
{
   
    public class ClientAddress
    {
	 string dbms;

        [Key]
        [ForeignKey("Client")]
        [Column("ClientId" ,Order = 1)]
	    public string ClientId { get; set; }
	      
        public virtual Client Client { get; set; }
     [Key]
        [Column("AddressId" ,Order = 2)]
	    public string AddressId { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("AddressContactState")]
        [Column("ContactStateCode")]
	    public string ContactStateCode { get; set; }
	      
        public virtual AddressContactState AddressContactState { get; set; }
        [ForeignKey("AddressType")]
        [Column("AddressTypeCode")]
	    public string AddressTypeCode { get; set; }
	      
        public virtual CustomsAddressType AddressType { get; set; }
        [ForeignKey("AddressPurpose")]
        [Column("AddressPurposeCode")]
	    public string AddressPurposeCode { get; set; }
	      
        public virtual AddressPurpose AddressPurpose { get; set; }
        [Column("IsPalestinianCity")]
	    public bool IsPalestinianCity { get; set; }
        [Column("IsHebrewAddress")]
	    public bool IsHebrewAddress { get; set; }
        [Column("BranchName")]
	    public string BranchName { get; set; }
        [Column("ContactIdentifier")]
	    public string ContactIdentifier { get; set; }
        [Column("ContactFirstName")]
	    public string ContactFirstName { get; set; }
        [Column("ContactLastName")]
	    public string ContactLastName { get; set; }
        [ForeignKey("ContactRoleType")]
        [Column("ContactRoleTypeCode")]
	    public string ContactRoleTypeCode { get; set; }
	      
        public virtual ContactRoleType ContactRoleType { get; set; }
        [ForeignKey("AuthorizedSigner1")]
        [Column("AuthorizedSignerPermit1")]
	    public string AuthorizedSignerPermit1 { get; set; }
	      
        public virtual AuthorizedSignerPermit AuthorizedSigner1 { get; set; }
        [ForeignKey("AuthorizedSigner2")]
        [Column("AuthorizedSignerPermit2")]
	    public string AuthorizedSignerPermit2 { get; set; }
	      
        public virtual AuthorizedSignerPermit AuthorizedSigner2 { get; set; }
        [ForeignKey("AuthorizedSigner3")]
        [Column("AuthorizedSignerPermit3")]
	    public string AuthorizedSignerPermit3 { get; set; }
	      
        public virtual AuthorizedSignerPermit AuthorizedSigner3 { get; set; }
        [ForeignKey("City")]
        [Column("LocalCityCode")]
	    public string LocalCityCode { get; set; }
	      
        public virtual City City { get; set; }
        [Column("LocalSecondLine")]
	    public string LocalSecondLine { get; set; }
        [Column("LocalStreetName")]
	    public string LocalStreetName { get; set; }
        [Column("LocalHouseLetter")]
	    public string LocalHouseLetter { get; set; }
        [Column("LocalEntrance")]
	    public string LocalEntrance { get; set; }
        [ForeignKey("Country")]
        [Column("EnglishCountryCode")]
	    public string EnglishCountryCode { get; set; }
	      
        public virtual CustomsCountry Country { get; set; }
        [ForeignKey("SubCountry")]
        [Column("EnglishSubCountryCode")]
	    public string EnglishSubCountryCode { get; set; }
	      
        public virtual SubCountry SubCountry { get; set; }
        [Column("EnglishCityName")]
	    public string EnglishCityName { get; set; }
        [Column("EnglishMainAddressLine")]
	    public string EnglishMainAddressLine { get; set; }
        [Column("EnglishPostalCode")]
	    public string EnglishPostalCode { get; set; }
        [Column("LocalApartment")]
	    public decimal? LocalApartment { get; set; }
        [Column("LocalPOBox")]
	    public string LocalPOBox { get; set; }
        [Column("LocalPostalCode")]
	    public string LocalPostalCode { get; set; }
        [Column("LocalHouseNumber")]
	    public string LocalHouseNumber { get; set; }
        [Column("CustomAddressCode")]
	    public string CustomAddressCode { get; set; }
    }
}
	 