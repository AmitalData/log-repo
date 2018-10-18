using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Customs.Data.EntityLists
{
   [DataContract]
   public partial class ClaimList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string ImporterClaimTypeCode  { get; set; }
       [DataMember]
       public string ImporterClaimTypeName  { get; set; }
       [DataMember]
       public string SoldierPersonalNumber  { get; set; }
       [DataMember]
       public DateTime? SubmitDate  { get; set; }
       [DataMember]
       public string PassportCountryTypeCode  { get; set; }
       [DataMember]
       public string PassportCountryTypeName  { get; set; }
       [DataMember]
       public string PassportNumber  { get; set; }
       [DataMember]
       public string PassportTypeCode  { get; set; }
       [DataMember]
       public string PassportTypeName  { get; set; }
       [DataMember]
       public string CustomsAddressCode  { get; set; }
       [DataMember]
       public string ContactPhoneAddressCode  { get; set; }
       [DataMember]
       public string ClaimSubmiterNumber  { get; set; }
       [DataMember]
       public string ClaimSubmiterTypeCode  { get; set; }
       [DataMember]
       public string ClaimSubmiterTypeName  { get; set; }
       [DataMember]
       public string HebrewCorporationName  { get; set; }
       [DataMember]
       public string AddressCode  { get; set; }
       [DataMember]
       public string BeneficiaryExternalID  { get; set; }
       [DataMember]
       public string BeneficiaryActivityTypeCode  { get; set; }
       [DataMember]
       public string BeneficiaryActivityTypeName  { get; set; }
       [DataMember]
       public string AccountCountryCode  { get; set; }
       [DataMember]
       public string AccountCountryName  { get; set; }
       [DataMember]
       public string BankTypeCode  { get; set; }
       [DataMember]
       public string AccountBranchCode  { get; set; }
       [DataMember]
       public string AccountBranchName  { get; set; }
       [DataMember]
       public string AccountNumber  { get; set; }
       [DataMember]
       public string AccountCurrencyTypeCode  { get; set; }
       [DataMember]
       public string AccountCurrencyTypeName  { get; set; }
       [DataMember]
       public string ForeignBank  { get; set; }
       [DataMember]
       public string ForeignBranch  { get; set; }
       [DataMember]
       public string ForeignAccountNumber  { get; set; }
       [DataMember]
       public string ImporterAffidavit  { get; set; }
       [DataMember]
       public string RawMaterialsDescription  { get; set; }
       [DataMember]
       public string CustomsFiles  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string TapagNumber  { get; set; }
       [DataMember]
       public string LeadingFileNumber  { get; set; }
       [DataMember]
       public string TapagTypeCode  { get; set; }
       [DataMember]
       public string TapagTypeName  { get; set; }
       [DataMember]
       public string CustomerId  { get; set; }
       [DataMember]
       public string CustomerName  { get; set; }
       [DataMember]
       public string ImporterName  { get; set; }
       [DataMember]
       public DateTime? CreateDate  { get; set; }
       [DataMember]
       public DateTime? FollowDate  { get; set; }
       [DataMember]
       public DateTime? ValidityDate  { get; set; }
       [DataMember]
       public bool IsClosed  { get; set; }
       [DataMember]
       public string TapagId  { get; set; }
       [DataMember]
       public string CustomsBranchCode  { get; set; }
       [DataMember]
       public string ReferantName  { get; set; }
       [DataMember]
       public bool IsSendClaimsRelatedEntity  { get; set; }
       [DataMember]
       public string CustomsBranchName  { get; set; }
   }

}
	 