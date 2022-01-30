using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.QueueServices
{
    public class CargoDisconnectQueueService
    {
        const int ShipmentTable_GetAllCustomsShipmentsThatContainForwardingShipments = 1;
        const int ShipmentTable_GetAllNonCustomShipmentsThatContainForwardingShipments = 2;
        const int ShipmentTable_GetShipmentOrders = 3;

        const string QueueTableName = "CargoDisconnectQueues";
        public void InsertToQueue(BulkDataPreperation bulkDataPreperation)
        {
            InsertShipmentIdsToDisconnectShipment(bulkDataPreperation);
        }

        private void InsertShipmentIdsToDisconnectShipment(BulkDataPreperation bulkDataPreperation)
        {
            var shipmentsQueryType = bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.CurrentCondition;
            var shipmentsDataTabel = bulkDataPreperation.SelectedDataTable;
            var disconnectQueueDateTable = GetDisconnectQueueDateTable();
            foreach (DataRow shipmentRow in shipmentsDataTabel.Rows)
            {
                AddRow(disconnectQueueDateTable, shipmentRow, shipmentsQueryType);
            }
            if (disconnectQueueDateTable.Rows.Count <= 0)
                return;
            InsertAsBullk(bulkDataPreperation, disconnectQueueDateTable);
        }

        private void AddRow(DataTable referencesDateTable, DataRow shipmentRow, int shipmentsQueryType)
        {
            switch (shipmentsQueryType)
            {
                case ShipmentTable_GetAllNonCustomShipmentsThatContainForwardingShipments:
                    AddFromForwardingRow(referencesDateTable, shipmentRow);
                    break;
                case ShipmentTable_GetShipmentOrders:
                    AddFromOrderRow(referencesDateTable, shipmentRow);
                    break;

            }

        }
        private void AddFromForwardingRow(DataTable referencesDateTable, DataRow shipmentRow)
        {
            if (shipmentRow["CustomFileId"] != null && shipmentRow["CustomFileId"]!= DBNull.Value && !string.IsNullOrEmpty(shipmentRow["CustomFileId"].ToString()))
                return;
            referencesDateTable.Rows.Add(CreateQueueRow(referencesDateTable, shipmentRow, Codes.ForwardingType));
        }
        private void AddFromOrderRow(DataTable referencesDateTable, DataRow shipmentRow)
        {
            if (shipmentRow["ShipmentId"] != null &&  shipmentRow["ShipmentId"] != DBNull.Value && !string.IsNullOrEmpty(shipmentRow["ShipmentId"].ToString()))
                return;
            referencesDateTable.Rows.Add(CreateQueueRow(referencesDateTable, shipmentRow, Codes.OrderType));
        }
        private void InsertAsBullk(BulkDataPreperation bulkDataPreperation, DataTable referencesDateTable)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(
                    bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.DestinationConnectionString))
            {
                bulkCopy.DestinationTableName = QueueTableName;
                bulkCopy.WriteToServer(referencesDateTable);
            }

        }
        private DataRow CreateQueueRow(DataTable referencesDateTable, DataRow shipmentRow,string type)
        {
            var row = referencesDateTable.NewRow();
            row["Id"] = DBNull.Value;
            row["Tenant"] = shipmentRow["Tenant"];
            row["ShipmentId"] = shipmentRow["Id"];
            row["ShipmentType"] = type;
            return row;
        }
        

        private DataTable GetDisconnectQueueDateTable()
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Tenant", typeof(int));
            table.Columns.Add("ShipmentId", typeof(string));
            table.Columns.Add("ShipmentType", typeof(string));
            return table;
        }
    }
}
