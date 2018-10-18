using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class CargoQueryRequestParams : RequestParamsBase
    {
        public string CustomsFile { get; set; }
        public string DeclarationNumber { get; set; }
        public string CargoTypeCode { get; set; }
        public string ManifestNumber { get; set; }
        public string SecondCargoID { get; set; }
        public string ThirdCargoID { get; set; }

        // added by Alaa
        public string DeclarationId { get; set; }
        public int? ConsignmentNumber { get; set; }
        public bool AutoSend { get; set; }
    }
}
