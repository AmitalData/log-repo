
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

namespace Logitude.Customs.BL.Messaging.ILSWS

{
    public class CourierECSWSTHRMessageResponseService
    {


        public AnalyzeResultModel AnalyzeQResponse(int tenant, CourierSWSHAWBResponse CourierSWSHAWBResponse, AnalyzeResultModel res)

        {

            if (CourierSWSHAWBResponse == null)
            {
                throw new Exception("(CourierSWSHAWBResponse == null)");
            }
            LogMessagingUtil.Instance.AppendLine($"AnalyzeResponse(StatusCode={CourierSWSHAWBResponse.StatusCode},{CourierSWSHAWBResponse.ErrorDescription})");
            var context = CustomContext.GetContext(tenant);
            var qs = new DeclarationQueryService(tenant);
            var idList = qs.GetListByCourierHAWB(CourierSWSHAWBResponse.CourierHawbNumber, tenant);
            string decID = null;
            if (idList.Count == 1)
            {
                decID = idList.FirstOrDefault();
                res.EntityReference = qs.GetCustomFileNoByDeclarationId(idList.FirstOrDefault(), tenant);
                res.EntityID =decID;
            }
            var myDeclarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
            var declarationCourierStatusQueryServicePM = myDeclarationCourierStatusQueryService.GetSingle(decID, true, false);
            res.EntityID = declarationCourierStatusQueryServicePM.CourierMasterId;
            declarationCourierStatusQueryServicePM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            switch (CourierSWSHAWBResponse.StatusCode)
            {
                case "1"://45997
                    {
                        //declarationPM.MamanStatusCode = "1";
                        declarationCourierStatusQueryServicePM.StorageSiteStatusCode = "1";
                    }
                    break;
                default:
                    //declarationPM.MamanStatusCode = "2";//45997
                    declarationCourierStatusQueryServicePM.StorageSiteStatusCode = "2";
                    break;
            }


            declarationCourierStatusQueryServicePM.StorageSiteErrorText = CourierSWSHAWBResponse.StatusCode + "," + CourierSWSHAWBResponse.ErrorDescription;

            using (var scope = TransactionFactory.GetNewTransaction())
            {
                var myDeclarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                myDeclarationCourierStatusUpdateService.Update(declarationCourierStatusQueryServicePM, true);
                scope.Complete();
            }
            res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.D;
            return res;
        }

        public void AnalyzeResponse(CourierWEBAPICommSettings settings, string webAPIResultString)
        {
            throw new Exception("use  SetInAnalyzeQResponseService by @intrface.ResponseCode");
            var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
            var def = customsPartnerFtpDetails.GetAllInterfaceDetails().First(r => r.Code == CustomsPartnerFtpDetails.InterfaceName_ECSWSTHR_IN);
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
