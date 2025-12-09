
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.Dca.Restore9100;
using Logitude.Server.Tools.Helpers;
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

        const string ShrinkMSG = "Shrink SEE CorrelationId";

        public override Action<NG_9101_MSG_OutgoingMessageResponse> GetActionShrinkCustomResponse()
        {
            return (customResponse) =>
            {

                if (customResponse == null) return;
                if (customResponse.OutgoingMessage == null) return;
                customResponse.OutgoingMessage.ToList().ForEach(curr => {

                    //var MD5Hash = MD5HashUtil.GetMD5Hash(curr.MSG);
                    
                    curr.MSG = ShrinkMSG;
                });





            };
        }

        public override void Update(NG_9101_MSG_OutgoingMessageResponse customResponse, MessageWaitingRequestParams requestParams)
        {
            int messageRestoreCount = 0;
            var succeeded = false;

            if (customResponse.OutgoingMessage == null)
            {
                this.MyResponseData = new MessageWaitingResponseData()
                {
                    Succeeded = true,
                    MessageRestoreCount = 0,
                    UserMessage = "No result was returned from the IIG MessageWaitingResponseData."
                };

            }
        
            else
            {
                if (customResponse.OutgoingMessage[0].MSG == ShrinkMSG)
                {
                    throw new System.Exception("Already Shrink");
                    this.MyResponseData = new MessageWaitingResponseData()
                    {
                        Succeeded = true,
                        MessageRestoreCount = 0,
                        UserMessage = "The message has already been shrinked and cannot be re-analyzed."
                    };
                }
                else
                {


                    var _CustomsSettingPM = CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant);

                    var interfaceTypeQueryService = new InterfaceTenantDefinitionQueryService(_CustomsSettingPM.Tenant);
                    //interfaceTypeQueryService.GetInterfaceManagementwithDefinition(_CustomsSettingPM.Tenant);
                    ////var interfaceTenantDefinitionQueryService = new InterfaceTenantDefinitionQueryService(_CustomsSettingPM.Tenant);

                    var _AllInterface = interfaceTypeQueryService.GetWithInterfaceManagementDefinition(_CustomsSettingPM.Tenant);


                    var _InterfaceListDCA = interfaceTypeQueryService.GetInterfaceListDCA(_AllInterface, _CustomsSettingPM.CompanyType);


                    var outgoingMessage9100ResponseAnalyze = new OutgoingMessage9100ResponseAnalyze(_CustomsSettingPM, _InterfaceListDCA);

                    outgoingMessage9100ResponseAnalyze.SaveInDB(customResponse.OutgoingMessage.ToList(), true);

                    LogMessagingUtil.Instance.AppendLine(outgoingMessage9100ResponseAnalyze.MyStringBuilder.ToString());
                    this.MyResponseData = new MessageWaitingResponseData()
                    {
                        Succeeded = true,
                        MessageRestoreCount = outgoingMessage9100ResponseAnalyze.correlationIdsCanClear.Count,
                        UserMessage = 
                        $"HowManyOtherWaitingMessages:{customResponse.Result.HowManyOtherWaitingMessages}"
                        + Environment.NewLine +
                        $"CorrelationIDs is saved in the database:{String.Join(Environment.NewLine, outgoingMessage9100ResponseAnalyze.correlationIdsCanClear.Select(r=>r.CorrelationIDs))}" 
                        + Environment.NewLine+ 
                        $"Error:{String.Join(Environment.NewLine, outgoingMessage9100ResponseAnalyze.exceptionBag.ToList())}"

                };
            }

            }
            //if (customResponse.MessageRestoreResponseOutput != null)
            //{
            //    succeeded = true;
            //    messageRestoreCount = customResponse.MessageRestoreResponseOutput.NumOfResults;
            //}

            
        }

        public override MessageWaitingResponseData GetResponse(NG_9101_MSG_OutgoingMessageResponse customResponse, MessageWaitingRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}

