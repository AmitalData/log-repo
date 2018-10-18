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
   public partial class FlightsSchedulesResponseList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string FromPortId  { get; set; }
       [DataMember]
       public string ToPortId  { get; set; }
       [DataMember]
       public string AirlineId  { get; set; }
       [DataMember]
       public string RequestId  { get; set; }
       [DataMember]
       public DateTime? ETD  { get; set; }
       [DataMember]
       public DateTime? ETA  { get; set; }
       [DataMember]
       public string FlightNumber  { get; set; }
       [DataMember]
       public string AirplaneType  { get; set; }
       [DataMember]
       public int NumberOfStops  { get; set; }
       [DataMember]
       public int ResultNumber  { get; set; }
       [DataMember]
       public int LineNumber  { get; set; }
       [DataMember]
       public string FromPortCode  { get; set; }
       [DataMember]
       public string FromPortName  { get; set; }
       [DataMember]
       public string ToPortCode  { get; set; }
       [DataMember]
       public string ToPortName  { get; set; }
       [DataMember]
       public bool MissingPort  { get; set; }
   }

}
	 