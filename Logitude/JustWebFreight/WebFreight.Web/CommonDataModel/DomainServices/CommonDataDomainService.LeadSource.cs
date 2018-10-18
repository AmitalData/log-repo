using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateLeadSourceList(LeadSourceList currentEntity)
        {
        }

        public IQueryable<LeadSource> GetLeadSourcees(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("LeadSource", "READ", tenant);

            leadSourceRepository = new LeadSourceRepository(tenant);
            return leadSourceRepository.GetLeadSources(0);
        }

        public IQueryable<LeadSourcePM> GetLeadSourceesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("LeadSource", "READ", tenant);

            leadSourceQuery = new LeadSourceQuery(tenant);
            return leadSourceQuery.GetLeadSourcePMsByTenant(tenant);
        }

        public LeadSourcePM GetSingleLeadSource(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("LeadSource", "READ", tenant);

            leadSourceQuery = new LeadSourceQuery(tenant);
            return leadSourceQuery.GetSinglePM(id, tenant);
        }

        public LeadSourcePM GetLeadSourceById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("LeadSource", "READ", tenant);

            leadSourceQuery = new LeadSourceQuery(tenant);
            return leadSourceQuery.GetSinglePM(id, tenant);
        }

        public LeadSourceList GetSingleLeadSourceList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("LeadSource", "READ", tenant);

            leadSourceRepository = new LeadSourceRepository(tenant);
            leadSourceQuery = new LeadSourceQuery(leadSourceRepository);
            LeadSourceList LeadSourceList = null;
            LeadSource LeadSource = leadSourceRepository.GetSingleLeadSource(id, tenant);

            if (LeadSource != null)
            {
                List<LeadSource> singleEntityList = new List<LeadSource>();
                singleEntityList.Add(LeadSource);

                IQueryable<LeadSource> iQueryable = singleEntityList.AsQueryable();
                IQueryable<LeadSourceList> iQueryableEntityList = leadSourceQuery.GetIQueryableEntityList(iQueryable);
                LeadSourceList = iQueryableEntityList.FirstOrDefault();
            }
            return LeadSourceList;
        }

        public IQueryable<LeadSourceList> GetLeadSourceLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("LeadSource", "READ", tenant);

            leadSourceRepository = new LeadSourceRepository(tenant);
            leadSourceQuery = new LeadSourceQuery(leadSourceRepository);
            IQueryable<LeadSource> LeadSourcees = leadSourceRepository.GetLeadSources(tenant);
            IQueryable<LeadSourceList> query2 = leadSourceQuery.GetIQueryableEntityList(LeadSourcees);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<LeadSourceList> GetLeadSourceFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("LeadSource", "READ", tenant);

            leadSourceRepository = new LeadSourceRepository(tenant);
            leadSourceQuery = new LeadSourceQuery(leadSourceRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<LeadSource> LeadSourcees = leadSourceRepository.GetLeadSources(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            LeadSourcees = filter.GetFilteredQuery<LeadSource>(nonListQueryOperation, LeadSourcees);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<LeadSourceList> query2 = leadSourceQuery.GetIQueryableEntityList(LeadSourcees);

            query2 = filter.GetFilteredQuery<LeadSourceList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(LeadSourceList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("LeadSource", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<LeadSourceList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<LeadSourceList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<LeadSourceList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<LeadSourceList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<LeadSourceList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Name);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Name);
            }
            //--------------------------------------------------------------------------------------------------

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetLeadSourceFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("LeadSource", "READ", tenant);

            leadSourceRepository = new LeadSourceRepository(tenant);
            leadSourceQuery = new LeadSourceQuery(leadSourceRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<LeadSource> LeadSourcees = leadSourceRepository.GetLeadSources(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            LeadSourcees = filter.GetFilteredQuery<LeadSource>(nonListQueryOperation, LeadSourcees);

            IQueryable<LeadSourceList> query2 = leadSourceQuery.GetIQueryableEntityList(LeadSourcees);

            query2 = filter.GetFilteredQuery<LeadSourceList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertLeadSource(LeadSourcePM LeadSource)
        {
            SecurityUtility.CheckContactFeature("LeadSource", "NEW", LeadSource.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(LeadSource.Tenant);
            }
            LeadSourceService service = new LeadSourceService(objectContext, LeadSource.Tenant);
            service.Create(LeadSource);

            TableLastUpdateClass.UpdateTableHistory(LeadSource.Tenant, "LeadSource");
        }

        public void UpdateLeadSource(LeadSourcePM currentLeadSource)
        {
            SecurityUtility.CheckContactFeature("LeadSource", "UPDATE", currentLeadSource.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentLeadSource.Tenant);
            }

            leadSourceRepository = new LeadSourceRepository(objectContext);

            string entityName = "LeadSource" + currentLeadSource.Id + currentLeadSource.Tenant;
            string entityPmName = "LeadSourcePM" + currentLeadSource.Id + currentLeadSource.Tenant;

            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            LeadSourceService service = new LeadSourceService(objectContext, currentLeadSource.Tenant);
            service.Update(currentLeadSource);
            TableLastUpdateClass.UpdateTableHistory(currentLeadSource.Tenant, "LeadSource");
        }

        public void DeleteLeadSource(LeadSourcePM LeadSource)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(LeadSource.Tenant);
            }
            leadSourceRepository = new LeadSourceRepository(objectContext);
            LeadSource entity = leadSourceRepository.GetSingleLeadSource(LeadSource.Id, LeadSource.Tenant);
            leadSourceRepository.Remove(entity);
        }
    }
}