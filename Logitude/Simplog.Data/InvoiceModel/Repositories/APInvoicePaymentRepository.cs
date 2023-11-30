using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class APInvoicePaymentRepository: IRepository<APInvoicePayment>
    {
        IInvoiceContext invoiceContext;
        public APInvoicePaymentRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public APInvoicePaymentRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public APInvoicePaymentRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public APInvoicePayment GetSingleAPInvoicePayment(string id, int tenant)
        {
            return (from a in context.APInvoicePayments
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<APInvoicePayment> GetAPInvoicePayments(string paymentId, string invoiceId, int tenant)
        {
            return (from a in context.APInvoicePayments where a.APPaymentId == paymentId && a.APInvoiceId == invoiceId && a.Tenant == tenant select a);
        }

        public IQueryable<APInvoicePayment> GetAPInvoicePaymentByPaymentId(string paymentid, int tenant)
        {
            return (from a in context.APInvoicePayments.Include("APInvoice") where a.APPaymentId == paymentid && a.Tenant == tenant select a);
        }

        public IQueryable<APPayment> GetAPInvoicePaymentTransferedByInvoiceId(string invoiceId, int tenant)
        {
            return (from a in context.APInvoicePayments join payment in context.APPayments on a.APPaymentId equals payment.Id where payment.TransferStatusCode == "TR" where a.APInvoiceId == invoiceId && a.Tenant == tenant select payment);
        }

        public IQueryable<APInvoicePayment> GetAPInvoicePaymentByInvoiceId(string invoiceId, int tenant)
        {
            return (from a in context.APInvoicePayments.Include("APPayment") where a.APInvoiceId == invoiceId && a.Tenant == tenant select a);
        }

        public double? GetInvoicePaymentTotalAmountForPayment(string paymentid, int tenant)
        {
            double? value = 0;
            
            IQueryable<APInvoicePayment> invoicepayments = (from a in context.APInvoicePayments where a.APPaymentId == paymentid && a.Tenant == tenant select a);
            if (invoicepayments != null && invoicepayments.Count() > 0)
            {
                value = invoicepayments.Sum(d => d.ForeignAmount);
            }

            return value;
        }

        public double? GetInvoicePaymentTotalAmountForInvoice(string invoiceid, int tenant)
        {
            double? value = 0;

            IQueryable<APInvoicePayment> invoicepayments = (from a in context.APInvoicePayments where a.APInvoiceId == invoiceid && a.Tenant == tenant select a);
            if (invoicepayments != null && invoicepayments.Count() > 0)
            {
                value = invoicepayments.Sum(d => d.ForeignAmount);
            }

            return value;
        }

        public void Add(APInvoicePayment entity)
        {
            context.APInvoicePayments.Add(entity);
        }

        public void Remove(APInvoicePayment entity)
        {
            context.APInvoicePayments.Attach(entity);
            context.APInvoicePayments.Remove(entity);
        }

        public void Update(APInvoicePayment entity)
        {
            try
            {
                context.APInvoicePayments.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<APInvoicePayment> All()
        {
            return context.APInvoicePayments.ToList();
        }

        public IInvoiceContext context
        {
            get {return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<APInvoicePayment> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public APInvoicePayment GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}