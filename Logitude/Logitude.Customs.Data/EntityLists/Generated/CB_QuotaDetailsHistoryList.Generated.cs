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
   public partial class CB_QuotaDetailsHistoryList
   {
   
       [Key]
       [DataMember]
       public int ID  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public DateTime? StartDate  { get; set; }
       [DataMember]
       public DateTime? EndDate  { get; set; }
       [DataMember]
       public string EntityStatusID  { get; set; }
       [DataMember]
       public bool IsImportLicenseRequired  { get; set; }
       [DataMember]
       public string MeasurementUnitID  { get; set; }
       [DataMember]
       public int? Quantity  { get; set; }
       [DataMember]
       public string QuotaComputationBasisID  { get; set; }
       [DataMember]
       public string QuotaIncrementID  { get; set; }
       [DataMember]
       public string RenewalMethodID  { get; set; }
       [DataMember]
       public DateTime? RenewalUntilDate  { get; set; }
       [DataMember]
       public int QuotaID  { get; set; }
       [DataMember]
       public int ChangeRequestTypePriority  { get; set; }
       [DataMember]
       public decimal? QuotaValueIncrement  { get; set; }
       [DataMember]
       public string PerYearFrequency  { get; set; }
       [DataMember]
       public string CurrencyTypeID  { get; set; }
   }

}
	 