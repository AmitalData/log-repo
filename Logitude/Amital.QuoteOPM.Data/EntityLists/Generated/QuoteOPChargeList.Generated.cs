using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Amital.QuoteOPM.Data.EntityLists
{
   [DataContract]
   public partial class QuoteOPChargeList
   {
          [DataMember]
       public string ChargesTypeId  { get; set; }
       [DataMember]
       public string VendorId  { get; set; }
       [DataMember]
       public string SaleCurrencyId  { get; set; }
       [DataMember]
       public bool IsAllIN  { get; set; }
       [DataMember]
       public string CostMeasurementId  { get; set; }
       [DataMember]
       public bool IsCostAllIn  { get; set; }
       [DataMember]
       public string TariffId  { get; set; }
       [DataMember]
       public string TariffNumber  { get; set; }
       [DataMember]
       public int TariffVersion  { get; set; }
       [DataMember]
       public string TariffLineId  { get; set; }
       [DataMember]
       public string TariffCostNo  { get; set; }
       [DataMember]
       public string TariffSaleNo  { get; set; }
   }

}
	 