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
   public partial class CustomsAirlineList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string AirlineCode  { get; set; }
       [DataMember]
       public string LocalName  { get; set; }
       [DataMember]
       public string EnglishName  { get; set; }
       [DataMember]
       public bool InActive  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string AirlinePrefix  { get; set; }
       [DataMember]
       public string ICAO  { get; set; }
       [DataMember]
       public string UnloadPortCode  { get; set; }
   }

}
	 