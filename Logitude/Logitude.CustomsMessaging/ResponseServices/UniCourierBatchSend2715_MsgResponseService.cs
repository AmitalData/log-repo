using Microsoft.Practices.Unity;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.Server.Tools;
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
using Logitude.CustomsMessaging.Utils;
using Simplog.Server.Infrastructure.Helpers;

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
            var myCustomsDocumentUpdateService = new CustomsDocumentUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();

            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");

            //var qs = new DeclarationCourierStatusQueryService(context);
            var repo = new DeclarationCourierStatusRepository(context);
            var listPoco = new List<DeclarationCourierStatus>();
            decimal minValPay = GetMinValPay(requestParams.Tenant); 
            if (customResponse.ServerSplitDeclarationsList != null && customResponse.ServerSplitDeclarationsList.Count > 0)
            {
                mess.AppendLine($"מפוצל כבר !!!");
                listPoco = repo.GetDeclarationsByIds(customResponse.ServerSplitDeclarationsList, requestParams.Tenant);
                bool isCreateNewDocumentVersion = customResponse.IsCreateNewDocumentVersion;
                Send2715WhereDocumentStatusCodeIs2(mess, context, /*myCustomsDocumentUpdateService,*/ listPoco, requestParams.CustomsRequestsSheetId, isCreateNewDocumentVersion);
            }
            else
            {
                mess.AppendLine($"ראשי - מפצל");
                mess.AppendLine($"כל ההצהרות יפוצלו.....");

                if (customResponse.ClientFilterDeclarationsList != null && customResponse.ClientFilterDeclarationsList.Count > 0)
                {
                    mess.AppendLine($"סומנו בצד הלקוח ");
                    listPoco = repo.GetDeclarationsByIds(customResponse.ClientFilterDeclarationsList, requestParams.Tenant);
                }
                else
                {
                    mess.AppendLine($"GetByMasterIDCourierDocumentStatus");
                    if (customResponse.IsWorkSheetFromExcel)
                    {
                        listPoco = repo.GetFromExcelCourierDocumentStatus(requestParams.Tenant, requestParams.LoggingUserId, "X",
                          customResponse.SelectedBOLValue,
                          customResponse.SelectedStatusValue,
                          customResponse.SelectedTotalInvoiceValue,
                          customResponse.SelectedFastIndividualProcessValue,
                          customResponse.SelectedCustomStatusValue,
                          customResponse.SelectedFinalReleaseValue, minValPay);
                    }
                    else
                    {
                        listPoco = repo.GetByMasterIDCourierDocumentStatus(requestParams.Tenant, requestParams.AppicationId, "X",
                            customResponse.SelectedBOLValue,
                            customResponse.SelectedStatusValue,
                            customResponse.SelectedTotalInvoiceValue,
                            customResponse.SelectedFastIndividualProcessValue,
                            customResponse.SelectedCustomStatusValue,
                            customResponse.SelectedFinalReleaseValue, minValPay);
                    }
                }
                if (listPoco.Count == 0)
                {
                    mess.AppendLine($"There ARE  NOT any Declarations 'R'eady to (Declaration) send  for master {requestParams.AppicationId} ");
                }

                //Send2715WhereDocumentStatusCodeIs2(mess, context, myCustomsDocumentUpdateService, listPoco);
                listPoco.Select(r => r.DeclarationId).ToList().ChunkBy(100)
    .ForEach(list100 =>
    {
        customResponse.ServerSplitDeclarationsList = list100;
        customResponse.LoggingUserId = requestParams.LoggingUserId;
        //CreateDCAInUCB2715_MsgMessagingService(customResponse, requestParams);
        var CreateDCAInUCB2715_MsgMessagingService = new CRSUtil();
        CreateDCAInUCB2715_MsgMessagingService
        .CreateCRS_DCAIn<DCAInUCB2715WithResponseContentHeader>(customResponse, (requestParams as RequestParamsBase), out string list);

    });
            }
            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();

            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }

    

        private static void Send2715WhereDocumentStatusCodeIs2(StringBuilder mess, ICustomContext context, 
            //CustomsDocumentUpdateService myCustomsDocumentUpdateService, 
            List<DeclarationCourierStatus> listPoco,string ParentId, bool IsCreateNewDocumentVersion = false)
        {
            foreach (var itemPoco in listPoco)
            {
                var customsDocumentQueryService = new CustomsDocumentQueryService(context);
                //var customsDocumentPMList = customsDocumentQueryService.GetCustomsDocumentPMListWithoutRequestedDoc(new GetTicketsParams() { ParentEntityId = itemPoco.DeclarationId, ParentEntityCode = "Declaration" }, itemPoco.Tenant);
                bool needCustomsDocumentMetaDataValues = true;
                var customsDocumentPMList = customsDocumentQueryService.GetCustomsDocumentPMListWithoutRequestedDocParentOnly(new GetTicketsParams() { ParentEntityId = itemPoco.DeclarationId, ParentEntityCode = "Declaration" }, itemPoco.Tenant
                    , needCustomsDocumentMetaDataValues);

                foreach (var customsDocumentPMItem in customsDocumentPMList)
                {
                    if (customsDocumentPMItem.DocumentStatusCode == "2" || String.IsNullOrWhiteSpace(customsDocumentPMItem.DocumentStatusCode))
                    {
                        try
                        {
                            using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
                            {
                                var context1 = CustomContext.GetContext(itemPoco.Tenant);//context each CRS TRANS
                                var myCustomsDocumentUpdateService = new CustomsDocumentUpdateService(context1, new Dictionary<string, IContext>(), itemPoco.Tenant);
                                if(IsCreateNewDocumentVersion)
                                {
                                    customsDocumentPMItem.DocumentVersion = customsDocumentPMItem.DocumentVersion + 1;
                                    customsDocumentPMItem.DocumentStatusCode = null;
                                    customsDocumentPMItem.CustomRecievedDate = null;
                                    customsDocumentPMItem.CustomsDocId = null;
                                    customsDocumentPMItem.ForceRemoveCustomsDocId = true;
                                }
                                customsDocumentPMItem.ChangeSetOp = ChangeSetOperation.Update;
                                customsDocumentPMItem.IsSendToQueue = false;
                                myCustomsDocumentUpdateService.AddPerfectCustomsDocumentMetaDataValues(customsDocumentPMItem);
                                myCustomsDocumentUpdateService.Update(customsDocumentPMItem, true);

                                customsDocumentPMItem.ChangeSetOp = ChangeSetOperation.Update;
                                customsDocumentPMItem.IsSendToQueue = true;
                                customsDocumentPMItem.ParentRequestId = ParentId;
                               myCustomsDocumentUpdateService.IgnoreSendFailure = true;
                                myCustomsDocumentUpdateService.Update(customsDocumentPMItem, true);
                                LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({itemPoco.DeclarationId})");
                                mess.AppendLine($" CreateSheetSBQMessage({itemPoco.DeclarationId})");
                                scopeNewCRS.Complete();
                            }

                        }
                        catch (System.Exception ee1)
                        {
                            LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPoco.DeclarationId}) : {ee1.Message}");
                            mess.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPoco.DeclarationId}) : {ee1.Message}");
                        }
                    }
                }
            }
        }

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCB2715WithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
        private decimal GetMinValPay(int tenant)
        {
            const decimal fallback = 75m;
            DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(tenant);
            var s = defaultValueQueryService.GetDefault("ISRAEL", "CGO_MINVAL_PAY", "NON", "NON", tenant);

            if (string.IsNullOrWhiteSpace(s)) return fallback;

            var normalized = s.Trim().Replace(",", ".");
            if (decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out var val) && val > 0)
                return val;

            return fallback;
        }

    }
}
