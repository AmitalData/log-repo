using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class OceanInsightsStatusLogMapping
	{
        public static void MapOceanInsightsStatusLog(OceanInsightsStatusLogPM itemPM, OceanInsightsStatusLog itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.CreateDate = DateTime.Now;
                
            }

            itemPoco.XML = itemPM.XML; 
            itemPoco.OceanInsigntRequestId = itemPM.OceanInsigntRequestId;
           
        }
    }
}