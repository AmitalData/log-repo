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
   public partial class DecCargoSplitConsPackDetList
   {
   
       [Key]
       [DataMember]
       public string DeclarationCargoSplitId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public int? DecCargoSplitConsLineNo  { get; set; }

       [Key]
       [DataMember]
       public int DecCargoSplitConsItemLine  { get; set; }

       [Key]
       [DataMember]
       public int PackageLine  { get; set; }
       [DataMember]
       public int? PackageQuantity  { get; set; }
       [DataMember]
       public decimal? GrossMassMeasure  { get; set; }
       [DataMember]
       public string PackageTypeCode  { get; set; }
       [DataMember]
       public string PackageTypeName  { get; set; }
       [DataMember]
       public string MarksNumbers  { get; set; }
       [DataMember]
       public string ManifestNumber  { get; set; }
   }

}
	 