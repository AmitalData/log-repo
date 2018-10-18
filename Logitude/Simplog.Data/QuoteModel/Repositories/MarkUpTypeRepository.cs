using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class MarkUpTypeRepository: IRepository<MarkUpType>
    {

        IQuotesContext quotesContext;
        public MarkUpTypeRepository(IQuotesContext context)
        {
            quotesContext = context;
        }

        public MarkUpTypeRepository()
        {
            quotesContext = new QuotesContext();
        }
        public MarkUpTypeRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }
        public MarkUpType GetSingleMarkUpType(string code)
        {
            return (from a in context.MarkUpTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }
        public IQueryable<MarkUpType> GetAll()
        {
            return context.MarkUpTypes;
        }
        public IQueryable<MarkUpType> GetMarkUpTypes()
        {
            return context.MarkUpTypes;
        }

        public void Add(MarkUpType entity)
        {
            context.MarkUpTypes.Add(entity);
        }

        public void Remove(MarkUpType entity)
        {
            context.MarkUpTypes.Attach(entity);
            context.MarkUpTypes.Remove(entity);
        }

        public void Update(MarkUpType entity)
        {
            context.MarkUpTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<MarkUpType> All()
        {
            return context.MarkUpTypes.ToList();
        }

        public IQuotesContext context
        {
            get { return quotesContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<MarkUpType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public MarkUpType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}