using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    public class P19R03_0010_SealCompletenes : PatchDistributionBase
    {
        public P19R03_0010_SealCompletenes()
             : base("SealCompletenes", new DateTime(2020, 02, 6))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {


            this.AddUpSqlScript(
                @"  CREATE TABLE SEALCOMPLETENESS 
   (CODE VARCHAR2(2 CHAR), 
    LOCALNAME NVARCHAR2(40),
    ENGLISHNAME VARCHAR2(40 CHAR),
    SEARCHFIELDS NVARCHAR2(1000),
    INACTIVE NUMBER(1, 0))");
            this.AddUpSqlScript("CREATE UNIQUE INDEX SYS_C00111919 ON SEALCOMPLETENESS (CODE) ");
            this.AddUpSqlScript("ALTER TABLE SEALCOMPLETENESS MODIFY(CODE NOT NULL ENABLE)");
            this.AddUpSqlScript("ALTER TABLE SEALCOMPLETENESS MODIFY(INACTIVE NOT NULL ENABLE)");
            this.AddUpSqlScript("ALTER TABLE SEALCOMPLETENESS ADD PRIMARY KEY(CODE)");


        }
    }
}
