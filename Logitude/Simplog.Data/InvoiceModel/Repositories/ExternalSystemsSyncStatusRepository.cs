using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
   public class ExternalSystemsSyncStatusRepository : IRepository<ExternalSystemsSyncStatus>
    {

        IInvoiceContext invoiceContext;
        public ExternalSystemsSyncStatusRepository()
        {
            invoiceContext = new InvoiceContext();
        }
        public ExternalSystemsSyncStatusRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
        public ExternalSystemsSyncStatusRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<ExternalSystemsSyncStatus> GetExternalSystemsSyncStatuss()
        {
            return context.ExternalSystemsSyncStatuses;
        }

        public IQueryable<ExternalSystemsSyncStatus> GetExternalSystemsSyncStatuss(int tenant)
        {
            return (from record in context.ExternalSystemsSyncStatuses where record.Tenant == tenant select record);
        }

        public IQueryable<ExternalSystemsSyncStatus> GetExternalSystemsSyncStatussByTenant(int tenant)
        {
            return (from record in context.ExternalSystemsSyncStatuses where record.Tenant == tenant select record);
        }



        public ExternalSystemsSyncStatus GetSingleExternalSystemsSyncStatus(string id, int tenant)
        {
            return (from record in context.ExternalSystemsSyncStatuses where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public ExternalSystemsSyncStatus GetSingleExternalSystemsSyncStatusBySubject(string subject, int tenant)
        {
            return (from record in context.ExternalSystemsSyncStatuses where record.Subject == subject && record.Tenant == tenant select record).FirstOrDefault();
        }


        public ExternalSystemsSyncStatus GetSingleExternalSystemsSyncStatusByCode(string id, int tenant)
        {
            return (from record in context.ExternalSystemsSyncStatuses where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }


        public void Add(ExternalSystemsSyncStatus entity)
        {
            context.ExternalSystemsSyncStatuses.Add(entity);
        }

        public void Remove(ExternalSystemsSyncStatus entity)
        {
            context.ExternalSystemsSyncStatuses.Attach(entity);
            context.ExternalSystemsSyncStatuses.Remove(entity);
        }

        public void Update(ExternalSystemsSyncStatus entity)
        {
            context.ExternalSystemsSyncStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExternalSystemsSyncStatus> All()
        {
            return context.ExternalSystemsSyncStatuses.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<ExternalSystemsSyncStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ExternalSystemsSyncStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}
