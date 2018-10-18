using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityPMs;

namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public partial class QuoteTemplateExcludedSectionMapping
    {
        public static void MappingQuoteTemplateExcludedSection(QuoteTemplateExcludedSectionPM entityPM, QuoteTemplateExcludedSection entityPoco, bool isNewEntity)
        {

            if (isNewEntity)
            {
                entityPoco.Id = entityPM.Id;
                entityPoco.Tenant = entityPM.Tenant;

            }

            entityPoco.QuoteId = entityPM.QuoteId;
            entityPoco.QuoteTemplateSectionId = entityPM.QuoteTemplateSectionId;
            entityPoco.QuoteTemplateId = entityPM.QuoteTemplateId;


        }



    }
}
