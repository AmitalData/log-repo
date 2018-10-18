using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.InvoiceModel.Repositories
{
    public class APPaymentStatusRepository: IRepository<APPaymentStatus>
    {
        IInvoiceContext invoiceContext;
        public APPaymentStatusRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public APPaymentStatusRepository()
        {
            invoiceContext = new InvoiceContext();
        }
        public APPaymentStatusRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
        public APPaymentStatus GetSingleAPPaymentStatus(string code)
        {
            return (from a in context.APPaymentStatus
                    where a.Code == code
                    select a).FirstOrDefault();
        }

     
        public IQueryable<APPaymentStatus> GetAPPaymentStatus()
        {
            return (from a in context.APPaymentStatus

                    select a);
        }
        public IQueryable<APPaymentStatus> GetAll()
        {
            return (from a in context.APPaymentStatus

                    select a);
        }

        public void Add(APPaymentStatus entity)
        {
            context.APPaymentStatus.Add(entity);
        }

        public void Remove(APPaymentStatus entity)
        {
            context.APPaymentStatus.Attach(entity);
            context.APPaymentStatus.Remove(entity);
        }

        public void Update(APPaymentStatus entity)
        {
            try
            {
                context.APPaymentStatus.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<APPaymentStatus> All()
        {
            return context.APPaymentStatus.ToList();
        }

        public IInvoiceContext context
        {
            get {return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<APPaymentStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public APPaymentStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}