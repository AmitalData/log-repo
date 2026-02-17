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
   public partial class CustomerTenantAccessRequestMapping
    {
       public static void MapEntity(CustomerTenantAccessRequestPM entityPM, CustomerTenantAccessRequest entityPOCO, bool isNewState)
       {
           if (isNewState)
           {
               entityPOCO.Tenant = entityPM.Tenant;
               entityPOCO.Id = entityPM.Id;
               entityPOCO.ForwarderId = entityPM.ForwarderId;
               entityPOCO.RequestDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
        

           }

           entityPOCO.RequestDateTime = entityPM.RequestDateTime;
           entityPOCO.RequestStatus = entityPM.RequestStatus;
           entityPOCO.ForwarderId = entityPM.ForwarderId;

          
       }
  
    }
}
