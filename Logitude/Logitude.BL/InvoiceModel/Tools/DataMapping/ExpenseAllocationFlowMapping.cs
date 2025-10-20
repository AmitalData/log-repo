using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class ExpenseAllocationFlowMapping
    {
        public static void MapEntity(ExpenseAllocationFlowPM entityPM, ExpenseAllocationFlow entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;
         
            }
            entity.RunDate = entityPM.RunDate;
            entity.SettingId = entityPM.SettingId;
            entity.Status = entityPM.Status;
            entity.JournalId = entityPM.JournalId;


        }


    }
}