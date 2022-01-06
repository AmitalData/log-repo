using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.QueueServices
{
    public class CargoReferencesSyncQueueService
    {
        const int ShipmentTable_GetAllCustomsShipmentsThatContainForwardingShipments = 1;
        const int ShipmentTable_GetAllNonCustomShipmentsThatContainForwardingShipments = 2;
        const int ShipmentTable_GetShipmentOrders = 3;
        public void InsertToQueue(BulkDataPreperation bulkDataPreperation)
        {
            var shipmentsQueryType = bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.CurrentCondition;
            switch (shipmentsQueryType)
            {
                case ShipmentTable_GetAllNonCustomShipmentsThatContainForwardingShipments:
                    InsertForwardingIdsToUpdateCustoms(bulkDataPreperation);
                    break;

            }
        }

        private void InsertForwardingIdsToUpdateCustoms(BulkDataPreperation bulkDataPreperation)
        {
            var shipmentsDataTabel = bulkDataPreperation.SelectedDataTable;
            var referencesDateTable = GetReferencesDateTable();
            foreach (DataRow shipmentRow in shipmentsDataTabel.Rows)
            {
                referencesDateTable.Rows.Add(CreateQueueRow(referencesDateTable, shipmentRow));
            }
        }

        private DataRow CreateQueueRow(DataTable referencesDateTable, DataRow shipmentRow)
        {
            var row = referencesDateTable.NewRow();
            row["ForwardingShipmentId"] = shipmentRow[""];
            row["ShipmentNeedUpdateType"] = shipmentRow[""];
            row["Tenant"] = shipmentRow["Tenant"];
            return row;
        }

        private DataTable GetReferencesDateTable()
        {
            var table = new DataTable();
            table.Columns.Add("ForwardingShipmentId", typeof(string));
            table.Columns.Add("ShipmentNeedUpdateType", typeof(string));
            table.Columns.Add("Tenant", typeof(int));
            return table;
        }
    }
}
