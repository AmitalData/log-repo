
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0027_ReferantException : PatchDistributionBase
    {
        public P19R03_0027_ReferantException()
         : base(" add ReferantData table  ", new DateTime(2020, 05, 03))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {
            this.AddUpSqlScript(@" CREATE TABLE REFERANTEXCEPTIONS  
                                   ( DECLARATIONID  VARCHAR2(15 CHAR) NOT NULL ENABLE,
                                   EXCEPTIONREASONSCODE  VARCHAR2(4 CHAR) DEFAULT NULL NOT NULL ENABLE,
                                   TENANT NUMBER(10, 0) NOT NULL ENABLE,
                                   EXCEPTIONREMARKS  NVARCHAR2(1024),
                                   STATUS NVARCHAR2(1),
                                   PRIMARY KEY (DECLARATIONID, EXCEPTIONREASONSCODE))");

            this.AddUpSqlScript(@" CREATE INDEX  IX_N651828831  ON REFERANTEXCEPTIONS (EXCEPTIONREASONSCODE) ");


            this.AddUpSqlScript("ALTER TABLE REFERANTEXCEPTIONS ADD CONSTRAINT FK_526683567 FOREIGN KEY (EXCEPTIONREASONSCODE) REFERENCES EXCEPTIONREASONS(CODE)");

        }
    }
}
