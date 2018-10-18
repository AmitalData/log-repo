using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class AccountingTransferLineMapping
    {
        public static void MapEntity(AccountingTransferLinePM entityPM, AccountingTransferLine entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;
                entity.AccountingTransferHeaderId = entityPM.AccountingTransferHeaderId;
            }
            
            entity.EntityId = entityPM.EntityId;
            entity.EntityReference = entityPM.EntityReference;
            entity.SearchFields = entityPM.SearchFields;
        }
    }
}