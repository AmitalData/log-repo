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
   public partial class DocumentTypeCustomsDataList
   {
   
       [Key]
       [DataMember]
       public string DocumentTypeId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string CustomsDoucumentTypeCode  { get; set; }
   }

}
	 