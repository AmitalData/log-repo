using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class AWBMessagingStockRepository : IRepository<AWBMessagingStock>
    {
        IShipmentsContext myContext;
        public IShipmentsContext Context
        {
            get { return myContext; }
        }

        IAWBStocksDataViewContext dataViewContext;
        public IAWBStocksDataViewContext DataViewContext
        {
            get { return dataViewContext; }
        }

        public AWBMessagingStockRepository(int tenant)
        {
            myContext = ShipmentsContext.GetContext(tenant);
            dataViewContext = AWBStocksDataViewContext.GetContext(tenant);
        }

        public AWBMessagingStockRepository(IShipmentsContext context)
        {
            myContext = context;
        }

        public AWBMessagingStock GetSingleAWBMessagingStock(string id)
        {
            return (from a in Context.AWBMessagingStocks where a.Id == id select a).FirstOrDefault();
        }

        public AWBStocksDataView GetSingleAWBStocksDataView(string id)
        {
            return (from a in DataViewContext.AWBStocksDataViews where a.Id == id select a).FirstOrDefault();
        }

        public IQueryable<AWBMessagingStock> GetAWBMessagingStocks()
        {
            return (from a in Context.AWBMessagingStocks select a);
        }

        public IQueryable<AWBStocksDataView> GetAWBStocksDataViews()
        {
            return (from a in DataViewContext.AWBStocksDataViews select a);
        }

        public IQueryable<AWBMessagingStock> GetAWBMessagingStocksByTenant(int tenant)
        {
            return (from a in Context.AWBMessagingStocks where a.TenantNumber == tenant select a);
        }

        public IQueryable<AWBStocksDataView> GetAWBStocksDataViewsByTenant(int tenant)
        {
            return (from a in DataViewContext.AWBStocksDataViews where a.TenantNumber == tenant select a);
        }

        public void Add(AWBMessagingStock entity)
        {
            Context.AWBMessagingStocks.Add(entity);
        }

        public void Remove(AWBMessagingStock entity)
        {
            Context.AWBMessagingStocks.Attach(entity);
            Context.AWBMessagingStocks.Remove(entity);
        }

        public void Update(AWBMessagingStock entity)
        {
            Context.AWBMessagingStocks.Attach(entity);
            Context.SetAsModified(entity);
        }

        public List<AWBMessagingStock> All()
        {
            return Context.AWBMessagingStocks.ToList();
        }

        public List<AWBMessagingStock> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public AWBMessagingStock GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }

    }
}
