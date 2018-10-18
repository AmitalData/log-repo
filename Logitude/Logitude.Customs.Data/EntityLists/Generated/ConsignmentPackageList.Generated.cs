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
   public partial class ConsignmentPackageList
   {
   
       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public int? ConsignmentNumber  { get; set; }

       [Key]
       [DataMember]
       public int LineNumber  { get; set; }
       [DataMember]
       public string PackageMeasureQualifierCode  { get; set; }
       [DataMember]
       public string PackageMeasureQualifierName  { get; set; }
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
       public int? SequenceNumeric  { get; set; }
       [DataMember]
       public string PackageQuantityTypeCode  { get; set; }
       [DataMember]
       public string PackageQuantityTypeName  { get; set; }
       [DataMember]
       public string GrossMassMeasureTypeCode  { get; set; }
       [DataMember]
       public string GrossMassMeasureTypeName  { get; set; }
   }

}
	 