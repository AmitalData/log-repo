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
    public class DF_MSG10042_CollateralAnswerApprovalMsgAnalyzerService
        : MessageAnalyzerServiceBase<GenericRequestParams, INF_MSG_GenericResponseData, COLT_NG_8213_MSG10042_CollateralAnswerApprovalMsg, DF_8213_CollateralAnswerApprovalMsgResponseService>, IMessageAnalyzerService
    {
        public DF_MSG10042_CollateralAnswerApprovalMsgAnalyzerService()
            : base("UnifreightIIG.Common.MessageLib.Collateral.DF_MSG10042_CollateralAnswerApprovalMsg.xsd")
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
            return _CustomResponse.AnswersApprovalList.ToString(); // to check with itzik about array
        }
    }
}
