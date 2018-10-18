using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        public static void MapAssembly(ShipmentAssemblyPM itemPM, ShipmentAssembly itemPoco, bool isNewEntity, string loggedContactId)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
                itemPoco.CreatedByUserId = loggedContactId;                
                itemPoco.CreateDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);                
            }

            itemPoco.UpdateDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);
            itemPoco.UpdatedByUserId = loggedContactId;
            itemPoco.House = itemPM.House;
            itemPoco.ShipperId = itemPM.ShipperId;
        }
    }
}
