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
   public partial class CB_CustomsBookAdditionsDetailsHistoryList
   {
   
       [Key]
       [DataMember]
       public string ID  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public DateTime UpdateDate  { get; set; }
       [DataMember]
       public string TypeID  { get; set; }
       [DataMember]
       public string Title  { get; set; }
       [DataMember]
       public DateTime? StartDate  { get; set; }
       [DataMember]
       public DateTime? EndDate  { get; set; }
       [DataMember]
       public string EntityStatusID  { get; set; }
       [DataMember]
       public string EnglishTitle  { get; set; }
       [DataMember]
       public string CustomsBookAdditionID  { get; set; }
       [DataMember]
       public int ChangeRequestTypePriority  { get; set; }
   }

}
	 