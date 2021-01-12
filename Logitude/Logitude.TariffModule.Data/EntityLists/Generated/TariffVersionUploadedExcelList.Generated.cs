using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.TariffModule.Data.EntityLists
{
   [DataContract]
   public partial class TariffVersionUploadedExcelList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime UploadDate  { get; set; }
       [DataMember]
       public string UploadedByUserId  { get; set; }
       [DataMember]
       public string TariffId  { get; set; }
       [DataMember]
       public int Version  { get; set; }
       [DataMember]
       public string DocumentId  { get; set; }
       [DataMember]
       public int NumberOfLines  { get; set; }
       [DataMember]
       public int Index  { get; set; }
       [DataMember]
       public string UploadedByUserName  { get; set; }
   }

}
	 