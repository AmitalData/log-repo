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
   public partial class DigitalPortalScreenList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public DateTime UpdateDate  { get; set; }
       [DataMember]
       public string ObjectTableId  { get; set; }
       [DataMember]
       public string ScreenCode  { get; set; }
       [DataMember]
       public string Name  { get; set; }
       [DataMember]
       public string Content  { get; set; }
       [DataMember]
       public string DraftContent  { get; set; }
       [DataMember]
       public string ProfileId  { get; set; }
       [DataMember]
       public string ProfileCode  { get; set; }
       [DataMember]
       public bool IsList  { get; set; }
   }

}
	 