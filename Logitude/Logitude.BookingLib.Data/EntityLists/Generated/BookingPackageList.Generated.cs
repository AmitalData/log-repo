using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.BookingLib.Data.EntityLists
{
   [DataContract]
   public partial class BookingPackageList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string BookingId  { get; set; }
       [DataMember]
       public string Description  { get; set; }
       [DataMember]
       public string PackageTypeId  { get; set; }
       [DataMember]
       public string ContainerNumber  { get; set; }
       [DataMember]
       public string Seal  { get; set; }
       [DataMember]
       public int? Quantity  { get; set; }
       [DataMember]
       public decimal? Weight  { get; set; }
       [DataMember]
       public decimal? Volume  { get; set; }
       [DataMember]
       public decimal? Tare  { get; set; }
       [DataMember]
       public decimal? Height  { get; set; }
       [DataMember]
       public decimal? Width  { get; set; }
       [DataMember]
       public decimal? Length  { get; set; }
       [DataMember]
       public string UnNumber  { get; set; }
       [DataMember]
       public string ClassNumber  { get; set; }
       [DataMember]
       public decimal? Temperature  { get; set; }
       [DataMember]
       public decimal? Ventilation  { get; set; }
       [DataMember]
       public string Seal2  { get; set; }
       [DataMember]
       public int? SOC  { get; set; }
       [DataMember]
       public string MarksAndNumbers  { get; set; }
       [DataMember]
       public string PackagingGroup  { get; set; }
       [DataMember]
       public string IMDGCode  { get; set; }
       [DataMember]
       public string Harmonize  { get; set; }
       [DataMember]
       public string MaterialDescription  { get; set; }
       [DataMember]
       public bool IsDangerous  { get; set; }
       [DataMember]
       public string OriginalBookingPackageId  { get; set; }
       [DataMember]
       public decimal? VolumetricWeight  { get; set; }
       [DataMember]
       public string CommodityId  { get; set; }
   }

}
	 