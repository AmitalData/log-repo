using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class ConfirmationNumberDefaultRepository : IRepository<ConfirmationNumberDefault>
    {
        IInvoiceContext invoiceContext;

        public ConfirmationNumberDefaultRepository(IInvoiceContext context)
        {
            invoiceContext = context;

        }

        public ConfirmationNumberDefaultRepository()
        {
            invoiceContext = new InvoiceContext();

        }

        public ConfirmationNumberDefaultRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }


        public void Add(ConfirmationNumberDefault entity)
        {
            context.ConfirmationNumberDefaults.Add(entity);
        }

        public void Remove(ConfirmationNumberDefault entity)
        {
            context.ConfirmationNumberDefaults.Attach(entity);
            context.ConfirmationNumberDefaults.Remove(entity);
        }

        public void Update(ConfirmationNumberDefault entity)
        {
            try
            {
                context.ConfirmationNumberDefaults.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ConfirmationNumberDefault> All()
        {
            return context.ConfirmationNumberDefaults.ToList();
        }

        public ConfirmationNumberDefault GetSingleConfirmationNumberDefault(string id , int tenant)
        {
            return context.ConfirmationNumberDefaults.Where(a=>a.Id==id).FirstOrDefault();

        }
        public List<ConfirmationNumberDefault> GetAll()
        {
            return context.ConfirmationNumberDefaults.ToList();
        }
        public IQueryable<ConfirmationNumberDefault> GetConfirmationNumberDefaults(int tenant)
        {
            return context.ConfirmationNumberDefaults.Where(a => a.Tenant == tenant);
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ConfirmationNumberDefault> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ConfirmationNumberDefault GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
