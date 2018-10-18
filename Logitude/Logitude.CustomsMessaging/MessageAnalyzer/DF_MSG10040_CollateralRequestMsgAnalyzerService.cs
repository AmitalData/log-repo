using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Collateral;

namespace Logitude.CustomsMessaging.MessageAnalyzer
{
    public class DF_MSG10040_CollateralRequestMsgAnalyzerService
        : MessageAnalyzerServiceBase<GenericRequestParams, INF_MSG_GenericResponseData,COLT_NG_8211_MSG10040_CollateralRequestMsg, DF_8211_CollateralRequestMsgResponseService>, IMessageAnalyzerService
    {
        public DF_MSG10040_CollateralRequestMsgAnalyzerService()
            : base("UnifreightIIG.Common.MessageLib.Collateral.DF_MSG10040_CollateralRequestMsg.xsd")
        {}

        public override string GetObjectTableName()
        {
            return "Customs.CustomsCollateral";
        }

        public override int ResolveTenant()
        {
            var tenant = 1;
            return tenant;
        }

        public override string GetLoggingEntityReference()
        {
            return _CustomResponse.CollateralRequestDetails.ToString(); // to check with itzik about array
        }
    }
}
