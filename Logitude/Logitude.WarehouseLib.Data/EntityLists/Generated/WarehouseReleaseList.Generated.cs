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
   public partial class WarehouseReleaseList
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
       public string ReleaseNumber  { get; set; }
       [DataMember]
       public string CustomerId  { get; set; }
       [DataMember]
       public string ShipmentId  { get; set; }
       [DataMember]
       public string ShipmentNumber  { get; set; }
       [DataMember]
       public string WarehouseId  { get; set; }
       [DataMember]
       public DateTime? ExpectedReleaseDate  { get; set; }
       [DataMember]
       public DateTime? ActualReleaseDate  { get; set; }
       [DataMember]
       public string ReleaseBy  { get; set; }
       [DataMember]
       public string SpecialInstruction  { get; set; }
       [DataMember]
       public string StatusCode  { get; set; }
       [DataMember]
       public int TotalPieces  { get; set; }
       [DataMember]
       public decimal TotalGrossWeight  { get; set; }
       [DataMember]
       public string GrossWeightUnitCode  { get; set; }
       [DataMember]
       public decimal TotalVolume  { get; set; }
       [DataMember]
       public string VolumeUnitCode  { get; set; }
       [DataMember]
       public string Notes  { get; set; }
       [DataMember]
       public string HouseNumber  { get; set; }
       [DataMember]
       public string MasterNumber  { get; set; }
       [DataMember]
       public string WarehouseName  { get; set; }
       [DataMember]
       public string CustomerName  { get; set; }
       [DataMember]
       public string References  { get; set; }
       [DataMember]
       public string StatusName  { get; set; }
       [DataMember]
       public string TransportModeId  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public DateTime? ActivityDate  { get; set; }
       [DataMember]
       public string ActivityTypeName  { get; set; }
       [DataMember]
       public string ActivityByUserName  { get; set; }
       [DataMember]
       public string Routing  { get; set; }
       [DataMember]
       public string DirectionName  { get; set; }
       [DataMember]
       public string TransportModeName  { get; set; }
       [DataMember]
       public string DirectionId  { get; set; }
       [DataMember]
       public DateTime? ReleaseDate  { get; set; }
       [DataMember]
       public int TotalQuantity  { get; set; }
       [DataMember]
       public string ChargeableWeightUnitCode  { get; set; }
       [DataMember]
       public string ConnectedTo  { get; set; }
       [DataMember]
       public string FromPortId  { get; set; }
       [DataMember]
       public string ToPortId  { get; set; }
       [DataMember]
       public string CustomerAddressId  { get; set; }
       [DataMember]
       public decimal TotalVolumetricWeight  { get; set; }
       [DataMember]
       public double? Ratio  { get; set; }
       [DataMember]
       public string TruckerId  { get; set; }
       [DataMember]
       public string TruckerReference  { get; set; }
       [DataMember]
       public string MasterShipmentNumber  { get; set; }
   }

}
	 