using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityPMs;

namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public class QuoteDocumentVersionMapping
    {

        internal static void MappingQuoteDocumentVersion(QuoteDocumentVersionPM itemPM, QuoteDocumentVersion itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.QuoteId = itemPM.QuoteId;
                itemPoco.VersionNumber = itemPM.VersionNumber;
           
            }


            itemPoco.Tenant = itemPM.Tenant;

            itemPoco.CreatedByUserId = itemPM.CreatedByUserId;
            itemPoco.CreateDate = itemPM.CreateDate;
            itemPoco.UpdatedByUserId = itemPM.UpdatedByUserId;
            itemPoco.UpdateDate = itemPM.UpdateDate;

            itemPoco.IsSent = itemPM.IsSent;
            itemPoco.QuoteTemplateId = itemPM.QuoteTemplateId;
            
            itemPoco.SendDate = itemPM.SendDate;
            itemPoco.VersionType = itemPM.VersionType;

            itemPoco.DocumentId = itemPM.DocumentId;
        }
    }
}