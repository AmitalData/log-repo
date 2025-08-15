using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CurrencyQuery : Logitude.BL.CommonDataModel.EntityQueries.ICurrencyQuery
    {
        CurrencyRepository repository;



        public CurrencyQuery(int tenant)
        {
            repository = new CurrencyRepository(tenant);
        }

        public CurrencyQuery(CurrencyRepository currencyRepository)
        {
            repository = currencyRepository;
        }

        public CurrencyPM GetSingleCurrencyByCode(string code, int tenant)
        {
            CurrencyPM entity = (from a in repository.context.Currencies
                                 where a.Tenant == tenant && a.Code == code
                                 select new CurrencyPM()
                                 {
                                     AddedManually = a.AddedManually,
                                     Code = a.Code,
                                     EnglishName = a.EnglishName,
                                     Id = a.Id,
                                     InActive = a.InActive,
                                     LocalName = a.LocalName,
                                     Notes = a.Notes,
                                     Tenant = a.Tenant,
                                     SearchFields = a.SearchFields,
                                     ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                     AccountingExternalCode = a.AccountingExternalCode,
                                     Sign = a.Sign,
                                 }).FirstOrDefault();
 

            return entity;
        }

        public CurrencyPM GetSinglePM(string id, int tenant)
        {
            string entityName = "CurrencyPM" + id + tenant;
               
            CurrencyPM entity;
            if (true)/// HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    var currencies = (from a in repository.context.Currencies
                                      where a.Tenant == tenant
                                      select new CurrencyPM()
                                      {
                                          AddedManually = a.AddedManually,
                                          Code = a.Code,
                                          EnglishName = a.EnglishName,
                                          Id = a.Id,
                                          InActive = a.InActive,
                                          LocalName = a.LocalName,
                                          Notes = a.Notes,
                                          SearchFields = a.SearchFields,
                                          Tenant = a.Tenant,
                                          ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                          AccountingExternalCode = a.AccountingExternalCode,
                                          Sign = a.Sign,
                                      });

                    foreach (var c in currencies)
                    {
                        string name = "CurrencyPM" + c.Id + tenant;

                        if (CacheManager.CacheWrapper.Get(name) == null)
                        {
                            CacheManager.CacheWrapper.Insert(name, c, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                    entity = (CurrencyPM)CacheManager.CacheWrapper.Get(entityName);
                }
                else
                {
                    entity = (CurrencyPM)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            else
            {
                entity = (from a in repository.context.Currencies
                          where a.Id == id && a.Tenant == tenant
                          select new CurrencyPM()
                          {
                              AddedManually = a.AddedManually,
                              Code = a.Code,
                              EnglishName = a.EnglishName,
                              Id = a.Id,
                              InActive = a.InActive,
                              LocalName = a.LocalName,
                              Notes = a.Notes,
                              SearchFields = a.SearchFields,
                              Tenant = a.Tenant,
                              ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                              AccountingExternalCode = a.AccountingExternalCode,
                              Sign = a.Sign,
                          }).FirstOrDefault();
            }

            if (entity != null)
            {
                entity.IsExternal = false;               

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);

                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        entity.IsExternal = true;
                    }
                }                

                CurrencyPM securedPm = new CurrencyPM();
                SecuredMapping.GetMappedPM(entity, securedPm, "Currency", tenant);

                return securedPm;
            }

            else
            {
                return null;
            }
        }

        public IQueryable<CurrencyPM> GetCurrenciesByTenantPM(int tenant)
        {
            IQueryable<CurrencyPM> currencies = from a in repository.context.Currencies
                                                where a.Tenant == tenant
                                                select new CurrencyPM()
                                                {
                                                    AddedManually = a.AddedManually,
                                                    Code = a.Code,
                                                    EnglishName = a.EnglishName,
                                                    Id = a.Id,
                                                    InActive = a.InActive,
                                                    LocalName = a.LocalName,
                                                    Notes = a.Notes,
                                                    Tenant = a.Tenant,
                                                    SearchFields = a.SearchFields,
                                                    ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                                    AccountingExternalCode = a.AccountingExternalCode,
                                                    Sign = a.Sign,
                                                };
            return currencies;
        }
        public List<string> GetCurencyCodesByTransactionsList(List<InterestTransactionList> interestTransactionLists, int tenant)
        {
            List<string> entityCurencyIds = interestTransactionLists.Select(d => d.CurrencyId).ToList();

            List<string> CurencyCodes = (from a in repository.context.Currencies
                                           where entityCurencyIds.Contains(a.Id) && a.Tenant == tenant
                                           select a.Id+","+a.Code).ToList();
 
            return CurencyCodes;
        }

        public IQueryable<CurrencyPM> GetCurrencyPMsByTenant(int tenant)
        {
            IQueryable<CurrencyPM> query = from a in repository.context.Currencies
                                          where a.Tenant == tenant
                                          select new CurrencyPM()
                                          {
                                              AddedManually = a.AddedManually,
                                              Code = a.Code,
                                              EnglishName = a.EnglishName,
                                              Id = a.Id,
                                              InActive = a.InActive,
                                              LocalName = a.LocalName,
                                              Notes = a.Notes,
                                              Tenant = a.Tenant,
                                              SearchFields = a.SearchFields,
                                              ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                              AccountingExternalCode = a.AccountingExternalCode,
                                              Sign = a.Sign,
                                          };
            return query;
        }

        public IQueryable<CurrencyPM> GetCurrencyByCodeOrName(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.Currencies
                         where a.Tenant == tenant
                         select new CurrencyPM()
                         {
                             AddedManually = a.AddedManually,
                             Code = a.Code,
                             EnglishName = a.EnglishName,
                             Id = a.Id,
                             InActive = a.InActive,
                             LocalName = a.LocalName,
                             Notes = a.Notes,
                             Tenant = a.Tenant,
                             SearchFields = a.SearchFields,
                             ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                             AccountingExternalCode = a.AccountingExternalCode,
                             Sign = a.Sign,
                         }).AsQueryable();

            IQueryable<CurrencyPM> query2 = null;
            if (!string.IsNullOrEmpty(code))
            {
                query2 = query.Where(d => d.Code.ToUpper().StartsWith(code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(name))
            {
                if (query2 != null)
                {
                    if (query2.Count() == 0)
                    {
                        query2 = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()));
                    }
                }
                else
                {
                    query2 = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()));
                }
            }
            if (query2 != null)
            {
                return query2;
            }
            else
                return query;
        }

        public IQueryable<CurrencyList> GetIQueryableEntityList(IQueryable<Currency> iQueryable)
        {
            IQueryable<CurrencyList> result = from entity in iQueryable
                                              select new CurrencyList()
                                              {
                                                  Notes = entity.Notes,
                                                  InActive = entity.InActive,
                                                  AddedManually = entity.AddedManually,
                                                  Code = entity.Code,
                                                  EnglishName = entity.EnglishName,
                                                  Id = entity.Id,
                                                  Tenant = entity.Tenant,
                                                  SearchFields = entity.SearchFields,
                                                  AccountingExternalCode = entity.AccountingExternalCode,
                                                  Sign = entity.Sign,
                                                  LocalName = entity.LocalName
                                              };
            return result;
        }

        public string GetCurrencyIdByCode(string code, int tenant)
        {
            return (from a in repository.context.Currencies
                    where a.Tenant == tenant && a.Code == code
                    select a.Id).FirstOrDefault();


        }

        public CurrencyPM GetSinglePMByCode(string code, int tenant)
        {
            CurrencyPM entity = (from a in repository.context.Currencies
                                 where a.Tenant == tenant && a.Code == code
                                 select new CurrencyPM()
                                 {
                                     AddedManually = a.AddedManually,
                                     Code = a.Code,
                                     EnglishName = a.EnglishName,
                                     Id = a.Id,
                                     InActive = a.InActive,
                                     LocalName = a.LocalName,
                                     Notes = a.Notes,
                                     Tenant = a.Tenant,
                                     SearchFields = a.SearchFields,
                                     ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                     AccountingExternalCode = a.AccountingExternalCode,
                                     Sign = a.Sign,
                                 }).FirstOrDefault();


            return entity;
        }
    }
}
