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
   public partial class OPSpecialServicesTypeList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string Code  { get; set; }
       [DataMember]
       public string EnglishName  { get; set; }
       [DataMember]
       public string LocalName  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public bool InActive  { get; set; }
   }

}
	 