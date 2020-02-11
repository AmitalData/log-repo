
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0008_Diamonds : PatchDistributionBase
    {
        public P19R03_0008_Diamonds()
             : base("הצהרת יהלומים ", new DateTime(2020, 02, 5))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {


            this.AddUpSqlScript("ALTER TABLE CustomDocumentTypes ADD (IsDiamondManadatory NUMBER(1,0)) ");
            this.AddUpSqlScript("ALTER TABLE Declarations ADD (IsDiamondDeclaration NUMBER(1,0) ) ");
            this.AddUpSqlScript("ALTER TABLE Declarations ADD (IsValidTicketsDiamond NUMBER(1,0)) ");
            this.AddUpSqlScript("ALTER TABLE Declarations ADD (IsMissMandatoryDiamond NUMBER(1,0))  ");



        }
    }
}
