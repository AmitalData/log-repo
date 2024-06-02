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
   public partial class CB_TariffComputedDataList
   {
   
       [Key]
       [DataMember]
       public string CB_ID  { get; set; }
       [DataMember]
       public int ID  { get; set; }
       [DataMember]
       public int TariffID  { get; set; }
       [DataMember]
       public int TDH_IDNum  { get; set; }
       [DataMember]
       public DateTime StartDate  { get; set; }
       [DataMember]
       public DateTime EndDate  { get; set; }
       [DataMember]
       public int? WithoutQuota_ComputationID  { get; set; }
       [DataMember]
       public int? WithinQuota_ComputationID  { get; set; }
       [DataMember]
       public int CustomsItemIDNum  { get; set; }
       [DataMember]
       public int? TradeAgreementID  { get; set; }
       [DataMember]
       public int? QuotaID  { get; set; }
   }

}
	 