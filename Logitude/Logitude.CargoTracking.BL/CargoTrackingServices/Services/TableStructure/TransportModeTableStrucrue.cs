using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure
{
    public static class TransportModeTableStrucrue
    {

        public static string CreateTable_Pre_TransportModes(string TableName)
        {
            string cmd = "If not exists (select * from sysobjects where name='" + TableName + "' and xtype='U')" +
                            "BEGIN " +
                            "CREATE TABLE[dbo].[" + TableName + "](" +
                            "[Id] CHAR(1) NOT NULL," +
                            "[SearchFields] NVARCHAR(1000) NULL," +
                            "[Name] VARCHAR(10) NOT NULL," +
                            "CONSTRAINT[PK_" + TableName + "] PRIMARY KEY([Id])" +
                            ")" +
                            " End ";

            return cmd;

        }


    }
}
