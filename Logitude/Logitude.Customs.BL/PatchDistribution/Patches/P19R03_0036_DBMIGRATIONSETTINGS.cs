#if true
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0036_DBMIGRATIONSETTINGS : PatchDistributionBase
    {
        public P19R03_0036_DBMIGRATIONSETTINGS()
            : base("DBMIGRATIONSETTINGS", new DateTime(2020, 05, 07))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {
            /*
CREATE TABLE "AMINETCST_MAIN"."DBMIGRATIONSETTINGS"
(
"MODE" VARCHAR2(10 CHAR) NOT NULL,
"MODULESLIST" VARCHAR2(1000 CHAR) NOT NULL
);
/


INSERT INTO "AMINETCST_MAIN"."DBMIGRATIONSETTINGS"("MODE", "MODULESLIST") VALUES('Include', 'Customs,Common,Infrastructure');
/

 *              */

            this.AddUpSqlScript(@"CREATE TABLE DBMIGRATIONSETTINGS ( ""MODE"" VARCHAR2(10 CHAR) NOT NULL,MODULESLIST VARCHAR2(1000 CHAR) NOT NULL)");
            this.AddUpSqlScript(@"INSERT INTO  DBMIGRATIONSETTINGS ( ""MODE"" , MODULESLIST) VALUES('Include', 'Customs,Common,Infrastructure')");
        }
    }
}


#endif