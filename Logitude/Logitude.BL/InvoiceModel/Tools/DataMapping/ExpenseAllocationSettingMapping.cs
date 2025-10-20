using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class ExpenseAllocationSettingMapping
    {
        public static void MapEntity(ExpenseAllocationSettingPM entityPM, ExpenseAllocationSetting entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;
         
            }

           entity.UpdateDate = entityPM.UpdateDate;
            entity.CreateDate = entityPM.CreateDate;
           entity.EntityId = entityPM.EntityId;
           entity.ObjectTableId = entityPM.ObjectTableId;
           entity.StartDateTime = entityPM.StartDateTime;
           entity.EndDateTime = entityPM.EndDateTime;
           entity.NumberOfPayments = entityPM.NumberOfPayments;
           entity.MonthInterval = entityPM.MonthInterval;
           entity.PaymentDateType = entityPM.PaymentDateType;
            entity.CreatedByUserId = entityPM.CreatedByUserId;
            entity.UpdatedByUserId = entityPM.UpdatedByUserId;

        }


    }
}