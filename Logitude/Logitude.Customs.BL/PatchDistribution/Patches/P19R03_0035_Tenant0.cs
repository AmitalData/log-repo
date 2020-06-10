
#if true
		 /**/ 
	
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0035_Tenant0 : PatchDistributionBase
    {
        public P19R03_0035_Tenant0()
            : base("Create Package TENANT0 ", new DateTime(2020, 06, 03))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {

            /*

Insert into PACKAGES (CODE,NAME,SEARCHFIELDS,INACTIVE,FEATUREPACKAGETYPECODE) values ('TNT0B','Tenant 0','TNT0B,Tenant 0',0,'BS');
Insert into PACKAGES (CODE,NAME,SEARCHFIELDS,INACTIVE,FEATUREPACKAGETYPECODE) values ('COU','Courier','COU,Courier',0,'BS');
Insert into PACKAGES (CODE,NAME,SEARCHFIELDS,INACTIVE,FEATUREPACKAGETYPECODE) values ('coup','Courier Package','coup,Courier Package',0,'PK');

Insert into PACKAGECONNECTEDPACKAGES (ID,PACKAGECODE,CONNECTEDPACKAGECODE) values ('G-21','coup','COU');

Insert into PACKAGES (CODE,NAME,SEARCHFIELDS,INACTIVE,FEATUREPACKAGETYPECODE) values ('TNT0p','Tenant 0 Package','TNT0p,Tenant 0 Package',0,'PK');

Insert into PACKAGECONNECTEDPACKAGES (ID,PACKAGECODE,CONNECTEDPACKAGECODE) values ('G-22','TNT0p','TNT0B');


LOAD CSV !!
UPDATE "AMINETNXT_GLOBAL"."TENANTMANAGEMENTS" SET PACKAGECODE = 'CUST' WHERE TENANTMANAGEMENTS.ID =0
UPDATE "AMINETNXT_GLOBAL"."TENANTMANAGEMENTS" SET PACKAGECODE = 'TNT0p' WHERE TENANTMANAGEMENTS.ID =0

            */

            this.AddUpSqlScript(@"Insert into PACKAGES (CODE,NAME,SEARCHFIELDS,INACTIVE,FEATUREPACKAGETYPECODE) values ('TNT0B','Tenant 0','TNT0B,Tenant 0',0,'BS')");

            this.AddUpSqlScript(@"Insert into PACKAGES (CODE,NAME,SEARCHFIELDS,INACTIVE,FEATUREPACKAGETYPECODE) values ('TNT0p','Tenant 0 Package','TNT0p,Tenant 0 Package',0,'PK')");
            this.AddUpSqlScript(@"Insert into PACKAGECONNECTEDPACKAGES (ID,PACKAGECODE,CONNECTEDPACKAGECODE) values ('G-22','TNT0p','TNT0B')");

        }
    }
}


#endif