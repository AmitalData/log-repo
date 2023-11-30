using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.Repositories
{
  public  class ARPaymentBankTranferRepository : IRepository<ARPaymentBankTranfer>
    {
        IInvoiceContext invoiceContext;

        public ARPaymentBankTranferRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public ARPaymentBankTranferRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public ARPaymentBankTranferRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public ARPaymentBankTranfer GetSingleARPaymentBankTranfer(string id, int tenant)
        {
            return (from a in context.ARPaymentBankTranfers where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }


        public IQueryable<ARPaymentBankTranfer> GetARPaymentBankTranfers(string paymentId, int tenant)
        {
            return (from a in context.ARPaymentBankTranfers where a.PaymentId == paymentId && a.Tenant == tenant select a);
        }

        public void Add(ARPaymentBankTranfer entity)
        {
            context.ARPaymentBankTranfers.Add(entity);
        }

        public void Remove(ARPaymentBankTranfer entity)
        {
            context.ARPaymentBankTranfers.Attach(entity);
            context.ARPaymentBankTranfers.Remove(entity);
        }

        public void RemoveARPaymentBankTransfers(string paymentId, int tenant)
        {
            var aRPaymentBankTranfers = (from a in context.ARPaymentBankTranfers where a.PaymentId == paymentId && a.Tenant == tenant select a).ToList();

            foreach (var entity in aRPaymentBankTranfers) {
                context.ARPaymentBankTranfers.Attach(entity);
                context.ARPaymentBankTranfers.Remove(entity);
            }
        }

        public void Update(ARPaymentBankTranfer entity)
        {
            try
            {
                context.ARPaymentBankTranfers.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ARPaymentBankTranfer> All()
        {
            return context.ARPaymentBankTranfers.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ARPaymentBankTranfer> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ARPaymentBankTranfer GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

    }
}
