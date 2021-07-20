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
   public partial class JournalAdditionalDataList
   {
          [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string JournalId  { get; set; }
       [DataMember]
       public string TaxReportId  { get; set; }
       [DataMember]
       public string TaxReportTransmitStatusCode  { get; set; }

       [Key]
       [DataMember]
       public int JournalLineNumber  { get; set; }
   }

}
	 