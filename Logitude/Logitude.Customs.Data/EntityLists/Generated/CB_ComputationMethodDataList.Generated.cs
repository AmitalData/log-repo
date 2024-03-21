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
   public partial class CB_ComputationMethodDataList
   {
          [DataMember]
       public int ID  { get; set; }
       [DataMember]
       public decimal? AlternateDefinedPerUnitMeasure  { get; set; }
       [DataMember]
       public decimal? AlternateRate  { get; set; }
       [DataMember]
       public string CalculationReference  { get; set; }
       [DataMember]
       public string ComputationMethodID  { get; set; }
       [DataMember]
       public string CurrencyTypeID  { get; set; }
       [DataMember]
       public decimal? DefinedPerUnitMethod  { get; set; }
       [DataMember]
       public string EnglishCalculationReference  { get; set; }
       [DataMember]
       public string MeasurementUnitID  { get; set; }
       [DataMember]
       public string Alternate_MeasurementUnitID  { get; set; }
       [DataMember]
       public decimal? OptionalTaxAddition  { get; set; }
       [DataMember]
       public decimal? Rate  { get; set; }
       [DataMember]
       public decimal? ReductionRate  { get; set; }
       [DataMember]
       public string TariffRelatedToQuotaID  { get; set; }
       [DataMember]
       public string Notes  { get; set; }
       [DataMember]
       public string EnglishNotes  { get; set; }

       [Key]
       [DataMember]
       public string CB_ID  { get; set; }
   }

}
	 