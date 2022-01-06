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

        internal void UpdateConnectedARInvoicePaymentRefeneces(ARPaymentPM aRPaymentPM)
        {
            List<ARPaymentInvoicePM> PaymentInvoices = aRPaymentPM.PaymentInvoices;

            foreach (ARPaymentInvoicePM item in PaymentInvoices)
            {
                this.UpdateARInvoicePaymentRefreneces(item, aRPaymentPM);
            } 

        }
          
        private void UpdateARInvoicePaymentRefreneces(ARPaymentInvoicePM item, ARPaymentPM aRPaymentPM)
        {
            List<ARInvoicePayment> allConnectedARInvoicePayment = invoicePaymentRepository.GetARInvoicePaymentByInvoiceId(item.ARInvoiceId, item.Tenant).ToList();
            string paymentRefrenece = CalculateARInvoicePaymentRefreneces(aRPaymentPM, allConnectedARInvoicePayment);  
            UpdateConnectedARInvoice(item, paymentRefrenece);
        }

        private void UpdateConnectedARInvoice(ARPaymentInvoicePM item, string paymentRefrenece)
        {
            ARInvoice invoice = invoiceRepository.GetSingleInvoice(item.ARInvoiceId);
            invoice.PaymentReferences = paymentRefrenece;
            invoiceRepository.Update(invoice);
        }

        private string CalculateARInvoicePaymentRefreneces(ARPaymentPM aRPaymentPM, List<ARInvoicePayment> allConnectedARInvoicePayment)
        {
            List<string> chequeOrPaymentRefList = new List<string>();

            if (allConnectedARInvoicePayment.Count > 0)
            {
                chequeOrPaymentRefList = GetChequeOrPaymentRefList(aRPaymentPM, allConnectedARInvoicePayment);
            }

            string paymentrefreece = string.Join(",", chequeOrPaymentRefList);
            return paymentrefreece; 
        }

        private List<string> GetChequeOrPaymentRefList(ARPaymentPM aRPaymentPM, List<ARInvoicePayment> allConnectedARInvoicePayment)
        {
            List<string> chequeOrPaymentRefList = new List<string>();

            foreach (ARInvoicePayment aRInvoicePayment in allConnectedARInvoicePayment)
            {
                CalculatePaymentRefList(aRPaymentPM, chequeOrPaymentRefList, aRInvoicePayment);
            }

            return chequeOrPaymentRefList;
        }

        private void CalculatePaymentRefList(ARPaymentPM aRPaymentPM, List<string> chequeOrPaymentRefList, ARInvoicePayment aRInvoicePayment)
        {
            ARPayment aRPayment = paymentRepository.GetSingleARPayment(aRInvoicePayment.ARPaymentId, aRInvoicePayment.Tenant); 
             
            GetARPaymentPaymentRef(aRPaymentPM, chequeOrPaymentRefList, aRPayment);
             
        }

        private static bool hasPaymentRefValue(ARPayment aRPayment)
        {
            return aRPayment != null && !string.IsNullOrEmpty(aRPayment.ChequeOrPaymentRef);
        }

        private static void GetARPaymentPaymentRef(ARPaymentPM aRPaymentPM, List<string> chequeOrPaymentRefList, ARPayment aRPayment)
        {
            if (hasPaymentRefValue(aRPayment))
            {
                chequeOrPaymentRefList.Add(aRPayment.ChequeOrPaymentRef);
            }
        }
 

        internal void CalculatePaymentReferences(ARInvoicePM entityPM, ARInvoice invoice)
        {
            var invoicesIds = entityPM.InvoicePayments;
            invoice.PaymentReferences = this.GetPaymentReferences(invoicesIds);
        }


        private string GetPaymentReferences(List<ARInvoicePaymentPM> invoicesIds)
        { 
            List<string> chequeOrPaymentRefList = new List<string>();

            foreach (ARInvoicePaymentPM aRInvoicePayment in invoicesIds)
            {
                CalculatePaymentRefList(chequeOrPaymentRefList, aRInvoicePayment);

            }
            string paymentRefrenece = string.Join(",", chequeOrPaymentRefList);

            return paymentRefrenece;
        }

        private void CalculatePaymentRefList(List<string> chequeOrPaymentRefList, ARInvoicePaymentPM aRInvoicePayment)
        {
            if (IsConnectedPayment(aRInvoicePayment))
            {
                InsertPaymentRef(chequeOrPaymentRefList, aRInvoicePayment);

            }
        }

        private static bool IsConnectedPayment(ARInvoicePaymentPM aRInvoicePayment)
        {
            return (int)aRInvoicePayment.ChangeSetOp != 3;
        }

        private void InsertPaymentRef(List<string> Ids, ARInvoicePaymentPM aRInvoicePayment)
        {
            ARPayment aRPayment = paymentRepository.GetSingleARPayment(aRInvoicePayment.ARPaymentId, aRInvoicePayment.Tenant);

            if (hasPaymentRefValue(aRPayment))
            {
                Ids.Add(aRPayment.ChequeOrPaymentRef);
            }
        }
    }
}
