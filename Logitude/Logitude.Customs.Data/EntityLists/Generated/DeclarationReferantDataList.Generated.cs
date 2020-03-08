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
       public DateTime ArrivalDate  { get; set; }
       [DataMember]
       public DateTime EstimatedArrivalDate  { get; set; }
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
       public bool IsExceptional  { get; set; }
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
   }

}
	 