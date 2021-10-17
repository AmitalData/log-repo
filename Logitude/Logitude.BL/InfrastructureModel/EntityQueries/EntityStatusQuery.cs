using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityLists;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class EntityStatusQuery
    {
        EntityStatusRepository repository;
        public EntityStatusQuery()
        {
            repository = new EntityStatusRepository(); 
        }

        public EntityStatusQuery(int tenant)
        {
            repository = new EntityStatusRepository(tenant);
        }

        public EntityStatusQuery(EntityStatusRepository entityStatusRepository)
        {
            repository = entityStatusRepository;
        }
           public EntityStatusPM GetSinglePM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                EntityStatusPM entity = null;
                if (HttpContext.Current != null)
                {
                    string entityName = "EntityStatusPM" + id + tenant;

                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.EntityStatus.Include("ObjectTable").Include("EntityStatusType")
                                              where a.Tenant == tenant
                                              select new EntityStatusPM()
                                              {
                                                  Id = a.Id,
                                                  Name = a.Name,
                                                  ObjectTableId = a.ObjectTableId,
                                                  StatusWeight = a.StatusWeight,
                                                  Tenant = a.Tenant,
                                                  ObjectTableName = a.ObjectTable.Name,
                                                  Code = a.Code,
                                                  SearchFields = a.SearchFields,
                                                  InActive = a.InActive,
                                                  DisplayName = !string.IsNullOrEmpty(a.DisplayName) ? a.DisplayName : a.Name,
                                                  EntityStatusTypeCode = a.EntityStatusTypeCode,
                                                  StatusLocalWeight = a.StatusLocalWeight,
                                              });
                        foreach (var s in entitystatuses)
                        {
                            string name = "EntityStatusPM" + s.Id + tenant;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (EntityStatusPM)CacheManager.CacheWrapper.Get(entityName);
                        //EntityStatusPM status = (from a in context.EntityStatus
                        //                          where a.Tenant == tenant && a.Id == id && !a.InActive
                        //                          select new EntityStatusPM()
                        //                          {
                        //                              Id = a.Id,
                        //                              Name = a.Name,
                        //                              ObjectTableId = a.ObjectTableId,
                        //                              StatusWeight = a.StatusWeight,
                        //                              Tenant = a.Tenant,
                        //                              ObjectTableName = a.ObjectTable.Name,
                        //                              Code = a.Code,
                        //                              SearchFields = a.SearchFields,
                        //                              InActive = a.InActive,
                        //                          }).FirstOrDefault();

                        //Entity = status;
                        //if (HttpContext.Current.Cache.Get(EntityName) == null)
                        //{
                        //    HttpContext.Current.Cache.Insert(EntityName, Entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        //}
                    }
                    else
                    {
                        entity = (EntityStatusPM)CacheManager.CacheWrapper.Get(entityName);
                        // HttpContext.Current.Cache.Insert(EntityNameValue, Entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                    }
                }
                else
                {
                    EntityStatusPM status = (from a in repository.context.EntityStatus.Include("ObjectTable")
                                             where a.Tenant == tenant && a.Id == id && !a.InActive
                                             select new EntityStatusPM()
                                             {
                                                 Id = a.Id,
                                                 Name = a.Name,
                                                 ObjectTableId = a.ObjectTableId,
                                                 StatusWeight = a.StatusWeight,
                                                 Tenant = a.Tenant,
                                                 ObjectTableName = a.ObjectTable.Name,
                                                 Code = a.Code,
                                                 SearchFields = a.SearchFields,
                                                 InActive = a.InActive,
                                                 DisplayName = !string.IsNullOrEmpty(a.DisplayName) ? a.DisplayName : a.Name,
                                                 EntityStatusTypeCode = a.EntityStatusTypeCode,
                                                 StatusLocalWeight = a.StatusLocalWeight,
                                             }).FirstOrDefault();

                    entity = status;
                }
                return entity;

            }
            return null;


        }
        public EntityStatusPM GetSingleEntityStatuPMs(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                EntityStatusPM entity = null;
                if (HttpContext.Current != null)
                {
                    string entityName = "EntityStatusPM" + id + tenant;

                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.EntityStatus.Include("ObjectTable")
                                              where a.Tenant == tenant
                                              select new EntityStatusPM()
                                              {
                                                  Id = a.Id,
                                                  Name = a.Name,
                                                  ObjectTableId = a.ObjectTableId,
                                                  StatusWeight = a.StatusWeight,
                                                  Tenant = a.Tenant,
                                                  ObjectTableName = a.ObjectTable.Name,
                                                  Code = a.Code,
                                                  SearchFields = a.SearchFields,
                                                  InActive = a.InActive,
                                                  DisplayName = !string.IsNullOrEmpty(a.DisplayName) ? a.DisplayName : a.Name,
                                                  EntityStatusTypeCode = a.EntityStatusTypeCode,
                                                  StatusLocalWeight = a.StatusLocalWeight,
                                              });
                        foreach (var s in entitystatuses)
                        {
                            string name = "EntityStatusPM" + s.Id + tenant;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (EntityStatusPM)CacheManager.CacheWrapper.Get(entityName);
                        //EntityStatusPM status = (from a in context.EntityStatus
                        //                          where a.Tenant == tenant && a.Id == id && !a.InActive
                        //                          select new EntityStatusPM()
                        //                          {
                        //                              Id = a.Id,
                        //                              Name = a.Name,
                        //                              ObjectTableId = a.ObjectTableId,
                        //                              StatusWeight = a.StatusWeight,
                        //                              Tenant = a.Tenant,
                        //                              ObjectTableName = a.ObjectTable.Name,
                        //                              Code = a.Code,
                        //                              SearchFields = a.SearchFields,
                        //                              InActive = a.InActive,
                        //                          }).FirstOrDefault();

                        //Entity = status;
                        //if (HttpContext.Current.Cache.Get(EntityName) == null)
                        //{
                        //    HttpContext.Current.Cache.Insert(EntityName, Entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        //}
                    }
                    else
                    {
                        entity = (EntityStatusPM)CacheManager.CacheWrapper.Get(entityName);
                        // HttpContext.Current.Cache.Insert(EntityNameValue, Entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                    }
                }
                else
                {
                    EntityStatusPM status = (from a in repository.context.EntityStatus.Include("ObjectTable")
                                             where a.Tenant == tenant && a.Id == id && !a.InActive
                                             select new EntityStatusPM()
                                             {
                                                 Id = a.Id,
                                                 Name = a.Name,
                                                 ObjectTableId = a.ObjectTableId,
                                                 StatusWeight = a.StatusWeight,
                                                 Tenant = a.Tenant,
                                                 ObjectTableName = a.ObjectTable.Name,
                                                 Code = a.Code,
                                                 SearchFields = a.SearchFields,
                                                 InActive = a.InActive,
                                                 DisplayName = !string.IsNullOrEmpty(a.DisplayName) ? a.DisplayName : a.Name,
                                                 EntityStatusTypeCode = a.EntityStatusTypeCode,
                                                 StatusLocalWeight = a.StatusLocalWeight,
                                             }).FirstOrDefault();

                    entity = status;
                }
                return entity;

            }
            return null;


        }


        public IQueryable<EntityStatusPM> GetEntityStatusPMsByTenant(int tenant)
        {
            return (from a in repository.context.EntityStatus.Include("ObjectTable")
                    where a.Tenant == tenant && a.InActive == false
                    select new EntityStatusPM()
                    {
                        Id = a.Id,
                        Name = a.Name,
                        ObjectTableId = a.ObjectTableId,
                        StatusWeight = a.StatusWeight,
                        Tenant = a.Tenant,
                        ObjectTableName = a.ObjectTable.Name,
                        Code = a.Code,
                        SearchFields = a.SearchFields,
                        InActive = a.InActive,
                        DisplayName = !string.IsNullOrEmpty(a.DisplayName) ? a.DisplayName : a.Name,
                        EntityStatusTypeCode = a.EntityStatusTypeCode,
                        StatusLocalWeight = a.StatusLocalWeight,
                    });
        }

        public IQueryable<EntityStatusList> GetIQueryableEntityList(IQueryable<EntityStatus> iQueryable)
        {
            IQueryable<EntityStatusList> result = from entity in iQueryable.Include("ObjectTable")
                                                  select new EntityStatusList()
                                                  {
                                                      Id = entity.Id,
                                                      ObjectTableId = entity.ObjectTableId,
                                                      Name = entity.Name,
                                                      Code = entity.Code,
                                                      SearchFields = entity.SearchFields,
                                                      Tenant = entity.Tenant,
                                                      InActive = entity.InActive,
                                                      ObjectTableName = entity.ObjectTable.Name,
                                                      StatusWeight = entity.StatusWeight,
                                                      DisplayName = !string.IsNullOrEmpty(entity.DisplayName) ? entity.DisplayName : entity.Name,
                                                      EntityStatusTypeCode = entity.EntityStatusTypeCode,
                                                      StatusLocalWeight = entity.StatusLocalWeight,
                                                  };
            return result;
        }

    }
}