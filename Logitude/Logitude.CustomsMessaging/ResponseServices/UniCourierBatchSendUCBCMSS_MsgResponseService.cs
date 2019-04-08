using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.Repsitories;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UniCourierBatchSendUCBCMSS_MsgResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DCAInUCBCMSSWithResponseContentHeader, GenericRequestParams>
    {

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCBCMSSWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DCAInUCBCMSSWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            var mess = new StringBuilder();
            var context = CustomContext.GetContext(requestParams.Tenant);           
            this.MyResponseData = new INF_MSG_GenericResponseData();

            var objectTableId =ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");

            var qs = new DeclarationCourierStatusQueryService(context);
            List<DeclarationPM> lockedDeclarations = new List<DeclarationPM>();
            List <DeclarationCourierStatusPM> listPM = new List<DeclarationCourierStatusPM>();
            listPM = qs.GetByMasterIDDeclarationCourierStatus(requestParams.Tenant, requestParams.AppicationId);
            if (listPM.Count == 0)
            {
                mess.AppendLine($"There ARE  NOT any Declarations 'R'eady to (Manifest) send for master {requestParams.AppicationId} ");
            }
            bool multiThread = false;
            if (multiThread)
            {
                var listOf50items = listPM.ChunkBy(50);
                Parallel.For(0, listOf50items.Count, new ParallelOptions { MaxDegreeOfParallelism = 5 }, count =>
                {
                    Debug.WriteLine($"Parallel-count {count}, CurrentThread{Thread.CurrentThread.ManagedThreadId.ToString()}");
                    
                    using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromSeconds(100)))
                    {
                        var context1 = CustomContext.GetContext(requestParams.Tenant);
                        foreach (DeclarationCourierStatusPM itemPM in listOf50items[count])
                        {
                            var changeStorgeSiteService = new ChangeStorgeSiteService(context1);
                            changeStorgeSiteService.ChangeStorgeSite(customResponse.StorageSiteCode, requestParams, mess, objectTableId, objectTableIdCourierMaster, lockedDeclarations, itemPM);

                        }
                        scope.Complete();
                    }
                });
            }
            else
            {
                var context1 = CustomContext.GetContext(requestParams.Tenant);
                foreach (DeclarationCourierStatusPM itemPM in listPM)
                {
                    var changeStorgeSiteService = new ChangeStorgeSiteService(context1);
                    changeStorgeSiteService.ChangeStorgeSite(customResponse.StorageSiteCode, requestParams, mess, objectTableId, objectTableIdCourierMaster, lockedDeclarations, itemPM);
                }

            }

            if(lockedDeclarations != null && lockedDeclarations.Count() > 0)
            {
                List<string> declarationsList = new List<string>();
                string message = string.Concat("אתר אחסון בטיסה השתנה ל ", customResponse.StorageSiteCode, ", אך ההצהרה לא ניתנת לעידכון. נא לעדכן ידנית");
                foreach (DeclarationPM itemDeclaration in lockedDeclarations)
                {
                    declarationsList.Add(itemDeclaration.CustomFileNo);
                }
                RaiseEvent(lockedDeclarations.FirstOrDefault(), declarationsList, "FSE", message);
            }

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }

        private void RaiseEvent(DeclarationPM declarationPM, List<string> declarationsList, string eventCode, string remarks)
        {
            try
            {
                string loggingUserId = "";
                loggingUserId = AuthenticationUtil.ResolveUserId(declarationPM.Tenant);

                var eventContextTagModel = declarationPM.CurrentContextTag as EventContextTagModel;
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = declarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = eventCode,
                    notes = "DO_NOT_RAISE_EVENT",
                    CommunicationLoggingEntityReference = declarationPM.DeclarationNumber,
                    EntityId = declarationPM.Id,
                    UserId = loggingUserId,
                    CommunicationSubject = "Event from logitude",

                    MyUnifreightEventParam = new UnifreightEventParam()
                    {
                        Code = eventCode,
                        Mode = UnifreightEventMode.@new,
                        EventDateTime = DateTime.Now,
                        Entname = "CFIFILEM",
                        PrimaryNum = declarationPM.CustomFileNo,
                        PrimaryNumList = declarationsList,
                        EventRemarks = remarks,
                        EventUser = loggingUserId,
                    }
                };

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent  eventCode = " + eventCode + " CustomFileNo= " + declarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel, true);
            }
            catch (System.Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }
    }

    class ChangeStorgeSiteService
    {
        private ICustomContext context;

        public ChangeStorgeSiteService(ICustomContext context)
        {
            this.context = context;
        }

        public void ChangeStorgeSite(string StorageSiteCode, GenericRequestParams requestParams, StringBuilder mess, string objectTableId, string objectTableIdCourierMaster, List<DeclarationPM> lockedDeclarations, DeclarationCourierStatusPM itemPM)
        {

            
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);

            try
            {
                var myDeclarationQueryService = new DeclarationQueryService(context);
                myDeclarationQueryService.LoadSupplierInvoicesItemsParentsOnly = true;
                DeclarationPM declarationPM = myDeclarationQueryService.GetSingle(itemPM.DeclarationId, true, false);
                if (declarationPM != null)
                {
                    bool isUpdateDeclaration = true;
                    var myCCUQUELOCKRepository = new CCUQUELOCKRepository(requestParams.Tenant);
                    try
                    {
                        var cculock = myCCUQUELOCKRepository.GetSingleGeneralLockNOWAIT("CCUFILEM", declarationPM.CustomFileNo);
                    }
                    catch (System.Exception)
                    {
                        LogMessagingUtil.Instance.AppendLine($"GetSingleGeneralLockNOWAIT(CCUFILEM, {declarationPM.CustomFileNo}) ==> Already Lock => try later (*5) ");
                        isUpdateDeclaration = false;
                    }
                    if (!myDeclarationUpdateService.CheckIfUpdatingAllowed(declarationPM))
                    {
                        LogMessagingUtil.Instance.AppendLine($"CheckIfUpdatingAllowed({declarationPM.CustomFileNo}) ");
                        isUpdateDeclaration = false;
                    }

                    if (isUpdateDeclaration)
                    {
                        if (declarationPM.Consignments != null && declarationPM.Consignments.Count > 0)
                        {
                            LogMessagingUtil.Instance.AppendLine("DeclarationUpdateService.Update for declaration: " + declarationPM.CustomFileNo + " declarationPM.ImporterName: " + declarationPM.ImporterName + "\n");
                            declarationPM.ChangeSetOp = ChangeSetOperation.Update;
                            declarationPM.Consignments.FirstOrDefault().ChangeSetOp = ChangeSetOperation.Update;
                            declarationPM.Consignments.FirstOrDefault().UnloadPortCode = /*customResponse.*/StorageSiteCode;
                            declarationPM.Consignments.FirstOrDefault().StorageSiteCode = /*customResponse.*/StorageSiteCode;
                            myDeclarationUpdateService.CourierStorageSiteChanged = true;
                            myDeclarationUpdateService.Update(declarationPM, true);

                            if (itemPM.CourierManifestStatusCode == "V")
                            {
                                var requestParams1170 = new MANIFESTRequestRequestParams()
                                {
                                    Tenant = requestParams.Tenant,
                                    LoggingEnabled = true,
                                    LoggingObjectTableId = objectTableId,
                                    LoggingEntityId = itemPM.DeclarationId,
                                    LoggingObjectTableId2 = requestParams.LoggingObjectTableId,
                                    LoggingEntityId2 = objectTableIdCourierMaster,
                                    InterfaceTypeCode = "1170",
                                    LoggingUserId = requestParams.LoggingUserId,
                                    RequestVIA = SendRequestVIA.WebServiceBatch,
                                    DeclarationId = itemPM.DeclarationId,
                                    LoggingEntityReference = itemPM.DeclarationId,

                                };
                                SBQMessageService.CreateSheetSBQMessage<MANIFESTRequestRequestParams>(requestParams1170, false);
                                LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({itemPM.DeclarationId})");
                                mess.AppendLine($" CreateSheetSBQMessage({itemPM.DeclarationId})");
                            }
                        }
                    }
                    else
                    {
                        lockedDeclarations.Add(declarationPM);
                    }
                }
            }
            catch (System.Exception ee1)
            {

                LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.DeclarationId}) : {ee1.Message}");
                mess.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.DeclarationId}) : {ee1.Message}");
            }
        }

    }
    public static class ListExtensions
    {
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
