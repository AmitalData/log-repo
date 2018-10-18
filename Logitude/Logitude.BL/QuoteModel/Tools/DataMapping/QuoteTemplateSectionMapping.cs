using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityPMs;

namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public class QuoteTemplateSectionMapping
    {
        internal static void MappingQuoteTemplateSection(QuoteTemplateSectionPM itemPM, QuoteTemplateSection itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
            }

            itemPoco.QuoteTemplateId = itemPM.QuoteTemplateId;

            if (string.IsNullOrEmpty(itemPM.QuoteId))
            {
                itemPoco.SectionDocId = itemPM.SectionDocId;

            }

            itemPoco.Order = itemPM.Order;
            itemPoco.Name = itemPM.Name;
            itemPoco.QuoteTemplateSectionTypeCode = itemPM.QuoteTemplateSectionTypeCode;
            itemPoco.IsCancel = itemPM.IsCancel;
                itemPoco.Description = itemPM.Description;
                   
        }
    }
}