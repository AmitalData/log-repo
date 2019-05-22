
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
    public class UniCourierBatchSend2715_MsgResponseService : ResponseServiceBase
        <INF_MSG_GenericResponseData, DCAInUCB2715WithResponseContentHeader, GenericRequestParams>
    {
        public override void Update(DCAInUCB2715WithResponseContentHeader customResponse, GenericRequestParams requestParams)
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
                //listPM = qs.GetByMasterIDCourierDeclarationStatusCode(requestParams.Tenant, requestParams.AppicationId, "R",
                //    customResponse.SelectedBOLValue,
                //    customResponse.SelectedStatusValue,
                //    customResponse.SelectedTotalInvoiceValue,
                //    customResponse.SelectedFastIndividualProcessValue,
                //    customResponse.SelectedCustomStatusValue);

            }
            if (listPM.Count == 0)
            {
                mess.AppendLine($"There ARE  NOT any Declarations 'R'eady to (Declaration) send  for master {requestParams.AppicationId} ");
            }
            //else // to check?!!!!
            //{
            //    listPM = listPM.Where(r => (r.CourierPaymentStatusCode == "R" || string.IsNullOrWhiteSpace(r.CourierPaymentStatusCode))).ToList();
            //    if (listPM.Count == 0)
            //    {
            //        mess.AppendLine($"יש להוסיף בדיקה לשדר מצהר תקינים ושדר הצהרה תקינים שרק הצהרות שלא שולמו ישלחו  {requestParams.AppicationId} ");
            //    }
            //}

            foreach (var itemPM in listPM)
            {
                var customsDocumentQueryService = new CustomsDocumentQueryService(context);
                var customsDocumentPMList = customsDocumentQueryService.GetCustomsDocumentPMListWithoutRequestedDoc(new GetTicketsParams() { ParentEntityId = itemPM.DeclarationId, ParentEntityCode = "Declaration" }, itemPM.Tenant);

                foreach (var customsDocumentPM in customsDocumentPMList)
                {
                    if (customsDocumentPM.DocumentStatusCode == "2")
                    {
                        try
                        {
                            var requestParams2750 = new GenericRequestParams()
                            {
                                Tenant = requestParams.Tenant,
                                LoggingEnabled = true,
                                LoggingObjectTableId = objectTableId,
                                LoggingEntityId = itemPM.DeclarationId,
                                LoggingObjectTableId2 = requestParams.LoggingObjectTableId,
                                LoggingEntityId2 = objectTableIdCourierMaster,
                                AppicationId = itemPM.DeclarationId,
                                InterfaceTypeCode = "2750",
                                //LoggingEntityReference = declarationNumber,
                                LoggingUserId = requestParams.LoggingUserId,
                                RequestVIA = SendRequestVIA.WebServiceBatch,
                            };

                            SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams2750, false);
                            LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({itemPM.DeclarationId})");
                            mess.AppendLine($" CreateSheetSBQMessage({itemPM.DeclarationId})");
                        }
                        catch (System.Exception ee1)
                        {
                            LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.DeclarationId}) : {ee1.Message}");
                            mess.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.DeclarationId}) : {ee1.Message}");
                        }
                    }
                }
            }

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();

            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCB2715WithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

    }
}
