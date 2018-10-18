using Simplog.Data.CommonDataModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class FBLStockRepository : IRepository<FBLStock>
    {
        IShipmentsContext shipmentContext;

        public FBLStockRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public FBLStockRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public FBLStockRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<FBLStock> GetFBLStocks(int tenant)
        {
            return (from record in context.FBLStocks where record.Tenant == tenant select record);
        }

        public FBLStock GetSingleFBLStock(string id, int tenant)
        {
            return (from record in context.FBLStocks where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public FBLStock GetSingleFBLStockByNumber(long number, int tenant)
        {
            FBLStock FBLStock = (from a in context.FBLStocks
                                   where a.Tenant == tenant && a.Number == number 
                                   select a).FirstOrDefault();
            return FBLStock;
        }

        public List<FBLStock> GetFBLStocksByInsertionDate(int tenant, DateTime insertionDate)
        {
            List<FBLStock> FBLStocks = (from a in context.FBLStocks
                                          where a.Tenant == tenant && a.InsertionDate == insertionDate
                                          select a).ToList();
            return FBLStocks;
        }

        public void Add(FBLStock entity)
        {
            context.FBLStocks.Add(entity);
        }

        public void Remove(FBLStock entity)
        {
            try
            {
                context.FBLStocks.Attach(entity);
            }
            catch { }
            context.FBLStocks.Remove(entity);
        }

        public void Update(FBLStock entity)
        {
            try
            {
                context.FBLStocks.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);
        }

        public List<FBLStock> All()
        {
            return context.FBLStocks.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<FBLStock> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public FBLStock GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}