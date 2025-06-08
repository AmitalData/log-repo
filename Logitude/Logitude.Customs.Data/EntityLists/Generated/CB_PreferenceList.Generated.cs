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
   public partial class CB_PreferenceList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string BackgroundColor  { get; set; }
       [DataMember]
       public string TextColor  { get; set; }
       [DataMember]
       public string UserId  { get; set; }
       [DataMember]
       public int Level  { get; set; }
   }

}
	 