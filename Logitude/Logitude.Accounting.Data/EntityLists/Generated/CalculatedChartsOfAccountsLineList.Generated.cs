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
   public partial class CalculatedChartsOfAccountsLineList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime CreateDateTime  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public DateTime UpdatedDateTime  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string CalculatedChartsOfAccountsId  { get; set; }
       [DataMember]
       public string CreatedByLocalName  { get; set; }
       [DataMember]
       public string CreatedByEnglishName  { get; set; }
       [DataMember]
       public string UpdatedByEnglishName  { get; set; }
       [DataMember]
       public string UpdatedByLocalName  { get; set; }
       [DataMember]
       public string GLAccountEnglishName  { get; set; }
       [DataMember]
       public string GLAccountLocalName  { get; set; }
       [DataMember]
       public string ChartOfAccountEnglishName  { get; set; }
       [DataMember]
       public string ChartOfAccountLocalName  { get; set; }
       [DataMember]
       public string LineTypeEnglishName  { get; set; }
       [DataMember]
       public string LineTypeLocalName  { get; set; }
       [DataMember]
       public int Line  { get; set; }
       [DataMember]
       public string ErrorLog  { get; set; }
   }

}
	 