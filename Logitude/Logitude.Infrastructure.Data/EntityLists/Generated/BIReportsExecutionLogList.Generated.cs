using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Infrastructure.Data.EntityLists
{
   [DataContract]
   public partial class BIReportsExecutionLogList
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
       public string StatusCode  { get; set; }
       [DataMember]
       public string ExceptionMessage  { get; set; }
       [DataMember]
       public DateTime? DoneDate  { get; set; }
       [DataMember]
       public string ReportFilterXML  { get; set; }
       [DataMember]
       public string BIReportId  { get; set; }
   }

}
	 