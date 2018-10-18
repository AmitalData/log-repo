using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class AWBStockUsageHistoryRepository : IRepository<AWBStockUsageHistory>
    {
        IShipmentsContext myContext;
        public IShipmentsContext Context
        {
            get { return myContext; }
        }

        public AWBStockUsageHistoryRepository(int tenant)
        {
            myContext = ShipmentsContext.GetContext(tenant);
        }

        public AWBStockUsageHistoryRepository(IShipmentsContext context)
        {
            myContext = context;
        }

        public AWBStockUsageHistory GetSingleAWBStockUsageHistory(string id, int tenant)
        {
            return (from a in Context.AWBStockUsageHistories.Include("LastActionByUser").Include("LastActionByUser.Contact")
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<AWBStockUsageHistory> GetAWBStockUsageHistories(string stockId, int tenant)
        {
            return (from a in Context.AWBStockUsageHistories.Include("LastActionByUser").Include("LastActionByUser.Contact")
                    where a.StockId == stockId && a.Tenant == tenant
                    select a);
        }

        public IQueryable<AWBStockUsageHistory> GetTenantAWBStockUsageHistory(int tenant)
        {
            return (from a in Context.AWBStockUsageHistories.Include("LastActionByUser").Include("LastActionByUser.Contact")
                    where a.Tenant == tenant
                    select a);
        }

        public int GetStockUsageCount(string stockId, int tenant)
        {
            return (from a in Context.AWBStockUsageHistories.Include("LastActionByUser").Include("LastActionByUser.Contact")
                    where a.StockId == stockId && a.Tenant == tenant
                    select a).Count();
        }

        public void Add(AWBStockUsageHistory entity)
        {
            Context.AWBStockUsageHistories.Add(entity);
        }

        public void Remove(AWBStockUsageHistory entity)
        {
            Context.AWBStockUsageHistories.Attach(entity);
            Context.AWBStockUsageHistories.Remove(entity);
        }

        public void Update(AWBStockUsageHistory entity)
        {
            Context.AWBStockUsageHistories.Attach(entity);
            Context.SetAsModified(entity);
        }

        public List<AWBStockUsageHistory> All()
        {
            return Context.AWBStockUsageHistories.ToList();
        }

        public List<AWBStockUsageHistory> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public AWBStockUsageHistory GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }


    }
}
