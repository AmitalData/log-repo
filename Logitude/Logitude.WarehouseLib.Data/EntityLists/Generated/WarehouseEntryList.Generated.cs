using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.WarehouseLib.Data.EntityLists
{
   [DataContract]
   public partial class WarehouseEntryList : CustomFieldList
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
       public string EntryNumber  { get; set; }
       [DataMember]
       public string CustomerId  { get; set; }
       [DataMember]
       public string ShipmentId  { get; set; }
       [DataMember]
       public string ShipmentNumber  { get; set; }
       [DataMember]
       public string WarehouseId  { get; set; }
       [DataMember]
       public DateTime? ExpectedEntryDate  { get; set; }
       [DataMember]
       public DateTime? ActualEntryDate  { get; set; }
       [DataMember]
       public string ReceivedBy  { get; set; }
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
       public string CustomerRef1  { get; set; }
       [DataMember]
       public string CustomerRef2  { get; set; }
       [DataMember]
       public string HouseNumber  { get; set; }
       [DataMember]
       public string MasterNumber  { get; set; }
       [DataMember]
       public string WarehouseName  { get; set; }
       [DataMember]
       public string CustomerName  { get; set; }
       [DataMember]
       public string StatusName  { get; set; }
       [DataMember]
       public string TransportModeId  { get; set; }
       [DataMember]
       public string FromPortId  { get; set; }
       [DataMember]
       public string ToPortId  { get; set; }
       [DataMember]
       public string TruckerId  { get; set; }
       [DataMember]
       public string TruckerReference  { get; set; }
       [DataMember]
       public string ShipperId  { get; set; }
       [DataMember]
       public string DirectionId  { get; set; }
       [DataMember]
       public string ShipmentTypeId  { get; set; }
       [DataMember]
       public string EntryReference  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string Origin  { get; set; }
       [DataMember]
       public string Destination  { get; set; }
       [DataMember]
       public string MainCarriageCarrierName  { get; set; }
       [DataMember]
       public string Routing  { get; set; }
       [DataMember]
       public DateTime? ActivityDate  { get; set; }
       [DataMember]
       public string ActivityTypeName  { get; set; }
       [DataMember]
       public string ActivityByUserName  { get; set; }
       [DataMember]
       public string DirectionName  { get; set; }
       [DataMember]
       public string TransportModeName  { get; set; }
       [DataMember]
       public bool ConnectedToShipment  { get; set; }
       [DataMember]
       public string FromAddressId  { get; set; }
       [DataMember]
       public string ToAddressId  { get; set; }
       [DataMember]
       public string ConsigneeId  { get; set; }
       [DataMember]
       public string ShipperReference1  { get; set; }
       [DataMember]
       public string ConsigneeReference1  { get; set; }
       [DataMember]
       public string ConsigneeReference2  { get; set; }
       [DataMember]
       public string ShipperReference2  { get; set; }
       [DataMember]
       public string ShipperName  { get; set; }
       [DataMember]
       public string ConsigneeName  { get; set; }
       [DataMember]
       public string Manufacturer  { get; set; }
       [DataMember]
       public string FromPartnerId  { get; set; }
       [DataMember]
       public string ToPartnerId  { get; set; }
       [DataMember]
       public string ChargeableWeightUnitCode  { get; set; }
       [DataMember]
       public decimal TotalVolumetricWeight  { get; set; }
       [DataMember]
       public DateTime? LastStatusUpdateDate  { get; set; }
       [DataMember]
       public string MasterHouse  { get; set; }
       [DataMember]
       public string EntryReferencesAndDate  { get; set; }
       [DataMember]
       public string ConnectedTo  { get; set; }
       [DataMember]
       public double? Ratio  { get; set; }
       [DataMember]
       public string MasterShipmentNumber  { get; set; }
       [DataMember]
       public string ConnectedToReferenceNumber  { get; set; }
   }

}
	 