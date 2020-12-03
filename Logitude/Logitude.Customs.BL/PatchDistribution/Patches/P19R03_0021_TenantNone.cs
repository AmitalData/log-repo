using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{

    class P19R03_0021_TenantNone : PatchDistributionBase
    {
        public P19R03_0021_TenantNone()
            : base("TenantNone", new DateTime(2020, 03, 05))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {

            this.AddUpSqlScript(
@"ALTER TABLE TENANTS drop  column CHECKDIGITCONTROLALGORITHMCODE");
            



            this.AddUpSqlScript(
@"ALTER TABLE TENANTS add CHECKDIGITCONTROLALGORITHMCODE NVARCHAR2(128) DEFAULT 'NONE' ");
        }
    }
}
