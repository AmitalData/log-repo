using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class CreditCardTypeMapping
    {
        public static void MapEntity(CreditCardTypePM entityPM, CreditCardType poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                poco.Tenant = entityPM.Tenant;
            }

            poco.Code = entityPM.Code;
            poco.InActive = entityPM.InActive;
            poco.Name = entityPM.Name;
            poco.Tenant = entityPM.Tenant;
            poco.SearchFields = entityPM.Code + "," + entityPM.Name;
            poco.BankAccountId = entityPM.BankAccountId;
        }
    }
}