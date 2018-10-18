using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Deficit;

namespace Logitude.CustomsMessaging.MessageAnalyzer
{
    public class DE_NG_280_MSG11_DebtNotificationMessageAnalyzerService
     : MessageAnalyzerServiceBase<GenericRequestParams, INF_MSG_GenericResponseData, DE_NG_280_MSG11_DebtNotificationMessage, DE_NG_280_MSG11_DebtNotificationMessageResponseService>, IMessageAnalyzerService
        
    {
        public DE_NG_280_MSG11_DebtNotificationMessageAnalyzerService()
            : base("UnifreightIIG.Common.MessageLib.Deficit.DE_NG_280_MSG11_DebtNotificationMessage.xsd")
        {}


        

        public override int ResolveTenant()
        {
            var tenant = 1;
            return tenant;
        }

        public override string GetObjectTableName()
        {
            return "Customs.Deficit";
        }
        public override string GetLoggingEntityReference()
        {
            return _CustomResponse.DebtNotificationMessag.debtNotificationID.ToString();
        }
#if false
        public override ResponseData.INF_MSG_GenericResponseData AnalyzeToOverride()
        {
            //XmlSerializer serializer = new XmlSerializer(typeof(CH_NG_190_MSG1_NoticeToClient));
            DE_NG_280_MSG11_DebtNotificationMessage requestMessage = null;///(CH_NG_190_MSG1_NoticeToClient)serializer.Deserialize(memstream);
            requestMessage = _CustomResponse;
            
            var reData = new Logitude.CustomsMessaging.Common.ResponseData.INF_MSG_GenericResponseData();

            var requestParams = new Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams();
            requestParams.Tenant = ResolveTenant();
            ///InsertPhysicalCheck(physicalCheckMessage);
            var customResponseService = new DE_NG_280_MSG11_DebtNotificationMessageResponseService();
            customResponseService.Update(requestMessage, requestParams);
            if (customResponseService.MyResponseData == null)
            {
                //??
                return null;
            }
            reData.ExceptionMessage = customResponseService.MyResponseData.ExceptionMessage;
            reData.ApplicationID = customResponseService.MyResponseData.ApplicationID;
            reData.HasException = customResponseService.MyResponseData.HasException;
            reData.Succeeded = customResponseService.MyResponseData.Succeeded;
            return reData;
        }
#endif

    }
}
