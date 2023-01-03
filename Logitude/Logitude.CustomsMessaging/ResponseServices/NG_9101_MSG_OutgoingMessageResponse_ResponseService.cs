
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.Dca.Restore9100;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageRestoreServiceReference;
using UnifreightIIG.Common.OutgoingMessageRequestServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class NG_9101_MSG_OutgoingMessageResponse_ResponseService :
  ResponseServiceBase<MessageWaitingResponseData, NG_9101_MSG_OutgoingMessageResponse, MessageWaitingRequestParams>
    {


        public override void Update(NG_9101_MSG_OutgoingMessageResponse customResponse, MessageWaitingRequestParams requestParams)
        {
            int messageRestoreCount = 0;
            var succeeded = false;
            if (customResponse.OutgoingMessage==null)
            {

            }
            else
            {
                var _CustomsSettingPM  = CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant);

                var interfaceTypeQueryService = new InterfaceTenantDefinitionQueryService(_CustomsSettingPM.Tenant);
                //interfaceTypeQueryService.GetInterfaceManagementwithDefinition(_CustomsSettingPM.Tenant);
                ////var interfaceTenantDefinitionQueryService = new InterfaceTenantDefinitionQueryService(_CustomsSettingPM.Tenant);

                var _AllInterface = interfaceTypeQueryService.GetWithInterfaceManagementDefinition(_CustomsSettingPM.Tenant);


                var _InterfaceListDCA = interfaceTypeQueryService.GetInterfaceListDCA(_AllInterface, _CustomsSettingPM.CompanyType);


                var outgoingMessage9100ResponseAnalyze = new OutgoingMessage9100ResponseAnalyze(_CustomsSettingPM, _InterfaceListDCA);

                outgoingMessage9100ResponseAnalyze.SaveInDB(customResponse.OutgoingMessage.ToList());

            }
            //if (customResponse.MessageRestoreResponseOutput != null)
            //{
            //    succeeded = true;
            //    messageRestoreCount = customResponse.MessageRestoreResponseOutput.NumOfResults;
            //}

            //this.MyResponseData = new MessageWaitingResponseData()
            //{
            //    Succeeded = succeeded,
            //    MessageRestoreCount = messageRestoreCount
            //};
        }

        public override MessageWaitingResponseData GetResponse(NG_9101_MSG_OutgoingMessageResponse customResponse, MessageWaitingRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}

