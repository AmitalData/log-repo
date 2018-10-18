using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.DCAParams
{
    public class DCAServerUploadResponse
    {
        public string  ComAmitalID{get;set;}
        public string ServerJobID { get; set; }
        public string FileName { get; set; }

        public string DcaMessage { get; set; }

        //public Logitude.Server.Tools.ExternalServices.DCAParams DCAParams { get; set; }
    }
}
