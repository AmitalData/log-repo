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
        private AWBMessagingStockQuery aWBMessagingStockQuery;
        private AWBMessagingStockRepository aWBMessagingStockRepository;

        private AWBStockUsageHistoryQuery aWBStockUsageHistoryQuery;
        private AWBStockUsageHistoryRepository aWBStockUsageHistoryRepository;

        public AWBMessagingStockPM GetSingleAWBMessagingStockPM(string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            SecurityUtility.CheckContactFeature("AWBMessagingStock", "READ", 0);

            aWBMessagingStockQuery = new AWBMessagingStockQuery(tenant);
            AWBMessagingStockPM myResult = aWBMessagingStockQuery.GetSinglePM(entityId, tenant);
            myResult.DummyTenant = tenant;

            return myResult;
        }

        public AWBMessagingStockList GetSingleAWBMessagingStockList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            SecurityUtility.CheckContactFeature("AWBMessagingStock", "READ", 0);

            aWBMessagingStockRepository = new AWBMessagingStockRepository(tenant);
            aWBMessagingStockQuery = new AWBMessagingStockQuery(aWBMessagingStockRepository);

            AWBMessagingStockList entityList = null;
            AWBStocksDataView entityPOCO = aWBMessagingStockRepository.GetSingleAWBStocksDataView(id);

            if (entityPOCO != null)
            {
                List<AWBStocksDataView> singleEntityList = new List<AWBStocksDataView>();
                singleEntityList.Add(entityPOCO);

                IQueryable<AWBStocksDataView> iQueryable = singleEntityList.AsQueryable();
                IQueryable<AWBMessagingStockList> iQueryableEntityList = aWBMessagingStockQuery.GetIQueryableEntityList(iQueryable, tenant);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public void UpdateAWBMessagingStock(AWBMessagingStockList entityList)
        {

        }

        public IQueryable<AWBMessagingStockList> GetAWBMessagingStockListForTenantManagmentTab(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            SecurityUtility.CheckContactFeature("AWBMessagingStock", "READ", 0);

            aWBMessagingStockRepository = new AWBMessagingStockRepository(tenant);
            aWBMessagingStockQuery = new AWBMessagingStockQuery(aWBMessagingStockRepository);

            IQueryable<AWBStocksDataView> iQueryable = aWBMessagingStockRepository.GetAWBStocksDataViewsByTenant(tenant);
            IQueryable<AWBMessagingStockList> query2 = aWBMessagingStockQuery.GetIQueryableEntityList(iQueryable, tenant);

            return query2;
        }

        public IQueryable<AWBMessagingStockList> GetAWBMessagingStockLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            SecurityUtility.CheckContactFeature("AWBMessagingStock", "READ", 0);

            aWBMessagingStockRepository = new AWBMessagingStockRepository(tenant);
            aWBMessagingStockQuery = new AWBMessagingStockQuery(aWBMessagingStockRepository);

            IQueryable<AWBStocksDataView> iQueryable = aWBMessagingStockRepository.GetAWBStocksDataViews();
            IQueryable<AWBMessagingStockList> query2 = aWBMessagingStockQuery.GetIQueryableEntityList(iQueryable, tenant);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AWBMessagingStockList> GetAWBMessagingStockFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            SecurityUtility.CheckContactFeature("AWBMessagingStock", "READ", 0);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            aWBMessagingStockRepository = new AWBMessagingStockRepository(0);
            aWBMessagingStockQuery = new AWBMessagingStockQuery(aWBMessagingStockRepository);

            IQueryable<AWBStocksDataView> iQueryable = aWBMessagingStockRepository.GetAWBStocksDataViews();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AWBStocksDataView>(nonListQueryOperation, iQueryable);
            int skippedEntities = queryOperations.PageIndex;

            IQueryable<AWBMessagingStockList> query2 = aWBMessagingStockQuery.GetIQueryableEntityList(iQueryable, tenant);

            query2 = filter.GetFilteredQuery<AWBMessagingStockList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AWBMessagingStockList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> objectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AWBMessagingStock", tenant).ToList();

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
                                query2 = sortClass.GetSorterQuery<AWBMessagingStockList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AWBMessagingStockList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AWBMessagingStockList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AWBMessagingStockList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AWBMessagingStockList, bool>(queryOperations, query2);
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
            SecurityUtility.CheckContactFeature("AWBMessagingStock", "READ", 0);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            aWBMessagingStockRepository = new AWBMessagingStockRepository(0);
            aWBMessagingStockQuery = new AWBMessagingStockQuery(aWBMessagingStockRepository);
            IQueryable<AWBStocksDataView> iQueryable = aWBMessagingStockRepository.GetAWBStocksDataViews();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            iQueryable = filter.GetFilteredQuery<AWBStocksDataView>(nonListQueryOperation, iQueryable);

            IQueryable<AWBMessagingStockList> query2 = aWBMessagingStockQuery.GetIQueryableEntityList(iQueryable, tenant);

            query2 = filter.GetFilteredQuery<AWBMessagingStockList>(listQueryOperation, query2);
            int count = query2.Count();

            return count;
        }

        public void InsertAWBMessagingStock(AWBMessagingStockPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            SecurityUtility.CheckContactFeature("AWBMessagingStock", "NEW", 0);

            if (objectContext == null)
            {
                objectContext = ShipmentsContext.GetContext(entityPM.TenantNumber);
            }

            AWBMessagingStockService service = new AWBMessagingStockService(objectContext, entityPM);
            service.Create();

            TableLastUpdateClass.UpdateTableHistory(entityPM.TenantNumber, "AWBMessagingStock");
        }

        public void UpdateAWBMessagingStock(AWBMessagingStockPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            SecurityUtility.CheckContactFeature("AWBMessagingStock", "UPDATE", 0);

            if (objectContext == null)
            {
                objectContext = ShipmentsContext.GetContext(entityPM.TenantNumber);
            }

            AWBMessagingStockService service = new AWBMessagingStockService(objectContext, entityPM);
            service.Update();

            if (this.ChangeSet != null)
            {
                this.ChangeSet.Associate(entityPM, service.entityPoco, MapStockPMToStock);
            }

            TableLastUpdateClass.UpdateTableHistory(entityPM.TenantNumber, "AWBMessagingStock");
        }

        private void MapStockPMToStock(AWBMessagingStockPM entityPM, AWBMessagingStock entityPOCO)
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

        public void DeleteAWBMessagingStock(AWBMessagingStockPM entityPM)
        {

        }

        // For Logged Tenant View Only
        public IQueryable<AWBMessagingStockList> GetLoggedTenantAWBMessagingStockLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AWBMessagingStock", "READ", tenant);

            aWBMessagingStockRepository = new AWBMessagingStockRepository(tenant);
            aWBMessagingStockQuery = new AWBMessagingStockQuery(aWBMessagingStockRepository);

            IQueryable<AWBStocksDataView> iQueryable = aWBMessagingStockRepository.GetAWBStocksDataViewsByTenant(tenant);
            IQueryable<AWBMessagingStockList> query2 = aWBMessagingStockQuery.GetIQueryableEntityList(iQueryable, tenant);

            return query2;
        }

        public IQueryable<AWBStockUsageHistoryList> GetLoggedTenantAWBStockUsageHistoryLists(string stockId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AWBMessagingStock", "READ", tenant);

            aWBStockUsageHistoryRepository = new AWBStockUsageHistoryRepository(tenant);
            aWBStockUsageHistoryQuery = new AWBStockUsageHistoryQuery(aWBStockUsageHistoryRepository);

            IQueryable<AWBStockUsageHistory> iQueryable = aWBStockUsageHistoryRepository.GetAWBStockUsageHistories(stockId, tenant);
            IQueryable<AWBStockUsageHistoryList> query2 = aWBStockUsageHistoryQuery.GetIQueryableEntityList(iQueryable, tenant);

            return query2;
        }
    }
}