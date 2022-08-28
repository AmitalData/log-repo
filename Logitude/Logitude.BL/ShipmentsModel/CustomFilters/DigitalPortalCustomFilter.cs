using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Logitude.BL.ShipmentsModel.CustomFilters
{
    public static class DigitalPortalCustomFilter
    {
        public static IQueryable<DigitalShipmentsDataView> ApplyShipperConsigneeFilter(QueryFilterItem item, IQueryable<DigitalShipmentsDataView> queryableData)
        {
            string[] values = item.FieldValue == null ? null : item.FieldValue.ToString().Split(',');
            if (values != null && values.Length > 0)
            {
                queryableData = queryableData.Where(d => values.Contains(d.ShipperId) || values.Contains(d.ConsigneeId));
            }

            return queryableData;
        }

        public static IQueryable<DigitalShipmentsDataView> ApplyDigitalPortalSearchFilter(QueryFilterItem item, IQueryable<DigitalShipmentsDataView> queryableData)
        {
            string digitalPortalSearchFields = item.FieldValue as string;
            digitalPortalSearchFields = digitalPortalSearchFields.ToLower().Trim();
            queryableData = queryableData.Where(d =>
            d.ShipmentNumber.Contains(digitalPortalSearchFields)
              || d.Master.Contains(digitalPortalSearchFields)
              || d.ConsigneeName.ToLower().StartsWith(digitalPortalSearchFields)
              || d.ShipperName.ToLower().StartsWith(digitalPortalSearchFields)
              || d.FromPortName.ToLower().StartsWith(digitalPortalSearchFields)
              || d.ToPortName.ToLower().StartsWith(digitalPortalSearchFields)
              || d.MainCarriageCarrierName.ToLower().StartsWith(digitalPortalSearchFields)
           );

            return queryableData;
        }

        public static IQueryable<DigitalShipmentsDataView> ApplyInTransitFilter(QueryFilterItem item, IQueryable<DigitalShipmentsDataView> queryableData, Simplog.Data.ShipmentsModel.Repositories.ShipmentRepository shipmentRepository)
        {
            queryableData = (from shipment in queryableData
                                                      where (shipment.MainCarriageATD != null || shipment.Transshipment1ATD != null || shipment.Transshipment2ATD != null || shipment.Transshipment3ATD != null)
                                                      && (shipment.Transshipment3ToPortId == null || shipment.Transshipment3ATA == null)
                                                      && (shipment.Transshipment2ToPortId == null || shipment.Transshipment2ATA == null)
                                                      && (shipment.Transshipment1ToPortId == null || shipment.Transshipment1ATA == null)
                                                      && (shipment.MainCarriageToPortId == null || shipment.MainCarriageATA == null)
                                                      && !(shipmentRepository.context.ShipmentPickUpDeliveries.Any(delivery => delivery.ShipmentId == shipment.Id && delivery.PickUpDeliveryTypeCode == "DELV" && delivery.ATA != null))
                                                      select shipment);

            return queryableData;
        }
    }
}