using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityPMs;

namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public class QuoteTemplateTextDesignMapping
    {
        internal static void MappingQuoteTemplateTextDesign(QuoteTemplateTextDesignPM itemPM, QuoteTemplateTextDesign itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
            }

       itemPoco.FontSize = itemPM.FontSize;
       itemPoco.TextColor = itemPM.TextColor;
       itemPoco.FontFamily = itemPM.FontFamily;
       itemPoco.BackgroundColor = itemPM.BackgroundColor;
       itemPoco.FontWeight = itemPM.FontWeight;
       itemPoco.Italic = itemPM.Italic;
       itemPoco.UnDerLine = itemPM.UnDerLine;
       itemPoco.Alignment = itemPM.Alignment;

        }
    }
}