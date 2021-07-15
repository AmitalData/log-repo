using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Amital.QuoteOPM.Data.EntityLists
{
   [DataContract]
   public partial class QuoteOPComputedFieldList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public bool ConnectedToShipment  { get; set; }
       [DataMember]
       public bool ConnectedToTicket  { get; set; }
       [DataMember]
       public string ToLocation  { get; set; }
       [DataMember]
       public string FromLocation  { get; set; }
       [DataMember]
       public string DeliveryTo  { get; set; }
       [DataMember]
       public string PickupFrom  { get; set; }
       [DataMember]
       public double? EstimatedPayablesInSales  { get; set; }
       [DataMember]
       public double? EstimatedPayablesInLocal  { get; set; }
       [DataMember]
       public double? EstimatedReceivablesInLocal  { get; set; }
       [DataMember]
       public double? EstimatedReceivablesInSales  { get; set; }
   }

}
	 