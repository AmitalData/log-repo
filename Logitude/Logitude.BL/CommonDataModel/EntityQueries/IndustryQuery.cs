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

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class IndustryQuery
    {
        IndustryRepository repository;
        public IndustryQuery()
        {
               repository = new IndustryRepository(); 
        }

        public IndustryQuery(int tenant)
        {
            repository = new IndustryRepository(tenant);
        }

        public IndustryQuery(IndustryRepository IndustryRepository)
        {
            repository = IndustryRepository;
        }

        public IndustryPM GetSinglePM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "IndustryPM" + id + tenant;
                IndustryPM entity;
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var Industryes = (from a in repository.context.Industries
                                        where a.Tenant == tenant
                                        select new IndustryPM()
                                        {
                                            Name = a.Name,
                                            Id = a.Id,
                                            Tenant = a.Tenant,
                                            SearchFields = a.SearchFields,
                                            Code = a.Code,
                                            InActive = a.InActive,
                                        });

                        foreach (var c in Industryes)
                        {
                            string cname = "IndustryPM" + c.Id + c.Tenant;

                            if (CacheManager.CacheWrapper.Get(cname) == null)
                            {
                                CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (IndustryPM)CacheManager.CacheWrapper.Get(entityName);

                    }
                    else
                    {
                        entity = (IndustryPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.Industries
                              where a.Tenant == tenant && a.Id == id
                              select new IndustryPM()
                              {
                                  Name = a.Name,
                                  Id = a.Id,
                                  Tenant = a.Tenant,
                                  SearchFields = a.SearchFields,
                                  Code = a.Code,
                                  InActive = a.InActive,
                              }).FirstOrDefault();
                }
                return entity;
            }
            return null;
        }

        public IndustryPM GetSinglePMByCode(string code, int tenant)
        {
            return (from a in repository.context.Industries
                    where a.Tenant == tenant && a.Code == code
                    select new IndustryPM()
                    {
                        Name = a.Name,
                        Id = a.Id,
                        Tenant = a.Tenant,
                        SearchFields = a.SearchFields,
                        Code = a.Code,
                        InActive = a.InActive,
                    }).FirstOrDefault();
        }

        public IQueryable<IndustryPM> GetIndustryPMsByTenant(int tenant)
        {
            IQueryable<IndustryPM> Industryes = from a in repository.context.Industries
                                            where a.Tenant == tenant
                                            select new IndustryPM()
                                            {
                                                Name = a.Name,
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                SearchFields = a.SearchFields,
                                                Code = a.Code,
                                                InActive = a.InActive,
                                            };
            return Industryes;
        }

        public IndustryPM GetIndustryByName(string name, int tenant)
        {
            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.Industries
                         where a.Tenant == tenant && a.Name == name
                         select new IndustryPM()
                         {
                             Name = a.Name,
                             Id = a.Id,
                             Tenant = a.Tenant,
                             SearchFields = a.SearchFields,
                             Code = a.Code,
                             InActive = a.InActive,
                         }).FirstOrDefault();
            return query;
        }

        public IQueryable<IndustryList> GetIQueryableEntityList(IQueryable<Industry> iQueryable)
        {
            IQueryable<IndustryList> result = from a in iQueryable
                                            select new IndustryList()
                                            {
                                                Name = a.Name,
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                SearchFields = a.SearchFields,
                                                Code = a.Code,
                                                InActive = a.InActive,
                                            };
            return result;
        }

        public Industry GetFirstIndustryForTenant(int tenant)
        {
            return (from a in repository.context.Industries
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }
    }
}