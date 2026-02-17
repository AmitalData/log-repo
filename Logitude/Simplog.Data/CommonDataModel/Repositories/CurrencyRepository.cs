using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CurrencyRepository:IRepository<Currency>
    {
        ICommonDataContext commonDataContext;

        public CurrencyRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CurrencyRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CurrencyRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<Currency> GetCurrencies(int tenant)
        {
            return (from record in context.Currencies where record.Tenant == tenant select record);
        }

        public static Currency GetSingleCurrency(string id, int tenant,bool getFromCache)
        {
            string entityName = "Currency" + id + tenant;
            Currency entity;
            if (getFromCache)
            {
               
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        ICommonDataContext context = CommonDataContext.GetContext(tenant);
                        var currencies = (from a in context.Currencies
                                          where a.Tenant == tenant
                                          select a);

                        foreach (var c in currencies)
                        {
                            string name = "Currency" + c.Id + tenant;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, c, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (Currency)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (Currency)CacheManager.CacheWrapper.Get(entityName);

                    }
                
         
            }
            else
            {
                ICommonDataContext context = CommonDataContext.GetContext(tenant);
                entity = (from record in context.Currencies where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
            }
            return entity;            
        }

        public static Currency GetSingleCurrencyByCode(string code, int tenant, bool getFromCache)
        {
            string entityName = "Currency" + code + tenant;
            Currency entity;
            if (getFromCache)
            {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        ICommonDataContext context = CommonDataContext.GetContext(tenant);
                        var currencies = (from a in context.Currencies
                                          where a.Tenant == tenant
                                          select a);

                        foreach (var c in currencies)
                        {
                            string name = "Currency" + c.Code + tenant;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, c, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (Currency)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (Currency)CacheManager.CacheWrapper.Get(entityName);

                    }
                
           
            }
            else
            {
                ICommonDataContext context = CommonDataContext.GetContext(tenant);
                entity = (from record in context.Currencies where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
            }
            return entity;
        }

        public  Currency GetSingleCurrency(string id, int tenant)
        {
         
            Currency entity = (from a in context.Currencies
                                          where a.Id == id
                                          select a).FirstOrDefault();

            return entity;
        }


        public Currency GetSingleCurrencyByCode(string code, int tenant)
        {

            Currency entity = (from a in context.Currencies
                               where a.Tenant == tenant && a.Code == code
                               select a).FirstOrDefault();

            return entity;
        }

        public Currency GetSingleCurrencyByIdOrCode(string InvoiceCurrency, int tenant)
        {
            return context.Currencies
                          .FirstOrDefault(a => a.Tenant == tenant && (a.Code == InvoiceCurrency || a.Id == InvoiceCurrency));
        }

        public Currency GetSingleCurrencyById(string id, int tenant, bool getFromCache)
        {
            string entityName = "Currency" + id + tenant;
            Currency entity;
            if (getFromCache)
            {
               
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        ICommonDataContext context = CommonDataContext.GetContext(tenant);
                        var currencies = (from a in context.Currencies
                                          where a.Tenant == tenant
                                          select a);

                        foreach (var c in currencies)
                        {
                            string name = "Currency" + c.Id + tenant;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, c, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (Currency)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (Currency)CacheManager.CacheWrapper.Get(entityName);

                    }
                
             
            }
            else
            {
                ICommonDataContext context = CommonDataContext.GetContext(tenant);
                entity = (from record in context.Currencies where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
            }
            return entity;
        }

        public void Add(Currency entity)
        {
            context.Currencies.Add(entity);
        }

        public void Remove(Currency entity)
        {
            try { context.Currencies.Attach(entity); }
            catch { }
            context.Currencies.Remove(entity);
        }

        public void Update(Currency entity)
        {
            try
            {
                context.Currencies.Attach(entity);
            }
            catch
            { }
            context.SetAsModified(entity);
        }

        public List<Currency> All()
        {
            return context.Currencies.ToList();

        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<Currency> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Currency GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}