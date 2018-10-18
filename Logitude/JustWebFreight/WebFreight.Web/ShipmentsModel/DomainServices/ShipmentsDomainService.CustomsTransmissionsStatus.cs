using Logitude.BL.ShipmentsModel.EntityLists;
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
        private CustomsTransmissionsStatusRepository CustomsTransmissionsStatusRepository;

        public void UpdateCustomsTransmissionsStatusList(CustomsTransmissionsStatusList entityList)
        {

        }

        public CustomsTransmissionsStatusList GetSingleCustomsTransmissionsStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            CustomsTransmissionsStatusRepository = new CustomsTransmissionsStatusRepository(tenant);
            CustomsTransmissionsStatus entityPOCO = CustomsTransmissionsStatusRepository.GetSingleCustomsTransmissionsStatus(code);
            CustomsTransmissionsStatusList entityList = new CustomsTransmissionsStatusList()
            {
                Code = entityPOCO.Code,
                Name = entityPOCO.Name,
                SearchFields = entityPOCO.SearchFields,
            };

            return entityList;
        }

        public IQueryable<CustomsTransmissionsStatusList> GetCustomsTransmissionsStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            CustomsTransmissionsStatusRepository = new CustomsTransmissionsStatusRepository(tenant);
            IQueryable<CustomsTransmissionsStatus> iQueryable = CustomsTransmissionsStatusRepository.GetCustomsTransmissionsStatus();
            var query2 = from entity in iQueryable
                         select new CustomsTransmissionsStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CustomsTransmissionsStatusList> GetCustomsTransmissionsStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            CustomsTransmissionsStatusRepository = new CustomsTransmissionsStatusRepository(tenant);
            IQueryable<CustomsTransmissionsStatus> iQueryable = CustomsTransmissionsStatusRepository.GetCustomsTransmissionsStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomsTransmissionsStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            var query2 = from entity in iQueryable
                         select new CustomsTransmissionsStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<CustomsTransmissionsStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CustomsTransmissionsStatusList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> myObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CustomsTransmissionsStatus", tenant).ToList();

                ObjectField objectField = (from a in myObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsTransmissionsStatusList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsTransmissionsStatusList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsTransmissionsStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsTransmissionsStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsTransmissionsStatusList, bool>(queryOperations, query2);
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

        public int GetCustomsTransmissionsStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            CustomsTransmissionsStatusRepository = new CustomsTransmissionsStatusRepository(tenant);
            IQueryable<CustomsTransmissionsStatus> iQueryable = CustomsTransmissionsStatusRepository.GetCustomsTransmissionsStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomsTransmissionsStatus>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new CustomsTransmissionsStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<CustomsTransmissionsStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}