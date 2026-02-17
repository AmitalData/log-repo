using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class APPaymentMethodMapping
    {
        public static void MapEntity(APPaymentMethodPM entityPM, APPaymentMethod poco, bool isNewEntity)
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
            poco.AccountingExternalId = entityPM.AccountingExternalId;
        }
    }
}
