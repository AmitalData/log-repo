using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public partial class QuoteMapping
    {
        public static void MapEntity(QuoteClosingReasonPM entityPM, QuoteClosingReason entityPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPoco.Id = entityPM.Id;
                entityPoco.Tenant = entityPM.Tenant;
                entityPoco.Code = entityPM.Code;
                entityPoco.CreateDate = entityPM.CreateDate;
                entityPoco.CreatedByUserId = entityPM.CreatedByUserId;
            }

            entityPoco.Name = entityPM.Name;
            entityPoco.UpdateDate = entityPM.UpdateDate;
            entityPoco.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPoco.Inactive = entityPM.Inactive;
            BuildSearchFields(entityPM, entityPoco);
        }

        private static void BuildSearchFields(QuoteClosingReasonPM entityPM, QuoteClosingReason entityPoco)
        {
            string mySearchFields = "";

            if (!string.IsNullOrEmpty(entityPM.Code))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.Code : mySearchFields + "," + entityPM.Code;
            }

            if (!string.IsNullOrEmpty(entityPM.Name))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.Name : mySearchFields + "," + entityPM.Name;
            }

            entityPM.SearchFields = mySearchFields;
            entityPoco.SearchFields = mySearchFields;
        }
    }
}
