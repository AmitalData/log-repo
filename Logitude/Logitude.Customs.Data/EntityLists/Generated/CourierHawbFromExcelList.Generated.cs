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
   public partial class CourierHawbFromExcelList
   {
          [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }

       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }
   }

}
	 