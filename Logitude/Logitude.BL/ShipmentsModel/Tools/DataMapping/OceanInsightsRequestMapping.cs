using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class OceanInsightsRequestMapping
    {
        public static void MapOceanInsightsRequest(OceanInsightsRequestPM itemPM, OceanInsightsRequest itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.CreateDate = DateTime.Now;
                itemPoco.IsClosed = false;
                itemPoco.OriginalResponse = null;
                itemPoco.ApiStatus = null;

            }

            itemPoco.ContainerNumber = itemPM.ContainerNumber; 
            itemPoco.OceanInsigntId = itemPM.OceanInsigntId;
            itemPoco.SCACCode = itemPM.SCACCode;
            itemPoco.UpdateDate = DateTime.Now; 
            itemPoco.Type = itemPM.Type;
            itemPoco.BLNumber = itemPM.BLNumber;
            itemPoco.FromPushPage = itemPM.FromPushPage;
			itemPoco.System = itemPM.System;
            itemPoco.Method = itemPM.Method;

        }
    }
}