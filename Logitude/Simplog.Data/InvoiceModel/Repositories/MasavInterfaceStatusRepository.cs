using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;


namespace Simplog.Data.InvoiceModel.Repositories
{
    public class MasavInterfaceStatusRepository : IRepository<MasavInterfaceStatus>
    {
        IInvoiceContext invoiceContext;

        public MasavInterfaceStatusRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public MasavInterfaceStatusRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public MasavInterfaceStatusRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

       public IQueryable<MasavInterfaceStatus> GetMasavInterfaceStatuses()
        {
            return (from a in context.MasavInterfaceStatuses
                    select a);
        }
        public MasavInterfaceStatus GetSingleMasavInterfaceStatus(string code)
        {
            return (from a in context.MasavInterfaceStatuses where a.Code == code
                    select a).FirstOrDefault();
        }
        public IQueryable<MasavInterfaceStatus> GetAll()
        {
            return (from a in context.MasavInterfaceStatuses select a);
        }

        public void Add(MasavInterfaceStatus entity)
        {
            context.MasavInterfaceStatuses.Add(entity);
        }

        public void Remove(MasavInterfaceStatus entity)
        {
            context.MasavInterfaceStatuses.Attach(entity);
            context.MasavInterfaceStatuses.Remove(entity);
        }

        public void Update(MasavInterfaceStatus entity)
        {
            try
            {
                context.MasavInterfaceStatuses.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<MasavInterfaceStatus> All()
        {
            return context.MasavInterfaceStatuses.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<MasavInterfaceStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public MasavInterfaceStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}