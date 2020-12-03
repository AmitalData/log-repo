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
   public partial class ReferantExceptionList
   {
   
       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string ExceptionReasonsCode  { get; set; }
       [DataMember]
       public string ExceptionRemarks  { get; set; }
       [DataMember]
       public string Status  { get; set; }
   }

}
	 