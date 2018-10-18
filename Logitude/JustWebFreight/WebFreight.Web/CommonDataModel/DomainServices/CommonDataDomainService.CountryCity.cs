using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
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
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        private CountryCityQuery countryCityQuery;
        private CountryCityRepository countryCityRepository;

        public void UpdateCountryCityList(CountryCityList currentEntity)
        {
        }

        public CountryCityPM GetCountryCityById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CountryCity", "READ", tenant);

            countryCityQuery = new CountryCityQuery(tenant);
            CountryCityPM CountryCity = countryCityQuery.GetSinglePM(id, tenant);
            return CountryCity;
        }

        public CountryCityList GetSingleCountryCityList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CountryCity", "READ", tenant);

            countryCityRepository = new CountryCityRepository(tenant);
            CountryCityList CountryCityList = null;
            CountryCity CountryCity = countryCityRepository.GetSingleCountryCity(id, tenant);

            if (CountryCity != null)
            {
                List<CountryCity> singleEntityList = new List<CountryCity>();
                singleEntityList.Add(CountryCity);

                countryCityQuery = new CountryCityQuery(countryCityRepository);
                IQueryable<CountryCity> iQueryable = singleEntityList.AsQueryable();
                IQueryable<CountryCityList> iQueryableEntityList = countryCityQuery.GetIQueryableEntityList(iQueryable);
                CountryCityList = iQueryableEntityList.FirstOrDefault();
            }
            return CountryCityList;
        }

        public IQueryable<CountryCityList> GetCountryCityLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CountryCity", "READ", tenant);

            countryCityRepository = new CountryCityRepository(tenant);
            countryCityQuery = new CountryCityQuery(countryCityRepository);

            IQueryable<CountryCity> CountryCitys = countryCityRepository.GetCountryCities(tenant);
            IQueryable<CountryCityList> query2 = countryCityQuery.GetIQueryableEntityList(CountryCitys);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CountryCityList> GetCountryCityFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CountryCity", "READ", tenant);

            countryCityRepository = new CountryCityRepository(tenant);
            countryCityQuery = new CountryCityQuery(countryCityRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CountryCity> CountryCities = countryCityRepository.GetCountryCities(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            CountryCities = filter.GetFilteredQuery<CountryCity>(nonListQueryOperation, CountryCities);

            int skippedPorts = queryOperations.PageIndex;
            IQueryable<CountryCityList> query2 = countryCityQuery.GetIQueryableEntityList(CountryCities);
            query2 = filter.GetFilteredQuery<CountryCityList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CountryCityList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CountryCity", tenant).ToList();
                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CountryCityList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CountryCityList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CountryCityList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CountryCityList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CountryCityList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.EnglishName);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.EnglishName);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetCountryCityFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CountryCity", "READ", tenant);

            countryCityRepository = new CountryCityRepository(tenant);
            countryCityQuery = new CountryCityQuery(countryCityRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<CountryCity> CountryCities = countryCityRepository.GetCountryCities(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            CountryCities = filter.GetFilteredQuery<CountryCity>(nonListQueryOperation, CountryCities);

            IQueryable<CountryCityList> query2 = countryCityQuery.GetIQueryableEntityList(CountryCities);

            query2 = filter.GetFilteredQuery<CountryCityList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertCountryCity(CountryCityPM entityPM)
        {
            SecurityUtility.CheckContactFeature("CountryCity", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            CountryCityService service = new CountryCityService(objectContext, entityPM.Tenant);
            service.Create(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "CountryCity");
        }

        public void UpdateCountryCity(CountryCityPM entityPM)
        {
            SecurityUtility.CheckContactFeature("CountryCity", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            CountryCityService service = new CountryCityService(objectContext, entityPM.Tenant);
            service.Update(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "CountryCity");
        }

        public void DeleteCountryCity(CountryCityPM entityPM)
        {

        }

    }
}