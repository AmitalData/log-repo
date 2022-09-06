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
       public bool? IsCancelled  { get; set; }
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
       public bool? IsPaymentDateNull  { get; set; }
       [DataMember]
       public bool? IsAvailabilityDateNull  { get; set; }
       [DataMember]
       public string DeclarationNumber  { get; set; }
       [DataMember]
       public bool? IsHatraDateNull  { get; set; }
       [DataMember]
       public int? RequestedCustomsDocId  { get; set; }
       [DataMember]
       public bool IsClose  { get; set; }
       [DataMember]
       public DateTime? PaymentDate_Date  { get; set; }
       [DataMember]
       public string PaymentDate_Time  { get; set; }
       [DataMember]
       public string PhysicalCheck  { get; set; }
       [DataMember]
       public string FclLcl  { get; set; }
       [DataMember]
       public int? PackageQuantity  { get; set; }
       [DataMember]
       public string ForwarderId  { get; set; }
       [DataMember]
       public string ForwarderName  { get; set; }
       [DataMember]
       public string FclLclName  { get; set; }
       [DataMember]
       public string TeamName  { get; set; }
       [DataMember]
       public string CancelRequestStatusCode  { get; set; }
       [DataMember]
       public bool? IsExceptionReasonsListNull  { get; set; }
       [DataMember]
       public bool IsManualPayment  { get; set; }
       [DataMember]
       public string PackageTypeCode  { get; set; }
       [DataMember]
       public string Commodity  { get; set; }
       [DataMember]
       public string LastStatusRemarks  { get; set; }
       [DataMember]
       public string ReferantUserName  { get; set; }
       [DataMember]
       public string DepartmentName  { get; set; }
       [DataMember]
       public string RemoveInclusiveVisibility  { get; set; }
       [DataMember]
       public string ReferantName  { get; set; }
       [DataMember]
       public string Mawb  { get; set; }
       [DataMember]
       public string Hawb  { get; set; }
       [DataMember]
       public bool ImporterApproval  { get; set; }
       [DataMember]
       public bool FieldC1  { get; set; }
       [DataMember]
       public bool FieldC2  { get; set; }
       [DataMember]
       public bool FieldC3  { get; set; }
       [DataMember]
       public bool FieldC4  { get; set; }
       [DataMember]
       public bool FieldC5  { get; set; }
       [DataMember]
       public bool FieldC6  { get; set; }
       [DataMember]
       public bool FieldC7  { get; set; }
       [DataMember]
       public bool FieldC8  { get; set; }
       [DataMember]
       public bool FieldC9  { get; set; }
       [DataMember]
       public bool FieldC10  { get; set; }
       [DataMember]
       public bool FieldC11  { get; set; }
       [DataMember]
       public bool FieldC12  { get; set; }
       [DataMember]
       public bool FieldC13  { get; set; }
       [DataMember]
       public bool FieldC14  { get; set; }
       [DataMember]
       public bool FieldC15  { get; set; }
       [DataMember]
       public bool FieldC16  { get; set; }
       [DataMember]
       public bool FieldC17  { get; set; }
       [DataMember]
       public bool FieldC18  { get; set; }
       [DataMember]
       public bool FieldC19  { get; set; }
       [DataMember]
       public bool FieldC20  { get; set; }
       [DataMember]
       public bool FieldC21  { get; set; }
       [DataMember]
       public bool FieldC22  { get; set; }
       [DataMember]
       public bool FieldC23  { get; set; }
       [DataMember]
       public bool FieldC24  { get; set; }
       [DataMember]
       public bool FieldC25  { get; set; }
       [DataMember]
       public bool FieldC26  { get; set; }
       [DataMember]
       public bool FieldC27  { get; set; }
       [DataMember]
       public bool FieldC28  { get; set; }
       [DataMember]
       public bool FieldC29  { get; set; }
       [DataMember]
       public bool FieldC30  { get; set; }
       [DataMember]
       public bool FieldC31  { get; set; }
       [DataMember]
       public bool FieldC32  { get; set; }
       [DataMember]
       public bool FieldC33  { get; set; }
       [DataMember]
       public bool FieldC34  { get; set; }
       [DataMember]
       public bool FieldC35  { get; set; }
       [DataMember]
       public bool FieldC36  { get; set; }
       [DataMember]
       public bool FieldC37  { get; set; }
       [DataMember]
       public bool FieldC38  { get; set; }
       [DataMember]
       public bool FieldC39  { get; set; }
       [DataMember]
       public bool FieldC40  { get; set; }
       [DataMember]
       public bool FieldC41  { get; set; }
       [DataMember]
       public bool FieldC42  { get; set; }
       [DataMember]
       public bool FieldC43  { get; set; }
       [DataMember]
       public bool FieldC44  { get; set; }
       [DataMember]
       public bool FieldC45  { get; set; }
       [DataMember]
       public bool FieldC46  { get; set; }
       [DataMember]
       public bool FieldC47  { get; set; }
       [DataMember]
       public bool FieldC48  { get; set; }
       [DataMember]
       public bool FieldC49  { get; set; }
       [DataMember]
       public bool FieldC50  { get; set; }
       [DataMember]
       public DateTime? FieldD1  { get; set; }
       [DataMember]
       public DateTime? FieldD2  { get; set; }
       [DataMember]
       public DateTime? FieldD3  { get; set; }
       [DataMember]
       public DateTime? FieldD4  { get; set; }
       [DataMember]
       public DateTime? FieldD5  { get; set; }
       [DataMember]
       public DateTime? FieldD6  { get; set; }
       [DataMember]
       public DateTime? FieldD7  { get; set; }
       [DataMember]
       public DateTime? FieldD8  { get; set; }
       [DataMember]
       public DateTime? FieldD9  { get; set; }
       [DataMember]
       public DateTime? FieldD10  { get; set; }
       [DataMember]
       public DateTime? FieldD11  { get; set; }
       [DataMember]
       public DateTime? FieldD12  { get; set; }
       [DataMember]
       public DateTime? FieldD13  { get; set; }
       [DataMember]
       public DateTime? FieldD14  { get; set; }
       [DataMember]
       public DateTime? FieldD15  { get; set; }
       [DataMember]
       public DateTime? FieldD16  { get; set; }
       [DataMember]
       public DateTime? FieldD17  { get; set; }
       [DataMember]
       public DateTime? FieldD18  { get; set; }
       [DataMember]
       public DateTime? FieldD19  { get; set; }
       [DataMember]
       public DateTime? FieldD20  { get; set; }
       [DataMember]
       public DateTime? FieldD21  { get; set; }
       [DataMember]
       public DateTime? FieldD22  { get; set; }
       [DataMember]
       public DateTime? FieldD23  { get; set; }
       [DataMember]
       public DateTime? FieldD24  { get; set; }
       [DataMember]
       public DateTime? FieldD25  { get; set; }
       [DataMember]
       public DateTime? FieldD26  { get; set; }
       [DataMember]
       public DateTime? FieldD27  { get; set; }
       [DataMember]
       public DateTime? FieldD28  { get; set; }
       [DataMember]
       public DateTime? FieldD29  { get; set; }
       [DataMember]
       public DateTime? FieldD30  { get; set; }
       [DataMember]
       public DateTime? FieldD31  { get; set; }
       [DataMember]
       public DateTime? FieldD32  { get; set; }
       [DataMember]
       public DateTime? FieldD33  { get; set; }
       [DataMember]
       public DateTime? FieldD34  { get; set; }
       [DataMember]
       public DateTime? FieldD35  { get; set; }
       [DataMember]
       public DateTime? FieldD36  { get; set; }
       [DataMember]
       public DateTime? FieldD37  { get; set; }
       [DataMember]
       public DateTime? FieldD38  { get; set; }
       [DataMember]
       public DateTime? FieldD39  { get; set; }
       [DataMember]
       public DateTime? FieldD40  { get; set; }
       [DataMember]
       public DateTime? FieldD41  { get; set; }
       [DataMember]
       public DateTime? FieldD42  { get; set; }
       [DataMember]
       public DateTime? FieldD43  { get; set; }
       [DataMember]
       public DateTime? FieldD44  { get; set; }
       [DataMember]
       public DateTime? FieldD45  { get; set; }
       [DataMember]
       public DateTime? FieldD46  { get; set; }
       [DataMember]
       public DateTime? FieldD47  { get; set; }
       [DataMember]
       public DateTime? FieldD48  { get; set; }
       [DataMember]
       public DateTime? FieldD49  { get; set; }
       [DataMember]
       public DateTime? FieldD50  { get; set; }
       [DataMember]
       public string FieldR1  { get; set; }
       [DataMember]
       public string FieldR2  { get; set; }
       [DataMember]
       public string FieldR3  { get; set; }
       [DataMember]
       public string FieldR4  { get; set; }
       [DataMember]
       public string FieldR5  { get; set; }
       [DataMember]
       public string FieldR6  { get; set; }
       [DataMember]
       public string FieldR7  { get; set; }
       [DataMember]
       public string FieldR8  { get; set; }
       [DataMember]
       public string FieldR9  { get; set; }
       [DataMember]
       public string FieldR10  { get; set; }
       [DataMember]
       public string FieldR11  { get; set; }
       [DataMember]
       public string FieldR12  { get; set; }
       [DataMember]
       public string FieldR13  { get; set; }
       [DataMember]
       public string FieldR14  { get; set; }
       [DataMember]
       public string FieldR15  { get; set; }
       [DataMember]
       public string FieldR16  { get; set; }
       [DataMember]
       public string FieldR17  { get; set; }
       [DataMember]
       public string FieldR18  { get; set; }
       [DataMember]
       public string FieldR19  { get; set; }
       [DataMember]
       public string FieldR20  { get; set; }
   }

}
	 