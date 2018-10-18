using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class QuickbooksSyncRequestTicketMapping
    {

        public static void MapEntity(QuickbooksSyncRequestTicketPM entityPM, QuickbooksSyncRequestTicket entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Ticket = entityPM.Ticket;
                entity.Tenant = entityPM.Tenant;
             
            }

            entity.UserName = entityPM.UserName;
            entity.Password = entityPM.Password;
            entity.ReferenceNumber = entityPM.ReferenceNumber;
            entity.ExternalTablesRequestCount = entityPM.ExternalTablesRequestCount;
            entity.RequestCount = entityPM.RequestCount;
            entity.IsCurrentInvoiceChecked = entityPM.IsCurrentInvoiceChecked;
        }

    }
}