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
   public partial class DeclarationReferantDataList
   {
   
       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string OrderNumber  { get; set; }
       [DataMember]
       public string VendorId  { get; set; }
       [DataMember]
       public DateTime? ArrivalDate  { get; set; }
       [DataMember]
       public DateTime? EstimatedArrivalDate  { get; set; }
       [DataMember]
       public decimal? Weight  { get; set; }
       [DataMember]
       public string ClassificationStatus  { get; set; }
       [DataMember]
       public string ControllerStatus  { get; set; }
       [DataMember]
       public string CollectionOfMoneyStatus  { get; set; }
       [DataMember]
       public DateTime? FollowUpDate  { get; set; }
       [DataMember]
       public bool WithPaper  { get; set; }
       [DataMember]
       public string IsClosedForFollowUp  { get; set; }
       [DataMember]
       public bool IsClassificationRemarks  { get; set; }
       [DataMember]
       public bool IsControllerRemarks  { get; set; }
       [DataMember]
       public string PreClassification  { get; set; }
       [DataMember]
       public string CustomFileNo  { get; set; }
       [DataMember]
       public string CustomerName  { get; set; }
       [DataMember]
       public string TransportModeId  { get; set; }
       [DataMember]
       public string DeclarationOfficeCode  { get; set; }
       [DataMember]
       public string VendorName  { get; set; }
       [DataMember]
       public string DeclarationStatusTypeName  { get; set; }
       [DataMember]
       public string DeclarationOfficeName  { get; set; }
       [DataMember]
       public string DeclarationStatusTypeCode  { get; set; }
       [DataMember]
       public string ATAOrETA  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string ExceptionReasonsList  { get; set; }
       [DataMember]
       public string DepartmentId  { get; set; }
       [DataMember]
       public string ReferentUserId  { get; set; }
       [DataMember]
       public string Actions  { get; set; }
       [DataMember]
       public DateTime? AvailabilityDate  { get; set; }
       [DataMember]
       public bool NewFile  { get; set; }
       [DataMember]
       public bool Favorite  { get; set; }
       [DataMember]
       public int SortedColumns  { get; set; }
       [DataMember]
       public bool IsCustomerLogBoxActivated  { get; set; }
       [DataMember]
       public bool IsCancelled  { get; set; }
       [DataMember]
       public string ClassifiedUserName  { get; set; }
       [DataMember]
       public string ControllerUserName  { get; set; }
       [DataMember]
       public string CollectorUserName  { get; set; }
       [DataMember]
       public string LastStatusName  { get; set; }
       [DataMember]
       public DateTime? LastStatusDate  { get; set; }
       [DataMember]
       public bool OrderMoney  { get; set; }
       [DataMember]
       public string Team  { get; set; }
       [DataMember]
       public string StorageSiteCode  { get; set; }
       [DataMember]
       public DateTime? TaxationDateTime  { get; set; }
       [DataMember]
       public string ProcedureCurrentCode  { get; set; }
       [DataMember]
       public DateTime? HatraDate  { get; set; }
       [DataMember]
       public DateTime? PaymentDate  { get; set; }
       [DataMember]
       public string ImporterCode  { get; set; }
       [DataMember]
       public string CustomerCode  { get; set; }
       [DataMember]
       public string ImporterFile  { get; set; }
       [DataMember]
       public string AEOImporter  { get; set; }
       [DataMember]
       public DateTime? FileOpenDate  { get; set; }
       [DataMember]
       public string StorageSiteName  { get; set; }
       [DataMember]
       public string ImporterName  { get; set; }
       [DataMember]
       public string ProcedureCurrentName  { get; set; }
       [DataMember]
       public bool IsPaymentDateNull  { get; set; }
       [DataMember]
       public bool IsAvailabilityDateNull  { get; set; }
       [DataMember]
       public string DeclarationNumber  { get; set; }
       [DataMember]
       public bool IsHatraDateNull  { get; set; }
       [DataMember]
       public int? RequestedCustomsDocId  { get; set; }
       [DataMember]
       public bool IsClose  { get; set; }
       [DataMember]
       public DateTime? PaymentDate_Date  { get; set; }
       [DataMember]
       public string PaymentDate_Time  { get; set; }
       [DataMember]
       public int? PhysicalCheck  { get; set; }
       [DataMember]
       public int? PackageQuantity  { get; set; }
       [DataMember]
       public string ForwarderId  { get; set; }
       [DataMember]
       public string ForwarderName  { get; set; }
       [DataMember]
       public string FclLclName  { get; set; }
   }

}
	 