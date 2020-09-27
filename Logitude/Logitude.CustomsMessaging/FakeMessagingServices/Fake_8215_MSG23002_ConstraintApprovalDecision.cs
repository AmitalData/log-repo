using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Constraint;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    class Fake_8215_MSG23002_ConstraintApprovalDecision
    {

        private dynamic params1;
        private RequestContentHeader _requestContentHeader;
        private DeclarationPM _dec;
        private EV_NG_8215_MSG23002_ConstraintApprovalDecisionConstraintApprovalDecision _decision;
        internal EV_NG_8215_MSG23002_ConstraintApprovalDecision GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParamsData.Tenant);
            _dec = declarationQueryService.GetSingle(requestParamsData.AppicationId, false, false);
            var _constraintApprovalDecision = new EV_NG_8215_MSG23002_ConstraintApprovalDecision();
            _constraintApprovalDecision.RequestContentHeader = GetRequestContentHeader();
            if (requestParamsData.TestCase.Code == "8215SincroConstraintApprove")
            {
                _constraintApprovalDecision.ConstraintApprovalDecision = GetConstraintApproval(requestParamsData);
            }
            else
            {
                _constraintApprovalDecision.ConstraintApprovalDecision = GetConstraintDeny(requestParamsData);
            }

            return _constraintApprovalDecision;
        }

        public EV_NG_8215_MSG23002_ConstraintApprovalDecisionConstraintApprovalDecision GetConstraintApproval(GenericRequestParams requestParamsData)
        {
            _decision = new EV_NG_8215_MSG23002_ConstraintApprovalDecisionConstraintApprovalDecision();
            params1 = JObject.Parse(requestParamsData.TestCase.Param1);
            DeclarationConstraintQueryService declarationConstraintQueryService = new DeclarationConstraintQueryService(requestParamsData.Tenant);
            _decision.approvalNote = "נבדק";
            _decision.approvalDecision = 2;
            if (params1.constraintId != null)
            {
                    _decision.constraintId = Int32.Parse(Convert.ToString(params1.constraintId));
            }
            else
            {
                string constraintNumber = declarationConstraintQueryService.GetDeclarationConstraintByDeclarationId(requestParamsData.AppicationId);
                _decision.constraintId = int.Parse(constraintNumber);
            }
            _decision.ConstraintStatus = 5;
            _decision.LeadDocumentIDNum = _dec.DeclarationNumber;
            _decision.approvalUserNam = "FAKE-APPROV";
            _decision.approvalAuthorityDateSpecified = true;
            _decision.approvalAuthorityDate = DateTime.Now;
            return _decision;
        }
        public EV_NG_8215_MSG23002_ConstraintApprovalDecisionConstraintApprovalDecision GetConstraintDeny(GenericRequestParams requestParamsData)
        {
            _decision = new EV_NG_8215_MSG23002_ConstraintApprovalDecisionConstraintApprovalDecision();
            params1 = JObject.Parse(requestParamsData.TestCase.Param1);
            DeclarationConstraintQueryService declarationConstraintQueryService = new DeclarationConstraintQueryService(requestParamsData.Tenant);
            _decision.approvalNote = "FAKE NOTE";
            _decision.approvalDecision = 1;
            if (params1.constraintId != null)
            {
                _decision.constraintId = Int32.Parse(Convert.ToString(params1.constraintId));
            }
            else
            {
                string constraintNumber = declarationConstraintQueryService.GetDeclarationConstraintByDeclarationId(requestParamsData.AppicationId);
                _decision.constraintId = int.Parse(constraintNumber);
            }
            _decision.ConstraintStatus = 4;
            _decision.LeadDocumentIDNum = _dec.DeclarationNumber;
            _decision.approvalUserNam = "FAKE-APPROV";
            _decision.approvalAuthorityDateSpecified = true;
            _decision.approvalAuthorityDate = DateTime.Now;
            return _decision;
        }
        public RequestContentHeader GetRequestContentHeader()
        {
            _requestContentHeader = new RequestContentHeader
            {
                TransmitionDateTime = DateTime.Now,
                SenderID = 0
            };
            return _requestContentHeader;

        }
    }
}
