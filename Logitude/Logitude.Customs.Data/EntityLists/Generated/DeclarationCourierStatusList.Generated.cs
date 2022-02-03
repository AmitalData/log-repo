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
   public partial class DeclarationCourierStatusList
   {
   
       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string CourierManifestStatusCode  { get; set; }
       [DataMember]
       public string CourierManifestStatusName  { get; set; }
       [DataMember]
       public string CourierDeclarationStatusCode  { get; set; }
       [DataMember]
       public string CourierDeclarationStatusName  { get; set; }
       [DataMember]
       public string CourierPaymentStatusCode  { get; set; }
       [DataMember]
       public string CourierPaymentStatusName  { get; set; }
       [DataMember]
       public bool IsCourierMissingClassification  { get; set; }
       [DataMember]
       public bool IsClosedForFollowUp  { get; set; }
       [DataMember]
       public string HighLowValue  { get; set; }
       [DataMember]
       public string DocumentStatusCode  { get; set; }
       [DataMember]
       public string CourierHawb  { get; set; }
       [DataMember]
       public string ProcedureCurrentCode  { get; set; }
       [DataMember]
       public string ProcedureCurrentName  { get; set; }
       [DataMember]
       public string ImporterCode  { get; set; }
       [DataMember]
       public string CourierMasterId  { get; set; }
       [DataMember]
       public string CourierCustomStatusName  { get; set; }
       [DataMember]
       public string DeclarationStatusTypeName  { get; set; }
       [DataMember]
       public string CustomerName  { get; set; }
       [DataMember]
       public bool IsMNFTab  { get; set; }
       [DataMember]
       public bool IsPAYTab  { get; set; }
       [DataMember]
       public bool IsDECTab  { get; set; }
       [DataMember]
       public bool IsDOCTab  { get; set; }
       [DataMember]
       public bool IsSVGTab  { get; set; }
       [DataMember]
       public bool IsMNFRTab  { get; set; }
       [DataMember]
       public bool IsDECRTab  { get; set; }
       [DataMember]
       public bool IsHOLDTab  { get; set; }
       [DataMember]
       public bool IsACCTab  { get; set; }
       [DataMember]
       public string CourierSearchFields  { get; set; }
       [DataMember]
       public string CourierCustomStatusCode  { get; set; }
       [DataMember]
       public string ImporterName  { get; set; }
       [DataMember]
       public decimal? TotalInvoiceAmountInUSD  { get; set; }
       [DataMember]
       public string DeclarationNumber  { get; set; }
       [DataMember]
       public string CourierPendingReasonCode  { get; set; }
       [DataMember]
       public string CourierPendingReasonName  { get; set; }
       [DataMember]
       public string PendingRemarks  { get; set; }
       [DataMember]
       public string CourierSuspentionReasonName  { get; set; }
       [DataMember]
       public string AcceptanceStatusCode  { get; set; }
       [DataMember]
       public string CourierSuspentionCode  { get; set; }
       [DataMember]
       public string CourierSuspentionName  { get; set; }
       [DataMember]
       public string SpecialActionStatus  { get; set; }
       [DataMember]
       public string SpecialActionsErrorXml  { get; set; }
       [DataMember]
       public string CourierPendingReasonErrorPlace  { get; set; }
       [DataMember]
       public string FastIndividualProcessCode  { get; set; }
       [DataMember]
       public string ManualProcessCode  { get; set; }
       [DataMember]
       public string TerminalSuspentionNumber  { get; set; }
       [DataMember]
       public string LastMileStatusCode  { get; set; }
       [DataMember]
       public DateTime? LastMileStatusDate  { get; set; }
       [DataMember]
       public string LastMileStatusRemarks  { get; set; }
       [DataMember]
       public string StorageSiteStatusCode  { get; set; }
       [DataMember]
       public string StorageSiteErrorText  { get; set; }
       [DataMember]
       public string StorageSiteStatusName  { get; set; }
       [DataMember]
       public string CourierPendingReasonList  { get; set; }
       [DataMember]
       public string AirlineId  { get; set; }
       [DataMember]
       public string MAWB  { get; set; }
       [DataMember]
       public decimal? MasterGrossMassMeasure  { get; set; }
       [DataMember]
       public int? MasterPackageQuantity  { get; set; }
       [DataMember]
       public DateTime? MasterCreateDateTime  { get; set; }
       [DataMember]
       public string MasterGatewayPortCode  { get; set; }
       [DataMember]
       public DateTime? MasterEstimatedArrivalDate  { get; set; }
       [DataMember]
       public string MasterStorageSiteCode  { get; set; }
       [DataMember]
       public string MasterHAWB  { get; set; }
       [DataMember]
       public string LastMileStatusName  { get; set; }
       [DataMember]
       public string CustomFileNo  { get; set; }
       [DataMember]
       public string SortedImporterCode  { get; set; }
       [DataMember]
       public string SortedDocumentStatusCode  { get; set; }
       [DataMember]
       public string SortedCourierManifestStatus  { get; set; }
       [DataMember]
       public string SortedCourierDeclarationStatus  { get; set; }
       [DataMember]
       public bool Delivered  { get; set; }
       [DataMember]
       public string TruckerId  { get; set; }
       [DataMember]
       public string DistributionArea  { get; set; }
       [DataMember]
       public string CrateNumber  { get; set; }
       [DataMember]
       public bool AmendmentDontDisplayInList  { get; set; }
       [DataMember]
       public string TruckerName  { get; set; }
       [DataMember]
       public string CourierPendingReasonNameList  { get; set; }
       [DataMember]
       public bool IsAmendment  { get; set; }
       [DataMember]
       public string DeclarationStorageSiteCode  { get; set; }
       [DataMember]
       public DateTime? TerminalReleaseDate  { get; set; }
       [DataMember]
       public string LastMileServiceType  { get; set; }
       [DataMember]
       public string CargoDescription  { get; set; }
       [DataMember]
       public bool FinalRelease  { get; set; }
       [DataMember]
       public DateTime? HatraDate  { get; set; }
       [DataMember]
       public string CasualSupplierAddress  { get; set; }
       [DataMember]
       public string CasualImporterCity  { get; set; }
       [DataMember]
       public decimal GrossMassMeasure  { get; set; }
       [DataMember]
       public string IncoTermCode  { get; set; }
   }

}
	 