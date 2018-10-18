using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CompetitorRepository: IRepository<Competitor>
    {
        ICommonDataContext commonDataContext;

        public CompetitorRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CompetitorRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CompetitorRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public IQueryable<Competitor> GetCompetitors(int tenant)
        {
            return (from record in context.Competitors
                    where record.Tenant == tenant 
                    select record);
        }

        public Competitor GetSingleCompetitor(string Id, int tenant)
        {
            return (from a in commonDataContext.Competitors
                    where a.Id == Id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public void Add(Competitor entity)
        {
            context.Competitors.Add(entity);
        }

        public void Remove(Competitor entity)
        {
            context.Competitors.Attach(entity);
            context.Competitors.Remove(entity);
        }

        public void Update(Competitor entity)
        {
            try
            {
                context.Competitors.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<Competitor> All()
        {
            return context.Competitors.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<Competitor> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Competitor GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}