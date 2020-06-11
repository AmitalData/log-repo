using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0032_AddReferantException901 : PatchDistributionBase
    {
        public P19R03_0032_AddReferantException901()
            : base("Add Referant Exception 901 ", new DateTime(2020, 06, 03))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {

            this.AddUpSqlScript(@"INSERT INTO EXCEPTIONREASONS VALUES (
'901',1,'','נדרש תצהיר יבואן',1,null, 'נדרש תצהיר יבואן,901'
)");


        }
    }
}
