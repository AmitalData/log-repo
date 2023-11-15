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
   public partial class CustomsCountryTenantList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime UpdateDate  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public bool Inactive  { get; set; }
       [DataMember]
       public string MalamId  { get; set; }
       [DataMember]
       public string TarriffName  { get; set; }
       [DataMember]
       public string Code  { get; set; }
       [DataMember]
       public string TarriffCode  { get; set; }
   }

}
	 