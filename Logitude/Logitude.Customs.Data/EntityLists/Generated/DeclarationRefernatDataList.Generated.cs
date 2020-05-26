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
       public DateTime AvailabilityDate  { get; set; }
       [DataMember]
       public string ClassifiedUserId  { get; set; }
       [DataMember]
       public string ControllerUserId  { get; set; }
       [DataMember]
       public string CollectorUserId  { get; set; }
   }

}
	 