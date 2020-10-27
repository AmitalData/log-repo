using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure
{
    public static class PortTableStrucrue
    {

        public static string CreateTable_Pre_Ports(string TableName)
        {
            string cmd = "If not exists (select * from sysobjects where name='" + TableName + "' and xtype='U')" +
                            "BEGIN " +
                            "CREATE TABLE [dbo].[" + TableName + "](" +
                            "[Id] VARCHAR(15) NOT NULL," +
                            "[Tenant] INT NOT NULL," +
                            "[Code] VARCHAR(3) NOT NULL," +
                            "[EnglishName] VARCHAR(40) NULL," +
                            "[CountryId] VARCHAR(15) NOT NULL," +
                            "CONSTRAINT[PK_" + TableName + "] PRIMARY KEY([Id])" +
                            ")" +
                            " End ";

            return cmd;

        }

    }
}
