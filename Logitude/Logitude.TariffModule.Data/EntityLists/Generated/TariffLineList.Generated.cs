using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.TariffModule.Data.EntityLists
{
   [DataContract]
   public partial class TariffLineList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public DateTime? StartDate  { get; set; }
       [DataMember]
       public DateTime? ExpirationDate  { get; set; }
       [DataMember]
       public string TariffId  { get; set; }
       [DataMember]
       public int Version  { get; set; }
       [DataMember]
       public int? MinPrice  { get; set; }
       [DataMember]
       public string OriginPortName  { get; set; }
       [DataMember]
       public string OriginPortCode  { get; set; }
       [DataMember]
       public string DestinationPortName  { get; set; }
       [DataMember]
       public string DestinationPortCode  { get; set; }
   }

}
	 