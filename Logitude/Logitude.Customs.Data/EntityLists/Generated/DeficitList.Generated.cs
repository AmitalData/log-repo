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
   public partial class DeficitList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string NotificationTypeCode  { get; set; }
       [DataMember]
       public string NotificationTypeName  { get; set; }
       [DataMember]
       public string DebtNotificationNumber  { get; set; }
       [DataMember]
       public DateTime? ProductionDate  { get; set; }
       [DataMember]
       public string DebtNotificationReason  { get; set; }
       [DataMember]
       public string RealesGoodsDescription  { get; set; }
       [DataMember]
       public DateTime? ValidityDateTo  { get; set; }
       [DataMember]
       public string PaymentOrderNumber  { get; set; }
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
       public string ImporterId  { get; set; }
       [DataMember]
       public string ImporterName  { get; set; }
       [DataMember]
       public string CustomsBranchCode  { get; set; }
       [DataMember]
       public string CustomsBranchName  { get; set; }
       [DataMember]
       public string ProfessionUnitTypeCode  { get; set; }
       [DataMember]
       public string ProfessionUnitTypeName  { get; set; }
       [DataMember]
       public string SpecializationTypeCode  { get; set; }
       [DataMember]
       public string SpecializationTypeName  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public DateTime? FollowDate  { get; set; }
       [DataMember]
       public DateTime? ValidityDate  { get; set; }
       [DataMember]
       public bool IsClosed  { get; set; }
       [DataMember]
       public string TapagId  { get; set; }
   }

}
	 