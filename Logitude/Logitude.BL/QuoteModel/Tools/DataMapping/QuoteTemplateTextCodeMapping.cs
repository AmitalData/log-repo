using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityPMs;

namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public class QuoteTemplateTextCodeMapping
    {


        public static void MappingQuoteTemplateTextCode(QuoteTemplateTextCodePM itemPM, QuoteTemplateTextCode itemPoco, bool isNewEntity)
        {

            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.OriginalEnglishName = itemPM.EnglishName;
                itemPoco.OriginalLocalName = itemPM.LocalName;
            }

            itemPoco.TextCode = itemPM.TextCode;
            itemPoco.EnglishName = itemPM.EnglishName;
            itemPoco.LocalName = itemPM.LocalName;
            itemPoco.QuoteTemplateId = itemPM.QuoteTemplateId;
            itemPoco.Tenant = itemPM.Tenant;

            itemPoco.Area = itemPM.Area;
  

        }
    }
}