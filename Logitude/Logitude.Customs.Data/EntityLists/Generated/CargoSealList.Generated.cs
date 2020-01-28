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
   public partial class CargoSealList
   {
   
       [Key]
       [DataMember]
       public string CargoSealIdentifierId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string SealNumber  { get; set; }
       [DataMember]
       public string Remarks  { get; set; }
       [DataMember]
       public string SealCompletenessStateCode  { get; set; }
       [DataMember]
       public string SealCompletenessStatename  { get; set; }
       [DataMember]
       public string SealTypeCode  { get; set; }
       [DataMember]
       public string SealTypeName  { get; set; }
       [DataMember]
       public string UpdateReasonCode  { get; set; }
       [DataMember]
       public string UpdateReasonName  { get; set; }
       [DataMember]
       public string UpdateTypeCode  { get; set; }
       [DataMember]
       public string UpdateTypeName  { get; set; }
   }

}
	 