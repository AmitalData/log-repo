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
   public partial class QuoteOPPriceStepsList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string QuoteOPId  { get; set; }
       [DataMember]
       public string QuoteOPChargeId  { get; set; }
       [DataMember]
       public double? Step  { get; set; }
       [DataMember]
       public double? CostUnitPrice  { get; set; }
       [DataMember]
       public double? SaleUnitPrice  { get; set; }
       [DataMember]
       public double? MarkupValue  { get; set; }
       [DataMember]
       public string MeasurementUnit  { get; set; }
   }

}
	 