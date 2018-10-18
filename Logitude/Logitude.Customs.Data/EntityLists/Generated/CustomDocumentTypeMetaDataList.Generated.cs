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
   public partial class CustomDocumentTypeMetaDataList
   {
   
       [Key]
       [DataMember]
       public string MetaDataTypeCode  { get; set; }
       [DataMember]
       public bool Mandatory  { get; set; }
       [DataMember]
       public string Format  { get; set; }

       [Key]
       [DataMember]
       public string DocumentTypeCode  { get; set; }
       [DataMember]
       public string ValuesTable  { get; set; }
       [DataMember]
       public string MetaDataTypeName  { get; set; }
       [DataMember]
       public string DocumentTypeName  { get; set; }
       [DataMember]
       public bool IsLeading  { get; set; }
       [DataMember]
       public bool Inactive  { get; set; }
   }

}
	 