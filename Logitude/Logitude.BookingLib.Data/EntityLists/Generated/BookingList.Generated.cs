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
   public partial class BookingList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string BookingNumber  { get; set; }
       [DataMember]
       public string DirectionCode  { get; set; }
       [DataMember]
       public string TransportModeCode  { get; set; }
       [DataMember]
       public string ShipmentId  { get; set; }
       [DataMember]
       public DateTime? CreateDate  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string BookingStatusCode  { get; set; }
       [DataMember]
       public string Master  { get; set; }
       [DataMember]
       public string SpaceAllocationCode  { get; set; }
       [DataMember]
       public string MainCarriageCarrierId  { get; set; }
       [DataMember]
       public bool MainCarriageIsFromStack  { get; set; }
       [DataMember]
       public string MainCarriageSpaceAllocationCode  { get; set; }
       [DataMember]
       public string MainCarriageAllotmentIdentification  { get; set; }
       [DataMember]
       public string MainCarriageFromPortId  { get; set; }
       [DataMember]
       public string MainCarriageToPortId  { get; set; }
       [DataMember]
       public string MainCarriageCarrierPrefix  { get; set; }
       [DataMember]
       public string MainCarriageCarrierNumber  { get; set; }
       [DataMember]
       public DateTime? MainCarriageETD  { get; set; }
       [DataMember]
       public string Transshipment1FromPortId  { get; set; }
       [DataMember]
       public string Transshipment1ToPortId  { get; set; }
       [DataMember]
       public string Transshipment1CarrierId  { get; set; }
       [DataMember]
       public DateTime? Transshipment1ETD  { get; set; }
       [DataMember]
       public string Transshipment1AllotmentIdentification  { get; set; }
       [DataMember]
       public string Transshipment1CarrierPrefix  { get; set; }
       [DataMember]
       public string Transshipment1CarrierNumber  { get; set; }
       [DataMember]
       public string Transshipment1SpaceAllocationCode  { get; set; }
       [DataMember]
       public string Transshipment2FromPortId  { get; set; }
       [DataMember]
       public string Transshipment2ToPortId  { get; set; }
       [DataMember]
       public string Transshipment2CarrierId  { get; set; }
       [DataMember]
       public DateTime? Transshipment2ETD  { get; set; }
       [DataMember]
       public string Transshipment2AllotmentIdentification  { get; set; }
       [DataMember]
       public string Transshipment2CarrierPrefix  { get; set; }
       [DataMember]
       public string Transshipment2CarrierNumber  { get; set; }
       [DataMember]
       public string Transshipment2SpaceAllocationCode  { get; set; }
       [DataMember]
       public string FFRStatusCode  { get; set; }
       [DataMember]
       public DateTime? FFRStatusDate  { get; set; }
       [DataMember]
       public string FNAReason  { get; set; }
       [DataMember]
       public string ShipperId  { get; set; }
       [DataMember]
       public string ShipperAddressId  { get; set; }
       [DataMember]
       public string ShipperReference  { get; set; }
       [DataMember]
       public string ConsigneeId  { get; set; }
       [DataMember]
       public string ConsigneeAddressId  { get; set; }
       [DataMember]
       public string ConsigneeReference  { get; set; }
       [DataMember]
       public string IssuingCarrierAgentId  { get; set; }
       [DataMember]
       public string IssuingCarrierAddressId  { get; set; }
       [DataMember]
       public string IATACodeId  { get; set; }
       [DataMember]
       public string CASSCode  { get; set; }
       [DataMember]
       public string AWBSpecialHandlingCodeId1  { get; set; }
       [DataMember]
       public string AWBSpecialHandlingCodeId2  { get; set; }
       [DataMember]
       public string AWBSpecialHandlingCodeId3  { get; set; }
       [DataMember]
       public string AWBSpecialHandlingCodeId4  { get; set; }
       [DataMember]
       public string AWBSpecialHandlingCodeId5  { get; set; }
       [DataMember]
       public string AWBSpecialHandlingCodeId6  { get; set; }
       [DataMember]
       public string AWBSpecialHandlingCodeId7  { get; set; }
       [DataMember]
       public string AWBSpecialHandlingCodeId8  { get; set; }
       [DataMember]
       public string AWBSpecialHandlingCodeId9  { get; set; }
       [DataMember]
       public string AWBCarrierTarrifReference  { get; set; }
       [DataMember]
       public string DescriptionOfGoods  { get; set; }
       [DataMember]
       public string Notes  { get; set; }
       [DataMember]
       public string SpecialServicesRequest  { get; set; }
       [DataMember]
       public string OtherServicesInformation  { get; set; }
       [DataMember]
       public string BookingStatusName  { get; set; }
       [DataMember]
       public string SpaceAllocationName  { get; set; }
       [DataMember]
       public string FFRStatusName  { get; set; }
       [DataMember]
       public string Routing  { get; set; }
       [DataMember]
       public string FirstFlight  { get; set; }
       [DataMember]
       public string Airline  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string DirectionName  { get; set; }
       [DataMember]
       public string TransportModeName  { get; set; }
       [DataMember]
       public string IssuingCarrierIATACode  { get; set; }
       [DataMember]
       public string ShipperName  { get; set; }
       [DataMember]
       public string ConsigneeName  { get; set; }
       [DataMember]
       public string IssuingCarrierAgentName  { get; set; }
       [DataMember]
       public bool GrossWeightEdited  { get; set; }
       [DataMember]
       public bool ChargeableWeightEdited  { get; set; }
       [DataMember]
       public int? NumberOfPackages  { get; set; }
       [DataMember]
       public decimal? GrossWeight  { get; set; }
       [DataMember]
       public decimal? Volume  { get; set; }
       [DataMember]
       public decimal? VolumetricWeight  { get; set; }
       [DataMember]
       public decimal? ChargeableWeight  { get; set; }
       [DataMember]
       public string AWBCommodityItemNumber  { get; set; }
       [DataMember]
       public string GrossWeightUnitCode  { get; set; }
       [DataMember]
       public string ChargeableWeightUnitCode  { get; set; }
       [DataMember]
       public string DimensionsUnitCode  { get; set; }
       [DataMember]
       public string VolumeUnitCode  { get; set; }
       [DataMember]
       public decimal? GrossWeightInKG  { get; set; }
       [DataMember]
       public decimal? ChargeableWeightInKG  { get; set; }
       [DataMember]
       public decimal? Ratio  { get; set; }
       [DataMember]
       public decimal? DimFactor  { get; set; }
       [DataMember]
       public string MainCarriageFinalDestinationPortId  { get; set; }
       [DataMember]
       public string LongMaster  { get; set; }
       [DataMember]
       public bool IsDangerous  { get; set; }
       [DataMember]
       public string DangerousClassNumber  { get; set; }
       [DataMember]
       public string DangerousUnNumber  { get; set; }
       [DataMember]
       public string DangerousPackagingGroup  { get; set; }
       [DataMember]
       public string DangerousIMDGCode  { get; set; }
       [DataMember]
       public string DangerousFlashPoint  { get; set; }
       [DataMember]
       public string DangerousMaterialDescription  { get; set; }
       [DataMember]
       public string MainHarmonize  { get; set; }
       [DataMember]
       public string MainCarriageCarrierName  { get; set; }
       [DataMember]
       public string AWBHandlingInformation  { get; set; }
       [DataMember]
       public string AnswerOtherServicesInformation  { get; set; }
       [DataMember]
       public bool HasResponse  { get; set; }
       [DataMember]
       public string FMAAcknowledgementReason  { get; set; }
       [DataMember]
       public bool IsCancelled  { get; set; }
       [DataMember]
       public bool HasErrors  { get; set; }
       [DataMember]
       public bool WaitingForResponse  { get; set; }
       [DataMember]
       public string InterlineId  { get; set; }
       [DataMember]
       public string BookingLevelCode  { get; set; }
       [DataMember]
       public string BookingLevelName  { get; set; }
       [DataMember]
       public string AirlinePrefix  { get; set; }
       [DataMember]
       public string CreatedByUserName  { get; set; }
       [DataMember]
       public string BookingProductId  { get; set; }
       [DataMember]
       public string BookingProductName  { get; set; }
       [DataMember]
       public string LastSentByUserId  { get; set; }
       [DataMember]
       public string ShipmentNumber  { get; set; }
       [DataMember]
       public string MainCarriageCarrierCode  { get; set; }
       [DataMember]
       public string DescriptionOfGoodsService  { get; set; }
       [DataMember]
       public string AccountNumber  { get; set; }
       [DataMember]
       public DateTime? LastFSRStatusRequestDate  { get; set; }
   }

}
	 