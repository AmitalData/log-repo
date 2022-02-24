using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Accounting.Data.EntityLists
{
   [DataContract]
   public partial class TaxReportList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public DateTime LastUpdateDate  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public DateTime TaxReportMonth  { get; set; }
       [DataMember]
       public string TaxReportNumber  { get; set; }
       [DataMember]
       public string VatNumber  { get; set; }
       [DataMember]
       public string TaxReportTypeCode  { get; set; }
       [DataMember]
       public bool IsCancelled  { get; set; }
       [DataMember]
       public decimal? TaxableOutputAmount  { get; set; }
       [DataMember]
       public decimal? OutputTaxAmount  { get; set; }
       [DataMember]
       public decimal? TaxableOutputsWithDiffPercent  { get; set; }
       [DataMember]
       public decimal? OutputTaxAmountWithDiffPercent  { get; set; }
       [DataMember]
       public decimal? ExemptTaxableOutput  { get; set; }
       [DataMember]
       public int? OutputLinesCount  { get; set; }
       [DataMember]
       public decimal? OtherInputsTaxAmount  { get; set; }
       [DataMember]
       public decimal? EquipmentInputsTaxAmount  { get; set; }
       [DataMember]
       public int? InputLinesCount  { get; set; }
       [DataMember]
       public decimal? AmountForPayRefund  { get; set; }
       [DataMember]
       public string StatusCode  { get; set; }
       [DataMember]
       public DateTime? ProcessStartDate  { get; set; }
       [DataMember]
       public DateTime? ProcessEndDate  { get; set; }
       [DataMember]
       public int? ProcessProgress  { get; set; }
       [DataMember]
       public string StatusLocalName  { get; set; }
       [DataMember]
       public string CreatedByUserName  { get; set; }
       [DataMember]
       public string StatusEnglishName  { get; set; }
       [DataMember]
       public bool NeedsRebulid  { get; set; }
       [DataMember]
       public string UpdatedByUserName  { get; set; }
       [DataMember]
       public decimal? OutputTaxAmountRound  { get; set; }
       [DataMember]
       public decimal? InputsTaxAmountRound  { get; set; }
   }

}
	 