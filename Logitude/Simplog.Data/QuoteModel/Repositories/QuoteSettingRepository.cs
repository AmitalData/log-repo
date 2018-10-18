using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class QuoteSettingRepository : IRepository<QuoteSetting>
    {
        IQuotesContext quotesContext;
        public QuoteSettingRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }
        public QuoteSettingRepository(IQuotesContext context)
        {
            quotesContext = context;
        }

        public QuoteSetting GetSingleQuoteSetting(string id)
        {
            return (from a in context.QuoteSettings where a.Id == id select a).FirstOrDefault();
        }
        public QuoteSetting GetSingleQuoteSetting(int tenant)
        {
            return (from a in context.QuoteSettings where a.Tenant == tenant select a).FirstOrDefault();
        }

        public void Add(QuoteSetting entity)
        {
            context.QuoteSettings.Add(entity);
        }
        public void Remove(QuoteSetting entity)
        {
            context.QuoteSettings.Attach(entity);
            context.QuoteSettings.Remove(entity);
        }
        public void Update(QuoteSetting entity)
        {
            context.QuoteSettings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteSetting> All()
        {
            return context.QuoteSettings.ToList();
        }

        public IQuotesContext context
        {
            get { return quotesContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<QuoteSetting> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public QuoteSetting GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
