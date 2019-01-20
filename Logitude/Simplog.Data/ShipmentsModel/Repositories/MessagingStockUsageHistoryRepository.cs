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
    public class MessagingStockUsageHistoryRepository : IRepository<MessagingStockUsageHistory>
    {
        IShipmentsContext myContext;
        public IShipmentsContext Context
        {
            get { return myContext; }
        }

        public MessagingStockUsageHistoryRepository(int tenant)
        {
            myContext = ShipmentsContext.GetContext(tenant);
        }

        public MessagingStockUsageHistoryRepository(IShipmentsContext context)
        {
            myContext = context;
        }

        public MessagingStockUsageHistory GetSingleMessagingStockUsageHistory(string id, int tenant)
        {
            return (from a in Context.MessagingStockUsageHistories.Include("LastActionByUser").Include("LastActionByUser.Contact")
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<MessagingStockUsageHistory> GetMessagingStockUsageHistories(string stockId, int tenant)
        {
            return (from a in Context.MessagingStockUsageHistories.Include("LastActionByUser").Include("LastActionByUser.Contact")
                    where a.StockId == stockId && a.Tenant == tenant
                    select a);
        }

        public IQueryable<MessagingStockUsageHistory> GetTenantMessagingStockUsageHistory(int tenant)
        {
            return (from a in Context.MessagingStockUsageHistories.Include("LastActionByUser").Include("LastActionByUser.Contact")
                    where a.Tenant == tenant
                    select a);
        }

        public int GetStockUsageCount(string stockId, int tenant)
        {
            return (from a in Context.MessagingStockUsageHistories.Include("LastActionByUser").Include("LastActionByUser.Contact")
                    where a.StockId == stockId && a.Tenant == tenant
                    select a).Count();
        }

        public void Add(MessagingStockUsageHistory entity)
        {
            Context.MessagingStockUsageHistories.Add(entity);
        }

        public void Remove(MessagingStockUsageHistory entity)
        {
            Context.MessagingStockUsageHistories.Attach(entity);
            Context.MessagingStockUsageHistories.Remove(entity);
        }

        public void Update(MessagingStockUsageHistory entity)
        {
            Context.MessagingStockUsageHistories.Attach(entity);
            Context.SetAsModified(entity);
        }

        public List<MessagingStockUsageHistory> All()
        {
            return Context.MessagingStockUsageHistories.ToList();
        }

        public List<MessagingStockUsageHistory> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public MessagingStockUsageHistory GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }


    }
}
