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
   public partial class FlightsSchedulesRequestList
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
       public string ShipmentId  { get; set; }
       [DataMember]
       public string BookingId  { get; set; }
       [DataMember]
       public DateTime? ETD  { get; set; }
       [DataMember]
       public DateTime? ETA  { get; set; }
       [DataMember]
       public decimal? Volume  { get; set; }
       [DataMember]
       public decimal? GrossWeight  { get; set; }
       [DataMember]
       public string VolumeUnitCode  { get; set; }
       [DataMember]
       public string GrossWeightUnitCode  { get; set; }
       [DataMember]
       public DateTime? CreateDate  { get; set; }
       [DataMember]
       public DateTime? ResponseDate  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public string StatusCode  { get; set; }
       [DataMember]
       public string AnswerOSI  { get; set; }
       [DataMember]
       public string AnswerReasonForNoReply  { get; set; }
       [DataMember]
       public string RequestDetails  { get; set; }
   }

}
	 