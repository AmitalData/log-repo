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
   public partial class SupplierInvoiceList
   {
   
       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }

       [Key]
       [DataMember]
       public int InvoiceCounterKey  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public int? SequenceNumeric  { get; set; }
       [DataMember]
       public string InvoiceNumber  { get; set; }
       [DataMember]
       public DateTime? IssueDate  { get; set; }
       [DataMember]
       public string AccountTypeCode  { get; set; }
       [DataMember]
       public bool IsPreference  { get; set; }
       [DataMember]
       public string PreferenceDocumentTypeCode  { get; set; }
       [DataMember]
       public string PaymentTypeCode  { get; set; }
       [DataMember]
       public string InvoiceCurrencyTypeCode  { get; set; }
       [DataMember]
       public decimal? InvoiceAmount  { get; set; }
       [DataMember]
       public string ActualPayedCurrencyTypeCode  { get; set; }
       [DataMember]
       public decimal? ActualPayedAmount  { get; set; }
       [DataMember]
       public string VendorId  { get; set; }
       [DataMember]
       public string IncotermCode  { get; set; }
       [DataMember]
       public string IssueCountryCode  { get; set; }
       [DataMember]
       public string PaymentTermsCode  { get; set; }
       [DataMember]
       public decimal? TotalFreightInFreightCurrency  { get; set; }
       [DataMember]
       public decimal? TotalFreightInNIS  { get; set; }
       [DataMember]
       public decimal? ExchangeRate  { get; set; }
       [DataMember]
       public string IssueCountryName  { get; set; }
       [DataMember]
       public string PreferenceDocumentTypeName  { get; set; }
       [DataMember]
       public string InsruanceCurrencyTypeCode  { get; set; }
       [DataMember]
       public decimal? InsuranceAmount  { get; set; }
       [DataMember]
       public string VendorName  { get; set; }
       [DataMember]
       public decimal? InsruancePercentage  { get; set; }
       [DataMember]
       public string FreightCurrencyTypeCode  { get; set; }
       [DataMember]
       public bool IsPrimarySupplierInvoice  { get; set; }
       [DataMember]
       public string ActualPayedCurrencyTypeName  { get; set; }
       [DataMember]
       public bool IsAccumalated  { get; set; }
       [DataMember]
       public string AccumalationStateCode  { get; set; }
       [DataMember]
       public int? FullParentsCount  { get; set; }
       [DataMember]
       public int? FullChildrenCount  { get; set; }
       [DataMember]
       public decimal? VendorComissionPercentage  { get; set; }
       [DataMember]
       public decimal? InvoiceAmountInUSD  { get; set; }
       [DataMember]
       public string ChangeInSupplierInvoice  { get; set; }
       [DataMember]
       public string InvoiceCurrencyTypeName  { get; set; }
       [DataMember]
       public string BuyerName  { get; set; }
       [DataMember]
       public string BuyerAddress  { get; set; }
       [DataMember]
       public string BuyerCountryCode  { get; set; }
       [DataMember]
       public string BuyerRoleCode  { get; set; }
       [DataMember]
       public string PartyRelationshipCode  { get; set; }
       [DataMember]
       public string PartyRelationshipName  { get; set; }
       [DataMember]
       public string BuyerRoleName  { get; set; }
       [DataMember]
       public string BuyerCountryName  { get; set; }
       [DataMember]
       public decimal? ItemFOBAmountForeign  { get; set; }
       [DataMember]
       public decimal? ItemFOBAmountNIS  { get; set; }
       [DataMember]
       public string DutyRegimeProtocolCode_  { get; set; }
   }

}
	 