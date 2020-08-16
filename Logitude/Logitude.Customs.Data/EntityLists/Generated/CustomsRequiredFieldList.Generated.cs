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
   public partial class CustomsRequiredFieldList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string ObjectTableId  { get; set; }
       [DataMember]
       public string ObjectfieldId  { get; set; }
       [DataMember]
       public string ObjectFieldName  { get; set; }
       [DataMember]
       public string ObjectfieldCode  { get; set; }
       [DataMember]
       public bool? IsImport  { get; set; }
       [DataMember]
       public bool? IsExport  { get; set; }
   }

}
	 