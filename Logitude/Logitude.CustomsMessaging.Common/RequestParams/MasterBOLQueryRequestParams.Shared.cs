using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class MasterBOLQueryRequestParams : RequestParamsBase
    {
        public string CustomFileNo { get; set; }
        public string Date { get; set; }
        public string MasterBillOfLading { get; set; }
        public string InternalIdentifier { get; set; }
        public bool ReturnAllInernalCargos { get; set; }
        public bool ExactMatch { get; set; }
        public string DeclarationId { get; set; }
    }
}
