using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Docs;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    class Fake_SendDOC_MSG5101_GNMessageToAgent
    {
        public string msgString;
        public int msgCode;
        public string entityIdKey1;
        public int entityType;
        internal DOC_NG_5101_GNMessageToAgent GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {
            dynamic data = JObject.Parse(requestParamsData.TestCase.Param1);
            if(data != null)
            {
                AnalyzeParam1(data);
            }
            DOC_NG_5101_GNMessageToAgent MessageToAgent = new DOC_NG_5101_GNMessageToAgent
            {
                RequestContentHeader = GetRequestContentHeader(),
                MessageToAgent= GetMessageToAgent(GetDec(requestParamsData))
            };
            return MessageToAgent;
        }

        private RequestContentHeader GetRequestContentHeader()
        {
            return  new RequestContentHeader()
            {
                TransmitionDateTime = DateTime.Now
            };
            
        }
        private DOC_NG_5101_GNMessageToAgentMessageToAgent GetMessageToAgent(DeclarationPM dec)
        {
            DOC_NG_5101_GNMessageToAgentMessageToAgent msg = new DOC_NG_5101_GNMessageToAgentMessageToAgent();
            msg.RelatedEntity = GetRelatedEntity(dec);
            msg.msgCode = msgCode;
            msg.msgString = msgString;
            msg.SenderName = "Tester";
            return msg;
        }
        private ConnectedEntity GetRelatedEntity(DeclarationPM dec)
        {
            ConnectedEntity relatedEntity = new ConnectedEntity();
            if(entityIdKey1 != null)
            {
                relatedEntity.entityIdKey1= entityIdKey1;
            }
            relatedEntity.entityType = entityType;
            return relatedEntity;
        }
        private void AnalyzeParam1(dynamic data)
        {
            if(data.entityType != null&& data.entityType != "")
            {
                int.TryParse(Convert.ToString(data.entityType), out entityType);
            }
            if (data.entityIdKey1 != null && data.entityIdKey1 != "")
            {
                entityIdKey1 = data.entityIdKey1;
            }
            if (data.msgCode != null && data.msgCode != "")
            {
                int.TryParse(Convert.ToString(data.msgCode), out msgCode);
            }
            if (data.msgString != null && data.msgString != "")
            {
                msgString = data.msgString;
            }
        }
        private DeclarationPM GetDec(GenericRequestParams requestParamsData)
        {
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParamsData.Tenant);
            return declarationQueryService.GetSingleDeclarationById(requestParamsData.LoggingEntityId, requestParamsData.Tenant);
        }


    }
}
