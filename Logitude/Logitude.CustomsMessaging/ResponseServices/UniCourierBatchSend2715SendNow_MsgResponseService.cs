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
    public class UniCourierBatchSend2715SendNow_MsgResponseService : ResponseServiceBase
        <INF_MSG_GenericResponseData, DCAInUCB2715SendNowWithResponseContentHeader, GenericRequestParams>
    {
        public override void Update(DCAInUCB2715SendNowWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            var mess = new StringBuilder();
            var context = CustomContext.GetContext(requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();
            
            if (customResponse.ServerSplitDocumentList != null && customResponse.ServerSplitDocumentList.Count > 0)
            {
                mess.AppendLine($"מפוצל כבר !!!");
                foreach (var item in customResponse.ServerSplitDocumentList)
                {
                    mess.AppendLine($"ResolveAndReQueue "+ item);
                    MessagingServiceFactoryHelper.ResolveAndReQueue("2715", requestParams.Tenant, item);
                }
            }
            else
            {
                mess.AppendLine($"ראשי - מפצל");
                mess.AppendLine($"כל המסמכים יפוצלו.....");
                var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                CourierDeclarationQueryService _DeclarationQueryService = new CourierDeclarationQueryService(context);
                var customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(context);
                var declarations = _DeclarationQueryService.GetDeclarationIdsByCourierMasterID(customResponse.CourierMasterId, requestParams.Tenant);
                var listPoco = customsRequestsSheetQueryService.GetRequestByInterfaceTypeCodeAndStatus(requestParams.Tenant, "2715", objectTableId, declarations, "1");
                
                if (listPoco.Count == 0)
                {
                    mess.AppendLine($"There ARE  NOT any Documents 'R'eady to (Declaration) send  for master {requestParams.AppicationId} ");
                }

                listPoco.Select(r => r.Id).ToList().ChunkBy(100)
                    .ForEach(list100 =>
                    {
                        customResponse.ServerSplitDocumentList = list100;
                        customResponse.LoggingUserId = requestParams.LoggingUserId;
                        var CreateDCAInUCB2715SendNow_MsgMessagingService = new CRSUtil();
                        CreateDCAInUCB2715SendNow_MsgMessagingService.CreateCRS_DCAIn<DCAInUCB2715SendNowWithResponseContentHeader>(customResponse, (requestParams as RequestParamsBase));

                    });
            }
            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }

    

       public override INF_MSG_GenericResponseData GetResponse(DCAInUCB2715SendNowWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

    }
}
