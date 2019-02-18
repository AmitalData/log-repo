using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class MessagingStockRepository : IRepository<MessagingStock>
    {
        IShipmentsContext myContext;
        public IShipmentsContext Context
        {
            get { return myContext; }
        }

        IMessagingStockDataViewContext dataViewContext;
        public IMessagingStockDataViewContext DataViewContext
        {
            get { return dataViewContext; }
        }

        public MessagingStockRepository(int tenant)
        {
            myContext = ShipmentsContext.GetContext(tenant);
            dataViewContext = MessagingStockDataViewContext.GetContext(tenant);
        }
        public MessagingStockRepository(IShipmentsContext context)
        {
            myContext = context;
        }

        public MessagingStock GetSingleMessagingStock(string id)
        {
            return (from a in Context.MessagingStocks where a.Id == id select a).FirstOrDefault();
        }

        public MessagingStockDataView GetSingleMessagingStockDataView(string id)
        {
            return (from a in DataViewContext.MessagingStockDataViews where a.Id == id select a).FirstOrDefault();
        }

        public IQueryable<MessagingStock> GetMessagingStocks()
        {
            return (from a in Context.MessagingStocks select a);
        }

        public IQueryable<MessagingStockDataView> GetMessagingStockDataViews()
        {
            return (from a in DataViewContext.MessagingStockDataViews select a);
        }

        public IQueryable<MessagingStock> GetMessagingStocksByTenant(int tenant, string type)
        {
            return (from a in Context.MessagingStocks where a.TenantNumber == tenant && a.StockType.ToLower() == type.ToLower() select a);
        }

        public IQueryable<MessagingStockDataView> GetMessagingStockDataViewsByTenant(int tenant)
        {
            return (from a in DataViewContext.MessagingStockDataViews where a.TenantNumber == tenant select a);
        }

        public void Add(MessagingStock entity)
        {
            Context.MessagingStocks.Add(entity);
        }

        public void Remove(MessagingStock entity)
        {
            Context.MessagingStocks.Attach(entity);
            Context.MessagingStocks.Remove(entity);
        }

        public void Update(MessagingStock entity)
        {
            Context.MessagingStocks.Attach(entity);
            Context.SetAsModified(entity);
        }

        public List<MessagingStock> All()
        {
            return Context.MessagingStocks.ToList();
        }

        public List<MessagingStock> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public MessagingStock GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }

    }
}
