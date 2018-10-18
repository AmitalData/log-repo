using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.Server.Tools.Models;

namespace Logitude.Customs.BL.Messaging.Amital.CustomFile
{
    public class AmitalCustomFileCommunicationModel : AmitalCommunicationModelBase
    {
        public AmitalCustomFileCommunicationModel()
            : base(OperationMethod.DataAccess, "CWSFLOGIFILE", "DeclarationUpsertPut")
        {

        }
        public LOGICUSTFILE LogitudeFile { get; set; }
        
    }
}
