using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Customs
{
    public class CustomsRequestsSheetPostUpdateService
    {
        public static string GetCustomFileNo(int tenant, string entityId, string objectTableId)
        {
            if (String.IsNullOrWhiteSpace(entityId))
            {
                return null;
            }
            if (String.IsNullOrWhiteSpace(objectTableId))
            {
                return null;
            }
            if (objectTableId != ObjectTableRepository.GetObjectTableByName("Customs.Declaration", tenant))
            {
                return null;
            }
            var declarationQueryService = new DeclarationQueryService(tenant);
            var dec = declarationQueryService.GetSingle(entityId, false, true);
            if (dec == null)
            {
                //throw new Exception("Declaration not exist ??!!");
                return null;
            }
            return dec.CustomFileNo;
        }

        internal static void PostUpdateConnectedEntitys(
         int _Tenant, string customsRequestsSheetId,
         string objectTableId, string entityId, bool commonContextSaveChanges, bool customContextSaveChanges)
        {
            if (String.IsNullOrWhiteSpace(entityId) ||
                String.IsNullOrWhiteSpace(customsRequestsSheetId) ||
                string.IsNullOrWhiteSpace(objectTableId))
            {
                return;
            }
            
            var commonContext = CommonDataContext.GetContext(_Tenant);
            var customContext = CustomContext.GetContext(_Tenant);
            var communicationLogRepository = new CommunicationLogRepository(commonContext);
            var customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(customContext);
            var customsRequestsSheetUpdateService = new CustomsRequestsSheetUpdateService(customContext, new Dictionary<string, IContext>(), _Tenant);
            var customsRequestsSheet = customsRequestsSheetQueryService.GetSingle(customsRequestsSheetId, false, false);
            if (string.IsNullOrWhiteSpace(customsRequestsSheet.RequestComminicationId))
            {
                return;
                ///throw;
            }
            var communicationLog = communicationLogRepository.GetSingleCommunicationLog(customsRequestsSheet.RequestComminicationId, _Tenant);

            communicationLog.ObjectTableId = customsRequestsSheet.ObjectTableId1 = objectTableId;
            communicationLog.EntityId = customsRequestsSheet.EntityId1 = entityId;
            if (string.IsNullOrWhiteSpace(customsRequestsSheet.CustomFileNo))
            {
                customsRequestsSheet.CustomFileNo = GetCustomFileNo(customsRequestsSheet.Tenant, customsRequestsSheet.EntityId1, customsRequestsSheet.ObjectTableId1);
            }


            communicationLogRepository.Update(communicationLog);
            customsRequestsSheet.ChangeSetOp = ChangeSetOperation.Update;
            customsRequestsSheetUpdateService.Update(customsRequestsSheet, true);
            if (commonContextSaveChanges)
            {
                commonContext.SaveChanges();
            }
            if (customContextSaveChanges)
            {
                customContext.SaveChanges();
            }
        }
    }
}
