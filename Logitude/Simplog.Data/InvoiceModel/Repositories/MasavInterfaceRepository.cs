using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;


namespace Simplog.Data.InvoiceModel.Repositories
{
    public class MasavInterfaceRepository : IRepository<MasavInterface>
    {
        IInvoiceContext invoiceContext;

        public MasavInterfaceRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public MasavInterfaceRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public MasavInterfaceRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

       public MasavInterface GetSingleMasavInterface(string id,int tenant)
        {
            return (from a in context.MasavInterfaces.Include("Status")
                          where a.Id == id && a.Tenant== tenant
                          select a).FirstOrDefault();
        }

        public IQueryable<MasavInterface> GetMasavInterfaces(int tenant)
        {
            return (from a in context.MasavInterfaces
                    select a);
        }
        public IQueryable<MasavInterface> GetAll()
        {
            return (from a in context.MasavInterfaces select a);
        }

        public void Add(MasavInterface entity)
        {
            context.MasavInterfaces.Add(entity);
        }

        public void Remove(MasavInterface entity)
        {
            context.MasavInterfaces.Attach(entity);
            context.MasavInterfaces.Remove(entity);
        }

        public void Update(MasavInterface entity)
        {
            try
            {
                context.MasavInterfaces.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<MasavInterface> All()
        {
            return context.MasavInterfaces.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<MasavInterface> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public MasavInterface GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}