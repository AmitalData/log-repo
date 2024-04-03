using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Simplog.Data.InvoiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers.WorkerRole.DocsOut;

namespace WebFreight.Web.Helpers.BatchPrint
{
    public class ARInvoiceBatchPrinter : BatchPrinter
    {
        private ARInvoiceService aRInvoiceService;
        private IInvoiceContext invoiceContext;
        public ARInvoiceBatchPrinter(BatchPrinterArgs batchPrinterArgs) : base(batchPrinterArgs)
        {
            this.invoiceContext = InvoiceContext.GetContext(batchPrinterArgs.Tenant);
            this.aRInvoiceService = new ARInvoiceService(invoiceContext, batchPrinterArgs.Tenant, batchPrinterArgs.Email);
        }



        public override void CustomeValidation(PrintEntityKeys item)
        {
            ARInvoicePM aRInvoicePM = GetARInvoicePM(item);
            if (aRInvoicePM.IsConsolidationInvoice && documentType.Code != DocumentTypeCodes.ConsolidationInvoice)
                throw new Exception($"This invoice does not support the selected document type. Please print it with the 'Consolidation Invoice' document type");

            if ((aRInvoicePM.ARInvoiceTypeCode == ARnvoiceTypeCode.CreditNote || aRInvoicePM.ARInvoiceTypeCode == ARnvoiceTypeCode.Invoice) && !aRInvoicePM.IsConsolidationInvoice && documentType.Code != DocumentTypeCodes.ShipmentInvoice )
                throw new Exception($"This invoice does not support the selected document type. Please print it with the 'Shipment Invoice' document type");
            
            if ((aRInvoicePM.ARInvoiceTypeCode == ARnvoiceTypeCode.CustomsInvoice || aRInvoicePM.ARInvoiceTypeCode == ARnvoiceTypeCode.CustomsCreditNote) && documentType.Code != DocumentTypeCodes.CustomsInvoice)
                throw new Exception($"This invoice does not support the selected document type. Please print it with the 'Customs Invoice' document type");
            
            if (aRInvoicePM.ARInvoiceTypeCode == ARnvoiceTypeCode.ManifestInvoice && documentType.Code != DocumentTypeCodes.ManifestInvoice)
                throw new Exception($"This invoice does not support the selected document type. Please print it with the 'Manifest Invoice' document type");
        }
        
        private ARInvoicePM GetARInvoicePM(PrintEntityKeys item)
        {
            ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(_batchPrinterArgs.Tenant);
            return aRInvoiceQuery.GetSinglePM(item.ChildEntityId, _batchPrinterArgs.Tenant);
        }
        public override void AfterPrint(PrintEntityKeys item)
        {
            if (item.IsAlreadyPrinted) return;
            ARInvoicePrintDetailsService aRInvoicePrintDetailsService = new ARInvoicePrintDetailsService(_batchPrinterArgs.Tenant, item.ChildEntityId, _batchPrinterArgs.Email);
            aRInvoicePrintDetailsService.Update(documentOutId);
        }
    }
    public class DocumentTypeCodes
    {
        public const string ShipmentInvoice = "999S";
        public const string ConsolidationInvoice = "999C";
        public const string CustomsInvoice = "999CI";
        public const string ManifestInvoice = "999M";
    }
    public class ARnvoiceTypeCode
    {
        public const string CreditNote = "CD";
        public const string Invoice = "IN";
        public const string CustomsInvoice = "CI";
        public const string CustomsCreditNote = "CC";
        public const string ManifestInvoice = "MN";
    }
}