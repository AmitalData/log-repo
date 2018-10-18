using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class OceanInsightsRequestsCountMapping
    {
        public static void MapOceanInsightsRequestsCount(OceanInsightsRequestsCountPM itemPM, OceanInsightsRequestsCount itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.CreateDate = DateTime.Now;
                
            }

            itemPoco.ContainerNumber = itemPM.ContainerNumber; 
            itemPoco.OceanInsigntId = itemPM.OceanInsigntId; 
            itemPoco.Type = itemPM.Type;
            itemPoco.BLNumber = itemPM.BLNumber;
            itemPoco.ContainerSubscriptionId = itemPM.ContainerSubscriptionId;
        }
    }
}