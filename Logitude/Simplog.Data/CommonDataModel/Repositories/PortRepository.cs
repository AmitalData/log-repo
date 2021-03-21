using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class PortRepository : IRepository<Port>
    {
        ICommonDataContext commonDataContext;

        public PortRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public PortRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public PortRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public int GetPortsCount(int tenant)
        {
            return (from record in context.Ports.Include("Country") where record.Tenant == tenant select record).Count();
        }

        public Port GetUnsignedPort(int tenant)
        {
            Port data = (from r in context.Ports.Include("Country")
                         where r.Code == "---"
                         && r.Country.Code == "--"
                         && r.Tenant == tenant
                         select r).FirstOrDefault();
            return data;
        }

        public IQueryable<Port> GetPorts(int tenant)
        {
            return (from record in context.Ports.Include("Country") where record.Tenant == tenant select record);
        }

        public Port GetSinglePort(string id, int tenant)
        {
            Port entity = (from a in context.Ports.Include("Country").Include("State")
                           where a.Id == id
                           select a).FirstOrDefault();

            return entity;
        }

        public Port GetSinglePort(int tenant, string id)
        {
            Port entity = (from a in context.Ports.Include("Country").Include("State")
                           where a.Id == id
                           select a).FirstOrDefault();

            return entity;
        }

        public Port GetSinglePortByCodeCountryCode(int tenant, string code, string countryCode, bool getFromCache)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "Port" + code + tenant;
                Port entity;

                if (getFromCache)
                {
                    if (CacheManager.CacheWrapper != null)
                    {
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            entity = (from a in context.Ports.Include("Country")
                                      where a.Tenant == tenant && a.Code == code && a.Country.Code == countryCode
                                      select a).FirstOrDefault();


                            if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                            {
                                CacheManager.CacheWrapper.Insert(entityName, entity, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }

                        }
                        else
                        {
                            entity = (Port)CacheManager.CacheWrapper.Get(entityName);
                        }
                    }
                    else
                    {
                        entity = (from record in context.Ports.Include("Country") where record.Code == code && record.Country.Code == countryCode && record.Tenant == tenant select record).FirstOrDefault();
                    }
                }
                else
                {
                    entity = (from record in context.Ports.Include("Country") where record.Code == code && record.Country.Code == countryCode && record.Tenant == tenant select record).FirstOrDefault();
                }
                return entity;

            }
            return null;
        }

        public Port GetSinglePortByCodeCountryId(int tenant, string code, string countryId, bool getFromCache)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "Port" + code + tenant;
                Port entity;

                if (getFromCache)
                {
                    if (CacheManager.CacheWrapper != null)
                    {
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            entity = (from a in context.Ports.Include("Country")
                                      where a.Tenant == tenant && a.Code == code && a.CountryId == countryId
                                      select a).FirstOrDefault();


                            if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                            {
                                CacheManager.CacheWrapper.Insert(entityName, entity, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }

                        }
                        else
                        {
                            entity = (Port)CacheManager.CacheWrapper.Get(entityName);
                        }
                    }
                    else
                    {
                        entity = (from record in context.Ports.Include("Country") where record.Code == code && record.CountryId == countryId && record.Tenant == tenant select record).FirstOrDefault();
                    }
                }
                else
                {
                    entity = (from record in context.Ports.Include("Country") where record.Code == code && record.CountryId == countryId && record.Tenant == tenant select record).FirstOrDefault();
                }
                return entity;

            }
            return null;
        }

        public Port GetSinglePortByCode(int tenant, string code, bool getFromCache)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "Port" + code + tenant;
                Port entity;

                if (getFromCache)
                {
                    if (CacheManager.CacheWrapper != null)
                    {
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            entity = (from a in context.Ports.Include("Country")
                                      where a.Tenant == tenant && a.Code == code
                                      select a).FirstOrDefault();

                            if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                            {
                                CacheManager.CacheWrapper.Insert(entityName, entity, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }

                        }
                        else
                        {
                            entity = (Port)CacheManager.CacheWrapper.Get(entityName);
                        }
                    }
                    else
                    {
                        entity = (from record in context.Ports.Include("Country") where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
                    }
                }
                else
                {
                    entity = (from record in context.Ports.Include("Country") where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
                }
                return entity;

            }
            return null;
        }

        public Port GetAirlinePortByCode(int myTenant, string myCode, bool getFromCache)
        {
            if (!string.IsNullOrEmpty(myCode))
            {
                string entityName = "AirlinePort" + myCode + myTenant;
                Port entity;

                if (getFromCache)
                {
                    if (CacheManager.CacheWrapper != null)
                    {
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            entity = (from a in context.Ports.Include("Country")
                                      where a.Tenant == myTenant && a.Code == myCode && a.IsAir
                                      select a).FirstOrDefault();

                            if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                            {
                                CacheManager.CacheWrapper.Insert(entityName, entity, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }

                        else
                        {
                            entity = (Port)CacheManager.CacheWrapper.Get(entityName);
                        }
                    }

                    else
                    {
                        entity = (from a in context.Ports.Include("Country") where a.Code == myCode && a.Tenant == myTenant && a.IsAir select a).FirstOrDefault();
                    }
                }

                else
                {
                    entity = (from a in context.Ports.Include("Country") where a.Code == myCode && a.Tenant == myTenant && a.IsAir select a).FirstOrDefault();
                }

                return entity;
            }

            return null;
        }

        public Port GetAirlineSinglePortByCodeCountryCode(int tenant, string code, string countryCode, bool getFromCache)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "Port" + code + tenant;
                Port entity;

                if (getFromCache)
                {
                    if (CacheManager.CacheWrapper != null)
                    {
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            entity = (from a in context.Ports.Include("Country")
                                      where a.Tenant == tenant && a.Code == code && a.Country.Code == countryCode && a.IsAir
                                      select a).FirstOrDefault();


                            if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                            {
                                CacheManager.CacheWrapper.Insert(entityName, entity, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }

                        }
                        else
                        {
                            entity = (Port)CacheManager.CacheWrapper.Get(entityName);
                        }
                    }
                    else
                    {
                        entity = (from record in context.Ports.Include("Country") where record.Code == code && record.Country.Code == countryCode && a.IsAir && record.Tenant == tenant select record).FirstOrDefault();
                    }
                }
                else
                {
                    entity = (from record in context.Ports.Include("Country") where record.Code == code && record.Country.Code == countryCode && a.IsAir && record.Tenant == tenant select record).FirstOrDefault();
                }
                return entity;

            }
            return null;
        }

        public Port GetOceanPortByCode(int myTenant, string myCode, bool getFromCache)
        {
            if (!string.IsNullOrEmpty(myCode))
            {
                string entityName = "OceanPort" + myCode + myTenant;
                Port entity;

                if (getFromCache)
                {
                    if (CacheManager.CacheWrapper != null)
                    {
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            entity = (from a in context.Ports.Include("Country")
                                      where a.Tenant == myTenant && a.Code == myCode && a.IsOcean
                                      select a).FirstOrDefault();

                            if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                            {
                                CacheManager.CacheWrapper.Insert(entityName, entity, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }

                        else
                        {
                            entity = (Port)CacheManager.CacheWrapper.Get(entityName);
                        }
                    }

                    else
                    {
                        entity = (from a in context.Ports.Include("Country") where a.Code == myCode && a.Tenant == myTenant && a.IsOcean select a).FirstOrDefault();
                    }
                }

                else
                {
                    entity = (from a in context.Ports.Include("Country") where a.Code == myCode && a.Tenant == myTenant && a.IsOcean select a).FirstOrDefault();
                }

                return entity;
            }

            return null;
        }

        public IQueryable<Port> GetSinglePortByCode(string input, bool byCode, int tenant)
        {
            if (byCode)
            {
                IQueryable<Port> ports = (from a in context.Ports.Include("Country")
                                          where a.Tenant == tenant && a.Code.ToUpper() == input.ToUpper().Trim()
                                          select a);
                return ports;
            }
            else
            {
                IQueryable<Port> ports = (from a in context.Ports.Include("Country")
                                          where a.Tenant == tenant && a.EnglishName.ToUpper() == input.ToUpper().Trim()
                                          select a);
                return ports;
            }
        }

        public IQueryable<Port> GetPortsByNameOrCode(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in context.Ports.Include("Country")
                         where a.Tenant == tenant
                         select a).AsQueryable();

            IQueryable<Port> query2 = null;
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

        public Port GetFirstPort(int tenant)
        {
            return (from a in context.Ports.Include("Country")
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public void Add(Port entity)
        {
            context.Ports.Add(entity);
        }

        public void Remove(Port entity)
        {
            context.Ports.Attach(entity);
            context.Ports.Remove(entity);
        }

        public void Update(Port entity)
        {
            try
            {
                context.Ports.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<Port> All()
        {
            return context.Ports.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<Port> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Port GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Port GetOceanPortByCodeAndCountryCode(int tenant, string iPortCode, string iCountryCode, bool getFromCache)
        {
            Port iResult = null;

            if (!string.IsNullOrEmpty(iPortCode) && !string.IsNullOrEmpty(iCountryCode))
            {
                string entityName = "Port" + tenant + iPortCode + iCountryCode;

                if (getFromCache)
                {
                    if (CacheManager.CacheWrapper != null)
                    {
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            iResult = (from a in context.Ports.Include("Country")
                                       where a.Tenant == tenant
                                       && a.IsOcean == true
                                       && a.Code == iPortCode
                                       && a.Country.Code == iCountryCode
                                       select a).FirstOrDefault();

                            if (CacheManager.CacheWrapper.Get(entityName) == null && iResult != null)
                            {
                                CacheManager.CacheWrapper.Insert(entityName, iResult, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }

                        else
                        {
                            iResult = (Port)CacheManager.CacheWrapper.Get(entityName);
                        }
                    }

                    else
                    {
                        iResult = (from a in context.Ports.Include("Country")
                                   where a.Tenant == tenant
                                   && a.IsOcean == true
                                   && a.Code == iPortCode
                                   && a.Country.Code == iCountryCode
                                   select a).FirstOrDefault();
                    }
                }

                else
                {
                    iResult = (from a in context.Ports.Include("Country")
                               where a.Tenant == tenant
                               && a.IsOcean == true
                               && a.Code == iPortCode
                               && a.Country.Code == iCountryCode
                               select a).FirstOrDefault();
                }

            }

            return iResult;
        }

        public Port GetSinglePortIdByCombinedCode(string code, int tenant)
        {
            var entity = (from a in context.Ports.Include("Country").Include("State")
                          where a.CombinedCode == code && a.Tenant == tenant
                          select a).FirstOrDefault();
            return entity;
        }

        public Port GetOceanPortByCombinedCode(string code, int tenant)
        {
            var entity = (from a in context.Ports.Include("Country").Include("State")
                          where a.CombinedCode == code && a.Tenant == tenant && a.IsOcean
                          select a).FirstOrDefault();
            return entity;
        }

        public IQueryable<Port> GetAirlinePortsByName(string name, int tenant)
        {
            return (from a in context.Ports.Include("Country")
                    where a.Tenant == tenant && a.EnglishName == name && a.IsAir
                    select a);
        }

        public IQueryable<Port> GetOceanPortsByName(string name, int tenant)
        {
            return (from a in context.Ports.Include("Country")
                    where a.Tenant == tenant && a.EnglishName == name && a.IsOcean
                    select a);
        }
    }
}
