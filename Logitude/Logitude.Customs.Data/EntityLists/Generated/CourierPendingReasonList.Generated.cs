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
   public partial class CourierPendingReasonList
   {
          [DataMember]
       public string Id  { get; set; }

       [Key]
       [DataMember]
       public string Code  { get; set; }
       [DataMember]
       public string LocalName  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string EnglishName  { get; set; }
       [DataMember]
       public bool Inactive  { get; set; }
       [DataMember]
       public string ErrorPlace  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string UnifreightStatusCode  { get; set; }
       [DataMember]
       public string ErrorPlaceName  { get; set; }
       [DataMember]
       public string MamanSuspendedCode  { get; set; }
   }

}
	 