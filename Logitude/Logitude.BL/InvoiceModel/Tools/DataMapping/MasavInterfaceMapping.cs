using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class MasavInterfaceMapping
    {

        public static void MapEntity(MasavInterfacePM entityPM, MasavInterface entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;
                entity.CreateDate = entityPM.CreateDate;
                entity.CreatedByUserId = entityPM.CreatedByUserId;

            }
            entity.UpdateDate = entityPM.UpdateDate;
            entity.UpdatedByUserId = entityPM.UpdatedByUserId;
            entity.SearchFields = entityPM.SearchFields;
            entity.FromDate = entityPM.FromDate;
            entity.ToDate = entityPM.ToDate;
            entity.PaymentDate = entityPM.PaymentDate;
            entity.StatusCode = entityPM.StatusCode;

        }


    }
}