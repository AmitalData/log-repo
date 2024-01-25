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
   public partial class CB_TariffList
   {
   
       [Key]
       [DataMember]
       public string ID  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public string TradeAgreementID  { get; set; }
       [DataMember]
       public string CustomsItemID  { get; set; }
   }

}
	 