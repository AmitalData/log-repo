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
   public partial class DeclarationErrorMappingList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public string DocumentSectionCode  { get; set; }
       [DataMember]
       public string TagID  { get; set; }
       [DataMember]
       public string Field  { get; set; }
       [DataMember]
       public string Entity  { get; set; }
       [DataMember]
       public bool Skip  { get; set; }
   }

}
	 