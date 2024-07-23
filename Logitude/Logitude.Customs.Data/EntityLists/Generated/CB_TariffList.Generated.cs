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
          [DataMember]
       public int ID  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public int? TradeAgreementID  { get; set; }
       [DataMember]
       public int CustomsItemID  { get; set; }
       [DataMember]
       public string Title  { get; set; }

       [Key]
       [DataMember]
       public string CB_ID  { get; set; }
       [DataMember]
       public string Country  { get; set; }
       [DataMember]
       public string CustomsRate  { get; set; }
       [DataMember]
       public string CustomsRateWithinQuota  { get; set; }
       [DataMember]
       public int? QuotaID  { get; set; }
       [DataMember]
       public string MeasurementUnitName  { get; set; }
       [DataMember]
       public decimal? OptionalTaxAddition  { get; set; }
       [DataMember]
       public DateTime? StartDate  { get; set; }
       [DataMember]
       public DateTime? EndDate  { get; set; }
       [DataMember]
       public string TradeAgreementName  { get; set; }
   }

}
	 