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
   public partial class CB_CustomsItemLinkageList
   {
          [DataMember]
       public int ID  { get; set; }
       [DataMember]
       public string ChangeTypeID  { get; set; }
       [DataMember]
       public int CustomsItemDetailsHistoryID  { get; set; }
       [DataMember]
       public int Connect_CustItemDetailsHistID  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }

       [Key]
       [DataMember]
       public string CB_ID  { get; set; }
   }

}
	 