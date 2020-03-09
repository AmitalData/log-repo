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
   public partial class ConsignmentPackDangerList
   {
   
       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public int? ConsignmentNumber  { get; set; }

       [Key]
       [DataMember]
       public int? LineNumber  { get; set; }

       [Key]
       [DataMember]
       public int? DangerousLineNo  { get; set; }
       [DataMember]
       public string UNCode  { get; set; }
       [DataMember]
       public string UNName  { get; set; }
       [DataMember]
       public string DangerousGoodsPackingReqCode  { get; set; }
       [DataMember]
       public string FlashpointTemperature  { get; set; }
       [DataMember]
       public string StorageTemperature  { get; set; }
       [DataMember]
       public string ClassificationFourDigit  { get; set; }
   }

}
	 