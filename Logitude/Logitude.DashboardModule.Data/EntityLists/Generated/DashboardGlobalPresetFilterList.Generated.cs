using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.DashboardModule.Data.EntityLists
{
   [DataContract]
   public partial class DashboardGlobalPresetFilterList
   {
   
       [Key]
       [DataMember]
       public string Code  { get; set; }
       [DataMember]
       public string DisplayName  { get; set; }
       [DataMember]
       public string DataTypeCode  { get; set; }
       [DataMember]
       public bool IsDisabled  { get; set; }
       [DataMember]
       public bool IsMultiSelect  { get; set; }
       [DataMember]
       public string JoinedTableName  { get; set; }
       [DataMember]
       public int? Sort  { get; set; }
       [DataMember]
       public string JoinedTableDisplayField  { get; set; }
       [DataMember]
       public bool CanSearch  { get; set; }
   }

}
	 