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
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance
.AppendLine("ApplyForceSign");
            var listIn = new List<string>() {
                "2750",//"2715","2755", "1170"  ,"2340",
                "UCB1170"
                //,"UCB2715"
                ,"UCB2755", "UCB2750" };

            var signInterfaces = GetSignInterfaces(requestParams);

            bool courierForceSign = Server.Tools.Helpers.FeatureToggleHelper.HasFeatureToggle("CFS", requestParams.Tenant);//'Courier Force Sign
            if (!courierForceSign 
                ||
                CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant).CompanyType != "B"//Courier
                ||
                !signInterfaces.Select(r => r.Code).Union(listIn).Contains(requestParams.InterfaceTypeCode)
                )
            {
                return;
            }

        

            bool isPersonalSign = false;
            bool isMulti_CheckOnly = false;
            if (signInterfaces.Select(r => r.Code).ToList().Contains(requestParams.InterfaceTypeCode))
            {
                isPersonalSign = false;
            }
            else
            {
                isPersonalSign = IsPersonalSign(requestParams, ref isMulti_CheckOnly);
            }


            var sw = Stopwatch.StartNew();

            var signQueueHSMService = new SignQueueHSMService();
            if (!signQueueHSMService.IsHSMSign_IsOn(requestParams.Tenant, requestParams.HsmStationContext))
            {
                Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance
.AppendLine("!signQueueHSMService.IsHSMSign_IsOn(requestParams.Tenant)");
                string personId = SignQueue.Instance.GetUserPersonID(requestParams.LoggingUserId, requestParams.Tenant);

                string availableSignServer = null;
                if (isPersonalSign)
                {
                    availableSignServer = SignQueue.Instance.GetAvailableSignServer(requestParams.Tenant, SignQueueByType.SignQueueByPersonId, personId);

                }


                if (isPersonalSign && string.IsNullOrWhiteSpace(personId) && string.IsNullOrWhiteSpace(availableSignServer))
                {
                    if (Environment.CommandLine.ToLower().Contains("AmitalCustomsWindowsService.exe".ToLower()))
                    {
                        requestParams.ForcePersonalSign = isPersonalSign;//DEFAULT HSM
                        return;
                    }   
                    throw new CourierForceSignException("No Person ID is set for the user");
                }
                var dBSignStationCheckService = new DBSignStationCheckService();
                var signStation = dBSignStationCheckService.GetValidSignStation(requestParams.Tenant, personId, isPersonalSign);
                if (signStation == null && string.IsNullOrWhiteSpace(availableSignServer))
                {
                    if (Environment.CommandLine.ToLower().Contains("AmitalCustomsWindowsService.exe".ToLower()))
                    {
                        requestParams.ForcePersonalSign = isPersonalSign;//DEFAULT HSM
                        return;
                    }
                    throw new CourierForceSignException("No suitable signature position found");
                }
                LogMessagingUtil.Instance.AppendLine($"ApplyForceSign:memory:{signStation?.PersonId}");
                if (isMulti_CheckOnly)
                {
                    return;
                }
                //requestParams.ForcePersonalSign = false;
                requestParams.ForcePersonalSign = isPersonalSign;//DEFAULT HSM

            }
            else
            {
                Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance
.AppendLine("else !signQueueHSMService.IsHSMSign_IsOn(requestParams.Tenant)");

                var hSMSignStationCheckService = new HSMSignStationCheckService();
                var res = hSMSignStationCheckService.CheckIfHSMIsValid(requestParams.Tenant, isPersonalSign);
                LogMessagingUtil.Instance.AppendLine($"ApplyForceSign:HSM:Success={res.Success};{res.ErrorMessage};took={sw.Elapsed}");
                if (!res.Success)
                {
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance
.AppendLine("error:"+res.ErrorMessage);
                    throw new CourierForceSignException(res.ErrorMessage);
                }
                if (isMulti_CheckOnly)
                {
                    return;
                }
                requestParams.ForcePersonalSign = isPersonalSign;//DEFAULT HSM
                requestParams.SignMethodByQueue = SignMethodByQueueEnum.HSMSignQueue.ToString();
                requestParams.SignByPersonalId = res.MySignStationList.PersonId;

            }
            requestParams.RequestVIAChangeDue = $"Courier- Sign {requestParams.InterfaceTypeCode} ";
            requestParams.SignQueueByCompanyOrPersonal = (isPersonalSign ? SignQueueByType.SignQueueByPersonId : SignQueueByType.SignQueueByCustomsAgentId).ToString();
        }

        private static bool IsPersonalSign<TRequestParams>(TRequestParams requestParams, ref bool isMulti_CheckOnly) where TRequestParams : RequestParamsBase
        {
            bool isPersonalSign;
            switch (requestParams.InterfaceTypeCode)
            {
                case "UCB1170":
                case "UCB2755":
                case "UCB2715":
                    isMulti_CheckOnly = true;
                    isPersonalSign = false;
                    break;

                case "UCB2750":
                    isMulti_CheckOnly = true;
                    isPersonalSign = true;
                    break;

                case "2750":
                    isPersonalSign = true;

                    break;
                default:
                    throw new Exception($"unknown InterfaceTypeCode{requestParams.InterfaceTypeCode} ");
                    break;
            }

            return isPersonalSign;
        }

        private static IEnumerable<Data.EntityPOCOs.InterfaceManagement> GetSignInterfaces<TRequestParams>(TRequestParams requestParams) where TRequestParams : RequestParamsBase
        {
            var interfaceManagementQueryService = new InterfaceManagementQueryService(requestParams.Tenant);
            var signInterfaces = interfaceManagementQueryService.GetAllFromCache().Where(r => r.SignatureTypeCode == "C");
            return signInterfaces;
        }
    }

    public class CourierForceSignException : Exception
    {
        public CourierForceSignException(string message) : base(message)
        {
        }
    }
}
