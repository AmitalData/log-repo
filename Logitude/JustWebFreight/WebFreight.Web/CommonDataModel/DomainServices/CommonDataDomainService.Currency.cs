using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Logitude.Server.Tools.Counters;
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
        public void UpdateCurrencyList(CurrencyList currentEntity)
        {
        }

        public IQueryable<Currency> GetCurrencies(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Currency", "READ", tenant);

            currencyRepository = new CurrencyRepository(tenant);
            return currencyRepository.GetCurrencies(0);
        }

        public IQueryable<CurrencyPM> GetCurrenciesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Currency", "READ", tenant);

            currencyQuery = new CurrencyQuery(tenant);
            return currencyQuery.GetCurrenciesByTenantPM(tenant).Where(d => d.Tenant == tenant);
        }

        public bool DoesCurrencyCodeExist(string code, int tenant)
        {
            currencyRepository = new CurrencyRepository(tenant);
            return (currencyRepository.GetCurrencies(tenant).Where(d => d.Code == code && d.Tenant == tenant)).Any();
        }

        public IQueryable<CurrencyPM> GetCurrenciesSearch(string code, string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Currency", "READ", tenant);

            currencyQuery = new CurrencyQuery(tenant);
            IQueryable<CurrencyPM> q = currencyQuery.GetCurrencyByCodeOrName(code, name, tenant).Where(d => d.Tenant == tenant);
            return q;
        }

        public IQueryable<CurrencyPM> GetFirstCurrenciesByTenant(string input, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Currency", "READ", tenant);

            currencyQuery = new CurrencyQuery(tenant);
            input = input.ToUpper();
            return currencyQuery.GetCurrenciesByTenantPM(tenant).Where(d => d.Tenant == tenant).Where(p => p.Code.ToUpper().StartsWith(input) || p.EnglishName.ToUpper().StartsWith(input));
        }

        public IQueryable<CurrencyList> GetCurrencyLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Currency", "READ", tenant);

            currencyRepository = new CurrencyRepository(tenant);
            currencyQuery = new CurrencyQuery(currencyRepository);

            IQueryable<Currency> currencies = currencyRepository.GetCurrencies(tenant);
            IQueryable<CurrencyList> query2 = currencyQuery.GetIQueryableEntityList(currencies);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CurrencyList> GetCurrencyFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Currency", "READ", tenant);

            currencyRepository = new CurrencyRepository(tenant);
            currencyQuery = new CurrencyQuery(currencyRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Currency> currencies = currencyRepository.GetCurrencies(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            currencies = filter.GetFilteredQuery<Currency>(nonListQueryOperation, currencies);

            int skippedCurrencies = queryOperations.PageIndex;
            IQueryable<CurrencyList> query2 = currencyQuery.GetIQueryableEntityList(currencies);

            query2 = filter.GetFilteredQuery<CurrencyList>(listQueryOperation, query2);
           
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CurrencyList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Currency", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CurrencyList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CurrencyList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CurrencyList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CurrencyList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CurrencyList, bool>(queryOperations, query2);
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
            
            query2 = query2.Skip(skippedCurrencies);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetCurrencyFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Currency", "READ", tenant);

            currencyRepository = new CurrencyRepository(tenant);
            currencyQuery = new CurrencyQuery(currencyRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Currency> currencies = currencyRepository.GetCurrencies(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            currencies = filter.GetFilteredQuery<Currency>(nonListQueryOperation, currencies);

            IQueryable<CurrencyList> query2 = currencyQuery.GetIQueryableEntityList(currencies);

            query2 = filter.GetFilteredQuery<CurrencyList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public CurrencyList GetSingleCurrencyList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Currency", "READ", tenant);

            currencyRepository = new CurrencyRepository(tenant);
            currencyQuery = new CurrencyQuery(currencyRepository);

            CurrencyList currencyList = null;
            Currency currency = currencyRepository.GetSingleCurrency(id, tenant);

            if (currency != null)
            {
                List<Currency> singleEntityList = new List<Currency>();
                singleEntityList.Add(currency);

                IQueryable<Currency> iQueryable = singleEntityList.AsQueryable();
                IQueryable<CurrencyList> iQueryableEntityList = currencyQuery.GetIQueryableEntityList(iQueryable);
                currencyList = iQueryableEntityList.FirstOrDefault();
            }
            return currencyList;
        }

        public CurrencyPM GetSingleCurrency(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Currency", "READ", tenant);

            currencyQuery = new CurrencyQuery(tenant);
            return currencyQuery.GetSinglePM(id, tenant);
        }

        public IQueryable<CurrencyPM> GetCurrenciesByTenantInput(int tenant, string input, bool byCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Currency", "READ", tenant);

            currencyQuery = new CurrencyQuery(tenant);
            input = input.Trim().ToUpper();
            if (input != String.Empty)
            {

                if (byCode)
                {
                    return currencyQuery.GetCurrenciesByTenantPM(tenant).Where(d => d.Tenant == tenant && d.Code.ToUpper().StartsWith(input.ToUpper()));
                }
                else
                {
                    return currencyQuery.GetCurrenciesByTenantPM(tenant).Where(d => d.Tenant == tenant && d.EnglishName.ToUpper().StartsWith(input.ToUpper()));
                }
            }
            else
            {
                return currencyQuery.GetCurrenciesByTenantPM(tenant).Where(d => d.Tenant == tenant);
            }
        }

        public IQueryable<CurrencyPM> GetSingleCurrencyByTenantInput(int tenant, string input, bool byCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Currency", "READ", tenant);

            currencyQuery = new CurrencyQuery(tenant);
            if (byCode)
            {
                return currencyQuery.GetCurrenciesByTenantPM(tenant).Where(d => d.Tenant == tenant && d.Code.ToUpper() == input.ToUpper());
            }
            else
            {
                return currencyQuery.GetCurrenciesByTenantPM(tenant).Where(d => d.Tenant == tenant && d.EnglishName.ToUpper().StartsWith(input.ToUpper()));
            }
        }

        [Invoke]
        public CurrencyList CopyCurrencyToTenant(string currencyId, int tenant, double currecyRate,DateTime ratedate,int? unit = 1)
        {
            
            SecurityUtility.CheckContactFeature("Currency", "NEW", tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }
            currencyRepository = new CurrencyRepository(objectContext);
            currencyQuery = new CurrencyQuery(currencyRepository);
            tenantRepository = new TenantRepository(objectContext);
            Tenant tenantPoco = tenantRepository.GetSingleTenant(tenant);

           
            Currency zeroCurrency = currencyRepository.GetSingleCurrency(currencyId, 0);
            CurrencyPM tenantCurrency = currencyQuery.GetSingleCurrencyByCode(zeroCurrency.Code, tenant);
           
            if (tenantCurrency == null)
            {
                tenantCurrency = new CurrencyPM()
                {
                    //Id = IdCounter.GetNumber("Currency", tenant),
                    Code = zeroCurrency.Code,
                    AccountingExternalCode = zeroCurrency.AccountingExternalCode,
                    AddedManually = zeroCurrency.AddedManually,
                    EnglishName = zeroCurrency.EnglishName,
                    InActive = zeroCurrency.InActive,
                    LocalName = zeroCurrency.LocalName,
                    Notes = zeroCurrency.Notes,
                    SearchFields = zeroCurrency.SearchFields,
                    Tenant = tenant,
                    Sign = zeroCurrency.Sign,
                };

                this.InsertCurrency(tenantCurrency);
                
                objectContext.SaveChanges();
                //Tenant = tenantPM.Id,
                //  BaseCurrencyId = tenantPM.CurrencyId,
                //  ForeignCurrencyId = tenantPM.ProfitCurrencyId,
                //  LogDateTime = TenantContext.Current.GetCurrentDateTimeAsUtc(),
                //  ValueDate = TenantContext.Current.GetCurrentDateAsUtc(),
                //  Rate = tenantPM.ProfitCurrencyRate,
                using (TransactionScope scope = TransactionFactory.GetTransaction())//TransactionFactory.GetTransaction())
                {
                    IWebFreightContext webContext = WebFreightContext.GetContext(tenant);
                    RatesTable rate;
                    if (IsFullAccountingActivated(tenant))
                    {
                         rate = new RatesTable()
                     {
                        Id = IdCounter.GetNumber("RatesTable", tenant),
                        BaseCurrencyId = tenantPoco.CurrencyId,
                        ForeignCurrencyId = tenantCurrency.Id,
                        LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant),
                        Rate = CalculateRateAccordingUnit(currecyRate,unit),
                        Unit = unit,
                        Tenant = tenant,
                        ValueDate = ratedate.Date,//TenantServerConfigration.GetCurrentDateTime(tenant),

                     };
                    } else
                    {
                         rate = new RatesTable()
                        {
                            Id = IdCounter.GetNumber("RatesTable", tenant),
                            BaseCurrencyId = tenantPoco.CurrencyId,
                            ForeignCurrencyId = tenantCurrency.Id,
                            LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant),
                            Rate = currecyRate,
                            Tenant = tenant,
                            ValueDate = ratedate.Date,//TenantServerConfigration.GetCurrentDateTime(tenant),

                        };
                    }

                    webContext.RatesTable.Add(rate);
                    webContext.SaveChanges();

                    scope.Complete();
                }
            }

            CurrencyList List = this.GetSingleCurrencyList(tenantCurrency.Id, tenant);

            return List;

        }

        [Invoke]

        private double CalculateRateAccordingUnit(Double currecyRate, int? unit)
        {
             if (unit != null)
             {
                 if (unit > 0)
                 {
                   return (double)(currecyRate / unit);
                 }
             }
              return (double)currecyRate;
        }

        private bool IsFullAccountingActivated(int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            bool isFullAccountingActivated = tenantPOCO.AccountingActivated;
            return isFullAccountingActivated;
        }

        public Currency CreateCurrency(Currency currency)
        {
            SecurityUtility.CheckContactFeature("Currency", "NEW", currency.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currency.Tenant);
            }
            currencyRepository = new CurrencyRepository(objectContext);
            currency.Id = IdCounter.GetNumber("Currency", currency.Tenant).ToString();
            
            using (TransactionScope scope = TransactionFactory.GetTransaction())//TransactionFactory.GetTransaction())
            {
                currencyRepository.Add(currency);
                objectContext.SaveChanges();
                scope.Complete();

                return currency;
            }
        }

        public void InsertCurrency(CurrencyPM currency)
        {
            SecurityUtility.CheckContactFeature("Currency", "NEW", currency.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currency.Tenant);
            }
            currencyRepository = new CurrencyRepository(objectContext);           
            currency.Id = IdCounter.GetNumber("Currency", currency.Tenant).ToString();

            bool exist = (from a in currencyRepository.GetCurrencies(currency.Tenant)
                          where a.Code == currency.Code && a.Tenant == currency.Tenant
                          select a).Any();
            if (!exist)
            {
                CurrencyService service = new CurrencyService(objectContext, currency.Tenant);
                service.Create(currency);

                //Currency newCurrency = new Currency();
                //newCurrency.Id = currency.Id;
                //MapCurrencyCurrencyPM(currency, newCurrency);
                //currencyRepository.Add(newCurrency);
                
                ////create TraceEvent
                //WebFreightDomainService webfreightService = new WebFreightDomainService();
                //ContactRepository contactsRepositorypository = new ContactRepository(objectContext);
                //ContactQuery contactQuery = new ContactQuery(contactsRepositorypository);
                //ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), currency.Tenant, true);
                //if (contact != null) EventTracer.CreateTraceEvent(new TraceEvent(), "CRCR", currency.Tenant, contact.Id, currency.Id, null, "Currency", null, null, false);
                
                TableLastUpdateClass.UpdateTableHistory(currency.Tenant, "Currency");
            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", currency.Tenant);
                msg = msg.Replace("%Entity", "Currency");
                throw new Exception(msg);
            }
        }

        public string GetTenantCurrency(string currencyId, int currentTenant)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentTenant);
            }
            
            currencyRepository = new CurrencyRepository(objectContext);
            currencyQuery = new CurrencyQuery(currencyRepository);

            string id = currencyId;

            CurrencyPM currency = currencyQuery.GetSinglePM(currencyId, 0);
            if (currency != null)
            {
                CurrencyPM tenantCurrency = currencyQuery.GetSingleCurrencyByCode(currency.Code, currentTenant);

                if (tenantCurrency == null)
                {
                    tenantCurrency = new CurrencyPM()
                    {
                        Id = IdCounter.GetNumber("Currency", currentTenant),
                        AccountingExternalCode = currency.AccountingExternalCode,
                        AddedManually = currency.AddedManually,
                        Code = currency.Code,
                        EnglishName = currency.EnglishName,
                        InActive = currency.InActive,
                        LocalName = currency.LocalName,
                        Notes = currency.Notes,
                        Tenant = currentTenant,
                    };

                    CommonDataDomainService service = new CommonDataDomainService();
                    service.InsertCurrency(tenantCurrency);
                }

                id = tenantCurrency.Id;                 
            }

            return id;
        }

        public void UpdateCurrency(CurrencyPM currentCurrency)
        {
            SecurityUtility.CheckContactFeature("Currency", "UPDATE", currentCurrency.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentCurrency.Tenant);
            }
            currencyRepository = new CurrencyRepository(objectContext);
           
            string entityName = "Currency" + currentCurrency.Id + currentCurrency.Tenant;
            string entityPmName = "CurrencyPM" + currentCurrency.Id + currentCurrency.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            bool exist = (from a in currencyRepository.GetCurrencies(currentCurrency.Tenant)
                          where a.Code == currentCurrency.Code && a.Id != currentCurrency.Id && a.Tenant == currentCurrency.Tenant
                          select a).Any();

            if (!exist)
            {
                CurrencyService service = new CurrencyService(objectContext, currentCurrency.Tenant);
                service.Update(currentCurrency);
                TableLastUpdateClass.UpdateTableHistory(currentCurrency.Tenant, "Currency");
            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", currentCurrency.Tenant);
                msg = msg.Replace("%Entity", "Currency");
                throw new Exception(msg);

            }
        }

        public void DeleteCurrency(CurrencyPM currency)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currency.Tenant);
            }
            Currency entity = CurrencyRepository.GetSingleCurrency(currency.Id, currency.Tenant, false);
            currencyRepository.Remove(entity);
        }
    }
}