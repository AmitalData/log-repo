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
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.Linq;

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
            MyResponseData.Succeeded = true;
            MyResponseData.ApplicationID = customResponse.entityPM.Id;

            int count = Update(courierMasterPM);

            MyResponseData.UserMessage = "עודכנו " + count + " הצהרות";
        }

        private int Update(CourierMasterPM courierMasterPM)
        {
            CourierMasterPM cmPm = GetCourierMasterPM(courierMasterPM.Id, courierMasterPM.Tenant);
            //int count = connected ? UpdateAllConnected(cmPm) : UpdateAllNotConnected(cmPm);
            cmPm.ConnectedDeclarations = courierMasterPM.ConnectedDeclarations;
            cmPm.NotConnectedDeclarations = courierMasterPM.NotConnectedDeclarations;
            int count = OldUpdate(cmPm);

            return count;
        }

        private static CourierMasterPM GetCourierMasterPM(string courierMasterId, int tenant)
        {
            var cmPm = new CourierMasterPM();
            var cmPoco = new CourierMasterRepository(tenant).GetSingle(courierMasterId, tenant);
            new CourierMasterDataMapping().CustomPOCOToPM(cmPm, cmPoco);
            return cmPm;
        }

        private int OldUpdate(CourierMasterPM entityPM)
        {
            int counter = 0;
            DeclarationRepository declarationRepository;
            CourierDeclarationUpdateService courierDeclarationUpdateService;
            DeclarationCourierStatusRepository rep;
            InitServices(entityPM.Tenant, out declarationRepository, out courierDeclarationUpdateService, out rep);

            if (entityPM.ConnectedDeclarations != null && entityPM.ConnectedDeclarations.Length > 0)
            {
                CourierDeclarationQueryService service = new CourierDeclarationQueryService(entityPM.Tenant);
                int? maxSequenceNunmeric = 0;
                maxSequenceNunmeric = service.GetCourierMasterMaxSequenceNumeric(entityPM.Id, entityPM.Tenant);
                if (maxSequenceNunmeric == null) maxSequenceNunmeric = 0;
                entityPM.OpenDeclarations = rep.CountOpenDeclarations(entityPM.Id, entityPM.Tenant);
                if (entityPM.ConnectedDeclarations == "ALL")
                {

                    var decsC = declarationRepository.GetNotConnectedDeclarations(entityPM.Tenant);
                    foreach (var dec in decsC)
                    {
                        ++maxSequenceNunmeric;
                        CourierDeclarationPM courierDeclaration = new CourierDeclarationPM() { DeclarationId = dec.Id, CourierMasterId = entityPM.Id, Tenant = entityPM.Tenant, ChangeSetOp = ChangeSetOperation.Insert, SequenceNumeric = maxSequenceNunmeric };
                        courierDeclarationUpdateService.Update(courierDeclaration, false);
                        DeclarationCourierStatus decCourier = rep.GetDeclarationsById(dec.Id, dec.Tenant);
                        if (decCourier != null)
                        {
                            if (!decCourier.IsClosedForFollowUp)
                            {
                                entityPM.OpenDeclarations += 1;
                            }
                        }
                    }

                    counter += decsC.Count();
                }
                else
                {
                    //entityPM.ConnectedDeclarations = entityPM.ConnectedDeclarations.Substring(1, entityPM.ConnectedDeclarations.Length - 1);
                    entityPM.ConnectedDeclarations = entityPM.ConnectedDeclarations.Substring(0, entityPM.ConnectedDeclarations.Length - 1);
                    string[] items = entityPM.ConnectedDeclarations.Split(',');
                    //DeclarationCourierStatusRepository rep = new DeclarationCourierStatusRepository(context);
                    // entityPM.OpenDeclarations = rep.CountOpenDeclarations(entityPM.Id, entityPM.Tenant);
                    foreach (string item in items)
                    {
                        ++maxSequenceNunmeric;
                        CourierDeclarationPM courierDeclaration = new CourierDeclarationPM() { DeclarationId = item, CourierMasterId = entityPM.Id, Tenant = entityPM.Tenant, ChangeSetOp = ChangeSetOperation.Insert, SequenceNumeric = maxSequenceNunmeric };
                        courierDeclarationUpdateService.Update(courierDeclaration, false);
                        DeclarationCourierStatus decCourier = rep.GetDeclarationsById(item, entityPM.Tenant);
                        if (decCourier != null)
                        {
                            if (!decCourier.IsClosedForFollowUp)
                            {
                                entityPM.OpenDeclarations += 1;
                            }
                        }
                    }

                    counter += items.Count();
                }
            }

            if (entityPM.NotConnectedDeclarations != null && entityPM.NotConnectedDeclarations.Length > 0)
            {
                //entityPM.NotConnectedDeclarations = entityPM.NotConnectedDeclarations.Substring(1, entityPM.NotConnectedDeclarations.Length - 1);

                entityPM.OpenDeclarations = rep.CountOpenDeclarations(entityPM.Id, entityPM.Tenant);
                if (entityPM.NotConnectedDeclarations == "ALL")
                {
                    var decsCN = declarationRepository.GetCourierConnectedDeclaratins(entityPM.Id, entityPM.Tenant);
                    foreach (var item in decsCN)
                    {
                        CourierDeclarationPM courierDeclaration = new CourierDeclarationPM();
                        CourierDeclarationQueryService courierDeclarationDelQuery = new CourierDeclarationQueryService(entityPM.Tenant);
                        courierDeclaration = courierDeclarationDelQuery.GetSingle(item.Id, entityPM.Id, false, true);
                        courierDeclaration.ChangeSetOp = ChangeSetOperation.Delete;
                        courierDeclarationUpdateService.Update(courierDeclaration, true);
                        DeclarationCourierStatus decCourier = rep.GetDeclarationsById(item.Id, item.Tenant);
                        if (decCourier != null)
                        {
                            if (!decCourier.IsClosedForFollowUp)
                            {
                                entityPM.OpenDeclarations -= 1;
                            }
                        }
                    }

                    counter += decsCN.Count();
                }
                else
                {
                    entityPM.NotConnectedDeclarations = entityPM.NotConnectedDeclarations.Substring(0, entityPM.NotConnectedDeclarations.Length - 1);
                    string[] NotConnecteditems = entityPM.NotConnectedDeclarations.Split(',');
                    if (NotConnecteditems != null && NotConnecteditems.Length > 0)
                    {
                        foreach (string item in NotConnecteditems)
                        {
                            CourierDeclarationPM courierDeclaration = new CourierDeclarationPM();
                            CourierDeclarationQueryService courierDeclarationDelQuery = new CourierDeclarationQueryService(entityPM.Tenant);
                            courierDeclaration = courierDeclarationDelQuery.GetSingle(item, entityPM.Id, false, true);
                            courierDeclaration.ChangeSetOp = ChangeSetOperation.Delete;
                            courierDeclarationUpdateService.Update(courierDeclaration, true);
                            DeclarationCourierStatus decCourier = rep.GetDeclarationsById(item, entityPM.Tenant);
                            if (decCourier != null)
                            {
                                if (!decCourier.IsClosedForFollowUp)
                                {
                                    entityPM.OpenDeclarations -= 1;
                                }
                            }
                        }

                        counter += NotConnecteditems.Count();
                    }
                }
            }

            return counter;
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
