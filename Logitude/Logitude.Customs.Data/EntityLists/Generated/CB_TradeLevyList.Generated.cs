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
   public partial class CB_TradeLevyList
   {
   
       [Key]
       [DataMember]
       public int ID  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public string CustomsBookTypeID  { get; set; }
       [DataMember]
       public string LevyNumber  { get; set; }
       [DataMember]
       public DateTime? EndOfInquiryDate  { get; set; }
       [DataMember]
       public DateTime? EndOfLevyDate  { get; set; }
       [DataMember]
       public string InceptionCodeID  { get; set; }
       [DataMember]
       public DateTime? StartDate  { get; set; }
       [DataMember]
       public string TradeLevyStatusID  { get; set; }
       [DataMember]
       public int? ComputationMethodDataID  { get; set; }
       [DataMember]
       public string LevyTrustID  { get; set; }
       [DataMember]
       public string ParagraphTypeID  { get; set; }
   }

}
	 