using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        private AddressTypeQuery addressTypeQuery;
        private AddressTypeRepository addressTypeRepository;

        public IQueryable<AddressType> GetAddressTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            addressTypesRepository = new AddressTypeRepository(tenant);
            return addressTypesRepository.GetAddressTypes();
        }

        public IQueryable<AddressType> GetAddressTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            addressTypesRepository = new AddressTypeRepository(tenant);
            return addressTypesRepository.GetAddressTypes();
        }

        public void InsertAddressType(AddressType addressType)
        {
            addressTypesRepository.Add(addressType);
        }

        public void UpdateAddressType(AddressType currentaddressType)
        {
            addressTypesRepository.Update(currentaddressType);
        }

        public void DeleteAddressType(AddressType addressType)
        {
            addressTypesRepository.Remove(addressType);
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AddressTypeList> GetAddressTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            addressTypeRepository = new AddressTypeRepository(tenant);
            addressTypeQuery = new AddressTypeQuery(addressTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AddressType> AddressTypes = addressTypeRepository.GetAddressTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            AddressTypes = filter.GetFilteredQuery<AddressType>(nonListQueryOperation, AddressTypes);

            int skippedPorts = queryOperations.PageIndex;
            IQueryable<AddressTypeList> query2 = addressTypeQuery.GetIQueryableEntityList(AddressTypes);
            query2 = filter.GetFilteredQuery<AddressTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AddressTypeList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AddressType", tenant).ToList();
                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AddressTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AddressTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AddressTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AddressTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AddressTypeList, bool>(queryOperations, query2);
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

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAddressTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
 
            addressTypeRepository = new AddressTypeRepository(tenant);
            addressTypeQuery = new AddressTypeQuery(addressTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AddressType> AddressTypes = addressTypeRepository.GetAddressTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            AddressTypes = filter.GetFilteredQuery<AddressType>(nonListQueryOperation, AddressTypes);

            IQueryable<AddressTypeList> query2 = addressTypeQuery.GetIQueryableEntityList(AddressTypes);

            query2 = filter.GetFilteredQuery<AddressTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}