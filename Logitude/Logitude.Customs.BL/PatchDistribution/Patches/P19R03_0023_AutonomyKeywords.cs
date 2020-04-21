using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0023_AutonomyKeywords : PatchDistributionBase
    {
        public P19R03_0023_AutonomyKeywords()
         : base(" add searchfields to AutonomyKeywords  ", new DateTime(2020, 04, 19))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {
            this.AddUpSqlScript(@"ALTER TABLE CustomsAutonomyKeywords ADD  SearchFields NVARCHAR2(1000) NULL");
        }
    }
}
