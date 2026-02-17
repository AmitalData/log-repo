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
   
    public class Claim
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("ImporterClaimType")]
        [Column("ImporterClaimTypeCode")]
	    public string ImporterClaimTypeCode { get; set; }
	      
        public virtual ImporterTypeForClaim ImporterClaimType { get; set; }
        [Column("SoldierPersonalNumber")]
	    public string SoldierPersonalNumber { get; set; }
        [Column("SubmitDate")]
	    public DateTime? SubmitDate { get; set; }
        [ForeignKey("Client")]
        [Column("ClientId")]
	    public string ClientId { get; set; }
	      
        public virtual Client Client { get; set; }
        [ForeignKey("PassportCountryType")]
        [Column("PassportCountryTypeCode")]
	    public string PassportCountryTypeCode { get; set; }
	      
        public virtual CustomsCountry PassportCountryType { get; set; }
        [Column("PassportNumber")]
	    public string PassportNumber { get; set; }
        [ForeignKey("PassportType")]
        [Column("PassportTypeCode")]
	    public string PassportTypeCode { get; set; }
	      
        public virtual PassportType PassportType { get; set; }
        [Column("CustomsAddressCode")]
	    public string CustomsAddressCode { get; set; }
        [Column("ContactPhoneAddressCode")]
	    public string ContactPhoneAddressCode { get; set; }
        [Column("ClaimSubmiterNumber")]
	    public string ClaimSubmiterNumber { get; set; }
        [ForeignKey("ClaimSubmiterType")]
        [Column("ClaimSubmiterTypeCode")]
	    public string ClaimSubmiterTypeCode { get; set; }
	      
        public virtual CustomerActivityType ClaimSubmiterType { get; set; }
        [Column("HebrewCorporationName")]
	    public string HebrewCorporationName { get; set; }
        [Column("AddressCode")]
	    public string AddressCode { get; set; }
        [Column("BeneficiaryExternalID")]
	    public string BeneficiaryExternalID { get; set; }
        [ForeignKey("BeneficiaryActivityType")]
        [Column("BeneficiaryActivityTypeCode")]
	    public string BeneficiaryActivityTypeCode { get; set; }
	      
        public virtual CustomerActivityType BeneficiaryActivityType { get; set; }
        [ForeignKey("AccountCountry")]
        [Column("AccountCountryCode")]
	    public string AccountCountryCode { get; set; }
	      
        public virtual CustomsCountry AccountCountry { get; set; }
        [ForeignKey("BankCode")]
        [Column("BankTypeCode")]
	    public string BankTypeCode { get; set; }
	      
        public virtual Bank BankCode { get; set; }
        [ForeignKey("AccountBranch")]
        [Column("AccountBranchCode")]
	    public string AccountBranchCode { get; set; }
	      
        public virtual CustomsBranch AccountBranch { get; set; }
        [Column("AccountNumber")]
	    public string AccountNumber { get; set; }
        [ForeignKey("AccountCurrencyType")]
        [Column("AccountCurrencyTypeCode")]
	    public string AccountCurrencyTypeCode { get; set; }
	      
        public virtual CurrencyType AccountCurrencyType { get; set; }
        [Column("ForeignBank")]
	    public string ForeignBank { get; set; }
        [Column("ForeignBranch")]
	    public string ForeignBranch { get; set; }
        [Column("ForeignAccountNumber")]
	    public string ForeignAccountNumber { get; set; }
        [Column("ImporterAffidavit")]
	    public string ImporterAffidavit { get; set; }
        [Column("RawMaterialsDescription")]
	    public string RawMaterialsDescription { get; set; }
        [Column("CustomsFiles")]
	    public string CustomsFiles { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
    }
}
	 