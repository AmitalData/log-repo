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
    public class ConfirmationNumberStatusRepository : IRepository<ConfirmationNumberStatus>
    {
        IInvoiceContext invoiceContext;

        public ConfirmationNumberStatusRepository(IInvoiceContext context)
        {
            invoiceContext = context;

        }

        public ConfirmationNumberStatusRepository()
        {
            invoiceContext = new InvoiceContext();

        }

        public ConfirmationNumberStatusRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }


        public void Add(ConfirmationNumberStatus entity)
        {
            context.ConfirmationNumberStatuses.Add(entity);
        }

        public void Remove(ConfirmationNumberStatus entity)
        {
            context.ConfirmationNumberStatuses.Attach(entity);
            context.ConfirmationNumberStatuses.Remove(entity);
        }

        public void Update(ConfirmationNumberStatus entity)
        {
            try
            {
                context.ConfirmationNumberStatuses.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ConfirmationNumberStatus> All()
        {
            return context.ConfirmationNumberStatuses.ToList();
        }
        public List<ConfirmationNumberStatus> GetAll()
        {
            return context.ConfirmationNumberStatuses.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ConfirmationNumberStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ConfirmationNumberStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
