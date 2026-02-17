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
    public class DirectionQuery
    {
        DirectionRepository repository;

        public DirectionQuery()
        {
            repository = new DirectionRepository(); 
        }

        public DirectionQuery(int tenant)
        {
            repository = new DirectionRepository(tenant);
        }

        public DirectionQuery(DirectionRepository directionRepository)
        {
            repository = directionRepository;
        }

        public IQueryable<DirectionPM> GetEventTypePMsByTenant()
        {
            IQueryable<DirectionPM> directions = from a in repository.context.Directions

                                                 select new DirectionPM()
                                                 {
                                                     Id = a.Id,
                                                     Name = a.Name,
                                                     SearchFields = a.SearchFields,
                                                 };
            return directions;
        }

        public DirectionPM GetSingleDirectionPM(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                string entityName = "DirectionPM" + name;
                DirectionPM entity;
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var directions = (from a in repository.context.Directions
                                          select new DirectionPM() { Id = a.Id, Name = a.Name });
                        foreach (var s in directions)
                        {
                            string dname = "DirectionPM" + s.Id;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, System.DateTime.UtcNow.AddHours(30), TimeSpan.Zero);
                            }
                        }
                        entity = (DirectionPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (DirectionPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.Directions
                              where a.Name == name
                              select new DirectionPM() { Id = a.Id, Name = a.Name, SearchFields = a.SearchFields }).FirstOrDefault();
                }
                return entity;
            }
            return null;

            //return (from a in context.Directions
            //        where a.Name == name
            //        select new DirectionPM()
            //        {
            //            Id = a.Id,
            //            Name = a.Name,
            //        }).FirstOrDefault();
        }




        public DirectionPM GetSinglePM(string id, int tenant = 0)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "DirectionPM" + id;
                DirectionPM entity;
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var directions = (from a in repository.context.Directions
                                          select new DirectionPM() { Id = a.Id, Name = a.Name });
                        foreach (var s in directions)
                        {
                            string dname = "DirectionPM" + s.Id;
                            if (CacheManager.CacheWrapper.Get(dname) == null)
                            {
                                CacheManager.CacheWrapper.Insert(dname, s, null, System.DateTime.UtcNow.AddHours(30), TimeSpan.Zero);
                            }
                        }
                        entity = (DirectionPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (DirectionPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.Directions
                              where a.Id == id
                              select new DirectionPM() { Id = a.Id, Name = a.Name, SearchFields = a.SearchFields }).FirstOrDefault();
                }
                return entity;
            }
            return null;
        }

        public IQueryable<DirectionList> GetIQueryableEntityList(IQueryable<Direction> iQueryable)
        {
            IQueryable<DirectionList> result = from entity in iQueryable
                                               select new DirectionList()
                                               {
                                                   Name = entity.Name,
                                                   Id = entity.Id,
                                                   SearchFields = entity.SearchFields
                                               };
            return result;
        }
    }
}