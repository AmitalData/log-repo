using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.ExternalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Customs.SignQueueBL
{
    public class CreateSignQueueHybridExportDBService
    {
        public void CreateQueue(RequestParamsBase requestParams, string personId, SignQueueByType SignatureBy, string CustomsAgentId)
        {
            string queueName = "";
            if (SignatureBy == SignQueueByType.SignQueueByPersonId)
            {
                if (string.IsNullOrWhiteSpace(personId))
                {
                    throw new Exception($"personId is null, for userId {requestParams.LoggingUserId} - cache ?? ");
                }
                queueName = "PersonalSign_" + requestParams.Tenant + "_" + personId;
            }
            else
            {

                if (string.IsNullOrWhiteSpace(CustomsAgentId))
                {
                    throw new Exception($"CustomsAgentId is null, for tenant ?!? {requestParams.Tenant} - cache ?? ");
                }

                queueName = "CompanySign_" + requestParams.Tenant + "_" + CustomsAgentId;
            }

            Dictionary<string, string> messageProperties = new Dictionary<string, string>();
            messageProperties.Add("InterfaceTypeCode", requestParams.InterfaceTypeCode);
            messageProperties.Add("Tenant", requestParams.Tenant.ToString());
            messageProperties.Add("CorrelationId", requestParams.CustomsRequestsSheetId);
            var queueService = new Server.Tools.QueueService.CustomDbQueueService(queueName, 0);
            var queueSendModel = new Server.Tools.QueueService.QueueSendModel();
            queueSendModel.EntityCode = "CustomsRequestsSheet".ToLower();//"CustomsRequestsSheet";
            queueSendModel.EntityId = requestParams.CustomsRequestsSheetId;// MyCustomsRequestsSheetPM?.Id;
            queueSendModel.UseRabbitMQ = false;//never use rabbit !!!!
            queueSendModel.InterfaceTypeCode = requestParams.InterfaceTypeCode;
            queueSendModel.Tenant = requestParams.Tenant;
            queueService.Send(messageProperties, requestParams.Tenant, null, queueSendModel);
        }
    }
}
