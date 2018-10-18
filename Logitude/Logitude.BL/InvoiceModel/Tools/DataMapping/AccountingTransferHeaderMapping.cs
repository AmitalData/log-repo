using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class AccountingTransferHeaderMapping
    {
        public static void MapEntity(AccountingTransferHeaderPM entityPM, AccountingTransferHeader entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;
                entity.AccountingTransferTypeCode = entityPM.AccountingTransferTypeCode;
            }

            entity.TransferNumber = entityPM.TransferNumber;
            entity.TransferDate = entityPM.TransferDate;
            entity.FileName = entityPM.FileName;
            entity.UserId = entityPM.UserId;
            entity.Notes = entityPM.Notes;
            entity.SearchFields = entityPM.SearchFields;
        }
    }
}