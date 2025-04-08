using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.CustomsMessaging.Tasks
{
    public class UpdateClosedTables : ICustomsUpdateClosedTables
    {
        public void StartRun(string taskId, int seedDefaultTenant)
        {
            try
            {
                SystemTableRequestParams systemTableRequestParams = new SystemTableRequestParams();
                systemTableRequestParams.Tenant = seedDefaultTenant;
                systemTableRequestParams.UpdateAllTables = true;
                LoadCustomClosedTables.UpdateAllClosedTables(systemTableRequestParams.Tenant, systemTableRequestParams);
            }
            catch (Exception ex)
            {
                LogMessagingUtil.Instance.AppendLine($"RunPerTenant({seedDefaultTenant})");
                LogMessagingUtil.Instance.AppendLine($"ex({ex.InnerException})");

            }


        }
    }
}
