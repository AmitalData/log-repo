using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class DeclarationRestoreResponseData : INF_MSG_GenericResponseData
    {
        public string ResponseStatusXML { get; set; }
        public bool IsShowUserMessage { get; set; }
    }
}

