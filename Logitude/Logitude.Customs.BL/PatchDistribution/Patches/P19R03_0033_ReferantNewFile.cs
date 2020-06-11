using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0033_ReferantNewFile : PatchDistributionBase
    {
        public P19R03_0033_ReferantNewFile()
            : base("add newfile/favorite", new DateTime(2020, 06, 03))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {

            this.AddUpSqlScript(@"ALTER TABLE DeclarationReferantDatas ADD Newfile NUMBER(1, 0) DEFAULT 0 NOT NULL");
            this.AddUpSqlScript(@"ALTER TABLE DeclarationReferantDatas ADD FAVORITE NUMBER(1, 0) DEFAULT 0 NOT NULL");

        }
    }
}
