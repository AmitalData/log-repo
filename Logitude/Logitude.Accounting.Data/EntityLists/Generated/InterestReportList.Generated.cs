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
   public partial class InterestReportList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime? CreateDateTime  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string GLAccountId  { get; set; }
       [DataMember]
       public string ReportNumber  { get; set; }
       [DataMember]
       public DateTime? InterestCalculationDate  { get; set; }
       [DataMember]
       public decimal? TotalAmount  { get; set; }
       [DataMember]
       public decimal? OpenBalance  { get; set; }
       [DataMember]
       public decimal? CloseBalance  { get; set; }
       [DataMember]
       public string ARinvoiceId  { get; set; }
       [DataMember]
       public decimal? InvoiceAmount  { get; set; }
       [DataMember]
       public decimal? GLAccountInterestCreditLimit  { get; set; }
       [DataMember]
       public string InterestReportStatusCode  { get; set; }
   }

}
	 