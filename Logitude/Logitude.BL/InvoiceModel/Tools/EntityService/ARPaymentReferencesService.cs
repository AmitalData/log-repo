using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    class ARPaymentReferencesService
    {
        private IInvoiceContext objectContext;
        private ARInvoiceRepository invoiceRepository;  
        public ARPaymentReferencesService(IInvoiceContext objectContext)
        {
            this.objectContext = objectContext;
            this.invoiceRepository = new ARInvoiceRepository(this.objectContext); 
        } 
        internal void UpdateConnectedARInvoicePaymentRefeneces(ARPaymentPM aRPaymentPM)
        {  
            List<string> arInvoicesIds = aRPaymentPM.PaymentInvoices.GroupBy(d => d.ARInvoiceId).Select(d => d.FirstOrDefault().ARInvoiceId).ToList(); 

            foreach (string arInvoiceId in arInvoicesIds)
            {
                UpdateARInvoicePaymentRefreneces(arInvoiceId);
            }
        }

        private void UpdateARInvoicePaymentRefreneces(string arInvoiceId)
        {
            string paymentRefreneces = CalculateARInvoicePaymentsRefreneces(arInvoiceId);
            UpdateARInvoice(arInvoiceId, paymentRefreneces);
        }

        private string CalculateARInvoicePaymentsRefreneces(string arInvoiceId)
        {
            List<string> invoicesPayment = (from a in objectContext.ARInvoicePayments.Include("ARPayment")
                                            where a.ARInvoiceId == arInvoiceId && !string.IsNullOrEmpty(a.ARPayment.ChequeOrPaymentRef)
                                            select (a.ARPayment.ChequeOrPaymentRef)).ToList();
            string paymentRefreneces = string.Join(",", invoicesPayment);
            return paymentRefreneces;
        }

        private void UpdateARInvoice(string arInvoiceId, string paymentRefreneces)
        {
            ARInvoice invoice = invoiceRepository.GetSingleInvoice(arInvoiceId);
            invoice.PaymentReferences = paymentRefreneces;
            invoiceRepository.Update(invoice);
        }
        internal string GetARInvoicePaymentRefreneces(ARInvoice invoice)
        {
            var arInvoiceId = invoice.Id; 
            string paymentRefreneces = CalculateARInvoicePaymentsRefreneces(arInvoiceId);
            return paymentRefreneces;
        }
        
    }
}
