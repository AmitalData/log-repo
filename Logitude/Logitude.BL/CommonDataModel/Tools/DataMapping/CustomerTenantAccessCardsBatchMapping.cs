using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
  public  class CustomerTenantAccessCardsBatchMapping
    {
      public static void MapEntity(CustomerTenantAccessCardsBatchPM entityPM, CustomerTenantAccessCardsBatch entityPOCO, bool isNewState, Tenant loggedTenant)
        {
            if (isNewState)
            {
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.CustomerId = entityPM.CustomerId;
                entityPOCO.CustomerTenantAccessId = entityPM.CustomerTenantAccessId;
                entityPOCO.BatchNumber = entityPM.BatchNumber;
                entityPOCO.CreateDateTime = TenantServerConfigration.GetCurrentDateTime(loggedTenant.Id);

            }

            else
            {
                entityPOCO.CreateDateTime = entityPM.CreateDateTime;
            }
            entityPOCO.CustomerId = entityPM.CustomerId;
            entityPOCO.CustomerTenantAccessId = entityPM.CustomerTenantAccessId;
            entityPOCO.DoneDate = entityPM.DoneDate;
            entityPOCO.Status = entityPM.Status;
            entityPOCO.FromDatetime = entityPM.FromDatetime;
            entityPOCO.ToDatetime = entityPM.ToDatetime;
            entityPOCO.TotalShipment = entityPM.TotalShipment;
            entityPOCO.Totalsucceeded = entityPM.Totalsucceeded;
            entityPOCO.TotalFailed = entityPM.TotalFailed;

        }

     
    }
}
