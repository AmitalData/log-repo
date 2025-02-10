using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class SendRecoverDecRequestParams : RequestParamsBase
    {
        public string CourierMasterId { get; set; }
        public string HAWB { get; set; }
        public string CourierDeclarationStatusCode { get; set; }
        public List<string> Declarations { get; set; }
        public bool IsWorkSheetFromExcel { get; set; }
        
    }
}
