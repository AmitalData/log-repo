using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class QuoteStageRepository : IRepository<QuoteStage>
    {
        IQuotesContext myContext;
        public IQuotesContext Context
        {
            get { return myContext; }
        }

        public QuoteStageRepository(int tenant)
        {
            myContext = QuotesContext.GetContext(tenant);
        }

        public QuoteStageRepository(IQuotesContext context)
        {
            myContext = context;
        }

        public QuoteStage GetSingleQuoteStage(string id, int tenant)
        {
            return (from a in Context.QuoteStages.Include("UpdatedByUser").Include("UpdatedByUser.Contact") where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public QuoteStage GetSingleQuoteStageByCode(string code, int tenant)
        {
            return (from a in Context.QuoteStages.Include("UpdatedByUser").Include("UpdatedByUser.Contact") where a.Code == code && a.Tenant == tenant select a).FirstOrDefault();
        }

        public string GetQuoteStageIdByCode(string code, int tenant)
        {
            string myResult = null;

            QuoteStage myQuoteStage = (from a in Context.QuoteStages where a.Code == code && a.Tenant == tenant select a).FirstOrDefault();
            if (myQuoteStage != null)
            {
                myResult = myQuoteStage.Id;
            }

            return myResult;
        }
        
        public IQueryable<QuoteStage> GetQuoteStages(int tenant)
        {
            return (from a in Context.QuoteStages.Include("UpdatedByUser").Include("UpdatedByUser.Contact")
                    where a.Tenant == tenant 
                    select a);
        }

        public IQueryable<QuoteStage> GetQuoteStagesForSignup(int tenant)
        {
            IQueryable<QuoteStage> iQueryable = (from a in Context.QuoteStages
                                                 where a.Tenant == tenant 
                                                 select a);

            return iQueryable;
        }

        public void Add(QuoteStage entity)
        {
            Context.QuoteStages.Add(entity);
        }

        public void Remove(QuoteStage entity)
        {
            Context.QuoteStages.Attach(entity);
            Context.QuoteStages.Remove(entity);
        }

        public void Update(QuoteStage entity)
        {
            Context.QuoteStages.Attach(entity);
            Context.SetAsModified(entity);
        }

        public List<QuoteStage> All()
        {
            return Context.QuoteStages.ToList();
        }

        public List<QuoteStage> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public QuoteStage GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }
    }
}
