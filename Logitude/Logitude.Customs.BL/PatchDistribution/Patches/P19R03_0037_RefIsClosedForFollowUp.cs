using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0037_RefIsClosedForFollowUp : PatchDistributionBase
    {
        public P19R03_0037_RefIsClosedForFollowUp()
            : base("update IsClosedForFollowUp", new DateTime(2020, 06, 04))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {
            this.AddUpSqlScript(@"Update DeclarationReferantDatas set IsClosedForFollowUp=0 where IsClosedForFollowUp is NULL ");
            this.AddUpSqlScript(@"ALTER TABLE DeclarationReferantDatas modify IsClosedForFollowUp default 0");
        }
    }
}
