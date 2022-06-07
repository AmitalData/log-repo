using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.WorkerRole.DocsOut
{
    public class ARInvoicePrintDetailsService
    {
        private ARInvoiceService aRInvoiceService;
        private IInvoiceContext invoiceContext;
        private string aRInvoiceId;
        private int tenant;

        public ARInvoicePrintDetailsService(int tenant, string aRInvoiceId)
        {
            this.tenant = tenant;
            this.aRInvoiceId = aRInvoiceId;
            this.invoiceContext = InvoiceContext.GetContext(tenant);
            this.aRInvoiceService = new ARInvoiceService(invoiceContext, tenant);
        }

        public void Update()
        {

            ARInvoicePM aRInvoicePM = GetARInvoicePM();
            if (aRInvoicePM == null) return;
            if (string.IsNullOrEmpty(aRInvoicePM.IssuedByUserId)) return;
            aRInvoicePM.PrintByUserId = aRInvoicePM.IssuedByUserId;
            aRInvoicePM.PrintDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            aRInvoicePM.IsPrinted = GetIsPrintedValue(aRInvoicePM);
            aRInvoiceService.Update(aRInvoicePM);
        }


        private  bool GetIsPrintedValue(ARInvoicePM aRInvoicePM)
        {
            bool isPrinted = aRInvoicePM.IsPrinted;
            if (aRInvoicePM.IsFromInterestBatchInvoice == true) return isPrinted;
          
            switch (aRInvoicePM.StatusCode)
            {
                case ARInvoiceStatus.Unpaid:
                case ARInvoiceStatus.Void:
                case ARInvoiceStatus.Paid:
                case ARInvoiceStatus.PartiallyPaid:
                case ARInvoiceStatus.AutoCredited:
                case ARInvoiceStatus.AutoCredit: { isPrinted = true; break; }
            }
            return isPrinted;
        }

        private ARInvoicePM GetARInvoicePM()
        {
            ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(tenant);
            return aRInvoiceQuery.GetSinglePM(aRInvoiceId, tenant);
        }
    }

    public  class ARInvoiceStatus
    {
        public const string Unpaid = "AD";
        public const string Void  = "VD";
        public const string Paid  = "PD";
        public const string PartiallyPaid  = "PP";
        public const string AutoCredited = "AR";
        public const string AutoCredit= "AC";
    }

}