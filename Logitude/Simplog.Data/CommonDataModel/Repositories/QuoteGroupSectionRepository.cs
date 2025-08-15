using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class QuoteGroupSectionRepository : IRepository<QuoteGroupSection>
    {
        ICommonDataContext commonDataContext;
        
  

        public QuoteGroupSectionRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public QuoteGroupSectionRepository(ICommonDataContext context)
        {
            commonDataContext= context;
        }
        public IQueryable<QuoteGroupSection> GetQuoteGroupSections()
        {
            return context.QuoteGroupSections;
        }
        public IQueryable<QuoteGroupSection> GetQuoteGroupSections(int tenant)
        {
            return context.QuoteGroupSections;
        }

        public IQueryable<QuoteGroupSection> GetAll()
        {
            return context.QuoteGroupSections;
        }

        public QuoteGroupSection GetSingleQuoteGroupSection(string code,int tenant = 0)
        {
            return (from record in context.QuoteGroupSections where record.Code == code select record).FirstOrDefault();
        }

        public  QuoteGroupSection GetSingleQuoteGroupSectionUpdate(string code, int tenant)
        {
            return (from record in context.QuoteGroupSections where record.Code == code select record).FirstOrDefault();
        }

        public void Add(QuoteGroupSection entity)
        {
            context.QuoteGroupSections.Add(entity);
        }

        public void Remove(QuoteGroupSection entity)
        {
            try
            {
                context.QuoteGroupSections.Attach(entity);
            }
            catch { }
            context.QuoteGroupSections.Remove(entity);
        }

        public void Update(QuoteGroupSection entity)
        {
            try
            {
                context.QuoteGroupSections.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<QuoteGroupSection> All()
        {
            return context.QuoteGroupSections.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<QuoteGroupSection> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public QuoteGroupSection GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
