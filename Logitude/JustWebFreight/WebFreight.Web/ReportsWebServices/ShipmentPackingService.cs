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
    public class ShipmentPackingService
    {
        public ShipmentPackingService()
        {

        }

        public PackageItemProviderWithAllDetails GetPackageItemProviderWithAllDetails(ShipmentPackageItem item)
        {
            return new PackageItemProviderWithAllDetails()
            {
                Description = item.Description,
                Quantity = item.Quantity == null ? "" : String.Format("{0:#,0.00}", item.Quantity),
                Value = item.GoodsValue == null ? "" : String.Format("{0:#,0.00}", item.GoodsValue),
                Amount = item.Quantity == null || item.GoodsValue == null ? "" : String.Format("{0:#,0.00}", item.Quantity * item.GoodsValue)
            };
        }
        public void SetShipmentPackingDataProviderWeightDetails(ShipmentPackingDataProvider provider, ShipmentPM shipment)
        {
            double? grossWeightInKG = General.ComputeWeightInSelectedUnit(shipment.GrossWeight, shipment.GrossWeightUnitCode, "KG");
            double? grossWeightInLB = General.ComputeWeightInSelectedUnit(shipment.GrossWeight, shipment.GrossWeightUnitCode, "LB");
            if (grossWeightInKG != null)
            {
                provider.GrossWeightInKG = String.Format("{0:#,0.00}", grossWeightInKG.Value);
            }
            if (grossWeightInLB != null)
            {
                provider.GrossWeightInLB = String.Format("{0:#,0.00}", grossWeightInLB.Value);
            }
        }
        public void SetShipmentPackingDataProviderVolumeDetails(ShipmentPackingDataProvider provider, ShipmentPM shipment)
        {
            provider.VolumeInCBM = General.ComputeVolumeInSelectedUnit(shipment.Volume, shipment.VolumeUnitCode, "CBM");
            provider.VolumeInCBF = General.ComputeVolumeInSelectedUnit(shipment.Volume, shipment.VolumeUnitCode, "CBF");
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

        public string GetTotalReceivablesForFreightChargesByFreightChargesTypesIds(ShipmentPM shipment, List<string> freightChargesTypesIds)
        {
            if (freightChargesTypesIds == null || freightChargesTypesIds.Count == 0) return "0.00";
            if (shipment == null) return "0.00";
            if (shipment.ShipmentReceivables == null || shipment.ShipmentReceivables.Count == 0) return "0.00";

            List<ShipmentReceivablePM> shipmentReceivables = shipment.ShipmentReceivables
                .Where(shipmentReceivable => freightChargesTypesIds.Contains(shipmentReceivable.ChargesTypeId)).ToList();

            if (shipmentReceivables == null || shipmentReceivables.Count == 0) return "0.00";

            double? totalReceivablesForFreightCharges = GetTotalReceivables(shipmentReceivables);

            return String.Format("{0:#,0.00}", totalReceivablesForFreightCharges.Value);
        }

        public string GetTotalReceivablesForOtherChargesByFreightChargesTypesIds(ShipmentPM shipment, List<string> freightChargesTypesIds)
        {
            if (freightChargesTypesIds == null || freightChargesTypesIds.Count == 0) return "0.00";
            if (shipment == null) return "0.00";
            if (shipment.ShipmentReceivables == null || shipment.ShipmentReceivables.Count == 0) return "0.00";

            List<ShipmentReceivablePM> shipmentReceivables = shipment.ShipmentReceivables
                .Where(shipmentReceivable => !freightChargesTypesIds.Contains(shipmentReceivable.ChargesTypeId)).ToList();

            if (shipmentReceivables == null || shipmentReceivables.Count == 0) return "0.00";
            double? totalReceivablesForOtherCharges = GetTotalReceivables(shipmentReceivables);

            return String.Format("{0:#,0.00}", totalReceivablesForOtherCharges.Value);
        }

        private static double? GetTotalReceivables(List<ShipmentReceivablePM> shipmentReceivables)
        {
            return shipmentReceivables.Sum(shipmentReceivable =>
            {
                if (shipmentReceivable.TotalAmount == null) return 0;
                return shipmentReceivable.TotalAmount;
            });
        }
    }
}