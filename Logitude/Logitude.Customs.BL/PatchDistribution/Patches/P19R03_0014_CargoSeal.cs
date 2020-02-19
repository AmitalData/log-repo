using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    public class P19R03_0014_CargoSeal : PatchDistributionBase
    {
        public P19R03_0014_CargoSeal()
             : base("הצהרת  ", new DateTime(2020, 02, 6))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {


            this.AddUpSqlScript(
                @"  CREATE TABLE CARGOSEALS 
   (CARGOSEALIDENTIFIERID VARCHAR2(15 CHAR), 
    SEALNUMBER VARCHAR2(35 CHAR),
    TENANT NUMBER(10, 0),
    REMARKS NVARCHAR2(512),
    SEALCOMPLETENESSSTATECODE VARCHAR2(2 CHAR),
    SEALTYPECODE VARCHAR2(3 CHAR),
    UPDATEREASONCODE VARCHAR2(3 CHAR),
    UPDATETYPECODE VARCHAR2(2 CHAR)
   )");
            this.AddUpSqlScript("CREATE UNIQUE INDEX SYS_C00114903 ON CARGOSEALS (CARGOSEALIDENTIFIERID, SEALNUMBER)");
            this.AddUpSqlScript("CREATE INDEX IX_181547934 ON CARGOSEALS (CARGOSEALIDENTIFIERID)");
            this.AddUpSqlScript("CREATE INDEX IX_N1129193701 ON CARGOSEALS (SEALCOMPLETENESSSTATECODE)");
            this.AddUpSqlScript("CREATE INDEX IX_CARGOSEALS_SEALTYPECODE ON CARGOSEALS (SEALTYPECODE)");
            this.AddUpSqlScript("CREATE INDEX IX_CARGOSEALS_UPDATEREASONCODE ON CARGOSEALS (UPDATEREASONCODE)");
            this.AddUpSqlScript("CREATE INDEX IX_CARGOSEALS_UPDATETYPECODE ON CARGOSEALS (UPDATETYPECODE)");

            this.AddUpSqlScript("ALTER TABLE CARGOSEALS MODIFY(CARGOSEALIDENTIFIERID NOT NULL ENABLE)");
            this.AddUpSqlScript("ALTER TABLE CARGOSEALS MODIFY(SEALNUMBER NOT NULL ENABLE)");
            this.AddUpSqlScript("ALTER TABLE CARGOSEALS MODIFY(TENANT NOT NULL ENABLE)");
            this.AddUpSqlScript("ALTER TABLE CARGOSEALS ADD PRIMARY KEY(CARGOSEALIDENTIFIERID, SEALNUMBER)");
            //this.AddUpSqlScript("ALTER TABLE CARGOSEALS ADD PRIMARY KEY(SEALNUMBER)");




        }
    }
}
