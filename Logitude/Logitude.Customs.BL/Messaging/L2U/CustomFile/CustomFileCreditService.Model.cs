using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.L2U.CustomFile
{
    public partial class CustomFileCreditService
    {
        public class CustomFileCreditModel
        {
            public string LoggingUserId { get; set; }
            public int Tenant { get; set; }
            public string AppicationId { get; set; }
            public string Mode { get; set; } // Check or Transfer
        }
    }
}
