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
   public partial class DeclarationList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string CustomFileNo  { get; set; }
       [DataMember]
       public string CustomerId  { get; set; }
       [DataMember]
       public string ImporterId  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string DeclarationNumber  { get; set; }
       [DataMember]
       public string CustomerName  { get; set; }
       [DataMember]
       public string ExternalDeclarationNumber  { get; set; }
       [DataMember]
       public string DeclarationOfficeCode  { get; set; }
       [DataMember]
       public DateTime? TaxationDateTime  { get; set; }
       [DataMember]
       public string AgentId  { get; set; }
       [DataMember]
       public string ProcedureCurrentCode  { get; set; }
       [DataMember]
       public string ProcedureCurrentName  { get; set; }
       [DataMember]
       public string AutonomyRegionTypeCode  { get; set; }
       [DataMember]
       public string AutonomyRegionTypeName  { get; set; }
       [DataMember]
       public string ImporterPassCountryCode  { get; set; }
       [DataMember]
       public string ImporterPassCountryName  { get; set; }
       [DataMember]
       public string TransferImporterCountryCode  { get; set; }
       [DataMember]
       public string TransferImporterCountryName  { get; set; }
       [DataMember]
       public string ImporterEntitlementTypeCode  { get; set; }
       [DataMember]
       public string ImporterEntitlementTypeName  { get; set; }
       [DataMember]
       public string EntitleImporterCountryCode  { get; set; }
       [DataMember]
       public string EntitleImporterCountryName  { get; set; }
       [DataMember]
       public string DeclarationDocumentTypeCode  { get; set; }
       [DataMember]
       public bool IsChanged  { get; set; }
       [DataMember]
       public DateTime? PaymentDate  { get; set; }
       [DataMember]
       public DateTime? HatraDate  { get; set; }
       [DataMember]
       public string DeclarationStatusTypeCode  { get; set; }
       [DataMember]
       public decimal? LoadingFactor  { get; set; }
       [DataMember]
       public decimal? DealValue  { get; set; }
       [DataMember]
       public decimal? CIFValue  { get; set; }
       [DataMember]
       public decimal? TotalTax  { get; set; }
       [DataMember]
       public string FileState  { get; set; }
       [DataMember]
       public string DeclarationOfficeName  { get; set; }
       [DataMember]
       public string ImporterName  { get; set; }
       [DataMember]
       public string DepartmentId  { get; set; }
       [DataMember]
       public string DepartmentName  { get; set; }
       [DataMember]
       public string ReferentUserId  { get; set; }
       [DataMember]
       public string DeclarationStatusTypeName  { get; set; }
       [DataMember]
       public string StorageSiteCode  { get; set; }
       [DataMember]
       public decimal? PlatformFee  { get; set; }
       [DataMember]
       public string CustomerCode  { get; set; }
       [DataMember]
       public DateTime? CreateDateTime  { get; set; }
       [DataMember]
       public DateTime? UpdateDateTime  { get; set; }
       [DataMember]
       public bool IsCancelled  { get; set; }
       [DataMember]
       public string EntitleImporterName  { get; set; }
       [DataMember]
       public string TransportModeName  { get; set; }
       [DataMember]
       public string TransferImporterName  { get; set; }
       [DataMember]
       public decimal? DealValueWithoutFactor  { get; set; }
       [DataMember]
       public string ImporterCode  { get; set; }
       [DataMember]
       public string EntitleImporterCode  { get; set; }
       [DataMember]
       public string CustomsTapagNumeral  { get; set; }
       [DataMember]
       public string CustomsTapagFile  { get; set; }
       [DataMember]
       public string CreatedByUserName  { get; set; }
       [DataMember]
       public string ImporterTypeCode  { get; set; }
       [DataMember]
       public string TransferImporterTypeCode  { get; set; }
       [DataMember]
       public string EntitleImporterTypeCode  { get; set; }
       [DataMember]
       public string ImporterTypeName  { get; set; }
       [DataMember]
       public string TransferImporterTypeName  { get; set; }
       [DataMember]
       public string EntitleImporterTypeName  { get; set; }
       [DataMember]
       public string UserNotes  { get; set; }
       [DataMember]
       public bool FreightValuesFilled  { get; set; }
       [DataMember]
       public bool HasConstraint  { get; set; }
       [DataMember]
       public string PrimaryInvoiceCounterKey  { get; set; }
       [DataMember]
       public string PaymentOrderNumber  { get; set; }
       [DataMember]
       public string PaymentStatusCode  { get; set; }
       [DataMember]
       public bool IsSignedVersion  { get; set; }
       [DataMember]
       public string SignedByUserId  { get; set; }
       [DataMember]
       public string StorageSiteName  { get; set; }
       [DataMember]
       public string SignerPersonalId  { get; set; }
       [DataMember]
       public int? CustomsNumeral  { get; set; }
       [DataMember]
       public bool IsConvertedDeclaration  { get; set; }
       [DataMember]
       public string CorrectionsXml  { get; set; }
       [DataMember]
       public string RequestFileNumber  { get; set; }
       [DataMember]
       public bool IsReleaseFile  { get; set; }
       [DataMember]
       public bool IsConnectedToUnifreight  { get; set; }
       [DataMember]
       public string MainImporterEntitlemntTypeCode  { get; set; }
       [DataMember]
       public string TransImporterEntitleTypeCode  { get; set; }
       [DataMember]
       public string ImporterAddress  { get; set; }
       [DataMember]
       public string TransferImporterAddress  { get; set; }
       [DataMember]
       public string EntitleImporterAddress  { get; set; }
       [DataMember]
       public string ImporterPassportNumber  { get; set; }
       [DataMember]
       public string TransferPassportNumber  { get; set; }
       [DataMember]
       public string EntitlePassportNumber  { get; set; }
       [DataMember]
       public string FacilityTypeName  { get; set; }
       [DataMember]
       public string CalculatedImporterName  { get; set; }
       [DataMember]
       public string CalculatedTransferImporterName  { get; set; }
       [DataMember]
       public string CalculatedEntitleImporterName  { get; set; }
       [DataMember]
       public string StorageStatusCode  { get; set; }
       [DataMember]
       public string CasualSupplierName  { get; set; }
       [DataMember]
       public string CasualSupplierAddress  { get; set; }
       [DataMember]
       public bool IsCourierDeclaration  { get; set; }
       [DataMember]
       public string ManifestCargoStatusCode  { get; set; }
       [DataMember]
       public string ManifestErrorXml  { get; set; }
       [DataMember]
       public string CourierHAWB  { get; set; }
       [DataMember]
       public string StorageStatusName  { get; set; }
       [DataMember]
       public bool ExcludeConsignment  { get; set; }
       [DataMember]
       public string CourierCustomStatusCode  { get; set; }
       [DataMember]
       public string CourierSuspentionReasonCode  { get; set; }
       [DataMember]
       public string CourierReleaseStatusCode  { get; set; }
       [DataMember]
       public string CourierHataraStatusCode  { get; set; }
       [DataMember]
       public decimal? DealValueWithFactor  { get; set; }
       [DataMember]
       public bool IsValueForCustomsOnly  { get; set; }
       [DataMember]
       public string WeightValue  { get; set; }
       [DataMember]
       public string WeightValueName  { get; set; }
       [DataMember]
       public string CourierSearchFields  { get; set; }
       [DataMember]
       public string CourierCustomStatusName  { get; set; }
       [DataMember]
       public string ManifestCargoStatusName  { get; set; }
       [DataMember]
       public string MAWBCourierMaster  { get; set; }
       [DataMember]
       public string CourierSuspentionReasonName  { get; set; }
       [DataMember]
       public string AcceptanceStatusCode  { get; set; }
       [DataMember]
       public string CasualImporterAddress1  { get; set; }
       [DataMember]
       public string CasualImporterAddress2  { get; set; }
       [DataMember]
       public string CasualImporterCity  { get; set; }
       [DataMember]
       public string CasualImporterZipCode  { get; set; }
       [DataMember]
       public string CasualImporterFax  { get; set; }
       [DataMember]
       public string CasualImporterEmail  { get; set; }
       [DataMember]
       public string CasualImporterTel  { get; set; }
       [DataMember]
       public string CasualImporterContact  { get; set; }
       [DataMember]
       public string MamanStatusCode  { get; set; }
       [DataMember]
       public string MamanErrorXml  { get; set; }
       [DataMember]
       public string ItemsProcessTypesList  { get; set; }
       [DataMember]
       public bool IsClose  { get; set; }
       [DataMember]
       public string MamanStatusName  { get; set; }
       [DataMember]
       public string AcceptanceStatusName  { get; set; }
       [DataMember]
       public string CourierSuspentionCode  { get; set; }
       [DataMember]
       public string CourierSuspentionName  { get; set; }
       [DataMember]
       public string DepositionStatusCode  { get; set; }
   }

}
	 