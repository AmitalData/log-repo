using Logitude.BL.ShipmentsModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.DataContracts
{
    public class HouseSL
    {

        public string HouseNumber { get; set; }
        public string ShipmentNumber { get; set; }
        public PartnerSL Shipper  { get; set; }
        public DateTime? HAWBDate { get; set; }
        public string FreightPrepaidCollectId { get; set; }
        public string OtherPrepaidCollectId { get; set; }
        public string MainCarriageCarrierNumber { get; set; }
        public double? ValueOfGoods { get; set; }
        public bool IsDangerous { get; set; }
        
        public string IncotermCode { get; set; }
        public string IncotermName { get; set; }
        public string IncotermId { get; set; }
        public bool IncotermAddedManually { get; set; }


        public string MoveTypeId { get; set; }
        public string MoveTypeName { get; set; }
        public string MoveTypeCode { get; set; }
        public bool MoveTypeAddedManually { get; set; }
        public string MoveTypeTransportModeId { get; set; }


        public string CarrierCode { get; set; }
        public string CarrierName { get; set; }
        public string CarrierId { get; set; }
        public bool CarrierAddedManually { get; set; }


        public string GeneralDescriptionOfGoods { get; set; }


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
        public string ShipmentTypeId { get; set; }
        public string AgentName { get; set; }

        public string ConsigneeReference1 { get; set; }
        public string ConsigneeReference2 { get; set; }
        public string ShipperReference1 { get; set; }
        public string ShipperReference2 { get; set; }
        public string MainHarmonize { get; set; }



        public string ValueOfGoodsCurrencyId { get; set; }
        public string ValueOfGoodsCurrencyName { get; set; }
        public string ValueOfGoodsCurrencyCode { get; set; }
        public bool ValueOfGoodsCurrencyAddedManually { get; set; }



        public PartnerSL Notify1 { get; set; }
        public PartnerSL Consignee { get; set; }
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
