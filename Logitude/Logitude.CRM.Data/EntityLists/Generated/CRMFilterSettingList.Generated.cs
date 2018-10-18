using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.CRM.Data.EntityLists
{
   [DataContract]
   public partial class CRMFilterSettingList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string UserId  { get; set; }
       [DataMember]
       public string ControlNameSpace  { get; set; }
       [DataMember]
       public string FilterName  { get; set; }
       [DataMember]
       public string FilterValue  { get; set; }
   }

}
	 