using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class AdditionalServiceQuery
    {
        AdditionalServiceRepository repository;
        public AdditionalServiceQuery()
        {
            repository = new AdditionalServiceRepository(); 
        }

        public AdditionalServiceQuery(int tenant)
        {
            repository = new AdditionalServiceRepository(tenant);
        }

        public AdditionalServiceQuery(AdditionalServiceRepository AdditionalServiceRepository)
        {
            repository = AdditionalServiceRepository;
        }

        public AdditionalServicePM GetSinglePM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "AdditionalServicePM" + id + tenant;
                AdditionalServicePM entity;
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var AdditionalServicees = (from a in repository.context.AdditionalServices
                                        where a.Tenant == tenant
                                        select new AdditionalServicePM()
                                        {
                                            Name = a.Name,
                                            Id = a.Id,
                                            Tenant = a.Tenant,
                                            SearchFields = a.SearchFields,
                                            InActive = a.InActive,
                                        });

                        foreach (var c in AdditionalServicees)
                        {
                            string cname = "AdditionalServicePM" + c.Id + c.Tenant;

                            if (CacheManager.CacheWrapper.Get(cname) == null)
                            {
                                CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (AdditionalServicePM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (AdditionalServicePM)CacheManager.CacheWrapper.Get(entityName);                        
                    }
                }
                else
                {
                    entity = (from a in repository.context.AdditionalServices
                              where a.Tenant == tenant && a.Id == id
                              select new AdditionalServicePM()
                              {
                                  Name = a.Name,
                                  Id = a.Id,
                                  Tenant = a.Tenant,
                                  SearchFields = a.SearchFields,
                                  InActive = a.InActive,
                              }).FirstOrDefault();
                }

                return entity;
            }
            return null;
        }

        public IQueryable<AdditionalServicePM> GetAdditionalServicePMsByTenant(int tenant)
        {
            IQueryable<AdditionalServicePM> AdditionalServicees = from a in repository.context.AdditionalServices
                                            where a.Tenant == tenant
                                            select new AdditionalServicePM()
                                            {
                                                Name = a.Name,
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                SearchFields = a.SearchFields,
                                                InActive = a.InActive,
                                            };
            return AdditionalServicees;
        }

        public AdditionalServicePM GetAdditionalServiceByName(string name, int tenant)
        {
            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.AdditionalServices
                         where a.Tenant == tenant && a.Name == name
                         select new AdditionalServicePM()
                         {
                             Name = a.Name,
                             Id = a.Id,
                             Tenant = a.Tenant,
                             SearchFields = a.SearchFields,
                             InActive = a.InActive,
                         }).FirstOrDefault();
            return query;
        }

        public IQueryable<AdditionalServiceList> GetIQueryableEntityList(IQueryable<AdditionalService> iQueryable)
        {
            IQueryable<AdditionalServiceList> result = from a in iQueryable
                                            select new AdditionalServiceList()
                                            {
                                                Name = a.Name,
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                SearchFields = a.SearchFields,
                                                InActive = a.InActive,
                                            };
            return result;
        }
    }
}
