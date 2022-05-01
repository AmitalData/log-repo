using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class MultiUpdateRequestParams : RequestParamsBase
    {
        public string Declarationid { get; set; }
        public string ProcessTypeCode { get; set; }
        public string TaxExemptCode { get; set; }

        public string ClassificationCode { get; set; }

        public string[] DeclarationIds { get; set; }
        public string CourierMasterId { get; set; }
        public List<string> allWithoutdeclarationIdsList { get; set; }
        public bool checkboxAll { get; set; }
    }
}
