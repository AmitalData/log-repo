using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.CommonDataModel.Repositories
{
    public class RankRepository:IRepository<Rank>
    {
        ICommonDataContext commonDataContext;

        public RankRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public RankRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public RankRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<Rank> GetRanks(int tenant)
        {
            return (from record in context.Ranks where record.Tenant == tenant select record);
        }

        public Rank GetSingleRank(string id, int tenant)
        {
            return (from record in context.Ranks where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Rank GetSingleRankByCode(string code, int tenant)
        {
            return (from record in context.Ranks where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }

        public void Add(Rank entity)
        {
            context.Ranks.Add(entity);
        }

        public void Remove(Rank entity)
        {
            context.Ranks.Attach(entity);
            context.Ranks.Remove(entity);
        }

        public void Update(Rank entity)
        {
            try
            {
                context.Ranks.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<Rank> All()
        {
            return context.Ranks.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<Rank> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Rank GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
