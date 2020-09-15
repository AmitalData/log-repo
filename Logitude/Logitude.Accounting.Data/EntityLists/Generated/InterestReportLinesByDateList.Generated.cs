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
   public partial class InterestReportLinesByDateList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string InterestReportId  { get; set; }
       [DataMember]
       public DateTime FromDate  { get; set; }
       [DataMember]
       public DateTime ToDate  { get; set; }
       [DataMember]
       public int TotalInterestDays  { get; set; }
       [DataMember]
       public decimal TotalAmount  { get; set; }
       [DataMember]
       public decimal AccumulatedAmount  { get; set; }
       [DataMember]
       public decimal StandardInterestPercentage  { get; set; }
       [DataMember]
       public decimal ExceptionalInterestPercentage  { get; set; }
       [DataMember]
       public decimal CreditInterestPercentage  { get; set; }
       [DataMember]
       public decimal StandardInterestAmount  { get; set; }
       [DataMember]
       public decimal ExceptionalInterestAmount  { get; set; }
       [DataMember]
       public decimal CreditInterestAmount  { get; set; }
       [DataMember]
       public decimal CalculatedStandInterestAmount  { get; set; }
       [DataMember]
       public decimal CalculatedExcepInterestAmount  { get; set; }
       [DataMember]
       public decimal CalculatedCreditInterestAmount  { get; set; }
       [DataMember]
       public string CalculationDetails  { get; set; }
       [DataMember]
       public int LineNumber  { get; set; }
       [DataMember]
       public decimal TotalInterest  { get; set; }
       [DataMember]
       public bool? IsOpenBalanceLine  { get; set; }
   }

}
	 