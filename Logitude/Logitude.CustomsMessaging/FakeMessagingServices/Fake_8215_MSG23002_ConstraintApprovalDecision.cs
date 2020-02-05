using Logitude.Customs.Data.EntityPOCOs;
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

        private RequestContentHeader _requestContentHeader;
        private EV_NG_8215_MSG23002_ConstraintApprovalDecisionConstraintApprovalDecision _constraintApprovalDecision;
        public Fake_8215_MSG23002_ConstraintApprovalDecision()
        {
            this._requestContentHeader = new RequestContentHeader();
            this._constraintApprovalDecision = new EV_NG_8215_MSG23002_ConstraintApprovalDecisionConstraintApprovalDecision();
        }
        public EV_NG_8215_MSG23002_ConstraintApprovalDecisionConstraintApprovalDecision GetConstraintApproval()
        {

            return _constraintApprovalDecision;
        }
        public EV_NG_8215_MSG23002_ConstraintApprovalDecisionConstraintApprovalDecision GetConstraintDeny()
        {
            return _constraintApprovalDecision;
        }
        public RequestContentHeader GetRequestContentHeader()
        {
            _requestContentHeader.TransmitionDateTime = DateTime.Now;
            _requestContentHeader.SenderID = 0;
            return _requestContentHeader;

        }
        public 
    }
}
