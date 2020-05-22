
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0024_DocIndex : PatchDistributionBase
    {
        public P19R03_0024_DocIndex()
         : base(" add searchfields to AutonomyKeywords  ", new DateTime(2020, 04, 19))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {
            this.AddUpSqlScript(@"  CREATE INDEX IX_DOCFILINGS_EXTERNALENTITYN ON DOCUMENTSFILINGS (EXTERNALENTITYNAME)");
            this.AddUpSqlScript(@"  CREATE INDEX DOCFILINGS_EXTENTITYREF ON DOCUMENTSFILINGS (EXTERNALENTITYREFERENCE)");
            
        }
    }
}
