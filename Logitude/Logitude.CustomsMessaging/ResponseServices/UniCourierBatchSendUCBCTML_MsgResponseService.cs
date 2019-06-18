
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Messaging.ILOVS;
using Logitude.Customs.BL.Messaging.Maman;
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

            var list = new List<string>();//&& declaration.Consignments.FirstOrDefault().StorageSiteCode == "ILOVL"
            if (def.DEFDATA.Contains("ILMMN") ) // Maman
            {
                list.Add("ILMMN");
            }
            if (def.DEFDATA.Contains("ILOVL") ) // OVS
            {
                list.Add("ILOVL");
            }



            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
            var qs = new DeclarationCourierStatusQueryService(context);
            var listPM = new List<KeyValuePair<string, string>>();


            if (list.Count == 0)
            {
                mess.AppendLine($"UniCourierBatchSendUCBCTML_MsgResponseService:default CGO_CUST_MAMAN no ILMMN/+ILOVL !!!!!!!!!!!!!! ");
            }
            else
            {
                listPM = qs.GetByMasterIDStorageSiteCode(requestParams.Tenant, requestParams.AppicationId, list);

            }

            if (listPM.Count == 0)
            {
                mess.AppendLine($"There ARE  NOT any Declarations GetByMasterIDStorageSiteCode {requestParams.AppicationId } where storage {def.DEFDATA}");
            }
            var decSend = new HashSet<string>();
            foreach (var itemPM in listPM)
            {

                if (decSend.Contains(itemPM.Key))//GetByMasterIDStorageSiteCode can return few by consigenment !
                {
                    continue;
                }
                decSend.Add(itemPM.Key);
                try
                {
                    string response = "";
                    using (var scope= TransactionFactory.GetNewTransaction())
                    {

                        if (def.DEFDATA.Contains("ILMMN") && itemPM.Value == "ILMMN") // Maman
                        {
                            var courierGWMessageECTHRDataMamanService = new CourierGWMessageECTHRDataMamanRequestService();
                            response = courierGWMessageECTHRDataMamanService.BuildQueueSendWebAPI(itemPM.Key, requestParams.Tenant);
                        }
                        else if (def.DEFDATA.Contains("ILOVL") && itemPM.Value == "ILOVL") // OVS
                        {
                            var courierGWMessageECTHRDataMamanService = new CourierOVSECTHMessageRequestService();
                            response = courierGWMessageECTHRDataMamanService.BuildQueueSendWebAPI(itemPM.Key, requestParams.Tenant);
                        }

                        scope.Complete();
                    }
                    mess.AppendLine($" BuildQueueSendWebAPI({itemPM.Key}) respons {response}");

                }
                catch (System.Exception ee1)
                {

                    LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.Key}) : {ee1.Message}");
                    mess.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPM.Key}) : {ee1.Message}");
                }
            }

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();

            //this.MyRequestSheetParam.RequestDescription = "Build Custom Zip File";
            ///LogitudeSettings.HandleBuildObjectTablesZipFilesData_Inject(false, true);

            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCBCTMLWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }


    }
}
