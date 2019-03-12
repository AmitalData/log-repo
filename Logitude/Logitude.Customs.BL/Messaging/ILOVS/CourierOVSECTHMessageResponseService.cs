    
//https://docs.google.com/document/d/1bFMdrDnByDpvLcvE9H5eOfCAzbVdeoUypbzhwxbr0Po/edit#heading=h.hjpcmz7krlpn

using Logitude.BL.Helpers;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue;
using Logitude.Customs.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.Utils;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.ILOVS
{
    public class CourierOVSECTHMessageResponseService
       // : IWebAPIMessage2MamanAnalyzer
    //: WebAPIMessage2MamanBase///using  by SendWEBAPIMessage2MamanWRWR
    {


        //public void AnalyzeResponse(Courier2MamanCommSettings settings, GWMessageECTHRData  courierOVSHAWBResponse)
        public void AnalyzeQResponse(CourierWEBAPICommSettings settings, string webAPIResultString)

        {

            var courierOVSHAWBResponse = ProxyUtil.JsonConvertDeserializeTyped<CourierOVSHAWBResponse>(webAPIResultString);
            if (courierOVSHAWBResponse == null)
            {
                throw new Exception("(courierOVSHAWBResponse == null)");
            }
            LogMessagingUtil.Instance.AppendLine($"AnalyzeResponse(StatusCode={courierOVSHAWBResponse.StatusCode},{courierOVSHAWBResponse.ErrorDescription})");
            var context = CustomContext.GetContext(settings.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myCourierMasterQueryService = new CourierMasterQueryService(context);
            var declarationPM = myDeclarationQueryService.GetSingle(settings.DeclarationId, false, false);
            declarationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;



            switch (courierOVSHAWBResponse.StatusCode)
            {
                case "1"://45997
                    {
                        declarationPM.MamanStatusCode = "1";
                    }
                    break;
                default:
                    declarationPM.MamanStatusCode = "2";//45997
                    break;
            }


            declarationPM.MamanErrorXml = courierOVSHAWBResponse.StatusCode + "," + courierOVSHAWBResponse.ErrorDescription;

            using (var scope = TransactionFactory.GetNewTransaction())
            {
                var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), settings.Tenant);
                myDeclarationUpdateService.Update(declarationPM, true);
                scope.Complete();
            }
        }

        public void AnalyzeResponse(CourierWEBAPICommSettings settings, string webAPIResultString)
        {
            throw new Exception("use  SetInAnalyzeQResponseService by @intrface.ResponseCode");
            var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
            var def =customsPartnerFtpDetails.GetAllInterfaceDetails().First(r => r.Code == CustomsPartnerFtpDetails.InterfaceName_ECOVSTHR_Response);
            var commSetting = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(settings);
            var analyzeQueueUtil = new AnalyzeQueueUtil();
            var new_analyze = analyzeQueueUtil
               .SaveMessageToAnalyzeQueue("", Encoding.UTF8.GetBytes(webAPIResultString), settings.Tenant,
               commSetting, def,
               new AnalyzeResultModel()
               {
                   EntityID = settings.DeclarationId,
                   ObjectTableID = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),

               });

            LogMessagingUtil.Instance.AppendLine($"new_analyze  CommunicationLogId = {new_analyze.CommunicationLogId}");

        }
    }


}
