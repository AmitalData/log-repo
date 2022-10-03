using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
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
    public class ExchangeRatesQuery : ICustomsExchangeRatesQuery
    {
        public void StartRun(string taskId, int seedDefaultTenant)
        {

            var customsSettingQueryService = new CustomsSettingQueryService(seedDefaultTenant);
            var allCustomsSetting = customsSettingQueryService.GetAll();
            allCustomsSetting.ForEach(t => RunPerTenant(t));


        }

        private void RunPerTenant(CustomsSettingPM t)
        {

            LogMessagingUtil.Instance.AppendLine($"RunPerTenant({t.Tenant}) send 8347");
            CD_NG_8347_Web01_CurrencyRateSearchRequestParams requestParams = new CD_NG_8347_Web01_CurrencyRateSearchRequestParams();
            try
            {

                var loggedUserId = AuthenticationUtil.ResolveUserId(t.Tenant);
                requestParams.LoggingEnabled = true;
                requestParams.LoggingUserId = loggedUserId;
                requestParams.Tenant = t.Tenant;
                requestParams.FromDate = DateTime.Now;
                requestParams.ToDate = DateTime.Now;
                requestParams.CurrencyTypeId = null;
                requestParams.RequestVIA = SendRequestVIA.WebServiceBatch;
                requestParams.ForcePersonalSign = false;
                // use messageing service

                var service = new CD_NG_8347_Web01_CurrencyRateSearchParamMessagingService();
                var responseData = service.Send(requestParams);
                LogMessagingUtil.Instance.AppendLine($"Tenant:({t.Tenant}) after send 8347");


            }

            catch (Exception ex)
            {
                LogMessagingUtil.Instance.AppendLine("Exception was thrown while sent 8347 " + t.Tenant + Environment.NewLine + ex.Message);
            }

        }

    }
}
