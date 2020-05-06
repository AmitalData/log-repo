
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0028_AvailabilityDate : PatchDistributionBase
    {
        public P19R03_0028_AvailabilityDate()
         : base(" enable null AvailabilityDate  ", new DateTime(2020, 05, 03))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {
            this.AddUpSqlScript(@"ALTER TABLE Declarations DROP COLUMN AvailabilityDate");
            this.AddUpSqlScript(@"ALTER TABLE Declarations ADD  AvailabilityDate TIMESTAMP(7) NULL");

        }
    }
}
