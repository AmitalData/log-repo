using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
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
using WebFreight.Web.Security;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private INTTRASIStatusRepository iNTTRASIStatusRepository;

        public INTTRASIStatusList GetSingleINTTRASIStatusList(string code, int tenant)
        {
            iNTTRASIStatusRepository = new INTTRASIStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            INTTRASIStatus entity = iNTTRASIStatusRepository.GetSingleINTTRASIStatus(code);
            INTTRASIStatusList entityList = new INTTRASIStatusList()
            {
                Code = entity.Code,
                Name = entity.Name,
                SearchFields = entity.SearchFields,
            };
            return entityList;
        }

        public IQueryable<INTTRASIStatusList> GetINTTRASIStatusLists(int tenant)
        {
            iNTTRASIStatusRepository = new INTTRASIStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            IQueryable<INTTRASIStatus> iQueryable = iNTTRASIStatusRepository.GetINTTRASIStatus();
            var query2 = from entity in iQueryable
                         select new INTTRASIStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<INTTRASIStatusList> GetINTTRASIStatusFilters(byte[] xmlFilters, int tenant)
        {
            iNTTRASIStatusRepository = new INTTRASIStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<INTTRASIStatus> iQueryable = iNTTRASIStatusRepository.GetINTTRASIStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<INTTRASIStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            var query2 = from entity in iQueryable
                         select new INTTRASIStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<INTTRASIStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(INTTRASIStatusList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("INTTRASIStatus", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<INTTRASIStatusList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<INTTRASIStatusList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<INTTRASIStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<INTTRASIStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<INTTRASIStatusList, bool>(queryOperations, query2);
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
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetINTTRASIStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            iNTTRASIStatusRepository = new INTTRASIStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<INTTRASIStatus> iQueryable = iNTTRASIStatusRepository.GetINTTRASIStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<INTTRASIStatus>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new INTTRASIStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<INTTRASIStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertINTTRASIStatus(INTTRASIStatus entity)
        {
            iNTTRASIStatusRepository = new INTTRASIStatusRepository(0);
            iNTTRASIStatusRepository.Add(entity);
        }

        public void UpdateINTTRASIStatus(INTTRASIStatus currentEntity)
        {
            iNTTRASIStatusRepository = new INTTRASIStatusRepository(0);
            iNTTRASIStatusRepository.Update(currentEntity);
        }

        public void DeleteINTTRASIStatus(INTTRASIStatus entity)
        {
            iNTTRASIStatusRepository = new INTTRASIStatusRepository(0);
            iNTTRASIStatusRepository.Remove(entity);
        }
    }
}