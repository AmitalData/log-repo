using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class FBLStockQuery
    {
        FBLStockRepository repository;

        public FBLStockQuery()
        {
            repository = new FBLStockRepository();
        }

        public FBLStockQuery(int tenant)
        {
            repository = new FBLStockRepository(tenant);
        }

        public FBLStockQuery(FBLStockRepository repository)
        {
            this.repository = repository;
        }

        public FBLStockPM GetSinglePM(string id, int tenant)
        {
            var FBLStock = (from a in repository.context.FBLStocks
                            where a.Tenant == tenant && a.Id == id
                            select new FBLStockPM()
                            {
                                Id = a.Id,

                                Tenant = a.Tenant,
                                Number = a.Number,
                                InsertionDate = a.InsertionDate,
                                Notes = a.Notes,

                                IsUsed = a.IsUsed,
                            }).FirstOrDefault();

            return FBLStock;
        }

        public FBLStockPM GetSingleFBLStockPMByNumber(int number, int tenant)
        {
            var FBLStock = (from a in repository.context.FBLStocks
                            where a.Tenant == tenant && a.Number == number && a.IsUsed == false
                            select new FBLStockPM()
                            {
                                Id = a.Id,

                                Tenant = a.Tenant,
                                Number = a.Number,
                                InsertionDate = a.InsertionDate,
                                Notes = a.Notes,

                                IsUsed = a.IsUsed,
                            }).FirstOrDefault();

            return FBLStock;
        }

        public IQueryable<FBLStockPM> GetFBLStockPMsByTenant(int tenant)
        {
            IQueryable<FBLStockPM> FBLStocks = from a in repository.context.FBLStocks
                                               where a.Tenant == tenant && a.IsUsed == false
                                               select new FBLStockPM()
                                               {
                                                   Id = a.Id,

                                                   Tenant = a.Tenant,
                                                   Number = a.Number,
                                                   InsertionDate = a.InsertionDate,
                                                   Notes = a.Notes,

                                                   IsUsed = a.IsUsed,
                                               };
            return FBLStocks;
        }

        public IQueryable<FBLStockPM> GetAllAvailablFBLStockPMsByTenant(int tenant)
        {
            IQueryable<FBLStockPM> FBLStocks = from a in repository.context.FBLStocks
                                               where a.Tenant == tenant && a.IsUsed == false
                                               select new FBLStockPM()
                                               {
                                                   Id = a.Id,

                                                   Tenant = a.Tenant,
                                                   Number = a.Number,
                                                   InsertionDate = a.InsertionDate,
                                                   Notes = a.Notes,

                                                   IsUsed = a.IsUsed,
                                               };
            return FBLStocks;
        }
    }
}