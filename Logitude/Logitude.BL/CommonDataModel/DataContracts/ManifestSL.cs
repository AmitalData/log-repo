using Logitude.BL.ShipmentsModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.DataContracts
{
    public class ManifestSL
    {

        public string AgentSharedKey { get; set; }
        public string AgentSharedManifestId { get; set; }
        public int SourceAgentTenant { get; set; }
        public int DestinationAgentTenant { get; set; }
        public bool IsDangerous { get; set; }
        
        public double? ValueOfGoods { get; set; }
        public string ShipmentTypeId { get; set; }
        public string ShipmentNumber { get; set; }
        public string MasterNumber { get; set; }
        public DateTime? HAWBDate { get; set; }
        public DateTime? MAWBOBLDate { get; set; }
        public DateTime MasterDate { get; set; }
        public string DirectionId { get; set; }
        public string TransportModeId { get; set; }

        public string IncotermCode { get; set; }
        public string IncotermName{ get; set; }
        public string IncotermId { get; set; }
        public bool IncotermAddedManually { get; set; }

        public string TruckNumber { get; set; }
        public string CarrierCode { get; set; }
        public string CarrierName { get; set; }
        public string CarrierId { get; set; }
        public bool CarrierAddedManually { get; set; }

        public string FreightPrepaidCollectId { get; set; }
        public string OtherPrepaidCollectId { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string HouseNumber { get; set; }
        
        public PartnerSL Consignee { get; set; }
        public string GeneralDescriptionOfGoods { get; set; }
        public PartnerSL Shipper { get; set; }
        public PartnerSL Notify1 { get; set; }
        

        public string GrossWeightUnitCode { get; set; }
        public string ChargeableWeightUnitCode { get; set; }
        public string VolumeUnitCode { get; set; }
        public string DimensionsUnitCode { get; set; }

        public double? ChargeableWeightInKG { get; set; }
        public bool GrossWeightEdited { get; set; }
        public double? GrossWeightInKG { get; set; }

        public double? GrossWeight { get; set; }
        public double? ChargeableWeight { get; set; }
        public double? TEU { get; set; }
        public int? PackagesQuantity { get; set; }

        public double? Volume { get; set; }
        public double? VolumetricWeight { get; set; }
        public int? NumberOfPackages { get; set; }
        public int? NumberOfContainers { get; set; }

        public double? OrderGrossWeight { get; set; }
        public string ShipmentTypeName { get; set; }
   
        public string AgentName { get; set; }

  
        public string AgentReference1 { get; set; }
        public string AgentReference2 { get; set; }

        public string ShipperName { get; set; }
        public string ConsigneeReference1 { get; set; }
        public string ConsigneeReference2 { get; set; }
        public string ShipperReference1 { get; set; }
        public string ShipperReference2 { get; set; }
        public string MainHarmonize { get; set; }


        public string MainCarriageCarrierNumber { get; set; }
        public string Transshipment1CarrierNumber { get; set; }
        public string Transshipment2CarrierNumber { get; set; }
        public string Transshipment3CarrierNumber { get; set; }


        public string MainCarriageAirlinePrefix { get; set; }
        public string MainCarriageVesselName { get; set; }
        public string MainCarriageVesselId { get; set; }
        public string MainCarriageVesselCode { get; set; }
        public bool MainCarriageVesselAddedManually { get; set; }

        public DateTime? MainCarriageETD { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public DateTime? MainCarriageETA { get; set; }
        public DateTime? MainCarriageATA { get; set; }

        public string MainCarriageMAWBOBL { get; set; }
        public string MainCarriageMAWBOBLDate { get; set; }
        public string InterlineId { get; set; }
        public string InterlineCode { get; set; }
        public string InterlineName { get; set; }
        public bool InterlineAddedManually { get; set; }






        public string Transshipment1CarrierName { get; set; }
        public string Transshipment1CarrierCode { get; set; }
        public string Transshipment1CarrierId { get; set; }
        public bool Transshipment1CarrierAddedManually { get; set; }
        public string Transshipment1AirlinePrefix { get; set; }
        public DateTime? Transshipment1ETD { get; set; }
        public DateTime? Transshipment1ATD { get; set; }

        public DateTime? Transshipment1ETA { get; set; }
        public DateTime? Transshipment1ATA { get; set; }



        public string Transshipment1MAWBOBL { get; set; }

        public string Transshipment1VesselName { get; set; }
        public string Transshipment1VesselId { get; set; }
        public string Transshipment1VesselCode { get; set; }
        public bool Transshipment1VesselAddedManually { get; set; }



        public string Transshipment2CarrierName { get; set; }
        public string Transshipment2CarrierCode { get; set; }
        public string Transshipment2CarrierId { get; set; }
        public bool Transshipment2CarrierAddedManually { get; set; }
        public string Transshipment2AirlinePrefix { get; set; }
        public DateTime? Transshipment2ETD { get; set; }
        public DateTime? Transshipment2ETA { get; set; }
        public DateTime? Transshipment2ATD { get; set; }
        public DateTime? Transshipment2ATA { get; set; }
        public string Transshipment2MAWBOBL { get; set; }
        public string Transshipment2VesselName { get; set; }
        public string Transshipment2VesselId { get; set; }
        public string Transshipment2VesselCode { get; set; }
        public bool Transshipment2VesselAddedManually { get; set; }



        public string Transshipment3CarrierName { get; set; }
        public string Transshipment3CarrierCode { get; set; }
        public string Transshipment3CarrierId { get; set; }
        public bool Transshipment3CarrierAddedManually { get; set; }
        public string Transshipment3AirlinePrefix { get; set; }
        public DateTime? Transshipment3ETD { get; set; }
        public DateTime? Transshipment3ETA { get; set; }
        public DateTime? Transshipment3ATD { get; set; }
        public DateTime? Transshipment3ATA { get; set; }
        public string Transshipment3MAWBOBL { get; set; }
        public string Transshipment3VesselName { get; set; }
        public string Transshipment3VesselId { get; set; }
        public string Transshipment3VesselCode { get; set; }
        public bool Transshipment3VesselAddedManually { get; set; }
        public string TrailerNumber { get; set; }

        

        public PortSL FinalDistenationPort { get; set; }
        public PortSL MainCarriageFromPort { get; set; }
        public PortSL MainCarriageToPort { get; set; }



        public PortSL Transshipment1FromPort { get; set; }
        public PortSL Transshipment1ToPort { get; set; }
        public PortSL Transshipment2FromPort { get; set; }
        public PortSL Transshipment2ToPort { get; set; }
        public PortSL Transshipment3FromPort { get; set; }
        public PortSL Transshipment3ToPort { get; set; }




        public string MoveTypeId { get; set; }
        public string MoveTypeName { get; set; }
        public string MoveTypeCode { get; set; }
        public bool MoveTypeAddedManually { get; set; }
        public string MoveTypeTransportModeId { get; set; }
        

        public string ValueOfGoodsCurrencyId { get; set; }
        public string ValueOfGoodsCurrencyName { get; set; }
        public string ValueOfGoodsCurrencyCode { get; set; }
        public bool   ValueOfGoodsCurrencyAddedManually { get; set; }

        public ShipmentPickUpDeliverySL ShipmentPickUp { get; set; }
        public ShipmentPickUpDeliverySL ShipmentDelivery { get; set; }

        public string LongMaster { get; set; }
        public List<HouseSL> Houses
        {
            get
            {
                if(houses == null)
                {
                    houses = new List<HouseSL>();
                }

                return houses;
            }

            set
            {
                houses = value;
            }
        }
        List<HouseSL> houses;

        private List<ShipmentPackagePM> shipmentPackages;
        public virtual List<ShipmentPackagePM> ShipmentPackages
        {
            get
            {
                if (shipmentPackages == null)
                {
                    shipmentPackages = new List<ShipmentPackagePM>();
                }

                return this.shipmentPackages;
            }

            set
            {
                if (value != null)
                {
                    shipmentPackages = value;
                }
            }
        }



    }

    





}
