using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    public class P19R03_0011_SealType : PatchDistributionBase
    {
        public P19R03_0011_SealType()
             : base("SealType", new DateTime(2020, 02, 6))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {


            this.AddUpSqlScript(
                @"  CREATE TABLE SEALTYPES 
   (CODE VARCHAR2(3 CHAR), 
    LOCALNAME NVARCHAR2(40),
    ENGLISHNAME NVARCHAR2(40),
    SEARCHFIELDS NVARCHAR2(1000),
    INACTIVE NUMBER(1, 0))");
            this.AddUpSqlScript("CREATE UNIQUE INDEX SYS_C00112757 ON SEALTYPES (CODE) ");
            this.AddUpSqlScript("ALTER TABLE SEALTYPES MODIFY(CODE NOT NULL ENABLE)");
            this.AddUpSqlScript("ALTER TABLE SEALTYPES MODIFY(INACTIVE NOT NULL ENABLE)");
            this.AddUpSqlScript("ALTER TABLE SEALTYPES ADD PRIMARY KEY(CODE)");




        }
    }
}
