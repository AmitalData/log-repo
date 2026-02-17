using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam : RequestParamsBase
    {
        public string DeclaretionId { get; set; }
        public string DocumentsFilingId { get; set; }
        public string DocumentsTicketId { get; set; }   
    }
}
