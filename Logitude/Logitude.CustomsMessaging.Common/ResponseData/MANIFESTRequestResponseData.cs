using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class MANIFESTRequestResponseData : ResponseDataBase
    {
        public string ApplicationID { get; set; }
        public string ResponseStatusXML { get; set; }
    }
}
