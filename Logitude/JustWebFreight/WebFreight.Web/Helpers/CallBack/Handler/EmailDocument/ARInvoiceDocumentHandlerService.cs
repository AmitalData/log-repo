using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.CallBack.Handler.EmailDocument
{
    public class ARInvoiceDocumentHandlerService
    {
        public static void SetInvoiceAsSent(EmailDocumentHandlerArgs emailDocumentHandlerArgs)
        {
            if (string.IsNullOrEmpty(emailDocumentHandlerArgs.EntityId) || string.IsNullOrEmpty(emailDocumentHandlerArgs.ObjectTableId)) return;
            ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(emailDocumentHandlerArgs.Tenant);
            ARInvoicePM aRInvoice = aRInvoiceQuery.GetSinglePM(emailDocumentHandlerArgs.EntityId, emailDocumentHandlerArgs.Tenant);
            if (aRInvoice == null || aRInvoice.Sent || aRInvoice.StatusCode == "LL") return;
            aRInvoice.Sent = true;
            aRInvoice.IsFromAutomation = true;
            IInvoiceContext MyContext = InvoiceContext.GetContext(aRInvoice.Tenant);
            ARInvoiceService service = new ARInvoiceService(MyContext, aRInvoice.Tenant);
            service.Update(aRInvoice, true);

        }
    }
}