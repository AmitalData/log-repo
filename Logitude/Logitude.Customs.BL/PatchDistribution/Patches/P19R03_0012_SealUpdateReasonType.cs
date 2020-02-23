using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    public class P19R03_0012_SealUpdateReasonType : PatchDistributionBase
    {
        public P19R03_0012_SealUpdateReasonType()
             : base("SealUpdateReasonType", new DateTime(2020, 02, 6))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {


            this.AddUpSqlScript(
                @"  CREATE TABLE SEALUPDATEREASONTYPES 
   (CODE VARCHAR2(4 CHAR), 
    SEARCHFIELDS NCLOB,
    LOCALNAME NVARCHAR2(300),
    INACTIVE NUMBER(1, 0),
    ENGLISHNAME VARCHAR2(100 CHAR))");
            this.AddUpSqlScript("CREATE UNIQUE INDEX SYS_C00112788 ON SEALUPDATEREASONTYPES (CODE) ");
            this.AddUpSqlScript("ALTER TABLE SEALUPDATEREASONTYPES MODIFY(CODE NOT NULL ENABLE)");
            this.AddUpSqlScript("ALTER TABLE SEALUPDATEREASONTYPES MODIFY(INACTIVE NOT NULL ENABLE)");
            this.AddUpSqlScript("ALTER TABLE SEALUPDATEREASONTYPES ADD PRIMARY KEY(CODE)");




        }
    }
}
