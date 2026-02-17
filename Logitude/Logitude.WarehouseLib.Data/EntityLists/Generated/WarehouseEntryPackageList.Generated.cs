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
   public partial class WarehouseEntryPackageList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
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
       [DataMember]
       public string WarehouseEntryId  { get; set; }
       [DataMember]
       public string ContainerNumber  { get; set; }
       [DataMember]
       public int Quantity  { get; set; }
       [DataMember]
       public decimal? Weight  { get; set; }
       [DataMember]
       public decimal? Volume  { get; set; }
       [DataMember]
       public string Description  { get; set; }
       [DataMember]
       public string PackageTypeId  { get; set; }
       [DataMember]
       public string Seal  { get; set; }
       [DataMember]
       public string Harmonize  { get; set; }
       [DataMember]
       public double? Width  { get; set; }
       [DataMember]
       public double? Length  { get; set; }
       [DataMember]
       public double? Height  { get; set; }
       [DataMember]
       public string PackageTypeName  { get; set; }
       [DataMember]
       public string Dimensions  { get; set; }
       [DataMember]
       public bool IsContainer  { get; set; }
       [DataMember]
       public int Instock  { get; set; }
       [DataMember]
       public DateTime? ActualEntryDate  { get; set; }
       [DataMember]
       public string Location  { get; set; }
       [DataMember]
       public string VolumeUnitCode  { get; set; }
       [DataMember]
       public string GrossWeightUnitCode  { get; set; }
       [DataMember]
       public bool IsConnectedToShipment  { get; set; }
       [DataMember]
       public string DirectionId  { get; set; }
       [DataMember]
       public string TransportModeId  { get; set; }
       [DataMember]
       public string FromPortId  { get; set; }
       [DataMember]
       public string ToPortId  { get; set; }
       [DataMember]
       public double? VolumetricWeight  { get; set; }
       [DataMember]
       public string ChargeableWeightUnitCode  { get; set; }
       [DataMember]
       public string Make  { get; set; }
       [DataMember]
       public string Model  { get; set; }
       [DataMember]
       public string Year  { get; set; }
       [DataMember]
       public string Color  { get; set; }
       [DataMember]
       public string ChassisNumber  { get; set; }
       [DataMember]
       public string RegistrationNumber  { get; set; }
       [DataMember]
       public string CountryId  { get; set; }
       [DataMember]
       public string ReleasesNumber  { get; set; }
   }

}
	 