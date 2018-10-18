using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityPMs;

namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public class QuoteTemplateSectionTypeMapping
    {
        internal static void MappingQuoteTemplateSectionType(QuoteTemplateSectionTypePM itemPM, QuoteTemplateSectionType itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Code = itemPM.Code;
            
            }

            itemPoco.Name = itemPM.Name;
        
        }
    }
}