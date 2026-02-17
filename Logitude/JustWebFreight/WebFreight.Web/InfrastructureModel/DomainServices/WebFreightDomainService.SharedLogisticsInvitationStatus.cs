using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
	{
        public void UpdateSharedLogisticsInvitationStatusList(SharedLogisticsInvitationStatusList currentEntity)
        {
        }

        public IQueryable<SharedLogisticsInvitationStatus> GetSharedLogisticsInvitationStatus(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            sharedLogisticsInvitationStatusRepository = new SharedLogisticsInvitationStatusRepository(tenant);
            return sharedLogisticsInvitationStatusRepository.GetSharedLogisticsInvitationStatus();
        }

        public IQueryable<SharedLogisticsInvitationStatusPM> GetSharedLogisticsInvitationStatusByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            sharedLogisticsInvitationStatusRepository = new SharedLogisticsInvitationStatusRepository(tenant);
            sharedLogisticsInvitationStatusQuery = new SharedLogisticsInvitationStatusQuery(sharedLogisticsInvitationStatusRepository);
            return sharedLogisticsInvitationStatusQuery.GetSharedLogisticsInvitationStatusPMs();
        }

        public SharedLogisticsInvitationStatusPM GetSingleSharedLogisticsInvitationStatus(int code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            sharedLogisticsInvitationStatusRepository = new SharedLogisticsInvitationStatusRepository(tenant);
            sharedLogisticsInvitationStatusQuery = new SharedLogisticsInvitationStatusQuery(sharedLogisticsInvitationStatusRepository);
            return sharedLogisticsInvitationStatusQuery.GetSingleSharedLogisticsInvitationStatusPM(code);
        }

        public SharedLogisticsInvitationStatusList GetSingleSharedLogisticsInvitationStatusList(int code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            sharedLogisticsInvitationStatusRepository = new SharedLogisticsInvitationStatusRepository(tenant);
            SharedLogisticsInvitationStatusList sharedLogisticsInvitationStatusList = null;
            SharedLogisticsInvitationStatus sharedLogisticsInvitationStatus = sharedLogisticsInvitationStatusRepository.GetSingleSharedLogisticsInvitationStatus(code);

            if (sharedLogisticsInvitationStatus != null)
            {
                List<SharedLogisticsInvitationStatus> singleEntityList = new List<SharedLogisticsInvitationStatus>();
                singleEntityList.Add(sharedLogisticsInvitationStatus);

                IQueryable<SharedLogisticsInvitationStatus> iQueryable = singleEntityList.AsQueryable();
                sharedLogisticsInvitationStatusQuery = new SharedLogisticsInvitationStatusQuery(sharedLogisticsInvitationStatusRepository);
                IQueryable<SharedLogisticsInvitationStatusList> iQueryableEntityList = sharedLogisticsInvitationStatusQuery.GetIQueryableEntityList(iQueryable);
                sharedLogisticsInvitationStatusList = iQueryableEntityList.FirstOrDefault();
            }
            return sharedLogisticsInvitationStatusList;
        }

        public IQueryable<SharedLogisticsInvitationStatusList> GetSharedLogisticsInvitationStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            sharedLogisticsInvitationStatusRepository = new SharedLogisticsInvitationStatusRepository(tenant);
            IQueryable<SharedLogisticsInvitationStatus> iQueryable = sharedLogisticsInvitationStatusRepository.GetSharedLogisticsInvitationStatus();
            sharedLogisticsInvitationStatusQuery = new SharedLogisticsInvitationStatusQuery(sharedLogisticsInvitationStatusRepository);
            IQueryable<SharedLogisticsInvitationStatusList> query2 = sharedLogisticsInvitationStatusQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<SharedLogisticsInvitationStatusList> GetSharedLogisticsInvitationStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            sharedLogisticsInvitationStatusRepository = new SharedLogisticsInvitationStatusRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<SharedLogisticsInvitationStatus> iQueryable = sharedLogisticsInvitationStatusRepository.GetSharedLogisticsInvitationStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<SharedLogisticsInvitationStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            sharedLogisticsInvitationStatusQuery = new SharedLogisticsInvitationStatusQuery(sharedLogisticsInvitationStatusRepository);
            IQueryable<SharedLogisticsInvitationStatusList> query2 = sharedLogisticsInvitationStatusQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<SharedLogisticsInvitationStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(SharedLogisticsInvitationStatusList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("SharedLogisticsInvitationStatus", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<SharedLogisticsInvitationStatusList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<SharedLogisticsInvitationStatusList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<SharedLogisticsInvitationStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<SharedLogisticsInvitationStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<SharedLogisticsInvitationStatusList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetSharedLogisticsInvitationStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            sharedLogisticsInvitationStatusRepository = new SharedLogisticsInvitationStatusRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<SharedLogisticsInvitationStatus> iQueryable = sharedLogisticsInvitationStatusRepository.GetSharedLogisticsInvitationStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<SharedLogisticsInvitationStatus>(nonListQueryOperation, iQueryable);
            sharedLogisticsInvitationStatusQuery = new SharedLogisticsInvitationStatusQuery(sharedLogisticsInvitationStatusRepository);
            IQueryable<SharedLogisticsInvitationStatusList> query2 = sharedLogisticsInvitationStatusQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<SharedLogisticsInvitationStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertSharedLogisticsInvitationStatus(SharedLogisticsInvitationStatus entity)
        {
            sharedLogisticsInvitationStatusRepository.Add(entity);
        }

        public void UpdateSharedLogisticsInvitationStatus(SharedLogisticsInvitationStatus currentEntity)
        {
            sharedLogisticsInvitationStatusRepository.Update(currentEntity);
        }

        public void DeleteSharedLogisticsInvitationStatus(SharedLogisticsInvitationStatus entity)
        {
            sharedLogisticsInvitationStatusRepository.Remove(entity);
        }
	}
}