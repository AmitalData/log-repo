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
   public partial class PendingByKeywordList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string CourierPendingReasonCode  { get; set; }
       [DataMember]
       public string CourierPendingReasonName  { get; set; }
       [DataMember]
       public string KeywordsList  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string SearchByFieldCode  { get; set; }
       [DataMember]
       public string SearchByFieldName  { get; set; }
       [DataMember]
       public int SearchToField  { get; set; }
       [DataMember]
       public int SerachType  { get; set; }
   }

}
	 