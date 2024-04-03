using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.Security;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.CustomFilters;
using Logitude.BL.Helpers;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public IQueryable<ContainerList> GetContainerFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Container", "READ", tenant);

            SecurityUtility.AuthenticationOnTenant(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            queryOperations.QueryFilterItems = GetQueryFilterItems("Container", tenant, queryOperations.QueryFilterItems);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            ContainerRepository containerRepository = new ContainerRepository(tenant);
            ContainerQuery containerQuery = new ContainerQuery(tenant);
            ContainerCustomFilter customfilters = new ContainerCustomFilter(tenant);
            IQueryable<Container> iQueryable = containerRepository.GetContainers(tenant);
            iQueryable = customfilters.GetFilteredQuery(queryOperations, iQueryable);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<Container>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;
            var query2 = containerQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<ContainerList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ContainerList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> entityObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Container", tenant).ToList();

                ObjectField objectField = (from a in entityObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<ContainerList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ContainerList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ContainerList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ContainerList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ContainerList, bool>(queryOperations, query2);
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
                query2 = query2.OrderByDescending(d => d.CreateDate);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            List<ContainerList> listQuery = query2.ToList(); 
            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("Container", tenant, listQuery.Cast<object>().ToList()); 

            return listQuery.AsQueryable();
        }

        public int GetContainerFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Container", "READ", tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            queryOperations.QueryFilterItems = GetQueryFilterItems("Container", tenant, queryOperations.QueryFilterItems);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            ContainerRepository containerRepository = new ContainerRepository(tenant);
            IQueryable<Container> iQueryable = containerRepository.GetContainers(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<Container>(nonListQueryOperation, iQueryable);
            ContainerQuery containerQuery = new ContainerQuery(tenant);
            var query2 = containerQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ContainerList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        private List<QueryFilterItem> GetQueryFilterItems(string objectTableName, int tenant, List<QueryFilterItem> queryFilterItems)
        {
            QueryOperations queryOperations = new QueryOperations();
            List<ObjectField> ContainerObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName(objectTableName, tenant);
            foreach (QueryFilterItem queryFilterItem in queryFilterItems)
            {
                SetQueryFilterItem(queryOperations, ContainerObjectFields, queryFilterItem);
            }
            return queryOperations.QueryFilterItems;
        }
        private void SetQueryFilterItem(QueryOperations queryOperations, List<ObjectField> ContainerObjectFields, QueryFilterItem queryFilterItem)
        {
            ObjectField field = ContainerObjectFields.FirstOrDefault(f => f.FieldName == queryFilterItem.FieldName);
            if (field == null)
            {
                queryOperations.SetFilter(queryFilterItem.FieldName, queryFilterItem.FieldValue, queryFilterItem.IsCustom, queryFilterItem.Operator, queryFilterItem.FieldValue2, queryFilterItem.DisplayInList);
                return;
            }
            string valuestring1 = queryFilterItem.FieldValue != null ? queryFilterItem.FieldValue.ToString() : null;
            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);
            string valuestring2 = queryFilterItem.FieldValue2 != null ? queryFilterItem.FieldValue2.ToString() : null;
            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);
            queryOperations.SetFilter(queryFilterItem.FieldName, value1, field.IsCustomFilter, queryFilterItem.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode, field.IsListFilter);
        }
    }
}