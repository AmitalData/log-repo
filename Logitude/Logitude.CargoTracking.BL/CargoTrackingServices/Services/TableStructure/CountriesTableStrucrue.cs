using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure
{
    public static class CountriesTableStrucrue
    {


        public static string CreateTable_Pre_Countries(string TableName)
        {
            string cmd = "If not exists (select * from sysobjects where name='" + TableName + "' and xtype='U')" +
                            "BEGIN " +
                            "CREATE TABLE[dbo].[" + TableName + "](" +
                            "[Id] VARCHAR(15) NOT NULL," +
                            "[Tenant] INT NOT NULL," +
                            "[LocalName] NVARCHAR(120) NULL," +
                            "[Code] CHAR(2) NOT NULL," +
                            "[EnglishName] VARCHAR(120) NOT NULL," +
                            "CONSTRAINT[PK_" + TableName + "] PRIMARY KEY([Id])" +
                            ")" +
                            " End ";

            return cmd;

        }

    }
}
