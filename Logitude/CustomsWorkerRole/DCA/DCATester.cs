using Logitude.Customs.BL.EntityQueryServices;
using Logitude.CustomsMessaging.Dca;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsWorkerRole.DCA
{
    public class DCATester
    {
        public static void DownloadFile(int tenant, string dcaFile, 
            //byte[] bytsDcaFile
            string base64StringInnerUTF8
            )
        {
            var customsSettingQueryService = new CustomsSettingQueryService(tenant);
            var setting = customsSettingQueryService.GetAll().First( rec=> rec.Tenant==tenant );

            var myDcaService = new DcaDownloadTenantService(setting);
            myDcaService.TestDownloadFile(dcaFile, 
                //bytsDcaFile
                base64StringInnerUTF8
                );
        }
    }
}
