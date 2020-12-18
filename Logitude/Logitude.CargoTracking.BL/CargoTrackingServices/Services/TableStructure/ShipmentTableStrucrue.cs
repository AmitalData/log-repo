using Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure
{
    public static class ShipmentTableStrucrue
    {
        public static TableStructureHelper tableStructureHelper = new TableStructureHelper("C:\\Source\\log-repo\\Logitude\\Logitude.CargoTracking.MetaData\\DBTables\\CargoTrackingShipment.dxml");

        public static string CreateTable_Pre_Shipments(string TableName)
        {
            string cmd = tableStructureHelper.GetTableStructure(TableName);
            //string cmd = "If not exists (select * from sysobjects where name='" + TableName + "' and xtype='U')" +
            //             "BEGIN " +
            //             "CREATE TABLE [dbo].[" + TableName + "](" +
            //             "[Tenant] INT NOT NULL," +
            //             "[EntityId] VARCHAR(15) NULL," +
            //             "[ForwardingShipmentHeaderId] VARCHAR(15) NULL," +
            //             "[CustomsShipmentHeaderId] VARCHAR(15) NULL," +
            //             "[EntityType] VARCHAR(1) NULL," +
            //             "[CurrentMilestoneCode] VARCHAR(2) NULL," +
            //             "[CurrentMilestoneDate] DATETIME NULL," +
            //             "[CustomerId] VARCHAR(15) NULL," +
            //             "[TransportModeId] VARCHAR(15) NULL," +
            //             "[DirectionId] VARCHAR(1) NOT NULL," +
            //             "[Master] VARCHAR(20) NULL," +
            //             "[House] VARCHAR(20) NULL," +
            //             "[ShipmentNumber] VARCHAR(20) NULL," +
            //             "[FromPortId] VARCHAR(15) NULL," +
            //             "[ToPortId] VARCHAR(15) NULL," +
            //             "[ShipperId] VARCHAR(15) NULL," +
            //             "[ConsigneeId] VARCHAR(15) NULL," +
            //             "[GrossWeight] FLOAT NULL," +
            //             "[Volume] FLOAT NULL," +
            //             "[PickupDone] BIT DEFAULT(0) NULL," +
            //             "[PickupDate] DATETIME NULL," +
            //             "[CreateDate] DATETIME NOT NULL," +
            //             "[SecurityKey] VARCHAR(40) NULL," +
            //             "[ConsigneeName] VARCHAR(70) NULL," +
            //             "[CustomerReference] VARCHAR(101) NULL," +
            //             "[IsMainRecord] BIT DEFAULT(0) NOT NULL," +
            //             "[PickupEstimationDate] DATETIME NULL," +
            //             "[FromWarehouseDate] DATETIME NULL," +
            //             "[FromWarehouseEstimationDate] DATETIME NULL," +
            //             "[FromWarehouseNotes] NVARCHAR(500) NULL," +
            //             "[DepartureDone] BIT DEFAULT(0) NULL," +
            //             "[DepartureDate] DATETIME NULL," +
            //             "[DepartureEstimationDate] DATETIME NULL," +
            //             "[ArrivalDone] BIT DEFAULT(0) NULL," +
            //             "[ArrivalDate] DATETIME NULL," +
            //             "[ArrivalEstimationDate] DATETIME NULL," +
            //             "[ToWarehouseDone] BIT DEFAULT(0) NULL," +
            //             "[ToWarehouseDate] DATETIME NULL," +
            //             "[ToWarehouseEstimationDate] DATETIME NULL," +
            //             "[ToWarehouseNotes] NVARCHAR(32) NULL," +
            //             "[CustomsPaymentDone] BIT DEFAULT(0) NULL," +
            //             "[CustomsPaymentDate] DATETIME NULL," +
            //             "[ClearanceDone] BIT DEFAULT(0) NULL," +
            //             "[ClearanceDate] DATETIME NULL," +
            //             "[DeliveredDone] BIT DEFAULT(0) NULL," +
            //             "[DeliveredDate] DATETIME NULL," +
            //             "[DeliveredEstimationDate] DATETIME NULL," +
            //             "[FromWarehouseDone] BIT DEFAULT(0) NULL," +
            //             "[FirstPickupETD] DATETIME NULL," +
            //             "[ShipperName] VARCHAR(70) NULL," +
            //             "[WarehouseLegActualEntryDate] DATETIME NULL," +
            //             "[WarehouseLegExpectedEntryDate] DATETIME NULL," +
            //             "[WarehouseLegRemarks] NVARCHAR(500) NULL," +
            //             "[DeclarationDate] DATETIME NULL," +
            //             "[CustomsClearanceDate] DATETIME NULL," +
            //             "[Id] INT IDENTITY(1,1) NOT NULL," +
            //             "[ContainersNumbers] NVARCHAR(MAX) NULL,"+
            //             "[PackagesQuantity] INT NULL," +
            //             "CONSTRAINT[PK_" + TableName + "] PRIMARY KEY([Id])" +
            //             ")   \n";

            //cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_Tenant_IsMainRecord_EntityId] ON [dbo].[" + TableName + "]([Tenant],[IsMainRecord],[EntityId])\n";
            //cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_EntityId] ON [dbo].[" + TableName + "]([EntityId])\n";
            //cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_Tenant_SecurityKey] ON [dbo].[" + TableName + "]([Tenant],[SecurityKey])\n";
            //cmd += "ALTER INDEX [IX_" + TableName + "_Tenant_IsMainRecord_EntityId] ON [dbo].[" + TableName + "] DISABLE \n";
            //cmd += "ALTER INDEX [IX_" + TableName + "_EntityId] ON [dbo].[" + TableName + "] DISABLE \n";
            //cmd += "ALTER INDEX [IX_" + TableName + "_Tenant_SecurityKey] ON [dbo].[" + TableName + "] DISABLE End \n";
            return cmd;

        }

        public static string CreateIndexAndRelations_Pre_Shipments(string TableName)
        {
            string cmd = "";

            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_Tenant_IsMainRecord_EntityId] ON [dbo].[" + TableName + "]([Tenant],[IsMainRecord],[EntityId])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_EntityId] ON [dbo].[" + TableName + "]([EntityId])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_Tenant_SecurityKey] ON [dbo].[" + TableName + "]([Tenant],[SecurityKey])\n";
            cmd += "ALTER INDEX [IX_" + TableName + "_Tenant_IsMainRecord_EntityId] ON [dbo].[" + TableName + "] DISABLE \n";
            cmd += "ALTER INDEX [IX_" + TableName + "_EntityId] ON [dbo].[" + TableName + "] DISABLE \n";
            cmd += "ALTER INDEX [IX_" + TableName + "_Tenant_SecurityKey] ON [dbo].[" + TableName + "] DISABLE End \n";
            return cmd;
        }

        public static string ReBuildIndexes_Pre_Shipments(string TableName)
        {
            string cmd = tableStructureHelper.GetTableStructureReBuildIndexs(TableName);
            //string cmd = "";
            //cmd += "ALTER INDEX [IX_" + TableName + "_Tenant_IsMainRecord_EntityId] ON [dbo].[" + TableName + "] REBUILD \n";
            //cmd += "ALTER INDEX [IX_" + TableName + "_EntityId] ON [dbo].[" + TableName + "] REBUILD \n";
            //cmd += "ALTER INDEX [IX_" + TableName + "_Tenant_SecurityKey] ON [dbo].[" + TableName + "] REBUILD  \n";

            return cmd;
        }

        public static string CreateConstraientWithRelations_Pre_Shipments(string TableName)
        {
            string cmd = tableStructureHelper.GetTableStructureUniqueConstraints(TableName);
                   cmd += tableStructureHelper.GetTableStructureRelations(TableName);
            //string cmd = "ALTER TABLE [dbo].[" + TableName + "] ADD CONSTRAINT [UQ_" + TableName + "_EntityType_EntityId_Tenant] UNIQUE([EntityType],[EntityId],[Tenant])\n";
            //cmd += "ALTER TABLE [dbo].[" + TableName + "] ADD CONSTRAINT [FK_" + TableName + "_CargoTrackingHeaderEntityTypes_EntityType] FOREIGN KEY([EntityType]) REFERENCES [dbo].[CargoTrackingHeaderEntityTypes]([Code])\n";
            //cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_EntityType] ON [dbo].[" + TableName + "]([EntityType])\n";
            //cmd += "ALTER TABLE [dbo].[" + TableName + "] ADD CONSTRAINT [FK_" + TableName + "_CargoTrackingMilestones_CurrentMilestoneCode] FOREIGN KEY([CurrentMilestoneCode]) REFERENCES [dbo].[CargoTrackingMilestones]([Code])\n";
            //cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_CurrentMilestoneCode] ON [dbo].[" + TableName + "]([CurrentMilestoneCode])\n";

            return cmd;
        }

        public static string ChaneNameScript(string Old, string New)
        {
            string cmd = tableStructureHelper.GetTableStructureChangeNameScript(Old, New);
            //string cmd = "EXEC sp_rename '" + Old + "', '" + New + "' \n ";
            //cmd += " exec sp_rename 'PK_" + Old + "', 'PK_" + New + "', 'object' \n ";
            //if (Old == "CargoTrackingShipments" || New == "CargoTrackingShipments" || New == "Pre_CargoTrackingShipments")
            //{

            //    cmd += " exec sp_rename 'FK_" + Old + "_CargoTrackingHeaderEntityTypes_EntityType', 'FK_" + New + "_CargoTrackingHeaderEntityTypes_EntityType', 'object' \n ";
            //    cmd += " exec sp_rename 'FK_" + Old + "_CargoTrackingMilestones_CurrentMilestoneCode', 'FK_" + New + "_CargoTrackingMilestones_CurrentMilestoneCode', 'object' \n ";
            //    cmd += " exec sp_rename 'UQ_" + Old + "_EntityType_EntityId_Tenant', 'UQ_" + New + "_EntityType_EntityId_Tenant', 'object' \n ";

            //}
            return cmd;
        }

    }
}
