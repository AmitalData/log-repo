using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.Tools.EntityService;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public void UpdateRatesTableList(RatesTableList currentEntity)
        {

        }

        //public IQueryable<RatesTable> GetRatesTables(int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    ratesTablesRepository = new RatesTableRepository(tenant);
        //    return ratesTablesRepository.GetRatesTablesByTenant(0);
        //}

        public RatesTablePM GetSingleRatesTable(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ratesTablesRepository = new RatesTableRepository(tenant);
            ratesTableQuery = new RatesTableQuery(ratesTablesRepository);
            return ratesTableQuery.GetSinglePM(id, tenant);
        }

        public RatesTableList GetSingleRatesTableList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ratesTablesRepository = new RatesTableRepository(tenant);
            RatesTableList ratesTableList = null;
            RatesTable ratesTable = ratesTablesRepository.GetSingleRatesTable(id, tenant);

            if (ratesTable != null)
            {
                List<RatesTable> singleEntityList = new List<RatesTable>();
                singleEntityList.Add(ratesTable);

                IQueryable<RatesTable> iQueryable = singleEntityList.AsQueryable();
                ratesTableQuery = new RatesTableQuery(ratesTablesRepository);
                IQueryable<RatesTableList> iQueryableEntityList = ratesTableQuery.GetIQueryableEntityList(iQueryable);
                ratesTableList = iQueryableEntityList.FirstOrDefault();
            }
            return ratesTableList;
        }

        public IQueryable<RatesTableList> GetRatesTableLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ratesTablesRepository = new RatesTableRepository(tenant);
            IQueryable<RatesTable> iQueryable = ratesTablesRepository.GetRatesTables(tenant);

            ratesTableQuery = new RatesTableQuery(ratesTablesRepository);
            IQueryable<RatesTableList> query2 = ratesTableQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<RatesTableList> GetRatesTableFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ratesTablesRepository = new RatesTableRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<RatesTable> iQueryable = ratesTablesRepository.GetRatesTables(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<RatesTable>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            ratesTableQuery = new RatesTableQuery(ratesTablesRepository);
            IQueryable<RatesTableList> query2 = ratesTableQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<RatesTableList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(RatesTableList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("RatesTable", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<RatesTableList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<RatesTableList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<RatesTableList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<RatesTableList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<RatesTableList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Id);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Id);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetRatesTableFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ratesTablesRepository = new RatesTableRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<RatesTable> iQueryable = ratesTablesRepository.GetRatesTables(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<RatesTable>(nonListQueryOperation, iQueryable);
            ratesTableQuery = new RatesTableQuery(ratesTablesRepository);
            IQueryable<RatesTableList> query2 = ratesTableQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<RatesTableList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public List<LastRate> GetCurrenciesExchangeRateByValueDate(int tenant, string baseCurrencyId, DateTime? date,bool calculateRateAccordingNumberUnit = false)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            List<LastRate> resultList = new List<LastRate>();

            if (string.IsNullOrEmpty(baseCurrencyId))
            {
                string msg = TranslateTextsClass.Translate("General.M.AccountingCurrencyIsNotSet", tenant);
                throw new ApplicationException(msg);
            }

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(tenant);
            }

            ratesTablesRepository = new RatesTableRepository(objectContext);
            ratesTableQuery = new RatesTableQuery(ratesTablesRepository,tenant);
            CurrencyRepository currencyRepository = new CurrencyRepository(tenant);
            Currency baseCurrency = currencyRepository.GetCurrencies(tenant).Where(r => r.Id == baseCurrencyId).FirstOrDefault();
            List<Currency> foreignCurrencies = currencyRepository.GetCurrencies(tenant).Where(c => c.Id != baseCurrencyId).ToList();

            foreach (Currency currency in foreignCurrencies)
            {
                LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, currency.Id, baseCurrencyId, date, calculateRateAccordingNumberUnit);
                if (lastRate != null)
                {
                    lastRate.BaseCurrencyId = baseCurrencyId;
                    lastRate.BaseCurrencyCode = baseCurrency.Code;
                    resultList.Add(lastRate);
                }
                else
                {
                    LastRate newLastRate = new LastRate()
                    {
                        Id = IdCounter.GetNumber("LastRate", tenant).ToString(),
                        Tenant = tenant,
                        ForeignCurrencyId = currency.Id,
                        ForeignCurrencyCode = currency.Code,
                        ForeignCurrencyName = currency.EnglishName,
                        BaseCurrencyId = baseCurrency.Id,
                        BaseCurrencyCode = baseCurrency.Code,
                        HistoryCount = 0,
                        Rate = null,
                    };
                    resultList.Add(newLastRate);
                }
            }
            return resultList;
        }

        public void UpdateLastRate(LastRate currentEntity) { }

        public void InsertLastRate(LastRate entity) { }

        public void InsertRatesTable(RatesTablePM entity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entity.Tenant);
            }

            RatesTableService service = new RatesTableService(objectContext , entity.Tenant);
            service.Create(entity);
        }

        public void UpdateRatesTable(RatesTablePM currentEntity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }

            RatesTableService service = new RatesTableService(objectContext, currentEntity.Tenant);
            service.Update(currentEntity);           
        }

        public void DeleteRatesTable(RatesTablePM entity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            ratesTablesRepository = new RatesTableRepository(objectContext);
            RatesTable ratesTable = ratesTablesRepository.GetSingleRatesTable(entity.Id, entity.Tenant);
            ratesTablesRepository.Remove(ratesTable);
        }

        public List<RatesTablePM> GetRatesByValueDate(int tenant, string baseCurrencyId, DateTime? date)
        {
            List<RatesTablePM> resultList = new List<RatesTablePM>();

            if (string.IsNullOrEmpty(baseCurrencyId))
            {
                string msg = TranslateTextsClass.Translate("General.M.AccountingCurrencyIsNotSet", tenant);
                throw new ApplicationException(msg);
            }

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(tenant);
            }

            ratesTablesRepository = new RatesTableRepository(objectContext);
            ratesTableQuery = new RatesTableQuery(ratesTablesRepository);
            CurrencyRepository currencyRepository = new CurrencyRepository(tenant);
            Currency baseCurrency = currencyRepository.GetCurrencies(tenant).Where(r => r.Id == baseCurrencyId).FirstOrDefault();
            List<Currency> foreignCurrencies = currencyRepository.GetCurrencies(tenant).Where(c => c.Id != baseCurrencyId).ToList();

            foreach (Currency currency in foreignCurrencies)
            {
                RatesTablePM rate = ratesTableQuery.GetLastRateByValueDate(tenant, currency.Id, baseCurrencyId, date);
                if (rate != null)
                {
                    rate.BaseCurrencyId = baseCurrencyId;
                    resultList.Add(rate);
                }
                else
                {
                    RatesTablePM newLastRate = new RatesTablePM()
                    {
                        Id = IdCounter.GetNumber("RatesTable", tenant).ToString(),
                        Tenant = tenant,
                        ForeignCurrencyId = currency.Id,
                        ForeignCurrencyCode = currency.Code,
                        ForeignCurrencyName = currency.EnglishName,
                        BaseCurrencyId = baseCurrency.Id,
                        Rate = null,
                    };
                    resultList.Add(newLastRate);
                }
            }
            return resultList;
        }
    }
}