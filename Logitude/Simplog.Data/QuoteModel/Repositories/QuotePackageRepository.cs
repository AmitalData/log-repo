using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class QuotePackageRepository: IRepository<QuotePackage>
    {
        IQuotesContext quoteContext;

        public QuotePackageRepository(IQuotesContext context)
        {
            quoteContext = context;
        }

        public QuotePackageRepository()
        {
            quoteContext = new  QuotesContext();
        }

        public QuotePackageRepository(int tenant)
        {
            quoteContext = QuotesContext.GetContext(tenant);
        }

        public IQueryable<QuotePackage> GetQuotePackages(int tenant)
        {
            return (from record in context.QuotePackages
                    where record.Tenant == tenant select record);
        }

        public QuotePackage GetSingleQuotePackage(string id, int tenant)
        {
            return (from record in context.QuotePackages where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public IQueryable<QuotePackage> GetQuotePackagesForQuoteTenant(string quoteId, int tenant)
        {
            IQueryable<QuotePackage> quotePackages = from a in context.QuotePackages
                                                     where a.Tenant == tenant
                                                     && a.QuoteId == quoteId                  
                                                     select a;
            return quotePackages;
        }

        public void Add(QuotePackage entity)
        {
            context.QuotePackages.Add(entity);
        }

        public void Remove(QuotePackage entity)
        {
            try
            {
                context.QuotePackages.Attach(entity);
            }
            catch { }
            context.QuotePackages.Remove(entity);
        }

        public void Update(QuotePackage entity)
        {
            try
            {
                context.QuotePackages.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<QuotePackage> All()
        {
            return context.QuotePackages.ToList();
        }

        public IQuotesContext context
        {
            get { return quoteContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<QuotePackage> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public QuotePackage GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}