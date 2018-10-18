using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        internal static void MapFollowUp(ShipmentFollowUpPM itemPM, FollowUp itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
                itemPoco.JobId = itemPM.JobId;
            }

            itemPoco.Date = itemPM.Date;
            itemPoco.Done = itemPM.Done;
            itemPoco.DoneDateTime = itemPM.DoneDateTime;
            itemPoco.DoneNote = itemPM.DoneNote;
            itemPoco.DocumentsFilingId = itemPM.ExternalDocumentId;
            itemPoco.InternalDocumentId = itemPM.InternalDocumentId;
            itemPoco.IsNew = itemPM.IsNew;
            itemPoco.LegType = itemPM.LegType;
            itemPoco.Notes = itemPM.Note;
            itemPoco.EventTypeId = itemPM.EventTypeId;
            itemPoco.OwnerUserId = itemPM.OwnerUserId;
            itemPoco.DocumentTypeId = itemPM.DocumentTypeId;
            itemPoco.Area = itemPM.Area;
            itemPoco.AutomationId = itemPM.AutomationId;
        }
    }
}