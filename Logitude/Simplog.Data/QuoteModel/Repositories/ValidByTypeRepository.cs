using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class ValidByTypeRepository : IRepository<ValidByType>
    {

        IQuotesContext quotesContext;
        public ValidByTypeRepository(IQuotesContext context)
        {
            quotesContext = context;
        }

        public ValidByTypeRepository()
        {
            quotesContext = new QuotesContext();
        }
        public ValidByTypeRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }
        public ValidByType GetSingleValidByType(string code)
        {
            return (from a in context.ValidByTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }
        public IQueryable<ValidByType> GetAll()
        {
            return context.ValidByTypes;
        }
        public IQueryable<ValidByType> GetValidByTypes()
        {
            return context.ValidByTypes;
        }

        public void Add(ValidByType entity)
        {
            context.ValidByTypes.Add(entity);
        }

        public void Remove(ValidByType entity)
        {
            context.ValidByTypes.Attach(entity);
            context.ValidByTypes.Remove(entity);
        }

        public void Update(ValidByType entity)
        {
            context.ValidByTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ValidByType> All()
        {
            return context.ValidByTypes.ToList();
        }

        public IQuotesContext context
        {
            get { return quotesContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ValidByType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ValidByType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}