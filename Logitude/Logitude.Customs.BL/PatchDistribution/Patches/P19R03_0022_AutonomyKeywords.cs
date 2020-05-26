using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0022_AutonomyKeywords : PatchDistributionBase
    {
        public P19R03_0022_AutonomyKeywords()
         : base(" DELETE CONSTRAINT uq_1AutonomyKeywords  ", new DateTime(2020, 04, 1))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {
            this.AddUpSqlScript(@"ALTER TABLE CustomsAutonomyKeywords DROP CONSTRAINT uq_1AutonomyKeywords");
        }
    }
}
