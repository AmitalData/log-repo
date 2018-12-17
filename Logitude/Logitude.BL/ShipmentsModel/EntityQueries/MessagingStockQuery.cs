using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class MessagingStockQuery
    {
        MessagingStockRepository repository;

        public MessagingStockQuery(int tenant)
        {
            repository = new MessagingStockRepository(tenant);
        }

        public MessagingStockQuery(MessagingStockRepository repository)
        {
            this.repository = repository;
        }

        public MessagingStockPM GetSinglePM(string id, int tenant)
        {
            DateTime? nowDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

            MessagingStockPM entityPM = (from a in repository.Context.MessagingStocks
                                         where a.Id == id
                                         select new MessagingStockPM()
                                         {
                                             Id = a.Id,
                                             DummyTenant = tenant,
                                             TenantNumber = a.TenantNumber,
                                             Amount = a.Amount,
                                             CreateDate = a.CreateDate,
                                             CreatedByUserId = a.CreatedByUserId,
                                             EndDate = a.EndDate,
                                             IsCancelled = a.IsCancelled,
                                             Notes = a.Notes,
                                             Remaining = a.Remaining,
                                             StartDate = a.StartDate,
                                             UpdateDate = a.UpdateDate,
                                             UpdatedByUserId = a.UpdatedByUserId,
                                             SearchFields = a.SearchFields,
                                             TotalPrice = a.TotalPrice,
                                             Status = a.IsCancelled ? "Cancelled" : (a.Remaining == 0 ? "Used" : (a.EndDate <= nowDateTime ? "Expired" : (a.Amount > a.Remaining ? "Active" : "New"))),
                                             StockType = a.StockType,
                                         }).FirstOrDefault();

            MessagingStockUsageHistoryQuery myQuery = new MessagingStockUsageHistoryQuery(entityPM.TenantNumber);
            entityPM.StockUsageHistories = myQuery.GetStockUsageHistoriesByStockId(id).ToList();

            return entityPM;
        }
        public IQueryable<MessagingStockList> GetIQueryableEntityList(IQueryable<MessagingStockDataView> iQueryable, int tenant = 0)
        {
            DateTime? nowDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

            IQueryable<MessagingStockList> myResult =
                                                        from a in iQueryable
                                                        select new MessagingStockList()
                                                        {
                                                            Id = a.Id,
                                                            TenantNumber = a.TenantNumber,
                                                            Amount = a.Amount,
                                                            CreateDate = a.CreateDate,
                                                            CreatedByUserId = a.CreatedByUserId,
                                                            EndDate = a.EndDate,
                                                            IsCancelled = a.IsCancelled,
                                                            Notes = a.Notes,
                                                            Remaining = a.Remaining,
                                                            StartDate = a.StartDate,
                                                            UpdateDate = a.UpdateDate,
                                                            UpdatedByUserId = a.UpdatedByUserId,
                                                            SearchFields = a.SearchFields,
                                                            TotalPrice = a.TotalPrice,
                                                            Status = a.IsCancelled ? "Cancelled" : (a.Remaining == 0 ? "Used" : (a.EndDate <= nowDateTime ? "Expired" : (a.Amount > a.Remaining ? "Active" : "New"))),
                                                            StockType = a.StockType,
                                                        };
            return myResult;
        }

        public IQueryable<MessagingStockList> GetIQueryableEntityList(IQueryable<MessagingStock> iQueryable, int tenant = 0)
        {
            DateTime? nowDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

            IQueryable<MessagingStockList> myResult =
                                                        from a in iQueryable
                                                        select new MessagingStockList()
                                                        {
                                                            Id = a.Id,
                                                            TenantNumber = a.TenantNumber,
                                                            Amount = a.Amount,
                                                            CreateDate = a.CreateDate,
                                                            CreatedByUserId = a.CreatedByUserId,
                                                            EndDate = a.EndDate,
                                                            IsCancelled = a.IsCancelled,
                                                            Notes = a.Notes,
                                                            Remaining = a.Remaining,
                                                            StartDate = a.StartDate,
                                                            UpdateDate = a.UpdateDate,
                                                            UpdatedByUserId = a.UpdatedByUserId,
                                                            SearchFields = a.SearchFields,                                                             
                                                            TotalPrice = a.TotalPrice,
                                                            Status = a.IsCancelled ? "Cancelled" : (a.Remaining == 0 ? "Used" : (a.EndDate <= nowDateTime ? "Expired" : (a.Amount > a.Remaining ? "Active" : "New"))),
                                                            StockType = a.StockType,
                                                        };
            return myResult;
        }
    }
}