using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{

    
    class P19R03_0017_CargoSealID : PatchDistributionBase
    {
        public P19R03_0017_CargoSealID()
             : base("הוספת שדה ID לcargo seal", new DateTime(2020, 02, 25))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {

            this.AddUpSqlScript("ALTER TABLE CargoSeals DROP PRIMARY KEY");
            this.AddUpSqlScript("ALTER TABLE CargoSeals ADD Id VARCHAR2(15 CHAR) DEFAULT '' NOT NULL");
            this.AddUpSqlScript("ALTER TABLE CargoSeals ADD CONSTRAINT PK_CargoSeals PRIMARY KEY(Id)");
 
        }
    }
}
