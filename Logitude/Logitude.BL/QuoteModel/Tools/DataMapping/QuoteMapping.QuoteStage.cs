using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityPMs;

namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public partial class QuoteMapping
    {
        public static void MapEntity(QuoteStagePM entityPM, QuoteStage entityPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPoco.Id = entityPM.Id;
                entityPoco.Tenant = entityPM.Tenant;                
                entityPoco.Code = entityPM.Code;
                entityPoco.Rank = entityPM.Rank;
            }
            
            entityPoco.Name = entityPM.Name;
            entityPoco.MaxDays = entityPM.MaxDays;
            entityPoco.UpdateDate = entityPM.UpdateDate;
            entityPoco.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPoco.InActive = entityPM.InActive;
            BuildSearchFields(entityPM, entityPoco);
        }

        private static void BuildSearchFields(QuoteStagePM entityPM, QuoteStage entityPoco)
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