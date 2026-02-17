using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private MessagingStockQuery messagingStockQuery;
        private MessagingStockRepository messagingStockRepository;

        private MessagingStockUsageHistoryQuery messagingStockUsageHistoryQuery;
        private MessagingStockUsageHistoryRepository messagingStockUsageHistoryRepository;

        public MessagingStockPM GetSingleAWBMessagingStockPM(string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            SecurityUtility.CheckContactFeature("MessagingStock", "READ", 0);

            messagingStockQuery = new MessagingStockQuery(tenant);
            MessagingStockPM myResult = messagingStockQuery.GetSinglePM(entityId, tenant);
            myResult.DummyTenant = tenant;

            return myResult;
        }

        public MessagingStockList GetSingleAWBMessagingStockList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            SecurityUtility.CheckContactFeature("MessagingStock", "READ", 0);

            messagingStockRepository = new MessagingStockRepository(tenant);
            messagingStockQuery = new MessagingStockQuery(messagingStockRepository);

            MessagingStockList entityList = null;
            MessagingStockDataView entityPOCO = messagingStockRepository.GetSingleMessagingStockDataView(id);

            if (entityPOCO != null)
            {
                List<MessagingStockDataView> singleEntityList = new List<MessagingStockDataView>();
                singleEntityList.Add(entityPOCO);

                IQueryable<MessagingStockDataView> iQueryable = singleEntityList.AsQueryable();
                IQueryable<MessagingStockList> iQueryableEntityList = messagingStockQuery.GetIQueryableEntityList(iQueryable, tenant);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public void UpdateAWBMessagingStock(MessagingStockList entityList)
        {

        }

        public IQueryable<MessagingStockList> GetMessagingStockListForTenantManagmentTab(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            SecurityUtility.CheckContactFeature("AWBMessagingStock", "READ", 0);

            messagingStockRepository = new MessagingStockRepository(tenant);
            messagingStockQuery = new MessagingStockQuery(messagingStockRepository);

            IQueryable<MessagingStockDataView> iQueryable = messagingStockRepository.GetMessagingStockDataViewsByTenant(tenant);
            IQueryable<MessagingStockList> query2 = messagingStockQuery.GetIQueryableEntityList(iQueryable, tenant);

            return query2;
        }

        public IQueryable<MessagingStockList> GetAWBMessagingStockLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            SecurityUtility.CheckContactFeature("AWBMessagingStock", "READ", 0);

            messagingStockRepository = new MessagingStockRepository(tenant);
            messagingStockQuery = new MessagingStockQuery(messagingStockRepository);

            IQueryable<MessagingStockDataView> iQueryable = messagingStockRepository.GetMessagingStockDataViews();
            IQueryable<MessagingStockList> query2 = messagingStockQuery.GetIQueryableEntityList(iQueryable, tenant);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<MessagingStockList> GetAWBMessagingStockFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            SecurityUtility.CheckContactFeature("AWBMessagingStock", "READ", 0);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            messagingStockRepository = new MessagingStockRepository(0);
            messagingStockQuery = new MessagingStockQuery(messagingStockRepository);

            IQueryable<MessagingStockDataView> iQueryable = messagingStockRepository.GetMessagingStockDataViews();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<MessagingStockDataView>(nonListQueryOperation, iQueryable);
            int skippedEntities = queryOperations.PageIndex;

            IQueryable<MessagingStockList> query2 = messagingStockQuery.GetIQueryableEntityList(iQueryable, tenant);

            query2 = filter.GetFilteredQuery<MessagingStockList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(MessagingStockList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> objectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("MessagingStock", tenant).ToList();

                ObjectField objectField = (from a in objectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<MessagingStockList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<MessagingStockList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<MessagingStockList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<MessagingStockList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<MessagingStockList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.CreateDate);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderBy(d => d.TenantNumber).ThenBy(d => d.CreateDate);
            }

            query2 = query2.Skip(skippedEntities);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAWBMessagingStockFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            SecurityUtility.CheckContactFeature("MessagingStock", "READ", 0);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            messagingStockRepository = new MessagingStockRepository(0);
            messagingStockQuery = new MessagingStockQuery(messagingStockRepository);
            IQueryable<MessagingStockDataView> iQueryable = messagingStockRepository.GetMessagingStockDataViews();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            iQueryable = filter.GetFilteredQuery<MessagingStockDataView>(nonListQueryOperation, iQueryable);

            IQueryable<MessagingStockList> query2 = messagingStockQuery.GetIQueryableEntityList(iQueryable, tenant);

            query2 = filter.GetFilteredQuery<MessagingStockList>(listQueryOperation, query2);
            int count = query2.Count();

            return count;
        }

        public void InsertAWBMessagingStock(MessagingStockPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            SecurityUtility.CheckContactFeature("MessagingStock", "NEW", 0);

            if (objectContext == null)
            {
                objectContext = ShipmentsContext.GetContext(entityPM.TenantNumber);
            }

            MessagingStockService service = new MessagingStockService(objectContext, entityPM);
            service.Create();

            TableLastUpdateClass.UpdateTableHistory(entityPM.TenantNumber, "MessagingStock");
        }

        public void UpdateAWBMessagingStock(MessagingStockPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            SecurityUtility.CheckContactFeature("MessagingStock", "UPDATE", 0);

            if (objectContext == null)
            {
                objectContext = ShipmentsContext.GetContext(entityPM.TenantNumber);
            }

            MessagingStockService service = new MessagingStockService(objectContext, entityPM);
            service.Update();

            if (this.ChangeSet != null)
            {
                this.ChangeSet.Associate(entityPM, service.entityPoco, MapStockPMToStock);
            }

            TableLastUpdateClass.UpdateTableHistory(entityPM.TenantNumber, "MessagingStock");
        }

        private void MapStockPMToStock(MessagingStockPM entityPM, MessagingStock entityPOCO)
        {
            string myStatus = "New";
            DateTime? nowDateTime = TenantServerConfigration.GetCurrentDateTime(entityPOCO.TenantNumber);

            if (entityPOCO.IsCancelled)
            {
                myStatus = "Cancelled";
            }

            else if (entityPOCO.Remaining == 0)
            {
                myStatus = "Used";
            }

            else if (entityPOCO.EndDate <= nowDateTime)
            {
                myStatus = "Expired";
            }

            else if (entityPOCO.Amount > entityPOCO.Remaining)
            {
                myStatus = "Active";
            }

            entityPM.Status = myStatus;
        }

        public void DeleteAWBMessagingStock(MessagingStockPM entityPM)
        {

        }

        // For Logged Tenant View Only
        public IQueryable<MessagingStockList> GetLoggedTenantAWBMessagingStockLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("MessagingStock", "READ", tenant);

            messagingStockRepository = new MessagingStockRepository(tenant);
            messagingStockQuery = new MessagingStockQuery(messagingStockRepository);

            IQueryable<MessagingStockDataView> iQueryable = messagingStockRepository.GetMessagingStockDataViewsByTenant(tenant);
            IQueryable<MessagingStockList> query2 = messagingStockQuery.GetIQueryableEntityList(iQueryable, tenant);

            return query2;
        }

        public IQueryable<MessagingStockUsageHistoryList> GetLoggedTenantMessagingStockUsageHistoryLists(string stockId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("MessagingStock", "READ", tenant);

            messagingStockUsageHistoryRepository = new MessagingStockUsageHistoryRepository(tenant);
            messagingStockUsageHistoryQuery = new MessagingStockUsageHistoryQuery(messagingStockUsageHistoryRepository);

            IQueryable<MessagingStockUsageHistory> iQueryable = messagingStockUsageHistoryRepository.GetMessagingStockUsageHistories(stockId, tenant);
            IQueryable<MessagingStockUsageHistoryList> query2 = messagingStockUsageHistoryQuery.GetIQueryableEntityList(iQueryable, tenant);

            return query2;
        }
    }
}