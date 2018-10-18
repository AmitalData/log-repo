using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateCountryList(CountryList currentEntity)
        {
        }

        public IQueryable<Country> GetCountries(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Country", "READ", tenant);

            countryRepository = new CountryRepository(tenant);
            return this.countryRepository.GetCountries(0);
        }

        public IQueryable<CountryPM> GetCountryPMsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Country", "READ", tenant);

            countryQuery = new CountryQuery(tenant);
            return this.countryQuery.GetCountryPMsByTenant(tenant).Where(c => c.Tenant == tenant);
        }

        public IQueryable<Country> GetCountriesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Country", "READ", tenant);

            countryRepository = new CountryRepository(tenant);
            return this.countryRepository.GetCountries(tenant).Where(c => c.Tenant == tenant);
        }

        public CountryPM GetSingleCountryPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Country", "READ", tenant);

            countryQuery = new CountryQuery(tenant);
            return countryQuery.GetSinglePM(id, tenant);
        }

        public IQueryable<CountryList> GetCountryLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Country", "READ", tenant);

            countryRepository = new CountryRepository(tenant);
            countryQuery = new CountryQuery(countryRepository);

            IQueryable<Country> countries = countryRepository.GetCountries(tenant);
            IQueryable<CountryList> query2 = countryQuery.GetIQueryableEntityList(countries);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CountryList> GetCountryFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Country", "READ", tenant);

            countryRepository = new CountryRepository(tenant);
            countryQuery = new CountryQuery(countryRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Country> countries = countryRepository.GetCountries(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            countries = filter.GetFilteredQuery<Country>(nonListQueryOperation, countries);
            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CountryList> query2 = countryQuery.GetIQueryableEntityList(countries);

            query2 = filter.GetFilteredQuery<CountryList>(listQueryOperation, query2);
            //-------------------------------------------------------------------------------
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CountryList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Country", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CountryList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CountryList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CountryList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CountryList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CountryList, bool>(queryOperations, query2);
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
            //--------------------------------------------------------------------------------------------------
            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetCountryFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Country", "READ", tenant);

            countryRepository = new CountryRepository(tenant);
            countryQuery = new CountryQuery(countryRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Country> countries = countryRepository.GetCountries(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            countries = filter.GetFilteredQuery<Country>(nonListQueryOperation, countries);

            IQueryable<CountryList> query2 = countryQuery.GetIQueryableEntityList(countries);

            query2 = filter.GetFilteredQuery<CountryList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public CountryList GetSingleCountryList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Country", "READ", tenant);

            countryRepository = new CountryRepository(tenant);

            CountryList countryList = null;
            Country country = countryRepository.GetSingleCountry(id, tenant);

            if (country != null)
            {
                List<Country> singleEntityList = new List<Country>();
                singleEntityList.Add(country);

                countryQuery = new CountryQuery(countryRepository);
                IQueryable<Country> iQueryable = singleEntityList.AsQueryable();
                IQueryable<CountryList> iQueryableEntityList = countryQuery.GetIQueryableEntityList(iQueryable);
                countryList = iQueryableEntityList.FirstOrDefault();
            }
            return countryList;
        }

        public IQueryable<CountryPM> GetCountriesByTenantInput(int tenant, string input, bool byCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Country", "READ", tenant);

            countryQuery = new CountryQuery(tenant);

            input = input.Trim().ToUpper();
            if (input != String.Empty)
            {
                if (byCode)
                {
                    return countryQuery.GetCountryPMsByTenant(tenant).Where(d => d.Tenant == tenant && d.Code.ToUpper().StartsWith(input.ToUpper()));
                }
                else
                {
                    return countryQuery.GetCountryPMsByTenant(tenant).Where(d => d.Tenant == tenant && d.EnglishName.ToUpper().StartsWith(input.ToUpper()));
                }
            }
            else
            {
                return countryQuery.GetCountryPMsByTenant(tenant).Where(d => d.Tenant == tenant);
            }
        }

        public IQueryable<CountryPM> GetSingleCountryByTenantInput(int tenant, string input, bool byCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Country", "READ", tenant);

            countryQuery = new CountryQuery(tenant);
            if (byCode)
            {
                return countryQuery.GetCountryPMsByTenant(tenant).Where(d => d.Tenant == tenant && d.Code.ToUpper() == input.ToUpper());
            }
            else
            {
                return countryQuery.GetCountryPMsByTenant(tenant).Where(d => d.Tenant == tenant && d.EnglishName.ToUpper().StartsWith(input.ToUpper()));
            }
        }

        public IQueryable<CountryPM> GetCountriesFirstFifty(int tenant, string input)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Country", "READ", tenant);

            countryQuery = new CountryQuery(tenant);
            if (input != String.Empty)
            {
                return this.countryQuery.GetCountryPMsByTenant(tenant).Where(c => c.Tenant == tenant && (c.EnglishName.ToLower().StartsWith(input.ToLower()) || c.Code.ToLower().StartsWith(input.ToLower()))).Take(50).OrderBy(c => c.EnglishName);
            }
            else
            {
                return this.countryQuery.GetCountryPMsByTenant(tenant).Where(c => c.Tenant == tenant).Take(50).OrderBy(c => c.EnglishName);
            }
        }

        public IQueryable<CountryPM> GetCountriesSearch(string code, string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Country", "READ", tenant);

            countryQuery = new CountryQuery(tenant);
            IQueryable<CountryPM> q = countryQuery.GetCountriesByCodeOrName(code, name, tenant);
            return q;
        }

        public void InsertCountry(CountryPM entityPm)
        {
            SecurityUtility.CheckContactFeature("Country", "NEW", entityPm.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPm.Tenant);
            }

            CountryService service = new CountryService(objectContext, entityPm.Tenant);
            service.Create(entityPm);

            TableLastUpdateClass.UpdateTableHistory(entityPm.Tenant, "Country");
        }

        public void UpdateCountry(CountryPM currententityPm)
        {
            SecurityUtility.CheckContactFeature("Country", "UPDATE", currententityPm.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currententityPm.Tenant);
            }

            CountryService service = new CountryService(objectContext, currententityPm.Tenant);
            service.Update(currententityPm);

            TableLastUpdateClass.UpdateTableHistory(currententityPm.Tenant, "Country");
        }

        public void DeleteCountry(CountryPM entityPm)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPm.Tenant);
            }

            Country removedEntity = CountryRepository.GetSingleCountry(entityPm.Id, entityPm.Tenant, false);
            this.countryRepository.Remove(removedEntity);
        }
    }
}