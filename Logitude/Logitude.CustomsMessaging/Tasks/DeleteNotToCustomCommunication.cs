using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Helpers;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.CustomsMessaging.Tasks
{
    public class DeleteNotToCustomCommunication : ICustomsDeleteNotToCustomCommunication
    {
        public void StartRun(string taskId, int seedDefaultTenant)
        {

            var customsSettingQueryService = new CustomsSettingQueryService(seedDefaultTenant);
            var allCustomsSetting = customsSettingQueryService.GetAll();
            allCustomsSetting.ForEach(t => RunPerTenant(t, taskId));


        }

        private void RunPerTenant(CustomsSettingPM t, string taskId)
        {
            LogMessagingUtil.Instance.AppendLine(value: $"RunPerTenant({t.Tenant})");
            SchedulerParamQueryService schedulerParamQueryService = new SchedulerParamQueryService(t.Tenant);
            var SchedularParams = schedulerParamQueryService.GetAllByProcedureCode(t.Tenant, taskId);
            string From=null, To=null, Subject=null, CommunicationTypeStatus = null;
            int days = 90;
            foreach(var schedularParam in SchedularParams)
            {
                switch (schedularParam.Parameter)
                {
                    case "Days":
                        int.TryParse(schedularParam.ParameterValue, out days);
                        break;
                    case "From":
                        From = schedularParam.ParameterValue;
                        break;
                    case "To":
                        To = schedularParam.ParameterValue;
                        break;
                    case "Subject":
                        Subject = schedularParam.ParameterValue;
                        break;
                    case "CommunicationTypeStatus":
                        CommunicationTypeStatus = schedularParam.ParameterValue;
                        break;
                    default:
                        break;
                }
            }
            LogMessagingUtil.Instance.AppendLine(value: $"Days({days.ToString()})");
            LogMessagingUtil.Instance.AppendLine(value: $"From({From.ToString()})");
            LogMessagingUtil.Instance.AppendLine(value: $"To({To.ToString()})");
            LogMessagingUtil.Instance.AppendLine(value: $"Subject({Subject.ToString()})");
            LogMessagingUtil.Instance.AppendLine(value: $"CommunicationTypeStatus({CommunicationTypeStatus.ToString()})");

            CustomsStoredProcedures.DeleteCommunicationLogs(t.Tenant, days, From, To, Subject, CommunicationTypeStatus);
        }
    }
}
