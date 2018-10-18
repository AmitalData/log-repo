using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityPMs;

namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public class QuoteTemplateTableDesignMapping
    {
        internal static void MappingQuoteTemplateTableDesign(QuoteTemplateTableDesignPM itemPM, QuoteTemplateTableDesign itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
            }

            itemPoco.BorderColor = itemPM.BorderColor;
            itemPoco.BorderTypeCode = itemPM.BorderTypeCode;
            itemPoco.BorderThickness = itemPM.BorderThickness;
            itemPoco.LinesDesignId = itemPM.LinesDesignId;
            itemPoco.HeaderDesignId = itemPM.HeaderDesignId;
            itemPoco.GroupByDesignId = itemPM.GroupByDesignId;


        }
    }
}