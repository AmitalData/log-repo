using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Tasks
{
    public class CloseCourierMasterService: ICustomsCloseCourierMasterService
    {
        public void StartRun(string taskId, int seedDefaultTenant)
        {

            //seedDefaultTenant==0
            var customsSettingQueryService = new CustomsSettingQueryService(seedDefaultTenant);
            var allCustomsSetting = customsSettingQueryService.GetAll();
            allCustomsSetting.ForEach(t => RunPerTenant(t));
            for (int i = 0; i <= 3; i++)
            {
                LogMessagingUtil.Instance.AppendLine("Log warning # " + i + " , Be careful !!");

            }


        }

        private void RunPerTenant(CustomsSettingPM t)
        {
            LogMessagingUtil.Instance.AppendLine($"RunPerTenant({t.Tenant})");
        }
    }
    
}
