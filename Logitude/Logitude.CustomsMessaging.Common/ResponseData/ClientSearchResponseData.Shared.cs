using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class ClientSearchResponseData:ResponseDataBase
    {
        public string Message { get; set; }
        public bool CanContinue { get; set; }
        public string ResponseStatusXML { get; set; }
    }
}
