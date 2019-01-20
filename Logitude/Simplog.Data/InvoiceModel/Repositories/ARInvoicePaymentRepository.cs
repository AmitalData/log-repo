using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class ARInvoicePaymentRepository : IRepository<ARInvoicePayment>
    {
        IInvoiceContext invoiceContext;
        public ARInvoicePaymentRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public ARInvoicePaymentRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public ARInvoicePaymentRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public ARInvoicePayment GetSingleARInvoicePayment(string id, int tenant)
        {
            return (from a in context.ARInvoicePayments
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ARInvoicePayment> GetARInvoicePayments(string paymentId, string invoiceId, int tenant)
        {
            return (from a in context.ARInvoicePayments where a.ARPaymentId == paymentId && a.ARInvoiceId == invoiceId && a.Tenant == tenant select a);
        }

        public IQueryable<ARInvoicePayment> GetARInvoicePaymentByPaymentId(string paymentid, int tenant)
        {
            return (from a in context.ARInvoicePayments where a.ARPaymentId == paymentid && a.Tenant == tenant select a);
        }

        public IQueryable<ARInvoicePayment> GetARInvoicePaymentByInvoiceId(string invoiceId, int tenant)
        {
            return (from a in context.ARInvoicePayments where a.ARInvoiceId == invoiceId && a.Tenant == tenant select a);
        }


        public IQueryable<ARPayment> GetARInvoicePaymentTransferedByInvoiceId(string invoiceId, int tenant)
        {
            return ( from a in context.ARInvoicePayments join payment in context.ARPayments on a.ARPaymentId equals payment.Id where payment.TransferStatusCode == "TR" where  a.ARInvoiceId == invoiceId && a.Tenant == tenant   select payment);
        }

        public double? GetInvoicePaymentTotalAmountForPayment(string paymentid, int tenant)
        {
            double? value = 0;
            
            IQueryable<ARInvoicePayment> invoicepayments = (from a in context.ARInvoicePayments where a.ARPaymentId == paymentid && a.Tenant == tenant select a);
            if (invoicepayments != null && invoicepayments.Count() > 0)
            {
                value = invoicepayments.Sum(d => d.ForeignAmount);
            }

            return value;
        }

        public double? GetInvoicePaymentTotalAmountForInvoice(string invoiceid, int tenant)
        {
            double? value = 0;

            IQueryable<ARInvoicePayment> invoicepayments = (from a in context.ARInvoicePayments where a.ARInvoiceId == invoiceid && a.Tenant == tenant select a);
            if (invoicepayments != null && invoicepayments.Count() > 0)
            {
                value = invoicepayments.Sum(d => d.ForeignAmount);
            }

            return value;
        }

        public void Add(ARInvoicePayment entity)
        {
            context.ARInvoicePayments.Add(entity);
        }

        public void Remove(ARInvoicePayment entity)
        {
            context.ARInvoicePayments.Attach(entity);
            context.ARInvoicePayments.Remove(entity);
        }

        public void Update(ARInvoicePayment entity)
        {
            try
            {
                context.ARInvoicePayments.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ARInvoicePayment> All()
        {
            return context.ARInvoicePayments.ToList();
        }

        public IInvoiceContext context
        {
            get {return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ARInvoicePayment> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ARInvoicePayment GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }


    }
}