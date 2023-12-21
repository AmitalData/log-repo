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
   public partial class SupplierInvioceExportDefaultList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string AccountTypeCode  { get; set; }
       [DataMember]
       public string PartyRelationshipCode  { get; set; }
       [DataMember]
       public string BuyerRoleCode  { get; set; }
       [DataMember]
       public string ProcessTypeCode  { get; set; }
       [DataMember]
       public string TransactionNatureCode  { get; set; }
       [DataMember]
       public string ClaimReasonCode  { get; set; }
   }

}
	 