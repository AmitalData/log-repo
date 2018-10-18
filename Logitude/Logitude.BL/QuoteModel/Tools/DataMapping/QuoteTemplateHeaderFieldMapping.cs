using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityPMs;

namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public class QuoteTemplateHeaderFieldMapping
    {

        public static void MappingQuoteTemplateHeaderField(QuoteTemplateHeaderFieldPM entityPM, QuoteTemplateHeaderField entityPoco, bool isNewEntity)
        {

            if (isNewEntity)
            {
                entityPoco.Id = entityPM.Id;
                entityPoco.Tenant = entityPM.Tenant;

            }

            entityPoco.Column = entityPM.Column;
            entityPoco.Row = entityPM.Row;
            entityPoco.FieldCode = entityPM.FieldCode;
                  entityPoco.QuoteTemplateId = entityPM.QuoteTemplateId;
       

        }


    }
}