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
   public partial class ProceduralFaultsConnEntityList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string ProceduralFaultId  { get; set; }
       [DataMember]
       public string EntityType  { get; set; }
       [DataMember]
       public string EntityIdKey1  { get; set; }
       [DataMember]
       public string EntityIdKey2  { get; set; }
       [DataMember]
       public string EntityIdKey3  { get; set; }
       [DataMember]
       public string EntityPath  { get; set; }
   }

}
	 