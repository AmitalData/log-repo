using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class FaultProceduralRequestParams : RequestParamsBase
    {
        public string Client { get; set; }
        public string ImporterExternalID { get; set; }
        public string CustomsFile { get; set; }
        public string DeclarationNumber { get; set; }
        public string ProceduralFaultCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string AgentExternalID { get; set; }

        public string ImporterId { get; set; }
        public string ImporterCode { get; set; }
        public string ImporterName { get; set; }
        public string PredefinedValue { get; set; }

        public string CustomerId { get; set; }//14511
    }
}

