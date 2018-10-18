using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class ExternalSystemsTablesCodeMapping
    {
        public static void MapEntity(ExternalSystemsTablesCodePM entityPM, ExternalSystemsTablesCode entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;
         
            }

            entity.LogitudeTable = entityPM.LogitudeTable;
            entity.Code = entityPM.Code;
            entity.Name = entityPM.Name;
            entity.CreatedDate = entityPM.CreatedDate;
            entity.UpdatedDate = entityPM.UpdatedDate;
            entity.SearchFields = entityPM.Code + ',' + entityPM.Name + ',' + entityPM.LogitudeTable;
        }


    }
}