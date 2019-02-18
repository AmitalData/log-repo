using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Infrastructure.Data.EntityLists
{
   [DataContract]
   public partial class BIReportList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public DateTime UpdateDate  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string Name  { get; set; }
       [DataMember]
       public string Description  { get; set; }
       [DataMember]
       public string DWQueryId  { get; set; }
       [DataMember]
       public bool Inactive  { get; set; }
       [DataMember]
       public string TypeCode  { get; set; }
       [DataMember]
       public string AGGridOptionsXML  { get; set; }
       [DataMember]
       public string BIReportFolderId  { get; set; }
       [DataMember]
       public string CreatedByUserName  { get; set; }
       [DataMember]
       public string UpdatedByUserName  { get; set; }
   }

}
	 