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
        public override INF_MSG_GenericResponseData GetResponse(DCI_CourierMastersConnectedResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return MyResponseData;
        }

        public override void Update(DCI_CourierMastersConnectedResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            CourierMasterPM courierMasterPM = customResponse.entityPM;
            MyResponseData = new INF_MSG_GenericResponseData();
            MyRequestSheetParam = MyRequestSheetParam ?? new RequestSheetParam();
            MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            //MyRequestSheetParam.CustomFileNo = customResponse.CustomFileNo;
            MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.CourierDeclarationStatus");
            MyResponseData.ApplicationID = customResponse.entityPM.Id;

            string msg = Update(courierMasterPM, customResponse, requestParams);

            MyResponseData.UserMessage = msg;
            MyResponseData.Succeeded = true;
        }

        private string Update(CourierMasterPM courierMasterPM, DCI_CourierMastersConnectedResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            var mess = new StringBuilder();

            CourierMasterPM cmPm = GetCourierMasterPM(courierMasterPM.Id, courierMasterPM.Tenant);
            //int count = connected ? UpdateAllConnected(cmPm) : UpdateAllNotConnected(cmPm);
            cmPm.ConnectedDeclarations = courierMasterPM.ConnectedDeclarations;
            cmPm.NotConnectedDeclarations = courierMasterPM.NotConnectedDeclarations;

            if (customResponse?.ServerSplitDeclarationsList != null && customResponse?.ServerSplitDeclarationsList.Count > 0)
            {
                mess.AppendLine($"מפוצל כבר !!!");

                int counter = customResponse.connect ?
                    ConnectedDeclaration(customResponse.ServerSplitDeclarationsList, cmPm) :
                    DisconnectedDeclaration(customResponse.ServerSplitDeclarationsList, cmPm);

                mess.AppendLine($"{(customResponse.connect ? "קושרו" : "נותקו")} {counter} הצהרות");
            }
            else
            {
                mess.AppendLine($"ראשי - מפצל");
                mess.AppendLine($"כל ההצהרות יפוצלו.....");
                mess.AppendLine($"נעשו {CreateChunkMessages(cmPm, customResponse, requestParams)} פיצולים");
            }

            return mess.ToString();
        }

        private static CourierMasterPM GetCourierMasterPM(string courierMasterId, int tenant)
        {
            var cmPm = new CourierMasterPM();
            var cmPoco = new CourierMasterRepository(tenant).GetSingle(courierMasterId, tenant);
            new CourierMasterDataMapping().CustomPOCOToPM(cmPm, cmPoco);
            return cmPm;
        }

        private int CreateChunkMessages(CourierMasterPM courierMasterPM, DCI_CourierMastersConnectedResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            int count = 0;
            DeclarationRepository declarationRepository;
            CourierDeclarationUpdateService courierDeclarationUpdateService;
            DeclarationCourierStatusRepository rep;
            InitServices(courierMasterPM.Tenant, out declarationRepository, out courierDeclarationUpdateService, out rep);

            bool connect = courierMasterPM.ConnectedDeclarations == "ALL";
            var decids = connect ?
                declarationRepository.GetNotConnectedDeclarations(courierMasterPM.Tenant).Select(r => r.Id).ToList() :
                courierMasterPM.ConnectedDeclarations.Substring(0, courierMasterPM.ConnectedDeclarations.Length - 1).Split(',').ToList();

            decids.ChunkBy(100).ForEach(list100 =>
            {
                count++;
                customResponse.ServerSplitDeclarationsList = list100;
                customResponse.LoggingUserId = requestParams.LoggingUserId;
                customResponse.connect = connect;
                new CRSUtil().CreateCRS_DCAIn<DCI_CourierMastersConnectedResponseContentHeader>(customResponse, (requestParams as RequestParamsBase));
            });

            return count;
        }

        private int ConnectedDeclaration(List<string> declarationIds, CourierMasterPM entityPM)
        {
            DeclarationRepository declarationRepository;
            CourierDeclarationUpdateService courierDeclarationUpdateService;
            DeclarationCourierStatusRepository rep;
            InitServices(entityPM.Tenant, out declarationRepository, out courierDeclarationUpdateService, out rep);

            int? maxSequenceNunmeric = new CourierDeclarationQueryService(entityPM.Tenant).GetCourierMasterMaxSequenceNumeric(entityPM.Id, entityPM.Tenant);

            var decsC = declarationRepository.GetDeclarationsById(declarationIds);
            foreach (var dec in decsC)
            {
                ++maxSequenceNunmeric;
                CourierDeclarationPM courierDeclaration = new CourierDeclarationPM() { DeclarationId = dec.Id, CourierMasterId = entityPM.Id, Tenant = entityPM.Tenant, ChangeSetOp = ChangeSetOperation.Insert, SequenceNumeric = maxSequenceNunmeric };
                courierDeclarationUpdateService.Update(courierDeclaration, false);
                DeclarationCourierStatus decCourier = rep.GetDeclarationsById(dec.Id, dec.Tenant);
                if (decCourier != null && !decCourier.IsClosedForFollowUp)
                    entityPM.OpenDeclarations += 1;
            }

            return decsC.Count;
        }

        private int DisconnectedDeclaration(List<string> declarationIds, CourierMasterPM entityPM)
        {
            DeclarationRepository declarationRepository;
            CourierDeclarationUpdateService courierDeclarationUpdateService;
            DeclarationCourierStatusRepository rep;
            InitServices(entityPM.Tenant, out declarationRepository, out courierDeclarationUpdateService, out rep);

            var decs = declarationRepository.GetDeclarationsById(declarationIds);
            foreach (var item in decs)
            {
                CourierDeclarationPM courierDeclaration = new CourierDeclarationPM();
                CourierDeclarationQueryService courierDeclarationDelQuery = new CourierDeclarationQueryService(entityPM.Tenant);
                courierDeclaration = courierDeclarationDelQuery.GetSingle(item.Id, entityPM.Id, false, true);
                courierDeclaration.ChangeSetOp = ChangeSetOperation.Delete;
                courierDeclarationUpdateService.Update(courierDeclaration, true);
                DeclarationCourierStatus decCourier = rep.GetDeclarationsById(item.Id, item.Tenant);
                if (decCourier != null && !decCourier.IsClosedForFollowUp)
                    entityPM.OpenDeclarations -= 1;
            }

            return decs.Count();
        }

        private static void InitServices(int tenant, out DeclarationRepository declarationRepository, out CourierDeclarationUpdateService courierDeclarationUpdateService, out DeclarationCourierStatusRepository rep)
        {
            ICustomContext context = CustomContext.GetContext(tenant);
            declarationRepository = new DeclarationRepository(context);
            courierDeclarationUpdateService = new CourierDeclarationUpdateService(context, new Dictionary<string, IContext>(), tenant);
            rep = new DeclarationCourierStatusRepository(context);
        }
    }
}
