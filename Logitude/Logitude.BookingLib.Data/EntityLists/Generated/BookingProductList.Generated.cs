using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.BookingLib.Data.EntityLists
{
   [DataContract]
   public partial class BookingProductList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public string Code  { get; set; }
       [DataMember]
       public string Name  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string AirlineId  { get; set; }
       [DataMember]
       public bool InActive  { get; set; }
   }

}
	 