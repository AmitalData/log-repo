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
   public partial class CustomsDocumentMetaDataValueList
   {
   
       [Key]
       [DataMember]
       public string CustomsDocumentId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string MetaDataTypeCode  { get; set; }
       [DataMember]
       public string MetaDataValue  { get; set; }
       [DataMember]
       public string MetaDataTypeName  { get; set; }
   }

}
	 