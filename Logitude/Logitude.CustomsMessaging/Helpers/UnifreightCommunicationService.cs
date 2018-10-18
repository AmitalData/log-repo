using Logitude.CustomsMessaging.RequestParams;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Logitude.CustomsMessaging.Helpers
{
    public class UnifreightCommunicationService
    {
        internal static void PushUnifreightQueue(CommunicationsParams communicationsParams)
        {
            string communicationLogId = Communications.AddCommunicationLog(communicationsParams);
            Communications.SendCommunicationLogMessageToQueue("Amitalqueue", communicationLogId, communicationsParams.Tenant);
        }
    }
}
