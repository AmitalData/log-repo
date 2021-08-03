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
   public partial class CustomerTenantAccessMapping
   {
       internal static void MapCustomerTenantAccessCard(CustomerTenantAccessCardPM itemPM, CustomerTenantAccessCard itemPoco, bool isNewEntity,string loggedContactId,Tenant loggedTenant)
       {
         

           if (isNewEntity)
           {
               itemPoco.Tenant = itemPM.Tenant;
               itemPoco.CustomerId = itemPM.CustomerId;
               itemPoco.CustomerTenantAccessId = itemPM.CustomerTenantAccessId;
               itemPoco.CreateByUserId = loggedContactId;
               itemPoco.CreateDate = TenantServerConfigration.GetCurrentDateTime(loggedTenant.Id);
               itemPoco.IsImportActivated = true;
                itemPoco.IsExportActivated = false;
            }
           else
           {
               itemPoco.CreateDate = itemPM.CreateDate;
           }

           itemPoco.CreateByUserId = loggedContactId;
           itemPoco.CustomerTenantAccessId = itemPM.CustomerTenantAccessId;
           itemPoco.LastShipmentDateInQueue = itemPM.LastShipmentDateInQueue;
           //itemPoco.HybridStartDate = itemPM.HybridStartDate;
           itemPoco.LastMappingDateTime = itemPM.LastMappingDateTime;
           itemPoco.StatusTypeCode = itemPM.StatusTypeCode;
           itemPoco.UpdateDateTime = TenantServerConfigration.GetCurrentDateTime(loggedTenant.Id);
            itemPoco.IsImportActivated = itemPM.IsImportActivated;
            itemPoco.IsExportActivated = itemPM.IsExportActivated;
        }

    }
}
