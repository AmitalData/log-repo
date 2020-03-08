
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    
    class P19R03_0019_SealB2Pioneers : PatchDistributionBase
    {
        public P19R03_0019_SealB2Pioneers()
            : base("הוספת SealB2Pioneers", new DateTime(2020, 03, 03))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {
            //Insert into PACKAGECONNECTEDPACKAGES (ID,PACKAGECODE,CONNECTEDPACKAGECODE) values ('G-12','SEALA','SEALB');
            this.AddUpSqlScript(//---values ('SEALB','Seal Base','SEALB,Seal Base',0,'BS')
@"INSERT into PACKAGECONNECTEDPACKAGES (ID,PACKAGECODE,CONNECTEDPACKAGECODE) 
Select 'G-11','PION','SEALB'
from dual
where 
exists( select * from PACKAGEs WHERE code IN('PION'))AND 
exists( select * from PACKAGEs WHERE code IN('SEALB'))
");
        }
    }
}
