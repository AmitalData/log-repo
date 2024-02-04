using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Utils;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DCI_CourierMastersConnectedResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DCI_CourierMastersConnectedResponseContentHeader, GenericRequestParams>
    {
        DeclarationRepository declarationRepository;
        CourierDeclarationUpdateService courierDeclarationUpdateService;
        DeclarationCourierStatusRepository rep;

        public override INF_MSG_GenericResponseData GetResponse(DCI_CourierMastersConnectedResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return MyResponseData;
        }

        public override void Update(DCI_CourierMastersConnectedResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            MyResponseData = new INF_MSG_GenericResponseData();
            MyRequestSheetParam = MyRequestSheetParam ?? new RequestSheetParam();
            MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            //MyRequestSheetParam.CustomFileNo = customResponse.CustomFileNo;
            MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.CourierDeclarationStatus");
            MyResponseData.ApplicationID = customResponse.courierMasterId;

            string msg = UpdateDb(customResponse, requestParams);

            MyResponseData.UserMessage = msg;
            MyResponseData.Succeeded = true;
        }

        private string UpdateDb(DCI_CourierMastersConnectedResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            var mess = new StringBuilder();
            InitServices(customResponse.tenant);

            //int count = connected ? UpdateAllConnected(cmPm) : UpdateAllNotConnected(cmPm);
            customResponse.ServerSplitDeclarationsList = customResponse.ServerSplitDeclarationsList.Where(x => !string.IsNullOrEmpty(x)).ToList();

            if (customResponse.ServerSplitDeclarationsList != null && customResponse.ServerSplitDeclarationsList.Count > 0)
            {
                mess.AppendLine($"מפוצל כבר !!!");

                int counter = customResponse.Connect ?
                    ConnectedDeclaration(customResponse.ServerSplitDeclarationsList, customResponse.courierMasterId, customResponse.tenant) :
                    DisconnectedDeclaration(customResponse.ServerSplitDeclarationsList, customResponse.courierMasterId, customResponse.tenant);

                mess.AppendLine($"{(customResponse.Connect ? "קושרו" : "נותקו")} {counter} הצהרות");
            }
            else
            {
                mess.AppendLine($"ראשי - מפצל");
                mess.AppendLine($"כל ההצהרות יפוצלו.....");
                mess.AppendLine($"נעשו {CreateChunkMessages(customResponse, requestParams)} פיצולים");
            }

            return mess.ToString();
        }

        private CourierMasterPM GetCourierMasterPM(string courierMasterId, int tenant) =>
            new CourierMasterQueryService(tenant).GetSingle(courierMasterId, true, true);

        private int CreateChunkMessages(DCI_CourierMastersConnectedResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            int count = 0;

            count += UpdateConnectedDeclaration(customResponse, requestParams);

            count += UpdateDisconnectedDeclaration(customResponse, requestParams);

            return count;
        }

        private int UpdateDisconnectedDeclaration(DCI_CourierMastersConnectedResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            int count;
            List<string> decsIds;

            if (customResponse.disconnectedAll)
                decsIds = declarationRepository.GetNotConnectedDeclarations(customResponse.tenant)
                    .Select(r => r.Id)
                    .Where(x => !customResponse.disconnectedItems.Contains(x)).ToList();
            else
                decsIds = customResponse.disconnectedItems.ToList();

            count = CreateChunkMessages(customResponse, requestParams, true, decsIds);

            return count;
        }

        private int UpdateConnectedDeclaration(DCI_CourierMastersConnectedResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            int count;
            List<string> decsIds;

            if (!customResponse.connectedAll)
                decsIds = declarationRepository.GetCourierConnectedDeclaratins(customResponse.courierMasterId, customResponse.tenant)
                    .Select(r => r.Id)
                    .Where(x => !customResponse.connectedItems.Contains(x)).ToList();
            else
                decsIds = customResponse.connectedItems.ToList();

            count = CreateChunkMessages(customResponse, requestParams, false, decsIds);

            return count;
        }

        private int CreateChunkMessages(DCI_CourierMastersConnectedResponseContentHeader customResponse, GenericRequestParams requestParams, bool connect, List<string> decsIds)
        {
            int count = 0;
            decsIds.ChunkBy(100).ForEach(list100 =>
            {
                count++;
                customResponse.ServerSplitDeclarationsList = list100;
                customResponse.LoggingUserId = requestParams.LoggingUserId;
                customResponse.Connect = connect;
                new CRSUtil().CreateCRS_DCAIn<DCI_CourierMastersConnectedResponseContentHeader>(customResponse, (requestParams as RequestParamsBase), out string list);
            });
            return count;
        }

        private int ConnectedDeclaration(List<string> declarationIds, string courierMasterId, int tenant)
        {        
            int? maxSequenceNunmeric = new CourierDeclarationQueryService(tenant).GetCourierMasterMaxSequenceNumeric(courierMasterId, tenant);

            courierDeclarationUpdateService.FastInsert(declarationIds, tenant, courierMasterId, maxSequenceNunmeric);
            return declarationIds.Count;
        }

        private int DisconnectedDeclaration(List<string> declarationIds, string courierMasterId, int tenant)
        {            
            CourierMasterPM entityPM = GetCourierMasterPM(courierMasterId, tenant);

            var decs = declarationRepository.GetDeclarationsById(declarationIds);
            foreach (var item in decs)
            {
                CourierDeclarationPM courierDeclaration = new CourierDeclarationPM();
                CourierDeclarationQueryService courierDeclarationDelQuery = new CourierDeclarationQueryService(tenant);
                courierDeclaration = courierDeclarationDelQuery.GetSingle(item.Id, courierMasterId, false, true);
                courierDeclaration.ChangeSetOp = ChangeSetOperation.Delete;
                courierDeclarationUpdateService.Update(courierDeclaration, true);
                DeclarationCourierStatus decCourier = rep.GetDeclarationsById(item.Id, item.Tenant);
                if (decCourier != null && !decCourier.IsClosedForFollowUp)
                    entityPM.OpenDeclarations -= 1;
            }

            return decs.Count();
        }

        private void InitServices(int tenant)
        {
            ICustomContext context = CustomContext.GetContext(tenant);
            declarationRepository = new DeclarationRepository(context);
            courierDeclarationUpdateService = new CourierDeclarationUpdateService(context, new Dictionary<string, IContext>(), tenant);
            rep = new DeclarationCourierStatusRepository(context);
        }
    }
}
