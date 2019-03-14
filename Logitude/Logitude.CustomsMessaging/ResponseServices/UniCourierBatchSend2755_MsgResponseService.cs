using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UniCourierBatchSend2755_MsgResponseService : ResponseServiceBase
        //<TResponseData, TCustomResponse, TRequestParams>
        <INF_MSG_GenericResponseData, DCAInUCB2755WithResponseContentHeader, GenericRequestParams>
    {
        private Customs.Def.EntityPMs.DeclarationPM _MyDeclarationPM;
        public override void Update(DCAInUCB2755WithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            var mess = new StringBuilder();
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();

            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
            var qs = new DeclarationCourierStatusQueryService(context);
            List<DeclarationCourierStatusPM> listPM = new List<DeclarationCourierStatusPM>();
            if (customResponse.DeclarationsList != null && customResponse.DeclarationsList.Count > 0)
            {
                listPM = qs.GetDeclarationsByIds(customResponse.DeclarationsList, requestParams.Tenant);
            }
            else
            {
                listPM = qs.GetByMasterIDCourierPaymentStatusCode(requestParams.Tenant, requestParams.AppicationId, "R", "L");
            }


            listPM = listPM.Where(r => r.FastIndividualProcessCode == "F").ToList();

            if (listPM.Count == 0)
            {
                mess.AppendLine($"There ARE  NOT any Declarations (LOW Val.) 'R'eady to (DEc.Payment) send  for master {requestParams.AppicationId} ");
            }
            
            var dic = new Dictionary<string, string>();
            dic.Add("InternalBankId", customResponse.InternalBankId);

            var UnifreightListOnServerOnly = UnifreightListsUtil.Serialize(dic);
            var repo = new CourierPendingReasonRepository(requestParams.Tenant);
            var allCourierPendingReason =repo.GetAll(requestParams.Tenant);
            foreach (var itemPM in listPM)
            {


                // If  ErrorPlace.CourierPendingReasons == 1 Display error Message 
                //"קיים Pending עם עצירה בתשלום הצהרה"


                if (!String.IsNullOrWhiteSpace(itemPM.CourierPendingReasonCode))
                {

                    if (allCourierPendingReason.First(r=> r.Code == itemPM.CourierPendingReasonCode).ErrorPlace == "1")
                    {
                        mess.AppendLine($" קיים Pending " +
                            $"עם עצירה בתשלום הצהרה ({itemPM.DeclarationId})");
                        continue;
                    }
                }
                try
                {
                    var requestParams2755 = new GenericRequestParams()
                    {
                        Tenant = requestParams.Tenant,
                        //IsFakeResponse = true,
                        //RequestName = requestName,
                        //ResponseName = responseName,
                        LoggingEnabled = true,
                        LoggingObjectTableId = objectTableId,
                        LoggingEntityId = itemPM.DeclarationId,
                        LoggingObjectTableId2 = requestParams.LoggingObjectTableId ,
                        LoggingEntityId2 = objectTableIdCourierMaster,
                        AppicationId = itemPM.DeclarationId,
                        InterfaceTypeCode = "2755",
                        //LoggingEntityReference = declarationNumber,
                        LoggingUserId = requestParams.LoggingUserId,
                        RequestVIA = SendRequestVIA.WebServiceBatch,
                        UnifreightListOnServerOnly = UnifreightListOnServerOnly ,

                    };

                    SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams2755, false);
                    LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({itemPM.DeclarationId})");
                    mess.AppendLine($" CreateSheetSBQMessage({itemPM.DeclarationId})");

                }
                catch (System.Exception ee1)
                {

                    LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.DeclarationId}) : {ee1.Message}");
                    mess.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.DeclarationId}) : {ee1.Message}");
                }
            }

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();

            //this.MyRequestSheetParam.RequestDescription = "Build Custom Zip File";
            ///LogitudeSettings.HandleBuildObjectTablesZipFilesData_Inject(false, true);

            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCB2755WithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }


    }
}
