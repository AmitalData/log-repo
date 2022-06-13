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
   public partial class AvailableStatusFieldList
   {
          [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string FieldCode  { get; set; }
       [DataMember]
       public bool IsAvailable  { get; set; }
       [DataMember]
       public string StatusFieldType  { get; set; }
   }

}
	 