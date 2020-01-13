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
   public partial class DeclarationPendingList
   {
          [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string DeclarationID  { get; set; }

       [Key]
       [DataMember]
       public string CourierPendingReasonCode  { get; set; }
       [DataMember]
       public string CourierPendingReasonName  { get; set; }
       [DataMember]
       public string PendingRemarks  { get; set; }
       [DataMember]
       public string Status  { get; set; }
   }

}
	 