using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.CRM.Data.EntityLists
{
   [DataContract]
   public partial class OpportunityProductLocationList
   {
   
       [Key]
       [DataMember]
       public string OpportunityId  { get; set; }

       [Key]
       [DataMember]
       public string OpportunityProductTypeCode  { get; set; }

       [Key]
       [DataMember]
       public int LineNumber  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string CountryId  { get; set; }
       [DataMember]
       public decimal? TEU  { get; set; }
       [DataMember]
       public int? NumberOfShipments  { get; set; }
       [DataMember]
       public decimal? ChargeableWeight  { get; set; }
       [DataMember]
       public decimal? Revenue  { get; set; }
   }

}
	 