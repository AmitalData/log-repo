using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Fault;

namespace Logitude.CustomsMessaging.MessageAnalyzer
{
    class EV_NG_8218_MSG14100_ProceduralFaultMsgAnalyzerService: MessageAnalyzerServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        EV_NG_8218_MSG14100_ProceduralFaultMsg,
        EV_NG_8218_MSG14100_ProceduralFaultMsgResponseService>, IMessageAnalyzerService
    {

        public override string GetObjectTableName()
        {
            return "Customs.Fault";// to check???? which table?
        }

        public override int ResolveTenant()
        {
            var tenant = 1;
            return tenant;
        }

        public override string GetLoggingEntityReference()
        {
            return _CustomResponse.ProceduralFaultDetails[0].proceduralFaultNumber.ToString(); // to check what to do? to take the first one?
        }

        public EV_NG_8218_MSG14100_ProceduralFaultMsgAnalyzerService()
            : base("UnifreightIIG.Common.MessageLib.Fault.EV_NG_8218_MSG14100_ProceduralFaultMsg.xsd")
        {}

        public override INF_MSG_GenericResponseData AnalyzeToOverride()
        {
            EV_NG_8218_MSG14100_ProceduralFaultMsg ProceduralFaultMessage = _CustomResponse;
            var reData = new Logitude.CustomsMessaging.Common.ResponseData.INF_MSG_GenericResponseData();

            var requestParams = new Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams();
            requestParams.Tenant = ResolveTenant();

            EV_NG_8218_MSG14100_ProceduralFaultMsgResponseService customResponseService = new EV_NG_8218_MSG14100_ProceduralFaultMsgResponseService();
            customResponseService.Update(ProceduralFaultMessage, requestParams);
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
    }
}
