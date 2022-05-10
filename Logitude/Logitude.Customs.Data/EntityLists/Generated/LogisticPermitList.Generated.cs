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
   public partial class LogisticPermitList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime? TransmitDate  { get; set; }
       [DataMember]
       public string ActionCode  { get; set; }
       [DataMember]
       public string CargoIdentifierType  { get; set; }
       [DataMember]
       public string CargoIdentifierKey1  { get; set; }
       [DataMember]
       public string CargoIdentifierKey2  { get; set; }
       [DataMember]
       public string CargoIdentifierKey3  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
   }

}
	 