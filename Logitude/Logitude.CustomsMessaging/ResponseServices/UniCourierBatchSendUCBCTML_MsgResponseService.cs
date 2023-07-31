
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.BL.BL;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Messaging.ILOVS;
using Logitude.Customs.BL.Messaging.ILSWS;
using Logitude.Customs.BL.Messaging.Maman;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.CustomsMessaging.Utils;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UniCourierBatchSendUCBCTML_MsgResponseService : ResponseServiceBase
        //<TResponseData, TCustomResponse, TRequestParams>
        <INF_MSG_GenericResponseData, DCAInUCBCTMLWithResponseContentHeader, GenericRequestParams>
    {
        private Customs.Def.EntityPMs.DeclarationPM _MyDeclarationPM;
        public override void Update(DCAInUCBCTMLWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            var mess = new StringBuilder();
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            
            this.MyResponseData = new INF_MSG_GenericResponseData();

            var amitalContext = AmitalContext.GetContext(requestParams.Tenant);
            var myGDFDATAQueryService = new GDFDATAQueryService(amitalContext);
            var def = myGDFDATAQueryService.GetSingle("ISRAEL", "CGO_CUST_MAMAN", "NON", "NON", false, true);
            def.DEFDATA = def.DEFDATA ?? "";

            var port2SendList = new List<string>();//&& declaration.Consignments.FirstOrDefault().StorageSiteCode == "ILOVL"
            if (def.DEFDATA.Contains("ILMMN")) // Maman
            {
                port2SendList.Add("ILMMN");
            }
            if (def.DEFDATA.Contains("ILOVL")) // OVS
            {
                port2SendList.Add("ILOVL");
            }
            if (def.DEFDATA.Contains("ILSWS")) // OVS
            {
                port2SendList.Add("ILSWS");
            }



            if (customResponse.ServerSplitDeclarationsList != null && customResponse.ServerSplitDeclarationsList.Count > 0)
            {

                mess.AppendLine($"מפוצל כבר !!!");
                var DeclarationIdStorageSiteCodeList= customResponse.ServerSplitDeclarationsList.Select(keyVal =>
                new KeyValuePair<string, string>(keyVal.Split(',')[0], keyVal.Split(',')[1])).ToList();
                
                foreach (var itemDeclarationIdStorageSiteCode in DeclarationIdStorageSiteCodeList)
                {

                    BuildQueueSendWebAPIMethod(requestParams, mess, def, itemDeclarationIdStorageSiteCode);

                }

            }
            else
            {


                var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
                var qs = new DeclarationCourierStatusQueryService(context);
                var listKeyValuePair = new List<KeyValuePair<string, string>>();


                if (port2SendList.Count == 0)
                {
                    mess.AppendLine($"UniCourierBatchSendUCBCTML_MsgResponseService:default CGO_CUST_MAMAN no ILMMN/+ILOVL !!!!!!!!!!!!!! ");
                }
                else
                {
                    if (customResponse.IsWorkSheetFromExcel)
                    {
                        listKeyValuePair = qs.GetFromExcelStorageSiteCode(requestParams.Tenant, requestParams.LoggingUserId, port2SendList);
                    }
                    else
                    {
                        listKeyValuePair = qs.GetByMasterIDStorageSiteCode(requestParams.Tenant, requestParams.AppicationId, port2SendList);
                    }

                }

                if (listKeyValuePair.Count == 0)
                {
                    mess.AppendLine($"There ARE  NOT any Declarations GetByMasterIDStorageSiteCode {requestParams.AppicationId } where storage {def.DEFDATA}");
                }
                var list2split = new List<KeyValuePair<string, string>>();

                var decSend = new HashSet<string>();
                foreach (var itemDeclarationIdStorageSiteCode in listKeyValuePair)
                {

                    if (decSend.Contains(itemDeclarationIdStorageSiteCode.Key))//GetByMasterIDStorageSiteCode can return few by consigenment !
                    {
                        continue;
                    }
                    decSend.Add(itemDeclarationIdStorageSiteCode.Key);

                    ///BuildQueueSendWebAPIMethod(requestParams, mess, def, itemDeclarationIdStorageSiteCode);

                    list2split.Add(itemDeclarationIdStorageSiteCode);

                }
                list2split.ToList().ChunkBy(100)
        .ForEach(list100 =>
        {
            customResponse.ServerSplitDeclarationsList = 
            list100.Select(r => r.Key + "," + r.Value).ToList();
            customResponse.LoggingUserId = requestParams.LoggingUserId;
            //CreateDCAInUCB1170_MsgMessagingService(customResponse, requestParams);
            var CreateDCAInUCB1170_MsgMessagingService = new CRSUtil();
            CreateDCAInUCB1170_MsgMessagingService
            .CreateCRS_DCAIn<DCAInUCBCTMLWithResponseContentHeader>(customResponse, (requestParams as RequestParamsBase));

        });

            }
            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();

            //this.MyRequestSheetParam.RequestDescription = "Build Custom Zip File";
            ///LogitudeSettings.HandleBuildObjectTablesZipFilesData_Inject(false, true);

            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }

        private object KeyValuePair<T1, T2>()
        {
            throw new NotImplementedException();
        }

        private static void BuildQueueSendWebAPIMethod(GenericRequestParams requestParams, StringBuilder mess, Unifreight.BL.EntityPMs.UGenerated.GDFDATAPM def, KeyValuePair<string, string> itemDeclarationIdStorageSiteCode)
        {
            try
            {
                string response = "";
                 using (var scope = TransactionFactory.GetNewTransaction())
                {
                    FeatureQuery featureQuery = new FeatureQuery();
                    var features = featureQuery.GetAllowedFeaturesForLoggedUser(AuthenticationUtil.ResolveUserId(requestParams.Tenant), requestParams.Tenant);
                    var feature = features.Features.FirstOrDefault(x => x.Code == "CancelOldCommunication");
                    if (feature != null)
                    {
                        try
                        {
                            var cancelOldCommunicationLogs = new CancelOldCommunicationLogs();
                            cancelOldCommunicationLogs.CancelOldECTHRDataMaman(requestParams.Tenant, itemDeclarationIdStorageSiteCode.Key);
                        }
                        catch
                        {

                        }
                    }
                    if (def.DEFDATA.Contains("ILMMN") && itemDeclarationIdStorageSiteCode.Value == "ILMMN") // Maman
                    {
                        var courierGWMessageECTHRDataMamanService = new CourierGWMessageECTHRDataMamanRequestService();
                        response = courierGWMessageECTHRDataMamanService.BuildQueueSendWebAPI(itemDeclarationIdStorageSiteCode.Key, requestParams.Tenant);
                    }
                    else if (def.DEFDATA.Contains("ILOVL") && itemDeclarationIdStorageSiteCode.Value == "ILOVL") // OVS
                    {
                        var courierGWMessageECTHRDataMamanService = new CourierOVSECTHMessageRequestService();
                        response = courierGWMessageECTHRDataMamanService.BuildQueueSendWebAPI(itemDeclarationIdStorageSiteCode.Key, requestParams.Tenant);
                    }
                    else if (def.DEFDATA.Contains("ILSWS") && itemDeclarationIdStorageSiteCode.Value == "ILSWS") // OVS
                    {
                        var courierECSWSTHRMessageRequestService = new CourierECSWSTHRMessageRequestService();
                        response = courierECSWSTHRMessageRequestService.BuildQueueSendWebAPI(itemDeclarationIdStorageSiteCode.Key, requestParams.Tenant);
                    }

                    scope.Complete();
                }
                mess.AppendLine($" BuildQueueSendWebAPI({itemDeclarationIdStorageSiteCode.Key}) respons {response}");

            }
            catch (System.Exception ee1)
            {

                LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({itemDeclarationIdStorageSiteCode.Key}) : {ee1.Message}");
                mess.AppendLine($"Exception!!!CreateSheetSBQMessage({itemDeclarationIdStorageSiteCode.Key}) : {ee1.Message}");
            }
        }

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCBCTMLWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }


    }
}
