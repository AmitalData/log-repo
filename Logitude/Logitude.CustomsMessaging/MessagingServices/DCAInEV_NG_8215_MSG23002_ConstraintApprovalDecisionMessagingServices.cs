using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Constraint;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInEV_NG_8215_MSG23002_ConstraintApprovalDecisionMessagingServices : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        EV_NG_8215_MSG23002_ConstraintApprovalDecision,
        DCAInCustomRequestService,
        EV_NG_8215_MSG23002_ConstraintApprovalDecisionResponseService, DCAInRequestHeader>
    {
        protected override EV_NG_8215_MSG23002_ConstraintApprovalDecision CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "8215"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(EV_NG_8215_MSG23002_ConstraintApprovalDecision customsResponse)
        {
            var tableName = "Customs.DeclarationConstraint";

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                //LoggingEntityId = customsResponse.ResponseContentHeader.ApplicationID.ToString(), // to check with itzik about array
            };
            return myGenericRequestParams;
        }

    }
}
