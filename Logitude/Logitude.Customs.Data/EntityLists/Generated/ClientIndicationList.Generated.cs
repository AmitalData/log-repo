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
   public partial class ClientIndicationList
   {
   
       [Key]
       [DataMember]
       public string IndicationId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string ClientId  { get; set; }
       [DataMember]
       public string CustomerIndicationTypeID  { get; set; }
       [DataMember]
       public bool? IsActive  { get; set; }
       [DataMember]
       public DateTime? StartDate  { get; set; }
       [DataMember]
       public DateTime? EndDate  { get; set; }
       [DataMember]
       public DateTime? CreateDate  { get; set; }
       [DataMember]
       public string CustomerIndicationTypeName  { get; set; }
   }

}
	 