using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
     public class ConstraintApprovalRequestParams: RequestParamsBase
    {
         public string ConstraintNumber { get; set; }
         public string ConstraintTypeName { get; set; }
         public string ConstraintStatusName { get; set; }
         public string AgentExplanation { get; set; }
         public string DeclarationId { get; set; }
         public string ApprovalDecision {get; set;}

    }
}
