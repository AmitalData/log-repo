using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Accounting.Data.EntityLists
{
   [DataContract]
   public partial class ReconcileExternalPageList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string GLAccountId  { get; set; }
       [DataMember]
       public int PageNo  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string CreatedByUserName  { get; set; }
       [DataMember]
       public DateTime FromDate  { get; set; }
       [DataMember]
       public DateTime ToDate  { get; set; }
       [DataMember]
       public decimal StartBalance  { get; set; }
       [DataMember]
       public decimal CloseBalance  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public string ApprovedByUserId  { get; set; }
       [DataMember]
       public string StatusCode  { get; set; }
       [DataMember]
       public string StatusName  { get; set; }
       [DataMember]
       public string StatusLocalName  { get; set; }
       [DataMember]
       public string EntryTypeCode  { get; set; }
       [DataMember]
       public string EntryTypeEnglishName  { get; set; }
       [DataMember]
       public string EntryTypeLocalName  { get; set; }
       [DataMember]
       public string ObjectTableId  { get; set; }
       [DataMember]
       public string EntityId  { get; set; }
   }

}
	 