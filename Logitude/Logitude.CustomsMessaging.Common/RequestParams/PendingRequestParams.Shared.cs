using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class PendingRequestParams : RequestParamsBase
    {
        public string CourierMasterId { get; set; }
        public string MAWB { get; set; }
        public string[] PendingCode { get; set; }
        public string[] DeclarationsList { get; set; }
        public bool IsWorkSheetFromExcel { get; set; }

    }
}


