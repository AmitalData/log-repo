using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class ConfirmationNumberDefaultMapping
    {
        public static void MapEntity(ConfirmationNumberDefaultPM entityPM, ConfirmationNumberDefault entity, bool isNewState)
        {
            if (isNewState)
            {
                //entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;

            }
            entity.Id = entityPM.Id;
            entity.FromDate = entityPM.FromDate;
            entity.AmountForConfirmationNumber = entityPM.AmountForConfirmationNumber;
            entity.InActive = entityPM.InActive;
            entity.SearchFields = entityPM.SearchFields;
           
        }
    }
}
