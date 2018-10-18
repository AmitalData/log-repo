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
   public partial class BookingLastRequestList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string BookingId  { get; set; }
       [DataMember]
       public DateTime? CreateDate  { get; set; }
       [DataMember]
       public DateTime? ETD  { get; set; }
       [DataMember]
       public string FlightNumber  { get; set; }
       [DataMember]
       public string Origin  { get; set; }
       [DataMember]
       public string Destination  { get; set; }
       [DataMember]
       public string CarrierId  { get; set; }
   }

}
	 