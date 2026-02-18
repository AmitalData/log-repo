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
    public class LeadSourceQuery
    {
        LeadSourceRepository repository;
        public LeadSourceQuery()
        {
               repository = new LeadSourceRepository(); 
        }

        public LeadSourceQuery(int tenant)
        {
            repository = new LeadSourceRepository(tenant);
        }

        public LeadSourceQuery(LeadSourceRepository LeadSourceRepository)
        {
            repository = LeadSourceRepository;
        }

        public LeadSourcePM GetSinglePM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "LeadSourcePM" + id + tenant;
                LeadSourcePM entity;
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var LeadSourcees = (from a in repository.context.LeadSources
                                        where a.Tenant == tenant
                                        select new LeadSourcePM()
                                        {
                                            Name = a.Name,
                                            Id = a.Id,
                                            Tenant = a.Tenant,
                                            SearchFields = a.SearchFields,
                                            Code = a.Code,
                                            InActive = a.InActive,
                                        });

                        foreach (var c in LeadSourcees)
                        {
                            string cname = "LeadSourcePM" + c.Id + c.Tenant;

                            if (CacheManager.CacheWrapper.Get(cname) == null)
                            {
                                CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (LeadSourcePM)CacheManager.CacheWrapper.Get(entityName);

                    }
                    else
                    {
                        entity = (LeadSourcePM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.LeadSources
                              where a.Tenant == tenant && a.Id == id
                              select new LeadSourcePM()
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

        public LeadSourcePM GetSinglePMByCode(string code, int tenant)
        {
            return (from a in repository.context.LeadSources
                    where a.Tenant == tenant && a.Code == code
                    select new LeadSourcePM()
                    {
                        Name = a.Name,
                        Id = a.Id,
                        Tenant = a.Tenant,
                        SearchFields = a.SearchFields,
                        Code = a.Code,
                        InActive = a.InActive,
                    }).FirstOrDefault();
        }

        public IQueryable<LeadSourcePM> GetLeadSourcePMsByTenant(int tenant)
        {
            IQueryable<LeadSourcePM> LeadSourcees = from a in repository.context.LeadSources
                                            where a.Tenant == tenant
                                            select new LeadSourcePM()
                                            {
                                                Name = a.Name,
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                SearchFields = a.SearchFields,
                                                Code = a.Code,
                                                InActive = a.InActive,
                                            };
            return LeadSourcees;
        }

        public LeadSourcePM GetLeadSourceByName(string name, int tenant)
        {
            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.LeadSources
                         where a.Tenant == tenant && a.Name == name
                         select new LeadSourcePM()
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

        public IQueryable<LeadSourceList> GetIQueryableEntityList(IQueryable<LeadSource> iQueryable)
        {
            IQueryable<LeadSourceList> result = from a in iQueryable
                                            select new LeadSourceList()
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

        public LeadSource GetFirstLeadSourceForTenant(int tenant)
        {
            return (from a in repository.context.LeadSources
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public LeadSourcePM GetLeadSourceByCode(string code, int tenant)
        {
            
            var query = (from a in repository.context.LeadSources
                         where a.Tenant == tenant && a.Code == code
                         select new LeadSourcePM()
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


    }
}
