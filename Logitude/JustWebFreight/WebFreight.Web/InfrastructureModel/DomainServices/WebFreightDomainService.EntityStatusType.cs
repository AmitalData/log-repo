using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Security;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {

        public IQueryable<EntityStatusTypeList> GetEntityStatusTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("EntityStatusType", "READ", tenant);
            EntityStatusTypeRepository EntityStatusTypeRepository;
            EntityStatusTypeRepository = new EntityStatusTypeRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<EntityStatusType> EntityStatusTypes = EntityStatusTypeRepository.GetEntityStatusTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            EntityStatusTypes = filter.GetFilteredQuery<EntityStatusType>(nonListQueryOperation, EntityStatusTypes);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            EntityStatusTypeQuery EntityStatusTypeQuery = new EntityStatusTypeQuery(EntityStatusTypeRepository);
            IQueryable<EntityStatusTypeList> query2 = EntityStatusTypeQuery.GetIQueryableEntityList(EntityStatusTypes);

            query2 = filter.GetFilteredQuery<EntityStatusTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(EntityStatusTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> EntityStatusTypeObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("EntityStatusType", tenant).ToList();

                ObjectField objectField = (from a in EntityStatusTypeObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<EntityStatusTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<EntityStatusTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<EntityStatusTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<EntityStatusTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<EntityStatusTypeList, bool>(queryOperations, query2);
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

        public int GetEntityStatusTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            EntityStatusTypeRepository EntityStatusTypeRepository;
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("EntityStatusType", "READ", tenant);

            EntityStatusTypeRepository = new EntityStatusTypeRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<EntityStatusType> EntityStatusTypes = EntityStatusTypeRepository.GetEntityStatusTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            EntityStatusTypes = filter.GetFilteredQuery<EntityStatusType>(nonListQueryOperation, EntityStatusTypes);
            EntityStatusTypeQuery EntityStatusTypeQuery = new EntityStatusTypeQuery(EntityStatusTypeRepository);
            IQueryable<EntityStatusTypeList> query2 = EntityStatusTypeQuery.GetIQueryableEntityList(EntityStatusTypes);

            query2 = filter.GetFilteredQuery<EntityStatusTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

    }
}