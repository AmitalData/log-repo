using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class BankAccountLiteMapping
    {
        public static void MapEntity(BankAccountLitePM entityPM, BankAccountLite entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Tenant = entityPM.Tenant;
            }

            entity.CreateDate = entityPM.CreateDate;
            entity.UpdateDate = entityPM.UpdateDate;
            entity.LocalName = entityPM.LocalName;
            entity.EnglishName = entityPM.EnglishName;
            entity.BranchNumber = entityPM.BranchNumber;
            entity.AccountNumber = entityPM.AccountNumber;
            entity.IBAN = entityPM.IBAN;
            entity.SwiftCode = entityPM.SwiftCode;
            entity.BankCode = entityPM.BankCode;
            entity.CurrencyId = entityPM.CurrencyId;
            entity.BranchAddress = entityPM.BranchAddress;
            entity.Inactive = entityPM.Inactive;
            entity.VatNumber = entityPM.VatNumber;
            entity.SearchFields = entityPM.SearchFields;

        }
    }
}
