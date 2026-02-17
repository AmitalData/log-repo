using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class DeclarationRestoreRequestParams : GenericRequestParams
    {
        public string CustomsFile { get; set; }
        public string DeclarationNumber { get; set; }
        public string DeclarationId { get; set; }
    }
}
