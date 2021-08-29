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
   public partial class ChartOfAccountList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string Code  { get; set; }
       [DataMember]
       public string LocalName  { get; set; }
       [DataMember]
       public string EnglishName  { get; set; }
       [DataMember]
       public string ParentId  { get; set; }
       [DataMember]
       public string TypeCode  { get; set; }
       [DataMember]
       public bool? Inactive  { get; set; }
       [DataMember]
       public string TypeName  { get; set; }
       [DataMember]
       public string ParentName  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public int? ChartOfAccountSecurityLevel  { get; set; }
   }

}
	 