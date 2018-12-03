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
   public partial class BatchTaskExecutionList
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
       public string SearchFields  { get; set; }
       [DataMember]
       public string ClassName  { get; set; }
       [DataMember]
       public string PrametersXml  { get; set; }
       [DataMember]
       public string StatusCode  { get; set; }
       [DataMember]
       public string ErrorLog  { get; set; }
       [DataMember]
       public DateTime? StartDateTime  { get; set; }
       [DataMember]
       public DateTime? DoneDateTime  { get; set; }
       [DataMember]
       public string ProgressMessage  { get; set; }
       [DataMember]
       public int ProgressPercentage  { get; set; }
       [DataMember]
       public string StatusName  { get; set; }
       [DataMember]
       public string CreatedByUserName  { get; set; }
       [DataMember]
       public string Subject  { get; set; }
       [DataMember]
       public string CallStack  { get; set; }
   }

}
	 