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

using Simplog.Data.InfrastructureModel;
using System.Data.Common;
using System.Data.SqlClient;
using Devart.Data.Oracle;

using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Data.SqlClient;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.BL.Messaging.Customs.SignQueueBL;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UniExportBatchSend2755E_MsgResponseService: ResponseServiceBase
        //<TResponseData, TCustomResponse, TRequestParams>
        <INF_MSG_GenericResponseData, DCAInUCB2755EWithResponseContentHeader, GenericRequestParams>
    {
        public override void Update(DCAInUCB2755EWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            var mess = new StringBuilder();
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();

            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var repo = new DeclarationRepository(context);
            List<string> idList = new List<string>();

            if (customResponse.ServerSplitDeclarationsList != null && customResponse.ServerSplitDeclarationsList.Count > 0)
            {
                mess.AppendLine($"מפוצל כבר !!!");

                List<Declaration> ServerSplitDeclarationsList = repo.GetDeclarationsById(customResponse.ServerSplitDeclarationsList).ToList();
                Create2755ECRS(requestParams, mess, objectTableId, ServerSplitDeclarationsList);
            }
            else
            {
                mess.AppendLine($"ראשי - מפצל");
                mess.AppendLine($"כל ההצהרות יפוצלו.....");

                if (customResponse.ClientFilterDeclarationsList != null && customResponse.ClientFilterDeclarationsList.Count > 0)
                {
                    mess.AppendLine($"סומנו בצד הלקוח ");
                    idList = repo.GetDeclarationsById(customResponse.ClientFilterDeclarationsList)
                        .Where(d => d.Tenant == requestParams.Tenant)
                        .Select(d => d.Id).ToList();
                }
                else
                {
                    mess.AppendLine($"GetDeclarationsByFilters");
                    idList = GetDeclarationsByFilters(context, customResponse);
                    mess.AppendLine($"Found {idList.Count} declarations");
                }
                if (idList.Count == 0)
                {
                    mess.AppendLine($"There ARE  NOT any Declarations 'R'eady to (Declaration) send  for master {requestParams.AppicationId} ");
                }
                if (idList.Count > 0)
                {
                    idList.ChunkBy(100)
                       .ForEach(list100 =>
                       {
                           customResponse.ServerSplitDeclarationsList = list100;
                           customResponse.LoggingUserId = requestParams.LoggingUserId;

                           var createDCAInUCB2755E_MsgMessagingService = new CRSUtil();
                           createDCAInUCB2755E_MsgMessagingService.CreateCRS_DCAIn<DCAInUCB2755EWithResponseContentHeader>(customResponse, (requestParams as RequestParamsBase), out string list);

                       });
                }
            }

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }

        private List<string> GetDeclarationsByFilters(ICustomContext MyContext, DCAInUCB2755EWithResponseContentHeader customResponse)
        {
            customResponse.QueryOperations.GetAll = true;
            customResponse.QueryOperations.SortByColumnName = null;
            DeclarationListQueryService declarationQuery = new DeclarationListQueryService(MyContext);
            List<string> declarationIds = declarationQuery.GetByFilters(customResponse.QueryOperations, customResponse.tenant).Select(d => d.Id).ToList();

            if (customResponse.ExcludedIds != null && customResponse.ExcludedIds.Count > 0)
            {
                declarationIds = declarationIds.Where(id => !customResponse.ExcludedIds.Contains(id)).ToList();
            }
            return declarationIds;
        }

        private static void Create2755ECRS(GenericRequestParams requestParams, StringBuilder mess, string objectTableId, List<Declaration> listPoco)
        {
            var listDeclarationIdCreateCRS = new List<string>();

            foreach (var itemPM in listPoco)
            {
                try
                {
                    using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
                    {
                        var requestParams2755E = new GenericRequestParams()
                        {
                            AppicationId = itemPM.Id,
                            InterfaceTypeCode = "2755E",
                            RequestVIA = SendRequestVIA.WebServiceBatch,
                            Tenant = requestParams.Tenant,
                            //RequestName = "Declaration Request",
                            //ResponseName = "Declaration Response",

                            LoggingEnabled = true,
                            LoggingObjectTableId = objectTableId,
                            LoggingEntityId = itemPM.Id,
                            LoggingEntityReference = itemPM.DeclarationNumber,
                            LoggingUserId = requestParams.LoggingUserId,
                            ParentId = requestParams.CustomsRequestsSheetId,
                        };

                        SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams2755E, false);
                        LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({itemPM.Id})");
                        mess.AppendLine($" CreateSheetSBQMessage({itemPM.Id})");

                        scopeNewCRS.Complete();
                    }
                    listDeclarationIdCreateCRS.Add(itemPM.Id);

                }
                catch (System.Exception ee1)
                {

                    LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.Id}) : {ee1.Message}");
                    mess.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.Id}) : {ee1.Message}");
                }
            }
        }

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCB2755EWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
