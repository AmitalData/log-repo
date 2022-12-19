using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.ReportsWebServices
{
    public class ShipmentPackingDataProviderService
    {
        public ShipmentPackingDataProviderService()
        {

        }

        public PackageItemProviderDetails GetPackageItemDetails(ShipmentPackageItem item)
        {
            return new PackageItemProviderDetails()
            {
                Description = item.Description,
                Quantity = item.Quantity == null ? "" : item.Quantity + "",
                Value = item.GoodsValue == null ? "" : String.Format("{0:#,0.00}", item.GoodsValue),
                Amount = item.Quantity == null || item.GoodsValue == null ? "" : String.Format("{0:#,0.00}", item.Quantity * item.GoodsValue)
            };
        }
        public void MapWeightDetails(ShipmentPackingDataProvider provider, ShipmentPM shipment)
        {
            const string kiloGramUnitCode = "KG";
            const string boundUnitCode = "LB";

            double? grossWeightInKG = General.ComputeWeightInSelectedUnit(shipment.GrossWeight, shipment.GrossWeightUnitCode, kiloGramUnitCode);
            double? grossWeightInLB = General.ComputeWeightInSelectedUnit(shipment.GrossWeight, shipment.GrossWeightUnitCode, boundUnitCode);
            if (grossWeightInKG != null)
            {
                provider.GrossWeightInKG = String.Format("{0:#,0.00}", grossWeightInKG.Value);
            }
            if (grossWeightInLB != null)
            {
                provider.GrossWeightInLB = String.Format("{0:#,0.00}", grossWeightInLB.Value);
            }
        }
        public void MapVolumeDetails(ShipmentPackingDataProvider provider, ShipmentPM shipment)
        {
            const string VolumeCBMUnitCode = "CBM";
            const string VolumeCBFUnitCode = "CBF";
            provider.VolumeInCBM = General.ComputeVolumeInSelectedUnit(shipment.Volume, shipment.VolumeUnitCode, VolumeCBMUnitCode);
            provider.VolumeInCBF = General.ComputeVolumeInSelectedUnit(shipment.Volume, shipment.VolumeUnitCode, VolumeCBFUnitCode);
        }
        public void MapCarrierType(ShipmentPackingDataProvider provider, ShipmentPM shipment)
        {
            if (shipment == null || string.IsNullOrEmpty(shipment.TransportModeName)) return;
            if (shipment.TransportModeName == "Ocean")
            {
                provider.CarrierType = "Vessel";
                return;
            }
            if (shipment.TransportModeName == "Air")
            {
                provider.CarrierType = "Air";
                return;
            }
            if (shipment.TransportModeName == "Inland")
            {
                provider.CarrierType = "Truck";
                return;
            }
        }
        public void MapTotalFields(MapTotalFieldsParameters mapTotalFieldsParameters)
        {
            List<string> freightChargesTypesIds = GetFreightChargesIdsByTenant(mapTotalFieldsParameters.Tenant);
            
            double? totalReceivablesForFreightCharges = GetTotalReceivablesForFreightChargesByFreightChargesTypesIds(mapTotalFieldsParameters.Shipment, freightChargesTypesIds);
            double? totalReceivablesForOtherCharges = GetTotalReceivablesForOtherChargesByFreightChargesTypesIds(mapTotalFieldsParameters.Shipment, freightChargesTypesIds);
            double? totalAmount = totalReceivablesForFreightCharges + totalReceivablesForOtherCharges + mapTotalFieldsParameters.TotalAmountOfPackageItems;

            mapTotalFieldsParameters.ShipmentPackingDataProvider.TotalReceivablesForFreightCharges = GetFormatedNumber(totalReceivablesForFreightCharges);
            mapTotalFieldsParameters.ShipmentPackingDataProvider.TotalReceivablesForOtherCharges = GetFormatedNumber(totalReceivablesForOtherCharges);
            mapTotalFieldsParameters.ShipmentPackingDataProvider.TotalAmounts = GetFormatedNumber(totalAmount);
        }

        public decimal? GetTotalAmountOfPackageItems(List<ShipmentPackageItem> shipmentPackageItems)
        {
            if (shipmentPackageItems == null || shipmentPackageItems.Count == 0) return 0;

            decimal? totalAmountOfPackageItems = shipmentPackageItems.Sum(shipmentPackageItem => 
            {
                if (shipmentPackageItem.Quantity == null || shipmentPackageItem.GoodsValue == null) return 0;
                return shipmentPackageItem.Quantity * shipmentPackageItem.GoodsValue;
            });

            return totalAmountOfPackageItems;
        }

        public List<string> GetFreightChargesIdsByTenant(int tenant)
        {
            const string freightChargeGroupCode = "FRT";
            ChargesTypeQuery chargesTypeQuery = new ChargesTypeQuery(tenant);
            List<string> freightChargesTypesIds = chargesTypeQuery.GetChargesTypesIdsByChargeGroupCodeAndTenant(freightChargeGroupCode, tenant);
            return freightChargesTypesIds;
        }

        public double? GetTotalReceivablesForFreightChargesByFreightChargesTypesIds(ShipmentPM shipment, List<string> freightChargesTypesIds)
        {
            if (freightChargesTypesIds == null || freightChargesTypesIds.Count == 0) return 0;
            if (shipment == null) return 0;
            if (shipment.ShipmentReceivables == null || shipment.ShipmentReceivables.Count == 0) return 0;

            List<ShipmentReceivablePM> shipmentReceivables = shipment.ShipmentReceivables
                .Where(shipmentReceivable => freightChargesTypesIds.Contains(shipmentReceivable.ChargesTypeId)).ToList();

            if (shipmentReceivables == null || shipmentReceivables.Count == 0) return 0;

            double? totalReceivablesForFreightCharges = GetTotalReceivables(shipmentReceivables);

            return totalReceivablesForFreightCharges;
        }

        public double? GetTotalReceivablesForOtherChargesByFreightChargesTypesIds(ShipmentPM shipment, List<string> freightChargesTypesIds)
        {
            if (freightChargesTypesIds == null || freightChargesTypesIds.Count == 0) return 0;
            if (shipment == null) return 0;
            if (shipment.ShipmentReceivables == null || shipment.ShipmentReceivables.Count == 0) return 0;

            List<ShipmentReceivablePM> shipmentReceivables = shipment.ShipmentReceivables
                .Where(shipmentReceivable => !freightChargesTypesIds.Contains(shipmentReceivable.ChargesTypeId)).ToList();

            if (shipmentReceivables == null || shipmentReceivables.Count == 0) return 0;
            double? totalReceivablesForOtherCharges = GetTotalReceivables(shipmentReceivables);

            return totalReceivablesForOtherCharges;
        }

        private static double? GetTotalReceivables(List<ShipmentReceivablePM> shipmentReceivables)
        {
            return shipmentReceivables.Sum(shipmentReceivable =>
            {
                if (shipmentReceivable.AmountInProfitCurrency == null) return 0;
                return shipmentReceivable.AmountInProfitCurrency;
            });
        }

        public string GetFormatedNumber(double? number)
        {
            if (number == null) return "";
            return String.Format("{0:#,0.00}", number.Value);
        }

        public string GetDestinationPortCountryName(MapDestinationPortCountryNameParameters mapDestinationPortCountryNameParameters)
        {
            Country country = mapDestinationPortCountryNameParameters.CountryRepository.GetSingleCountryByIdAndTenant(mapDestinationPortCountryNameParameters.CountryId,
                mapDestinationPortCountryNameParameters.Tenant, true);
            if (country != null)
            {
                return country.EnglishName;
            }

            return "";
        }

        public DestinationPortDetails GetDestinationPortDetails(ShipmentPM shipment, Port mainCarriageToPort)
        {
            if (shipment.Transshipment3ToPortId != null)
            {
                return new DestinationPortDetails()
                {
                    Name = shipment.Transshipment3ToPortName,
                    CountryName = shipment.Transshipment3ToPortCountryName
                };
            }
            
            if (shipment.Transshipment2ToPortId != null)
            {
                return new DestinationPortDetails()
                {
                    Name = shipment.Transshipment2ToPortName,
                    CountryName = shipment.Transshipment2ToPortCountryName
                };
            }
            
            if (shipment.Transshipment1ToPortId != null)
            {
                return new DestinationPortDetails()
                {
                    Name = shipment.Transshipment1ToPortName,
                    CountryName = shipment.Transshipment1ToPortCountryName
                };
            }
            
            if (mainCarriageToPort != null)
            {
                return new DestinationPortDetails()
                {
                    Name = mainCarriageToPort.EnglishName,
                    CountryName = mainCarriageToPort.CountryName
                };
            }

            return new DestinationPortDetails()
            {
                Name = "",
                CountryName = ""
            };
        }
    }
}