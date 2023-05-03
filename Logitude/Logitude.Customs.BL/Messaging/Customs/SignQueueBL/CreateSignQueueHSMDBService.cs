using Logitude.Customs.BL.EntityQueryServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.ExternalServices;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Customs.SignQueueBL
{
    public class CreateSignQueueHSMDBService
    {
        public void CreateQueue(RequestParamsBase requestParams, string personId, SignQueueByType SignatureBy, string CustomsAgentId)
        {

            string queueName = SBQueueNames.CustomsHSMSignWR.ToString();// "HSMSign";

            Dictionary<string, string> messageProperties = new Dictionary<string, string>();
            messageProperties.Add("SignByPersonalId", requestParams.SignByPersonalId);//SignBy
            messageProperties.Add("SignQueueByCompanyOrPersonal", requestParams.SignQueueByCompanyOrPersonal);//SignBy
            var customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(requestParams.Tenant);
            var tenantPrioirty = customsRequestsSheetQueryService.GetTenantPriorityByEntityIDAndTeant(requestParams.CustomsRequestsSheetId, requestParams.Tenant);
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
            if (tenantPrioirty > 0)
            {
                queueSendModel.TenantPriority = tenantPrioirty;
            }
            queueService.Send(messageProperties, requestParams.Tenant, null, queueSendModel);
        }
    }
}
