using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.BL.GlobalModel.Tools.DataMapping
{
    public class TenantManagmentPrivateLablesMapping
    {
        public static void MapEntity(TenantManagmentPrivateLabelsPM entityPM, TenantManagmentPrivateLabels entityPOCO, bool isNewEntity)
        {
            if (isNewEntity)
            {
                //entityPOCO.Id = entityPM.Id;
            }
            entityPOCO.PrivateLabelName = entityPM.PrivateLabelName;
            entityPOCO.PrivateLabelShortName = entityPM.PrivateLabelShortName;
            entityPOCO.PrivateLabelUrl = entityPM.PrivateLabelUrl;
            entityPOCO.ReceiveAllStatuses = entityPM.ReceiveAllStatuses;
            entityPOCO.MainLogo = entityPM.MainLogo;
            entityPOCO.SmallLogo = entityPM.SmallLogo;
            entityPOCO.InActive = entityPM.InActive;
            entityPOCO.HybridPartnerId = entityPM.HybridPartnerId;
            entityPOCO.ContactUsEmail = entityPM.ContactUsEmail;
            entityPOCO.SearchFields = entityPM.PrivateLabelName + "," + entityPM.PrivateLabelShortName + "," + entityPM.PrivateLabelUrl + "," + entityPM.ContactUsEmail;
        }
    }
}
