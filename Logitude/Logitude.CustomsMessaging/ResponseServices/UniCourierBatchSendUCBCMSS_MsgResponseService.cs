using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();

            var objectTableId =ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");

            var qs = new DeclarationCourierStatusQueryService(context);
            List<string> lockedDeclarations = new List<string>();
            List <DeclarationCourierStatusPM> listPM = new List<DeclarationCourierStatusPM>();
            listPM = qs.GetByMasterIDDeclarationCourierStatus(requestParams.Tenant, requestParams.AppicationId);
            if (listPM.Count == 0)
            {
                mess.AppendLine($"There ARE  NOT any Declarations 'R'eady to (Manifest) send for master {requestParams.AppicationId} ");
            }

            foreach (DeclarationCourierStatusPM itemPM in listPM)
            {
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
                            lockedDeclarations.Add(declarationPM.CustomFileNo);
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
                                declarationPM.Consignments.FirstOrDefault().UnloadPortCode = customResponse.StorageSiteCode;
                                declarationPM.Consignments.FirstOrDefault().StorageSiteCode = customResponse.StorageSiteCode;
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
                                    SBQMessageService.CreateSheetSBQMessage<MANIFESTRequestRequestParams>(requestParams1170, false, DateTime.Now.AddMinutes(2));
                                    LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage From UniCourierBatchSendUCBCMSS_MsgResponseService ({itemPM.DeclarationId})");
                                    mess.AppendLine($" CreateSheetSBQMessage From UniCourierBatchSendUCBCMSS_MsgResponseService ({itemPM.DeclarationId})");
                                }
                            }
                        }
                        else
                        {
                            LogMessagingUtil.Instance.AppendLine($"GetSingleGeneralLockNOWAIT({declarationPM.CustomFileNo}) ");
                            //Task .....
                        }
                    }
                }
                catch (System.Exception ee1)
                {

                    LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.DeclarationId}) : {ee1.Message}");
                    mess.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.DeclarationId}) : {ee1.Message}");
                }
            }

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();

            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }
    }
}
