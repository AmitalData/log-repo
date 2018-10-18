using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class ConstraintAgentObjectionRequestParams : RequestParamsBase
    {
        public string ConstraintNumber { get; set; }
        public string AgentObjection { get; set; }
        public string DeclarationId { get; set; }

    }
}
