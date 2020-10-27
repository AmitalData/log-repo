using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure
{
    public static class ShipmentMasterTableStrucrue
    {



        public static string CreateTable_Pre_ShipmentMasters(string TableName)
        {
            string cmd = "If not exists (select * from sysobjects where name='" + TableName + "' and xtype='U')" +
                          "BEGIN " +
                          "CREATE TABLE [dbo].[" + TableName + "](" +
                          "[Id] VARCHAR(16) NOT NULL," +
                          "[Tenant] INT NOT NULL," +
                          "[Master] VARCHAR(20) NULL," +
                          "[MainCarriageATD] DATETIME NULL," +
                          "[MainCarriageETD] DATETIME NULL," +
                          "[MainCarriageATA] DATETIME NULL," +
                          "[MainCarriageETA] DATETIME NULL," +
                          "CONSTRAINT[PK_" + TableName + "] PRIMARY KEY([Id])" +
                          ")" +
                          " End ";
            return cmd;
        }

    }
}
