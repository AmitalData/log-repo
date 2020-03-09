using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    public class P19R03_0013_CargoSealIdentifier : PatchDistributionBase
    {
        public P19R03_0013_CargoSealIdentifier()
             : base("CargoSealIdentifier", new DateTime(2020, 02, 6))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {


            this.AddUpSqlScript(
                @"  CREATE TABLE CARGOSEALIDENTIFIERS 
  (	ID VARCHAR2(15 CHAR), 
    TENANT NUMBER(10, 0),
    DECLARATIONID VARCHAR2(15 CHAR),
    CARGOROWNUMBER VARCHAR2(9 CHAR),
    CONTAINERNUMBER VARCHAR2(11 CHAR),
    UPDATEDATE TIMESTAMP(7),
    IMPORTERID VARCHAR2(15 CHAR),
    CARGOIDENTIFIERTYPECODE VARCHAR2(4 CHAR),
    CARGOIDENTIFIERKEY1 VARCHAR2(35 CHAR),
    CARGOIDENTIFIERKEY2 VARCHAR2(35 CHAR),
    CARGOIDENTIFIERKEY3 VARCHAR2(35 CHAR),
    STATUS VARCHAR2(1 CHAR)
   )");
            this.AddUpSqlScript("CREATE UNIQUE INDEX SYS_C00113589 ON CARGOSEALIDENTIFIERS (ID) ");
            this.AddUpSqlScript("CREATE  INDEX IX_200093549 ON CARGOSEALIDENTIFIERS (DECLARATIONID) ");
            this.AddUpSqlScript("CREATE  INDEX IX_N1157464634 ON CARGOSEALIDENTIFIERS (IMPORTERID) ");
            this.AddUpSqlScript("CREATE  INDEX IX_1890377645 ON CARGOSEALIDENTIFIERS (CARGOIDENTIFIERTYPECODE) ");
            this.AddUpSqlScript("ALTER TABLE CARGOSEALIDENTIFIERS MODIFY(ID NOT NULL ENABLE)");
            this.AddUpSqlScript("ALTER TABLE CARGOSEALIDENTIFIERS MODIFY(TENANT NOT NULL ENABLE)");
            this.AddUpSqlScript("ALTER TABLE CARGOSEALIDENTIFIERS ADD PRIMARY KEY(ID)");




        }
    }
}
