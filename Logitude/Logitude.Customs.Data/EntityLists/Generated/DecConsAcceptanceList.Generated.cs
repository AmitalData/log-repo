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
   public partial class DecConsAcceptanceList
   {
          [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }

       [Key]
       [DataMember]
       public int ConsignmentNumber  { get; set; }

       [Key]
       [DataMember]
       public int LineNumber  { get; set; }
       [DataMember]
       public DateTime? LoadDate  { get; set; }
       [DataMember]
       public string PackageTypeCode  { get; set; }
       [DataMember]
       public int? PackageQuantity  { get; set; }
       [DataMember]
       public decimal? GrossMassMeasure  { get; set; }
   }

}
	 