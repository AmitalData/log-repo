using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0018_SealBAndAddOn : PatchDistributionBase
    {
        public P19R03_0018_SealBAndAddOn()
            : base("הוספת SEAL ADDON+BASE PACKAGE", new DateTime(2020, 02, 25))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {

            this.AddUpSqlScript("Insert into PACKAGES (CODE,NAME,SEARCHFIELDS,INACTIVE,FEATUREPACKAGETYPECODE) values ('SEALB','Seal Base','SEALB,Seal Base',0,'BS')");
            this.AddUpSqlScript("Insert into PACKAGES (CODE,NAME,SEARCHFIELDS,INACTIVE,FEATUREPACKAGETYPECODE) values ('SEALA','Seal Addon','SEALA,Seal Addon',0,'AD')");
            this.AddUpSqlScript("Insert into PACKAGECONNECTEDPACKAGES (ID,PACKAGECODE,CONNECTEDPACKAGECODE) values ('G-10','SEALA','SEALB')");

        }
    }
}
