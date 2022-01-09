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
    public class CargoReferencesSyncQueueService
    {
        const int ShipmentTable_GetAllCustomsShipmentsThatContainForwardingShipments = 1;
        const int ShipmentTable_GetAllNonCustomShipmentsThatContainForwardingShipments = 2;
        const int ShipmentTable_GetShipmentOrders = 3;

        const string QueueTableName = "CargoReferencesSyncQueues";
        public void InsertToQueue(BulkDataPreperation bulkDataPreperation)
        {
            InsertForwardingIdsToUpdateCustoms(bulkDataPreperation);
        }

        private void InsertForwardingIdsToUpdateCustoms(BulkDataPreperation bulkDataPreperation)
        {
            var shipmentsQueryType = bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.CurrentCondition;
            var shipmentsDataTabel = bulkDataPreperation.SelectedDataTable;
            var referencesDateTable = GetReferencesDateTable();
            foreach (DataRow shipmentRow in shipmentsDataTabel.Rows)
            {
                AddRow(referencesDateTable, shipmentRow, shipmentsQueryType);
            }
            if (referencesDateTable.Rows.Count <= 0)
                return;
            InsertAsBullk(bulkDataPreperation, referencesDateTable);
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
            if (shipmentRow["CustomFileId"] == null || string.IsNullOrEmpty(shipmentRow["CustomFileId"].ToString()))
                return;
            referencesDateTable.Rows.Add(CreateQueueRowFromForwardingRecord(referencesDateTable, shipmentRow));
        }
        private void AddFromOrderRow(DataTable referencesDateTable, DataRow shipmentRow)
        {
            if (shipmentRow["ShipmentId"] == null || string.IsNullOrEmpty(shipmentRow["ShipmentId"].ToString()))
                return;
            referencesDateTable.Rows.Add(CreateQueueRowFromOrderRecord(referencesDateTable, shipmentRow));
        }
        private void InsertAsBullk(BulkDataPreperation bulkDataPreperation, DataTable referencesDateTable)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(
                    bulkDataPreperation.CargoTrackingUpdateDataBaseArgs.BuildCargoArgs.DestinationConnectionString, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.DestinationTableName = QueueTableName;
                bulkCopy.WriteToServer(referencesDateTable);
            }

        }
        private DataRow CreateQueueRowFromForwardingRecord(DataTable referencesDateTable, DataRow shipmentRow)
        {
            var row = referencesDateTable.NewRow();
            row["Tenant"] = shipmentRow["Tenant"];
            row["ShipmentId"] = shipmentRow["Id"];
            row["SyncTo"] = shipmentRow["CustomFileId"];
            row["ShipmentType"] = Codes.ForwardingType;
            return row;
        }
        private DataRow CreateQueueRowFromOrderRecord(DataTable referencesDateTable, DataRow shipmentRow)
        {
            var row = referencesDateTable.NewRow();
            row["Tenant"] = shipmentRow["Tenant"];
            row["ShipmentId"] = shipmentRow["Id"]; 
            row["SyncTo"] = shipmentRow["ShipmentId"]; 
            row["ShipmentType"] = Codes.OrderType;
            return row;
        }

        private DataTable GetReferencesDateTable()
        {
            var table = new DataTable();
            table.Columns.Add("ShipmentId", typeof(string));
            table.Columns.Add("ShipmentType", typeof(string));
            table.Columns.Add("Tenant", typeof(string));
            return table;
        }
    }
}
