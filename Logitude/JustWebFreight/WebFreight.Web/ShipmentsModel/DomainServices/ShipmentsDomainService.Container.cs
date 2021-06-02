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
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            ContainerRepository containerRepository = new ContainerRepository(tenant);
            ContainerQuery containerQuery = new ContainerQuery(tenant);
            IQueryable<Container> iQueryable = containerRepository.GetContainers(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

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
            return query2;
        }

        public int GetContainerFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Container", "READ", tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            ContainerRepository containerRepository = new ContainerRepository(tenant);
            IQueryable<Container> iQueryable = containerRepository.GetContainers(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Container>(nonListQueryOperation, iQueryable);

            var query2 = from a in iQueryable
                         select new ContainerList()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             SearchFields = a.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<ContainerList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

    }
}