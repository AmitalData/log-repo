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
   public partial class ExportStorageList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public string ExportFileNo  { get; set; }
       [DataMember]
       public int StorageNo  { get; set; }
       [DataMember]
       public string StorageStatus  { get; set; }
       [DataMember]
       public string CargoTypeCode  { get; set; }
       [DataMember]
       public string CustomStatus  { get; set; }
   }

}
	 