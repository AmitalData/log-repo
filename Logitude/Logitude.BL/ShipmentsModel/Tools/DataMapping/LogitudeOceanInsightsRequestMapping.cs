using Logitude.BL.ShipmentsModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class LogitudeOceanInsightsRequestMapping
    {
        public static void MapLogitudeOceanInsightsRequest(LogitudeOceanInsightsRequestPM itemPM, LogitudeOceanInsightsRequest itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.CreateDate = DateTime.Now;

            }
            itemPoco.ContainerNumber = itemPM.ContainerNumber;
            itemPoco.OceanInsigntId = itemPM.OceanInsigntId;
            itemPoco.UpdateDate = DateTime.Now;
            itemPoco.BLNumber = itemPM.BLNumber;
            itemPoco.ShipmentId = itemPM.ShipmentId;
        }
    }
}
