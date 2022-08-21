using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Logitude.BL.ShipmentsModel.CustomFilters
{
    public static class DigitalPortalCustomFilter
    {
        public static IQueryable<ShipmentDataView> ApplyShipperConsigneeFilter(QueryFilterItem item, IQueryable<ShipmentDataView> queryableData)
        {
            string[] values = item.FieldValue == null ? null : item.FieldValue.ToString().Split(',');
            if (values != null && values.Length > 0)
            {
                queryableData = queryableData.Where(d => values.Contains(d.ShipperId) || values.Contains(d.ConsigneeId));
            }

            return queryableData;
        }

        public static IQueryable<ShipmentDataView> ApplyDigitalPortalSearchFilter(QueryFilterItem item, IQueryable<ShipmentDataView> queryableData)
        {
            string digitalPortalSearchFields = item.FieldValue as string;
            digitalPortalSearchFields = digitalPortalSearchFields.ToLower().Trim();
            queryableData = queryableData.Where(d =>
             d.ShipperReference1.Contains(digitalPortalSearchFields)
          || d.ShipperReference2.Contains(digitalPortalSearchFields)
          || d.ConsigneeReference1.Contains(digitalPortalSearchFields)
          || d.ConsigneeReference2.Contains(digitalPortalSearchFields)
          || d.ShipmentNumber.Contains(digitalPortalSearchFields)
          || d.MainCarriageCarrierNumber.Contains(digitalPortalSearchFields)
          || d.House.Contains(digitalPortalSearchFields)
          || d.Master.Contains(digitalPortalSearchFields)
          || d.AgentName.ToLower().StartsWith(digitalPortalSearchFields)
          || d.CustomAgentImportName.ToLower().StartsWith(digitalPortalSearchFields)
          || d.ConsigneeName.ToLower().StartsWith(digitalPortalSearchFields)
          || d.ShipperName.ToLower().StartsWith(digitalPortalSearchFields)
          || d.CustomerName.ToLower().StartsWith(digitalPortalSearchFields)
          || d.Notify1Name.ToLower().StartsWith(digitalPortalSearchFields)
          || d.Notify2Name.ToLower().StartsWith(digitalPortalSearchFields)
          || d.CustomAgentExportName.ToLower().StartsWith(digitalPortalSearchFields)
          || d.ShipperNotExporterName.ToLower().StartsWith(digitalPortalSearchFields)
          || d.ConsigneeNotImporterName.ToLower().StartsWith(digitalPortalSearchFields)
          || d.FreightForwarderName.ToLower().StartsWith(digitalPortalSearchFields)
          || d.ReleasingAgentName.ToLower().StartsWith(digitalPortalSearchFields)
          || d.ConsolidatorName.ToLower().StartsWith(digitalPortalSearchFields)
          || d.FromPortName.ToLower().StartsWith(digitalPortalSearchFields)
          || d.ToPortName.ToLower().StartsWith(digitalPortalSearchFields)
          || d.MainCarriageCarrierName.ToLower().StartsWith(digitalPortalSearchFields)
           );

            return queryableData;
        }
    }
}