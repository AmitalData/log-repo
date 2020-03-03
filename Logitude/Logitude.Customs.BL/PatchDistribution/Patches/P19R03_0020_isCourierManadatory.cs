
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{

    class P19R03_0020_isCourierManadatory : PatchDistributionBase
    {
        public P19R03_0020_isCourierManadatory()
            : base("isCourierManadatory", new DateTime(2020, 03, 03))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {

            this.AddUpSqlScript(
@"ALTER TABLE CustomDocumentTypes
ADD IsCourierManadatory NUMBER(1) DEFAULT 0 NOT NULL");


            this.AddUpSqlScript(
@"update CUSTOMDOCUMENTTYPES
set ISCOURIERMANADATORY='1'
where CODE='380'");
        }
    }
}
