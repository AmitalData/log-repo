using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CountryRepository : IRepository<Country>
    {
        ICommonDataContext commonDataContext;

        public CountryRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CountryRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CountryRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<Country> GetCountries(int tenant)
        {
            return (from record in context.Countries.Include("GlobalZone") where record.Tenant == tenant select record);
        }

        public Country GetSingleCountryByCode(string code, int tenant)
        {
            return (from record in context.Countries.Include("GlobalZone") where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Country GetSingleCountryByName(string name, int tenant)
        {
            return (from record in context.Countries.Include("GlobalZone") where record.EnglishName == name && record.Tenant == tenant select record).FirstOrDefault();
        }
        public Country GetSingleCountry(string id, int tenant)
        {
            return (from record in context.Countries.Include("GlobalZone") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public static Country GetSingleCountry(string id, int tenant,bool getFromCache)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "Country" + id + tenant;
                Country entity;
                if (getFromCache)
                {
                    if (HttpContext.Current != null)
                    {
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            ICommonDataContext context = CommonDataContext.GetContext(tenant);
                            var countries = from a in context.Countries.Include("GlobalZone")
                                            where a.Tenant == tenant
                                            select a;

                            if (tenant == 0)
                            {
                                countries = countries.Where(c => c.Id == id);
                            }

                            foreach (var c in countries)
                            {
                                string name = "Country" + c.Id + tenant;
                                if (CacheManager.CacheWrapper.Get(name) == null)
                                {
                                    CacheManager.CacheWrapper.Insert(name, c, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                                }
                            }
                            entity = (Country)CacheManager.CacheWrapper.Get(entityName);
                        }
                        else
                        {
                            entity = (Country)CacheManager.CacheWrapper.Get(entityName);
                        }
                    }
                    else
                    {
                        ICommonDataContext context = CommonDataContext.GetContext(tenant);
                        Country country = (from record in context.Countries.Include("GlobalZone") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
                        entity = country;
                    }
                }
                else
                {
                    ICommonDataContext context = CommonDataContext.GetContext(tenant);
                    Country country = (from record in context.Countries.Include("GlobalZone") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
                    entity = country;
                }
                return entity;
            }
            return null;
        }


        public  Country GetSingleCountryByIdAndTenant(string id, int tenant, bool getFromCache)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "Country" + id + tenant;
                Country entity;
                if (getFromCache)
                {
                    if (HttpContext.Current != null)
                    {
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                          
                            var countries = from a in context.Countries.Include("GlobalZone")
                                            where a.Tenant == tenant
                                            select a;

                            if (tenant == 0)
                            {
                                countries = countries.Where(c => c.Id == id);
                            }

                            foreach (var c in countries)
                            {
                                string name = "Country" + c.Id + tenant;
                                if (CacheManager.CacheWrapper.Get(name) == null)
                                {
                                    CacheManager.CacheWrapper.Insert(name, c, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                                }
                            }
                            entity = (Country)CacheManager.CacheWrapper.Get(entityName);
                        }
                        else
                        {
                            entity = (Country)CacheManager.CacheWrapper.Get(entityName);
                        }
                    }
                    else
                    {
                      
                        Country country = (from record in context.Countries.Include("GlobalZone") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
                        entity = country;
                    }
                }
                else
                {
                    
                    Country country = (from record in context.Countries.Include("GlobalZone") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
                    entity = country;
                }
                return entity;
            }
            return null;
        }

        
        public Country GetSingleCountryByCode(string code, int tenant, bool getFromCache)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "Country" + code + tenant;
                Country entity;
                if (getFromCache)
                {
                    if (HttpContext.Current != null)
                    {
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            var countries = from a in context.Countries.Include("GlobalZone")
                                            where a.Tenant == tenant
                                            select a;

                            if (tenant == 0)
                            {
                                countries = countries.Where(c => c.Code == code);
                            }

                            foreach (var c in countries)
                            {
                                string name = "Country" + c.Code + tenant;
                                if (CacheManager.CacheWrapper.Get(name) == null)
                                {
                                    CacheManager.CacheWrapper.Insert(name, c, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                                }
                            }
                            entity = (Country)CacheManager.CacheWrapper.Get(entityName);
                        }
                        else
                        {
                            entity = (Country)CacheManager.CacheWrapper.Get(entityName);
                        }
                    }
                    else
                    {
                        Country country = (from record in context.Countries.Include("GlobalZone") where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
                        entity = country;
                    }
                }
                else
                {
                    Country country = (from record in context.Countries.Include("GlobalZone") where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();

                    entity = country;
                }
                return entity;
            }
            return null;
        }

        public Country GetUnsignedCountry(int tenant)
        {
            Country data = (from r in context.Countries.Include("GlobalZone")
                         where r.Code == "--"
                         && r.GlobalZone.Code == "--"
                         && r.Tenant == tenant
                         select r).FirstOrDefault();
            return data;
        }

        public void Add(Country entity)
        {
            context.Countries.Add(entity);
        }

        public void Remove(Country entity)
        {
            context.Countries.Attach(entity);
            context.Countries.Remove(entity);
        }

        public void Update(Country entity)
        {
            context.Countries.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Country> All()
        {
            return context.Countries.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
                
        }


        public List<Country> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Country GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }


        public string GetCountryIdByCode(string code, int tenant)
        {
            return (from record in context.Countries.Include("GlobalZone") where record.Code == code && record.Tenant == tenant select record.Id).FirstOrDefault();
        }

    }
}