
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0025_ReferantData : PatchDistributionBase
    {
        public P19R03_0025_ReferantData()
         : base(" add ReferantData table  ", new DateTime(2020, 05, 03))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {
            this.AddUpSqlScript(@"  CREATE TABLE  DECLARATIONREFERANTDATAS  
                       ( DECLARATIONID  VARCHAR2(15 CHAR) NOT NULL ENABLE,
                         TENANT  NUMBER(10, 0) NOT NULL ENABLE,
                         ORDERNUMBER  VARCHAR2(30 CHAR),
                         VENDORID  VARCHAR2(15 CHAR),
                         ARRIVALDATE  TIMESTAMP(7) NOT NULL ENABLE,
                         ESTIMATEDARRIVALDATE  TIMESTAMP(7) NOT NULL ENABLE,
                         WEIGHT  NUMBER(15, 3),
                         CLASSIFICATIONSTATUS  VARCHAR2(1 CHAR),
                         CONTROLLERSTATUS  VARCHAR2(1 CHAR),
                         COLLECTIONOFMONEYSTATUS  VARCHAR2(1 CHAR),
                         FOLLOWUPDATE  TIMESTAMP(7),
                         WITHPAPER  NUMBER(1, 0) NOT NULL ENABLE,
                         ISCLOSEDFORFOLLOWUP  VARCHAR2(1 CHAR),
                         ISCLASSIFICATIONREMARKS  NUMBER(1, 0) NOT NULL ENABLE,
                         ISCONTROLLERREMARKS  NUMBER(1, 0) NOT NULL ENABLE,
                         PRECLASSIFICATION  VARCHAR2(1 CHAR),
                         SEARCHFIELDS NVARCHAR2(1000),
                         EXCEPTIONREASONSLIST  VARCHAR2(1000 CHAR),
                         PRIMARY KEY(DECLARATIONID) )");

            this.AddUpSqlScript(@" CREATE INDEX  IX_467805736  ON   DECLARATIONREFERANTDATAS  (VENDORID) ");


            this.AddUpSqlScript("ALTER TABLE DECLARATIONREFERANTDATAS ADD CONSTRAINT FK_N228579171 FOREIGN KEY(VENDORID) REFERENCES CUSTOMSVENDORS(ID)");

        }
    }
}
