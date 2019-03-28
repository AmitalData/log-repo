using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class ARInvoiceStockMapping
    {
        public static void MapEntity(ARInvoiceStockPM entityPM, ARInvoiceStock poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                poco.Id = entityPM.Id;
                poco.Tenant = entityPM.Tenant;
            }

            poco.Name = entityPM.Name;
            poco.Description = entityPM.Description;
            poco.Inactive = entityPM.Inactive;
            poco.CreateDate = entityPM.CreateDate;
            poco.CreatedByUserId = entityPM.CreatedByUserId;
            poco.UpdateDate = entityPM.UpdateDate;
            poco.UpdatedByUserId = entityPM.UpdatedByUserId;
            poco.StatusCode = entityPM.StatusCode;
            poco.StartDate = entityPM.StartDate;
            poco.EndDate = entityPM.EndDate;
            poco.Remaining = entityPM.Remaining;
            poco.Amount = entityPM.Amount;
        }
    }
}
