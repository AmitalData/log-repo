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
    public class AWBMessagingStockQuery
    {
        AWBMessagingStockRepository repository;

        public AWBMessagingStockQuery(int tenant)
        {
            repository = new AWBMessagingStockRepository(tenant);
        }

        public AWBMessagingStockQuery(AWBMessagingStockRepository repository)
        {
            this.repository = repository;
        }

        public AWBMessagingStockPM GetSinglePM(string id, int tenant)
        {
            DateTime? nowDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

            AWBMessagingStockPM entityPM = (from a in repository.Context.AWBMessagingStocks
                                            where a.Id == id
                                            select new AWBMessagingStockPM()
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
                                                Status = a.IsCancelled ? "Cancelled" : (a.Remaining == 0 ? "Used" : (a.EndDate <= nowDateTime ? "Expired" : (a.Amount > a.Remaining ? "Active" : "New")))
                                            }).FirstOrDefault();

            AWBStockUsageHistoryQuery myQuery = new AWBStockUsageHistoryQuery(entityPM.TenantNumber);
            entityPM.StockUsageHistories = myQuery.GetStockUsageHistoriesByStockId(id).ToList();

            return entityPM;
        }
        public IQueryable<AWBMessagingStockList> GetIQueryableEntityList(IQueryable<AWBStocksDataView> iQueryable, int tenant = 0)
        {
            DateTime? nowDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

            IQueryable<AWBMessagingStockList> myResult =
                                                        from a in iQueryable
                                                        select new AWBMessagingStockList()
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
                                                            Status = a.IsCancelled ? "Cancelled" : (a.Remaining == 0 ? "Used" : (a.EndDate <= nowDateTime ? "Expired" : (a.Amount > a.Remaining ? "Active" : "New")))
                                                        };
            return myResult;
        }

        public IQueryable<AWBMessagingStockList> GetIQueryableEntityList(IQueryable<AWBMessagingStock> iQueryable, int tenant = 0)
        {
            DateTime? nowDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

            IQueryable<AWBMessagingStockList> myResult =
                                                        from a in iQueryable
                                                        select new AWBMessagingStockList()
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
                                                            Status = a.IsCancelled ? "Cancelled" : (a.Remaining == 0 ? "Used" : (a.EndDate <= nowDateTime ? "Expired" : (a.Amount > a.Remaining ? "Active" : "New")))
                                                        };
            return myResult;
        }
    }
}