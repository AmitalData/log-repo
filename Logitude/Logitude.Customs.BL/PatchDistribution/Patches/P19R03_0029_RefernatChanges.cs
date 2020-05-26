
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0029_RefernatChanges : PatchDistributionBase
    {
        public P19R03_0029_RefernatChanges()
         : base(" enable null AvailabilityDate  ", new DateTime(2020, 05, 06))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {
            this.AddUpSqlScript(@"ALTER TABLE DECLARATIONREFERANTDATAS MODIFY ARRIVALDATE NULL");
            this.AddUpSqlScript(@"ALTER TABLE  DECLARATIONREFERANTDATAS MODIFY ESTIMATEDARRIVALDATE NULL");

        }
    }
}
