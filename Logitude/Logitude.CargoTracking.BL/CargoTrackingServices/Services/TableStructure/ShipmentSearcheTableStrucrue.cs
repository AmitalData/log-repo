using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure
{
    public static class ShipmentSearcheTableStrucrue
    {
        public static string CreateTable_Pre_ShipmentSearchs(string TableName)
        {

            string cmd = "If not exists (select * from sysobjects where name='" + TableName + "' and xtype='U')" +
                            "BEGIN " +
                            "CREATE TABLE [dbo].[" + TableName + "](" +
                            "[Tenant] INT NOT NULL," +
                            "[SearchFields] NVARCHAR(100) NULL," +
                            "[ShipmentDate] DATETIME NOT NULL," +
                            "[Id] INT IDENTITY(1,1) NOT NULL," +
                            "[ShipmentId] VARCHAR(15) NULL," +
                            "[IsPublic] BIT DEFAULT(0) NULL," +
                            "CONSTRAINT[PK_" + TableName + "] PRIMARY KEY([Id])" +
                            ") ";
            return cmd;

        }
        public static string CreateIndex_Pre_ShipmentSearchs(string TableName)
        {
            string cmd = "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_Tenant_SearchFields_IsPublic] ON [dbo].[" + TableName + "]([Tenant],[SearchFields],[IsPublic])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_ShipmentId] ON [dbo].[" + TableName + "]([ShipmentId])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_ShipmentDate] ON [dbo].[" + TableName + "]([ShipmentDate]) \n";
            cmd += "ALTER INDEX [IX_" + TableName + "_Tenant_SearchFields_IsPublic] ON [dbo].[" + TableName + "] DISABLE \n";
            cmd += "ALTER INDEX [IX_" + TableName + "_ShipmentId] ON [dbo].[" + TableName + "] DISABLE \n";
            cmd += "ALTER INDEX [IX_" + TableName + "_ShipmentDate] ON [dbo].[" + TableName + "] DISABLE End \n";
            return cmd;
        }



        public static string ReBuildIndexes_Pre_ShipmentSearchs(string TableName)
        {
            string cmd = "";
            cmd += "ALTER INDEX [IX_" + TableName + "_Tenant_SearchFields_IsPublic] ON [dbo].[" + TableName + "] REBUILD \n";
            cmd += "ALTER INDEX [IX_" + TableName + "_ShipmentId] ON [dbo].[" + TableName + "] REBUILD \n";
            cmd += "ALTER INDEX [IX_" + TableName + "_ShipmentDate] ON [dbo].[" + TableName + "] REBUILD  \n";

            return cmd;
        }



    }
}
