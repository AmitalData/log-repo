using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Amital.QuoteOPM.Data.EntityLists
{
   [DataContract]
   public partial class QuoteOPPackageList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string QuoteOPId  { get; set; }
       [DataMember]
       public string PackageTypeId  { get; set; }
       [DataMember]
       public string PackageTypeName  { get; set; }
       [DataMember]
       public int? Quantity  { get; set; }
       [DataMember]
       public double? GrossWeight  { get; set; }
       [DataMember]
       public double? Volume  { get; set; }
       [DataMember]
       public double? Height  { get; set; }
       [DataMember]
       public double? Width  { get; set; }
       [DataMember]
       public double? Length  { get; set; }
       [DataMember]
       public double? VolumetricWeight  { get; set; }
       [DataMember]
       public string Dimensions  { get; set; }
   }

}
	 