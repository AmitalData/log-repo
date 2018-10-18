//Yuval Chalup 18.06.2015 TASK-13858
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageToAgentServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class DOC_NG_5101_GNMessageToAgent_RequestService
        : RequestServiceBase<DOC_NG_5101_GNMessageToAgent, MessageToAgentRequestParams>
    {
        public override DOC_NG_5101_GNMessageToAgent GetRequest(MessageToAgentRequestParams requestParams)
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(dbContext);
            var myNotificationQueryService = new NotificationQueryService(dbContext);
            var myNotificationReplyQueryService = new NotificationReplyQueryService(dbContext);

            if (string.IsNullOrWhiteSpace(requestParams.NotificationId) || string.IsNullOrWhiteSpace(requestParams.DeclarationId))
            {
                return null;
            }
            NotificationPM myNotificationPM = myNotificationQueryService.GetSingle(requestParams.NotificationId, true, false);
            DeclarationPM myDeclarationPM = myDeclarationQueryService.GetSingle(requestParams.DeclarationId, true, false);
            if (myNotificationPM == null || myDeclarationPM == null)
            {
                return null;
            }

            int maxLine = myNotificationPM.NotificationRplies.Max(d => d.Line);
            NotificationReplyPM lastNotificationReplyPM = (from a in myNotificationPM.NotificationRplies
                                       where (a.Line == maxLine)
                                       select a).FirstOrDefault();

            DOC_NG_5101_GNMessageToAgent _DOC_NG_5101_GNMessageToAgent = new DOC_NG_5101_GNMessageToAgent();
            _DOC_NG_5101_GNMessageToAgent.MessageToAgent = new DOC_NG_5101_GNMessageToAgentMessageToAgent()
            {
                msgCode = 5,
                //msgString = myNotificationPM.ResponseNotes,
                msgString = lastNotificationReplyPM.ResponseToCustoms,
            };

            if (!string.IsNullOrWhiteSpace(myNotificationPM.Reference2Number))
            {
                int reference2Number;
                int.TryParse(myNotificationPM.Reference2Number, out reference2Number);

                _DOC_NG_5101_GNMessageToAgent.MessageToAgent.responseToMessage = reference2Number;
                _DOC_NG_5101_GNMessageToAgent.MessageToAgent.responseToMessageSpecified = true;
            }

            _DOC_NG_5101_GNMessageToAgent.MessageToAgent.RelatedEntity = new ConnectedEntity()
            {
                entityIdKey1 = myDeclarationPM.DeclarationNumber,
                entityType = 1055,
            };

            this.MyRequestSheetParam = new RequestSheetParam()
            {
                CustomFileNo = myDeclarationPM.CustomFileNo,
                ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                EntityId1 = myDeclarationPM.Id,
                RequestDescription = "מענה להודעות לסוכן " + myNotificationPM.Reference2Number,
            };

            return _DOC_NG_5101_GNMessageToAgent;
        }
    }
}
