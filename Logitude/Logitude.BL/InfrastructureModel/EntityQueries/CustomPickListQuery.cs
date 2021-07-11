using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class CustomPickListQuery
    {
        CustomPickListRepository repository;
        public CustomPickListQuery()
        {
            repository = new CustomPickListRepository(); 
        }

        public CustomPickListQuery(int tenant)
        {
            repository = new CustomPickListRepository(tenant);
        }

        public CustomPickListQuery(CustomPickListRepository customPickListRepository)
        {
            repository = customPickListRepository;
        }


        public IQueryable<CustomPickListPM> GetCustomPickListPMsByTenant(int tenant)
        {

            IQueryable<CustomPickListPM> customPickLists = from a in repository.context.CustomPickLists
                                                           where a.Tenant == tenant
                                                           select new CustomPickListPM()
                                                           {
                                                               Id = a.Id,
                                                               Code = a.Code,
                                                               Tenant = a.Tenant,
                                                               Value = a.Value,
                                                               IsMultipleChoice = a.IsMultipleChoice,

                                                           };

            return customPickLists;
        }





        public IQueryable<CustomPickListPM> GetCustomPickListPMsByCode(int tenant, string code)
        {

            IQueryable<CustomPickListPM> customPickLists = from a in repository.context.CustomPickLists
                                                           where a.Tenant == tenant && a.Code == code
                                                           select new CustomPickListPM()
                                                           {
                                                               Id = a.Id,
                                                               Code = a.Code,
                                                               Tenant = a.Tenant,
                                                               Value = a.Value,
                                                               IsMultipleChoice = a.IsMultipleChoice,

                                                           };

            return customPickLists;
        }





        public CustomPickListPM GetSinglePM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "CustomPickListPM" + id;
                CustomPickListPM entity;
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var cutomPickLists = (from a in repository.context.CustomPickLists
                                              select new CustomPickListPM() { Id = a.Id, Code = a.Code, Value = a.Value, Tenant = a.Tenant });
                        foreach (var s in cutomPickLists)
                        {
                            string name = "CustomPickListPM" + s.Id;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, System.DateTime.UtcNow.AddHours(30), TimeSpan.Zero);
                            }
                        }
                        entity = (CustomPickListPM)CacheManager.CacheWrapper.Get(entityName);

                    }
                    else
                    {
                        entity = (CustomPickListPM)CacheManager.CacheWrapper.Get(entityName);

                    }
                }
                else
                {
                    entity = (from a in repository.context.CustomPickLists
                              where a.Id == id && a.Tenant == tenant
                              select new CustomPickListPM() { Id = a.Id, Code = a.Code, Value = a.Value, Tenant = a.Tenant, IsMultipleChoice = a.IsMultipleChoice }).FirstOrDefault();
                }
                return entity;
            }
            return null;


        }

        public IQueryable<CustomPickListList> GetIQueryableEntityList(IQueryable<CustomPickList> iQueryable)
        {
            IQueryable<CustomPickListList> result = from entity in iQueryable
                                                    select new CustomPickListList()
                                                    {
                                                        Id = entity.Id,
                                                        Code = entity.Code,
                                                        Tenant = entity.Tenant,
                                                        Value = entity.Value,
                                                        IsMultipleChoice = entity.IsMultipleChoice,
                                                    };
            return result;
        }




        public List<CustomPickListList> GetCustomPickListListIsMultipleChoice()
        {
            List<CustomPickListList> result = (from entity in repository.context.CustomPickLists
                                                    where entity.IsMultipleChoice
                                                    select new CustomPickListList()
                                                    {
                                                        Id = entity.Id,
                                                        Code = entity.Code,
                                                        Tenant = entity.Tenant,
                                                        Value = entity.Value,
                                                        IsMultipleChoice = entity.IsMultipleChoice,
                                                    }).ToList();
            return result;
        }

        public string GetIdByCodeAndValueAndTenant(string code, string value, int tenant)
        {
            string result = (from entity in repository.context.CustomPickLists
                                               where entity.Code == code && entity.Value == value && entity.Tenant == tenant
                                               select entity.Id).FirstOrDefault();
            return result;
        }
    }
}