using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Constraint;

namespace Logitude.CustomsMessaging.MessageAnalyzer
{
    class EV_NG_8215_MSG23002_ConstraintApprovalDecisionAnalyzerService: MessageAnalyzerServiceBase<
        GenericRequestParams,//EV_NG_8215_MSG23002_ConstraintApprovalDecisionRequestParams, //to check???
        INF_MSG_GenericResponseData,
        EV_NG_8215_MSG23002_ConstraintApprovalDecision,
        EV_NG_8215_MSG23002_ConstraintApprovalDecisionResponseService>, IMessageAnalyzerService
    {

        public override string GetObjectTableName()
        {
            return "Customs.DeclarationConstraint";
        }

        public override int ResolveTenant()
        {
            var tenant = 1;
            return tenant;
        }

        public override string GetLoggingEntityReference()
        {
            return _CustomResponse.ConstraintApprovalDecision.constraintId.ToString();
        }

        public EV_NG_8215_MSG23002_ConstraintApprovalDecisionAnalyzerService()
            : base("UnifreightIIG.Common.MessageLib.Constraint.EV_NG_8215_MSG23002_ConstraintApprovalDecision.xsd")
        {}

        public override INF_MSG_GenericResponseData AnalyzeToOverride()
        {
            EV_NG_8215_MSG23002_ConstraintApprovalDecision declarationConstraintMessage = null;
            declarationConstraintMessage = _CustomResponse;
            var tenant = 1;
            var reData = new Logitude.CustomsMessaging.Common.ResponseData.INF_MSG_GenericResponseData();

            //var requestParams = new Logitude.CustomsMessaging.Common.RequestParams.EV_NG_8215_MSG23002_ConstraintApprovalDecisionRequestParams();
            var requestParams = new Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams();
            requestParams.Tenant = ResolveTenant();

            EV_NG_8215_MSG23002_ConstraintApprovalDecisionResponseService customResponseService = new EV_NG_8215_MSG23002_ConstraintApprovalDecisionResponseService();
            customResponseService.Update(declarationConstraintMessage, requestParams);
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
