using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class OceanInsightsStatusesMapping
    {
        public static void MapOceanInsightsStatuses(OceanInsightsStatusesPM itemPM, OceanInsightsStatuses itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.CreateDate = itemPM.CreateDate;
            }

            itemPoco.CommunicationLogId = itemPM.CommunicationLogId;
            itemPoco.ContentDocumentId = itemPM.ContentDocumentId; 
            itemPoco.OceanInsightsRequestId = itemPM.OceanInsightsRequestId; 
        }
    }
}