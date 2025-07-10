using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure.Helper
{
    public class TableStructureHelper
    {
        private DataSet xmlDataSet;
        private DataTable table;
        private DataTable tableCoulmns;
        private DataTable tableCoulmnsConstraint;
        private DataTable tableCoulmnsRelation;
        private DataTable tableIndexs;
        private DataTable tableUniqueConstraints;
        public string PrimarykeyColumn;
        public List<string> TableCoulmnsNameWithoutIDentity;

        public TableStructureHelper(string dxmlStructure)
        {
            InitializeDataTables(dxmlStructure);
        }


        private void InitializeDataTables(string dxmlStructure)
        {
            TableCoulmnsNameWithoutIDentity = new List<string>();
            xmlDataSet = new DataSet();
            xmlDataSet.ReadXml(XmlReader.Create(new StringReader(dxmlStructure)));
            table = xmlDataSet.Tables["Table"];
            tableCoulmns = table.ChildRelations["Table_Column"].ChildTable;
            tableIndexs = table.ChildRelations["Table_Index"] == null ? null : table.ChildRelations["Table_Index"].ChildTable;
            tableCoulmnsRelation = table.ChildRelations["Table_Relation"] == null ? null : table.ChildRelations["Table_Relation"].ChildTable;
            tableUniqueConstraints = table.ChildRelations["Table_UniqueConstraint"] == null ? null : table.ChildRelations["Table_UniqueConstraint"].ChildTable;
            tableCoulmnsConstraint = tableCoulmns.ChildRelations["Column_Constraints"].ChildTable;
        }
        public string GetTableStructure(string tableName)
        {

            string tableStructure = InitializeTableStructure(tableName);
            tableStructure += GetTableStructurCoulmns();
            tableStructure += GetTableStructurePrimartKey(tableName);
            tableStructure += GetTableStructureIndexs(tableName);
            return tableStructure;

        }

        private string InitializeTableStructure(string tableName)
        {
            string tableStructure = "If not exists (select * from sysobjects where name='" + tableName + "' and xtype='U')\n" +
                                  "BEGIN \n" +
                                  "CREATE TABLE [dbo].[" + tableName + "]( \n";

            return tableStructure;

        }

        private string GetTableStructurCoulmns()
        {
            string tableStructure = "";
            for (int i = 0; i < tableCoulmns.Rows.Count; i++)
            {
                if (tableCoulmns.Columns.Contains("Name") && tableCoulmns.Columns.Contains("Type"))
                    tableStructure += "[" + tableCoulmns.Rows[i]["Name"] + "] " + tableCoulmns.Rows[i]["Type"];
                if (tableCoulmns.Columns.Contains("Size"))
                    tableStructure += GetCoulmnSize(tableCoulmns.Rows[i]["Size"]);
                if (tableCoulmns.Columns.Contains("Identity"))
                {
                    string identityText = GetIsCoulmnIdentity(tableCoulmns.Rows[i]["Identity"]);
                    tableStructure += identityText;
                    if (string.IsNullOrEmpty(identityText))
                        TableCoulmnsNameWithoutIDentity.Add((string)tableCoulmns.Rows[i]["Name"]);
                }
                else
                    TableCoulmnsNameWithoutIDentity.Add((string)tableCoulmns.Rows[i]["Name"]);
                if (tableCoulmnsConstraint.Columns.Contains("Nullable"))
                    tableStructure += GetIsCoulmnNullable(tableCoulmnsConstraint.Rows[i]["Nullable"]);
                if (tableCoulmnsConstraint.Columns.Contains("PrimaryKey") && tableCoulmns.Columns.Contains("Name"))
                    SetPrimartKeyCoulmn(tableCoulmnsConstraint.Rows[i]["PrimaryKey"], (string)tableCoulmns.Rows[i]["Name"]);
                tableStructure += ",\n";
            }
            return tableStructure;
        }


        public string GetTableName()
        {
            string tableName = (string)table.Rows[0]["Name"];
            return tableName;
        }


        private string GetCoulmnSize(object coulmnnSize)
        {
            string tableCoulmnnSize = null;
            double? size = 0;
            try { size = double.Parse((string)coulmnnSize); }
            catch (Exception e) { size = 0; }
            if (size != null && size != 0)
                if (size == -1)
                    tableCoulmnnSize = " (Max) ";
                else
                    tableCoulmnnSize = " (" + size + ") ";

            return tableCoulmnnSize;
        }
        private string GetIsCoulmnNullable(object coulmnnNullable)
        {
            string tableIsCoulmnNullable = null;
            bool? nullable = true;
            try { nullable = bool.Parse((string)coulmnnNullable); }
            catch (Exception e) { nullable = true; }
            if (nullable == true) { tableIsCoulmnNullable += " null "; }
            else { tableIsCoulmnNullable += " not null "; }

            return tableIsCoulmnNullable;
        }

        private string GetIsCoulmnIdentity(object coulmnnIdentity)
        {
            string tableIsCoulmnIdentity = null;
            bool? isIdentity = false;
            try { isIdentity = bool.Parse((string)coulmnnIdentity); }
            catch (Exception e) { isIdentity = false; }
            if (isIdentity == true) { tableIsCoulmnIdentity += " IDENTITY(1,1) "; }

            return tableIsCoulmnIdentity;
        }

        private void SetPrimartKeyCoulmn(object coulmnnPrimarykey, string columnName)
        {
            bool? isPrimartKey = false;
            var isNull = coulmnnPrimarykey.GetType().Name == "DBNull";
            try { isPrimartKey = bool.Parse((string)coulmnnPrimarykey); }
            catch (Exception e) { isPrimartKey = false; }
            if (isPrimartKey == true) { PrimarykeyColumn = columnName; }
        }

        private string GetTableStructurePrimartKey(string tableName)
        {
            string tablePrimaryKey = null;

            if (!string.IsNullOrEmpty(PrimarykeyColumn))
            {
                tablePrimaryKey = "CONSTRAINT[PK_" + tableName + "] PRIMARY KEY([" + PrimarykeyColumn + "]) )\n";
            }

            return tablePrimaryKey;
        }

        public string GetTableStructureIndexs(string tableName)
        {
            string tableIndexsCommand = "";
            if (tableIndexs != null)
            {
                for (int j = 0; j < tableIndexs.Rows.Count; j++)
                {
                    if (tableIndexs.Columns.Contains("Columns"))
                    {
                        string coulmnIndexs = (string)tableIndexs.Rows[j]["Columns"];
                        tableIndexsCommand += "CREATE NONCLUSTERED INDEX [IX_" + tableName + "_" + coulmnIndexs.Replace(',', '_') + "] ON [dbo].[" + tableName + "](" + coulmnIndexs + ")\n";
                        tableIndexsCommand += "ALTER INDEX [IX_" + tableName + "_" + coulmnIndexs.Replace(',', '_') + "] ON [dbo].[" + tableName + "] DISABLE \n";

                    }
                }
            }

            if (tableName == "Pre_CargoTrackingShipments")
            {
                tableIndexsCommand += "IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_CargoTrackingShipments_EntityType_ForwardingShipmentHeaderId') CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_EntityType_ForwardingShipmentHeaderId] ON [dbo].[Pre_CargoTrackingShipments] ([EntityType], [ForwardingShipmentHeaderId]) INCLUDE ([CurrentMilestoneCode],[CurrentMilestoneDate],[PickupDone],[PickupDate],[CreateDate],[FromWarehouseDate],[DepartureDone],[DepartureDate],[ArrivalDone],[ArrivalDate],[ToWarehouseDone],[ToWarehouseDate],[CustomsPaymentDone],[CustomsPaymentDate],[ClearanceDone],[ClearanceDate],[DeliveredDone],[DeliveredDate],[FromWarehouseDone],[AssignedTruckerDone],[AssignedTruckerDate],[AssignedCustomsAgentDone],[AssignedCustomsAgentDate],[DeliveryDone],[DeliveryDate],[GoodsClassificationDate],[DocumentInspectionDate],[DocumentInspectionDone],[GoodsClassificationDone],[GatepassArrivedDate],[GatepassArrivedDone],[CreatedDone],[BookingDone],[BookingDate],[PaymentRequiredDone],[PaymentRequiredDate],[PaymentReceivedDone],[PaymentReceivedDate],[InvoicedDate],[InvoicedDone],[PickupEstimationDate],[FromWarehouseEstimationDate],[FromWarehouseNotes],[DepartureEstimationDate],[ArrivalEstimationDate])";
                tableIndexsCommand += "IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_CargoTrackingShipments_Tenant_CustomerId_IsMainRecord') CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_Tenant_CustomerId_IsMainRecord] ON [dbo].[Pre_CargoTrackingShipments] ([Tenant],[CustomerId],[IsMainRecord]) INCLUDE ([EntityId],[ForwardingShipmentHeaderId],[EntityType],[CurrentMilestoneCode],[CurrentMilestoneDate],[TransportModeId],[Master],[House],[ShipmentNumber],[FromPortId],[ToPortId],[GrossWeight],[CreateDate],[SecurityKey],[ConsigneeName],[ShipperName],[CustomerReference],[DepartureDate],[DepartureEstimationDate],[ArrivalDate],[ArrivalEstimationDate],[PackagesQuantity],[DirectionId],[ShipmentLevelCode],[GrossWeightUnitCode],[CurrentMilestoneExceptions],[ForwardingHouse],[ForwardingMaster],[ForwardingShipmentLevelCode],[BookingNotes],[PoNumber],[IsOperationalClosed],[ChargeableWeightInKG],[ChargeableWeight],[ChargeableWeightUnitCode])";
            }
            if (tableName == "Pre_CargoTrackingShipmentSearches")
            {
                tableIndexsCommand += "IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_CargoTrackingShipmentSearches_Tenant') CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipmentSearches_Tenant] ON [dbo].[Pre_CargoTrackingShipmentSearches] ([Tenant]) INCLUDE ([SearchFields], [ShipmentId])";
                tableIndexsCommand += "IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_CargoTrackingShipmentSearches_ReferenceFromShipmentId') CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipmentSearches_ReferenceFromShipmentId] ON [dbo].[Pre_CargoTrackingShipmentSearches] ([ReferenceFromShipmentId]) INCLUDE ([ShipmentId])";
                tableIndexsCommand += "IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_CargoTrackingShipmentSearches_ShipmentId_ReferenceType') CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipmentSearches_ShipmentId_ReferenceType] ON [dbo].[Pre_CargoTrackingShipmentSearches] ([ShipmentId],[ReferenceType]) INCLUDE ([Tenant],[SearchFields],[ShipmentDate],[IsPublic],[ReferenceFromShipmentId])";
            }

            tableIndexsCommand += " End \n";
            return tableIndexsCommand;
        }

        public string GetTableStructureReBuildIndexs(string tableName)
        {
            string tableIndexs = "";
            if (this.tableIndexs != null)
            {
                for (int j = 0; j < this.tableIndexs.Rows.Count; j++)
                {
                    if (this.tableIndexs.Columns.Contains("Columns"))
                    {
                        string coulmnIndexs = (string)this.tableIndexs.Rows[j]["Columns"];
                        tableIndexs += "ALTER INDEX [IX_" + tableName + "_" + coulmnIndexs.Replace(',', '_') + "] ON [dbo].[" + tableName + "] REBUILD \n";

                    }

                }

            }

            return tableIndexs;
        }

        public string GetTableStructureRelations(string tableName)
        {
            string tableStructureRelations = "";
            if (tableCoulmnsRelation != null)
            {
                for (int j = 0; j < tableCoulmnsRelation.Rows.Count; j++)
                {
                    if (tableCoulmnsRelation.Columns.Contains("ForeignKeyColumn") &&
                        tableCoulmnsRelation.Columns.Contains("ReferencedTable") &&
                        tableCoulmnsRelation.Columns.Contains("ReferencedColumn"))
                    {
                        string foreignKeyColumn = (string)tableCoulmnsRelation.Rows[j]["ForeignKeyColumn"];
                        string referencedTable = (string)tableCoulmnsRelation.Rows[j]["ReferencedTable"];
                        string referencedColumn = (string)tableCoulmnsRelation.Rows[j]["ReferencedColumn"];
                        tableStructureRelations += "ALTER TABLE [dbo].[" + tableName + "] ADD CONSTRAINT [FK_" + tableName + "_" + referencedTable + "_" + foreignKeyColumn + "] FOREIGN KEY([" + foreignKeyColumn + "]) REFERENCES [dbo].[" + referencedTable + "]([" + referencedColumn + "])\n";
                        tableStructureRelations += "CREATE NONCLUSTERED INDEX [IX_" + tableName + "_" + foreignKeyColumn + "] ON [dbo].[" + tableName + "]([" + foreignKeyColumn + "])\n";
                    }
                }

            }

            return tableStructureRelations;
        }

        public string GetTableStructureChangeNameScript(string oldTableName, string newTableName)
        {
            string tableStructureChaneNameScript = "";
            tableStructureChaneNameScript = "EXEC sp_rename '" + oldTableName + "', '" + newTableName + "' \n ";
            tableStructureChaneNameScript += " exec sp_rename 'PK_" + oldTableName + "', 'PK_" + newTableName + "', 'object' \n ";
            tableStructureChaneNameScript += GetTableStructureChaneNameScriptFromRelations(oldTableName, newTableName);
            tableStructureChaneNameScript += GetTableStructureChaneNameScriptFromUniqueConstraints(oldTableName, newTableName);


            return tableStructureChaneNameScript;
        }
        private string GetTableStructureChaneNameScriptFromUniqueConstraints(string oldTableName, string newTableName)
        {
            string tableStructureChaneNameScriptFromUniqueConstraints = "";
            if (tableUniqueConstraints != null && tableUniqueConstraints.Rows != null && tableUniqueConstraints.Rows.Count > 0)
            {
                for (int j = 0; j < tableUniqueConstraints.Rows.Count; j++)
                {
                    if (tableUniqueConstraints.Columns.Contains("Columns"))
                    {
                        string uniqeConstraintFields = (string)tableUniqueConstraints.Rows[j]["Columns"];
                        tableStructureChaneNameScriptFromUniqueConstraints += " exec sp_rename 'UQ_" + oldTableName + "_" + uniqeConstraintFields.Replace(',', '_') + "', 'UQ_" + newTableName + "_" + uniqeConstraintFields.Replace(',', '_') + "', 'object' \n ";

                    }

                }
            }

            return tableStructureChaneNameScriptFromUniqueConstraints;
        }
        private string GetTableStructureChaneNameScriptFromRelations(string oldTableName, string newTableName)
        {
            string tableStructureChaneNameScriptFromRelations = "";
            if (tableCoulmnsRelation != null && tableCoulmnsRelation.Rows != null && tableCoulmnsRelation.Rows.Count > 0)
            {

                for (int j = 0; j < tableCoulmnsRelation.Rows.Count; j++)
                {
                    if (tableCoulmnsRelation.Columns.Contains("ForeignKeyColumn") &&
                           tableCoulmnsRelation.Columns.Contains("ReferencedTable") &&
                           tableCoulmnsRelation.Columns.Contains("ReferencedColumn"))
                    {
                        string foreignKeyColumn = (string)tableCoulmnsRelation.Rows[j]["ForeignKeyColumn"];
                        string referencedTable = (string)tableCoulmnsRelation.Rows[j]["ReferencedTable"];
                        tableStructureChaneNameScriptFromRelations += " exec sp_rename 'FK_" + oldTableName + "_" + referencedTable + "_" + foreignKeyColumn + "', 'FK_" + newTableName + "_" + referencedTable + "_" + foreignKeyColumn + "', 'object' \n ";
                    }

                }
            }

            return tableStructureChaneNameScriptFromRelations;
        }

        public string GetTableStructureUniqueConstraints(string tableName)
        {
            string uniqueConstraints = "";
            if (tableUniqueConstraints != null)
            {
                for (int j = 0; j < tableUniqueConstraints.Rows.Count; j++)
                {
                    if (tableUniqueConstraints.Columns.Contains("Columns"))
                    {
                        string uniqeConstraintFields = (string)tableUniqueConstraints.Rows[j]["Columns"];
                        uniqueConstraints += "ALTER TABLE [dbo].[" + tableName + "] ADD CONSTRAINT [UQ_" + tableName + "_" + uniqeConstraintFields.Replace(',', '_') + "] UNIQUE(" + uniqeConstraintFields + ")\n";
                    }

                }
            }

            return uniqueConstraints;
        }




    }
}
