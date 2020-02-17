
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0009_AutomaticPayment : PatchDistributionBase
    {
        public P19R03_0009_AutomaticPayment()
             : base("תשלום אוטמטי ", new DateTime(2020, 02, 17))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {


             this.AddUpSqlScript("ALTER TABLE Declarations ADD (AvailabilityDate DATETIME) ");
            this.AddUpSqlScript("ALTER TABLE DECLARATIONPAYMENTS ADD (AutomaticPayment NUMBER(1,0)) ");




        }
    }
}
