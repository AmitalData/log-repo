using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CommodityRepository : IRepository<Commodity>
    {
        ICommonDataContext commonDataContext;

        public CommodityRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CommodityRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CommodityRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public int GetCommoditiesCount(int tenant)
        {
            return (from d in context.Commodities where d.Tenant == tenant select d).Count();
        }

        public bool IsCommodityExists(string code, int tenant)
        {
            bool myResult = false;

            if (context.Commodities.Where(d => d.Code == code && d.Tenant == tenant).Any())
            {
                myResult = true;
            }

            return myResult;
        }

        public IQueryable<Commodity> GetCommodities(int tenant)
        {
            return (from d in context.Commodities where d.Tenant == tenant select d);
        }

        public Commodity GetSingleCommodity(string id,int tenant)
        {
            return (from d in context.Commodities where d.Id == id && d.Tenant == tenant select d).FirstOrDefault();
        }
        public string GetSingleCommodityNameByCode(string code, int tenant)
        {
            if (string.IsNullOrEmpty(code))
                return "";

            return (from d in context.Commodities where d.Code == code && d.Tenant == tenant select d.Name).FirstOrDefault();
        }

        public void Add(Commodity entity)
        {
            context.Commodities.Add(entity);
        }

        public void Remove(Commodity entity)
        {
            try
            {
                context.Commodities.Attach(entity);
            }

            catch
            {

            }

            context.Commodities.Remove(entity);
        }

        public void Update(Commodity entity)
        {
            try
            {
                context.Commodities.Attach(entity);
            }

            catch
            {

            }

            context.SetAsModified(entity);
        }

        public List<Commodity> All()
        {
            return context.Commodities.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<Commodity> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Commodity GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Commodity> GetCommoditiesByAirlineId(string airlineId, int tenant)
        {
            return context.Commodities.Where(a => a.AirlineId == airlineId && a.Tenant == tenant);
        }
    }
}
