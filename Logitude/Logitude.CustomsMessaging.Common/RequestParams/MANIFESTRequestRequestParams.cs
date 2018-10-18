using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class MANIFESTRequestRequestParams : RequestParamsBase
    {
        public string DeclarationId { get; set; }
        public string ImportManifest { get; set; }
    }
}