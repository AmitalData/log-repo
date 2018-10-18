using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class ExternalSystemsMissingTranslationRepository : IRepository<ExternalSystemsMissingTranslation>
    {

        IInvoiceContext invoiceContext;
        public ExternalSystemsMissingTranslationRepository()
        {
            invoiceContext = new InvoiceContext();
        }
        public ExternalSystemsMissingTranslationRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
        public ExternalSystemsMissingTranslationRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<ExternalSystemsMissingTranslation> GetExternalSystemsMissingTranslations()
        {
            return context.ExternalSystemsMissingTranslations;
        }

        public IQueryable<ExternalSystemsMissingTranslation> GetExternalSystemsMissingTranslations(int tenant)
        {
            return (from record in context.ExternalSystemsMissingTranslations where record.Tenant == tenant select record);
        }

        public IQueryable<ExternalSystemsMissingTranslation> GetExternalSystemsMissingTranslationsByTenant(int tenant)
        {
            return (from record in context.ExternalSystemsMissingTranslations where record.Tenant == tenant select record);
        }



        public ExternalSystemsMissingTranslation GetSingleExternalSystemsMissingTranslation(string id, int tenant)
        {
            return (from record in context.ExternalSystemsMissingTranslations where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public ExternalSystemsMissingTranslation GetSingleExternalSystemsMissingTranslationByCode(string code, int tenant)
        {
            return (from record in context.ExternalSystemsMissingTranslations where record.Id == code && record.Tenant == tenant select record).FirstOrDefault();
        }


        public void Add(ExternalSystemsMissingTranslation entity)
        {
            context.ExternalSystemsMissingTranslations.Add(entity);
        }

        public void Remove(ExternalSystemsMissingTranslation entity)
        {
            context.ExternalSystemsMissingTranslations.Attach(entity);
            context.ExternalSystemsMissingTranslations.Remove(entity);
        }

        public void Update(ExternalSystemsMissingTranslation entity)
        {
            context.ExternalSystemsMissingTranslations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExternalSystemsMissingTranslation> All()
        {
            return context.ExternalSystemsMissingTranslations.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<ExternalSystemsMissingTranslation> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ExternalSystemsMissingTranslation GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }


    }
}
