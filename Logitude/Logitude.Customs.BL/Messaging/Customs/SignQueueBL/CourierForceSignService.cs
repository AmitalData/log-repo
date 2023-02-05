using Logitude.Customs.BL.EntityQueryServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.ExternalServices;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Customs.SignQueueBL
{
    public class CourierForceSignService
    {
        public void ApplyForceSign<TRequestParams>(ref TRequestParams requestParams) where TRequestParams : RequestParamsBase
        {

            bool courierForceSign = Server.Tools.Helpers.FeatureToggleHelper.HasFeatureToggle("CFS", requestParams.Tenant);//'Courier Force Sign
            if (!courierForceSign)
            {
                return;
            }
            if (CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant).CompanyType != "B"//Courier
                                                                                                      )
            {
                return;
            }                    
            var listIn= new List<string>() { 
                "2750","2715","2755", "1170"  ,
                "UCB1170","UCB2750","UCB2755" };

            if (!listIn.Contains(requestParams.InterfaceTypeCode))
            {
                return;
            }

            var hSMSignStationCheckService = new HSMSignStationCheckService();

            bool isPersonalSign = false;
            bool isMulti_CheckOnly = false;

            switch (requestParams.InterfaceTypeCode)
            {
                case "UCB1170":
                case "UCB2750":
                case "UCB2755":
                    
                    isMulti_CheckOnly = true;
                    isPersonalSign = false;
                    break;

                case "2755":
                case "2715":
                    isPersonalSign = false;
                    break;

                case "1170":
                case "2750":
                    isPersonalSign = true;
                     
                    break;
                default:
                    break;
            }
            var sw= Stopwatch.StartNew();
            requestParams.ForcePersonalSign = isPersonalSign;//DEFAULT HSM
            var res = hSMSignStationCheckService.CheckIfHSMIsValid(requestParams.Tenant, isPersonalSign);
            LogMessagingUtil.Instance.AppendLine($"ApplyForceSign:Success={res.Success};{res.ErrorMessage};took={sw.Elapsed}");
            if (!res.Success)
            {
                throw new CourierForceSignException(res.ErrorMessage);
            }
            if (isMulti_CheckOnly)
            {
                return;
            }
            requestParams.RequestVIAChangeDue = $"Courier- Sign {requestParams.InterfaceTypeCode} ";
            requestParams.SignMethodByQueue = SignMethodByQueueEnum.HSMSignQueue.ToString();
            requestParams.SignByPersonalId = res.MySignStationList.PersonId;
            requestParams.SignQueueByCompanyOrPersonal = (isPersonalSign ? SignQueueByType.SignQueueByPersonId : SignQueueByType.SignQueueByCustomsAgentId).ToString();
        }
    }

    public class CourierForceSignException : Exception
    {
        public CourierForceSignException(string message) : base(message)
        {
        }
    }
}
