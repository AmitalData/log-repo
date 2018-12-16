using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class MessagingStockUsageHistoryQuery
    {
        MessagingStockUsageHistoryRepository repository;

        public MessagingStockUsageHistoryQuery(int tenant)
        {
            repository = new MessagingStockUsageHistoryRepository(tenant);
        }

        public MessagingStockUsageHistoryQuery(MessagingStockUsageHistoryRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<MessagingStockUsageHistoryPM> GetStockUsageHistoriesByStockId(string stockId)
        {
            return (from a in repository.Context.MessagingStockUsageHistories
                    where a.StockId == stockId
                    select new MessagingStockUsageHistoryPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        StockId = a.StockId,
                        HAWB = a.HAWB,
                        MAWB = a.MAWB,
                        MessageType = a.MessageType,                        
                        ActionType = a.ActionType,
                        FirstActionDate = a.FirstActionDate,
                        LastActionDate = a.LastActionDate,
                        FirstActionByUserId = a.FirstActionByUserId,
                        LastActionByUserId = a.LastActionByUserId,
                        EntityId = a.EntityId,
                        EntityNumber = a.EntityNumber,
                    });
        }

        public IQueryable<MessagingStockUsageHistoryList> GetIQueryableEntityList(IQueryable<MessagingStockUsageHistory> iQueryable, int tenant)
        {
            IQueryable<MessagingStockUsageHistoryList> myResult =
                from a in iQueryable.Include("LastActionByUser").Include("LastActionByUser.Contact")
                select new MessagingStockUsageHistoryList()
                {
                    Id = a.Id,
                    Tenant = a.Tenant,
                    StockId = a.StockId,
                    HAWB = a.HAWB,
                    MAWB = a.MAWB,
                    MessageType = a.MessageType,
                    ActionType = a.ActionType,
                    FirstActionDate = a.FirstActionDate,
                    LastActionDate = a.LastActionDate,
                    FirstActionByUserId = a.FirstActionByUserId,
                    LastActionByUserId = a.LastActionByUserId,
                    LastActionByUserName = a.LastActionByUser == null ? "" : a.LastActionByUser.Contact.EnglishName,
                    EntityId = a.EntityId,
                    EntityNumber = a.EntityNumber,
                };

            return myResult;
        }


    }
}