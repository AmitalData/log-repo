using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class BorderTypeRepository : IRepository<BorderType>
    {
        public IQuotesContext quotesContext;

        public BorderTypeRepository(IQuotesContext context)
        {
            quotesContext = context;
        }

        public BorderTypeRepository()
        {
            quotesContext = new QuotesContext();
        }

        public BorderType GetSingleBorderType(string code)
        {
            return (from record in quotesContext.BorderTypes where record.Code == code select record).FirstOrDefault();
        }

        public void Add(BorderType entity)
        {
            quotesContext.BorderTypes.Add(entity);
        }

        public void Remove(BorderType entity)
        {
            quotesContext.BorderTypes.Attach(entity);
            quotesContext.BorderTypes.Remove(entity);
        }

        public void Update(BorderType entity)
        {
            quotesContext.BorderTypes.Attach(entity);
            quotesContext.SetAsModified(entity);
        }

        public List<BorderType> All()
        {
            return quotesContext.BorderTypes.ToList();
        }

        public void SubmitChanges()
        {
            quotesContext.SaveChanges();
        }

        public IQueryable<BorderType> GetBorderTypes()
        {
            return quotesContext.BorderTypes;
        }

        public List<BorderType> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public BorderType GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
