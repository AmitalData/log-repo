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
   public partial class DefaultValueList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string DefaultTypeId  { get; set; }
       [DataMember]
       public string Distr  { get; set; }
       [DataMember]
       public string BranchId  { get; set; }
       [DataMember]
       public string CardId  { get; set; }
       [DataMember]
       public string ShortValue  { get; set; }
       [DataMember]
       public string DefValue  { get; set; }
   }

}
	 