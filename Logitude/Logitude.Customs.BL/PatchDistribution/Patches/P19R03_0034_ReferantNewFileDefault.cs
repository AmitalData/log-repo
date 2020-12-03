using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0034_ReferantNewFileDefault : PatchDistributionBase
    {
        public P19R03_0034_ReferantNewFileDefault()
            : base("edit newfile default", new DateTime(2020, 06, 08))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {
            this.AddUpSqlScript(@"ALTER TABLE DeclarationReferantDatas modify Newfile default 1");
        }
    }
}
