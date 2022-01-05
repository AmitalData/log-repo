using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    class ARPaymentReferencesService
    {
        private IInvoiceContext objectContext;
        private ARInvoiceRepository invoiceRepository;
        private ARPaymentRepository paymentRepository;
        private ARInvoicePaymentRepository invoicePaymentRepository;

        public ARPaymentReferencesService(IInvoiceContext objectContext)
        {
            this.objectContext = objectContext;
            this.invoiceRepository = new ARInvoiceRepository(this.objectContext);
            this.paymentRepository = new ARPaymentRepository(this.objectContext);
            this.invoicePaymentRepository = new ARInvoicePaymentRepository(this.objectContext);
        }

        internal void CalculateARInvoicePaymentRefeneces(ARPaymentPM theEntityPm)
        {
            var invoicesIds = theEntityPm.PaymentInvoices;

            foreach (ARPaymentInvoicePM item in invoicesIds)
            {
                this.UpdateARInvoicePaymentRefreneces(item, theEntityPm);
            }

        }

        private void UpdateARInvoicePaymentRefreneces(ARPaymentInvoicePM item, ARPaymentPM aRPaymentPM)
        {
            List<ARInvoicePayment> allConnectedItems = invoicePaymentRepository.GetARInvoicePaymentByInvoiceId(item.ARInvoiceId, item.Tenant).ToList();

            string paymentRefrenece = "";
            List<string> Ids = new List<string>();

            if (allConnectedItems.Count > 0)
            {
                foreach (ARInvoicePayment aRInvoicePayment in allConnectedItems)
                {
                    ARPayment aRPayment = paymentRepository.GetSingleARPayment(aRInvoicePayment.ARPaymentId, aRInvoicePayment.Tenant);

                    if (aRPayment != null && aRPayment.ChequeOrPaymentRef != null)
                    {
                        paymentRefrenece = aRPayment.ChequeOrPaymentRef == null ? paymentRefrenece : paymentRefrenece + aRPayment.ChequeOrPaymentRef + ',';
                        if (aRPayment.Id == aRPaymentPM.PaymentInvoices[0].ARPaymentId)
                        {
                            Ids.Add(aRPaymentPM.ChequeOrPaymentRef);
                        }
                        else
                        {
                            Ids.Add(aRPayment.ChequeOrPaymentRef);
                        }

                    }
                }
            }

            paymentRefrenece = string.Join(",", Ids);

            var ar = invoiceRepository.GetSingleInvoice(item.ARInvoiceId);
            ar.PaymentReferences = paymentRefrenece;
            invoiceRepository.Update(ar);
        }

        internal void CalculatePaymentReferences(ARInvoicePM entityPM, ARInvoice invoice)
        {
            var invoicesIds = entityPM.InvoicePayments;
            invoice.PaymentReferences = this.GetPaymentReferences(invoicesIds);
        }

        private string GetPaymentReferences(List<ARInvoicePaymentPM> invoicesIds)
        {
            throw new NotImplementedException();
        }
    }
}
