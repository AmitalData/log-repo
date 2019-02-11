using Logitude.Customs.BL.CloseTables;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue
{
    public class SetInAnalyzeQResponseService : IWebAPIMessage2MamanAnalyzer
    //: WebAPIMessage2MamanBase///using  by SendWEBAPIMessage2MamanWRWR
    {
        

        public void AnalyzeResponse(CourierWEBAPICommSettings settings, string webAPIResultString)
        {
            var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
            var requestInterface = customsPartnerFtpDetails.GetAllInterfaceDetails().First(r => r.Code ==
            //CustomsPartnerFtpDetails.InterfaceName_ECOVSTHR_Response
            settings.MessageCode
            );
            var responseInterface = customsPartnerFtpDetails.GetAllInterfaceDetails().First(r => r.Code == requestInterface.ResponseCode);
            
            var commSetting = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(settings);
            var analyzeQueueUtil = new AnalyzeQueueUtil();
            var new_analyze = analyzeQueueUtil
               .SaveMessageToAnalyzeQueue("", Encoding.UTF8.GetBytes(webAPIResultString), settings.Tenant,
               commSetting, responseInterface,
               new AnalyzeResultModel()
               {
                   EntityID = settings.DeclarationId,
                   ObjectTableID = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),

               });

            LogMessagingUtil.Instance.AppendLine($"new_analyze  CommunicationLogId = {new_analyze.CommunicationLogId}");

        }
    }


}
