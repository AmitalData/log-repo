using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public partial class QuoteMapping
    {
        internal static void MapFollowUp(QuoteFollowUpPM itemPM, FollowUp itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.IsNew = itemPM.IsNew;
                itemPoco.JobId = itemPM.JobId;
                itemPoco.QuoteId = itemPM.QuoteId;
            }

            itemPoco.Date = itemPM.Date;
            itemPoco.Done = itemPM.Done;
            itemPoco.DoneDateTime = itemPM.DoneDateTime;
            itemPoco.DoneNote = itemPM.DoneNote;
            itemPoco.DocumentsFilingId = itemPM.ExternalDocumentId;
            itemPoco.InternalDocumentId = itemPM.InternalDocumentId;
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