using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.WarehouseLib.Data.EntityLists
{
   [DataContract]
   public partial class WarehouseEntryPackagesReleaseList
   {
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

       [Key]
       [DataMember]
       public string EntryPackageId  { get; set; }

       [Key]
       [DataMember]
       public string ReleasePackageId  { get; set; }
       [DataMember]
       public int Quantity  { get; set; }
       [DataMember]
       public bool IsCanceled  { get; set; }
   }

}
	 