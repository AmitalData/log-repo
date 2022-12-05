using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
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

        public PackageItemProviderWithAllDetails GetPackageItemProviderWithAllDetails(ShipmentPackageItem item)
        {
            return new PackageItemProviderWithAllDetails()
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
                if (shipmentReceivable.TotalAmount == null) return 0;
                return shipmentReceivable.TotalAmount;
            });
        }

        public string GetFormatedNumber(double? number)
        {
            if (number == null) return "";
            return String.Format("{0:#,0.00}", number.Value);
        }
    }
}