using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class AccountingPaymentMethodMapping
    {
        public static void MapEntity(AccountingPaymentMethodPM entityPM, AccountingPaymentMethod poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                poco.Tenant = entityPM.Tenant;
                poco.AddedManually = true;
            }

            poco.Code = entityPM.Code;
            poco.Inactive = entityPM.Inactive;
            poco.Name = entityPM.Name;
            poco.SearchFields = entityPM.Code + "," + entityPM.Name;
            poco.IsAP = entityPM.IsAP;
            poco.IsAR = entityPM.IsAR;
            poco.ARExternalId = entityPM.ARExternalId;
            poco.APExternalId = entityPM.APExternalId;            
        }
    }
}
