using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0030_ReferantAddOn : PatchDistributionBase
    {
        public P19R03_0030_ReferantAddOn()
            : base("הוספת REFERANT ADDON+BASE PACKAGE", new DateTime(2020, 05, 07))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {

            this.AddUpSqlScript("Insert into PACKAGES (CODE,NAME,SEARCHFIELDS,INACTIVE,FEATUREPACKAGETYPECODE) values ('DERE','Referant Data','DERE,Referant Data',0,'BS')");
            this.AddUpSqlScript("Insert into PACKAGES (CODE,NAME,SEARCHFIELDS,INACTIVE,FEATUREPACKAGETYPECODE) values ('REDEA','Referant Data','REDEA,Referant Data',0,'AD')");
            this.AddUpSqlScript("Insert into PACKAGECONNECTEDPACKAGES (ID,PACKAGECODE,CONNECTEDPACKAGECODE) values ('G-20','REDEA','DERE')");

        }
    }
}
