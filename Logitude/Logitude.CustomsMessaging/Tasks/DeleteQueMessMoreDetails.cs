using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Helpers;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Tasks
{
    public class DeleteQueMessMoreDetails : ICustomsDeleteQueMessMoreDetails
    {

        public void StartRun(string taskId, int seedDefaultTenant)
        {

            var customsSettingQueryService = new CustomsSettingQueryService(seedDefaultTenant);
            var allCustomsSetting = customsSettingQueryService.GetAll();
            allCustomsSetting.ForEach(t => RunPerTenant(t, taskId));


        }

        private void RunPerTenant(CustomsSettingPM t, string taskId)
        {
            LogMessagingUtil.Instance.AppendLine(value: $"taskId({taskId})");
            LogMessagingUtil.Instance.AppendLine(value: $"RunPerTenant({t.Tenant})");
            SchedulerParamQueryService schedulerParamQueryService = new SchedulerParamQueryService(t.Tenant);
            var SchedularParams = schedulerParamQueryService.GetAllByProcedureCode(t.Tenant, taskId);
            int days = 90;
            foreach (var schedularParam in SchedularParams)
            {
                switch (schedularParam.Parameter)
                {
                    case "Days":
                        int.TryParse(schedularParam.ParameterValue, out days);
                        break;
                    default:
                        break;
                }
            }
            LogMessagingUtil.Instance.AppendLine(value: $"Days({days.ToString()})");
           
            CustomsStoredProcedures.DeleteQueueMessageMoreDetails(t.Tenant, days);
        }
    }
}