using System;
using System.Linq;
using System.Web;

using Simplog.Data.ShipmentsModel.Repositories;

using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentLevelQuery
    {
        ShipmentLevelRepository repository;
         
        public ShipmentLevelQuery(int tenant)
        {
            repository = new ShipmentLevelRepository(tenant);
        }

        public ShipmentLevelQuery(ShipmentLevelRepository repository)
        {
            this.repository = repository;
        }

        public ShipmentLevelPM GetSingleShipmentLevelPM(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "ShipmentLevelPM" + code;
                ShipmentLevelPM entity;

                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.ShipmentLevels

                                              select new ShipmentLevelPM()
                                              {
                                                  Code = a.Code,
                                                  Name = a.Name,
                                                  SearchFields = a.SearchFields
                                              });
                        foreach (var s in entitystatuses)
                        {
                            string name = "ShipmentLevelPM" + s.Code;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, DateTime.UtcNow.AddHours(30), TimeSpan.Zero);
                            }
                        }
                        entity = (ShipmentLevelPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (ShipmentLevelPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.ShipmentLevels
                              where a.Code == code
                              select new ShipmentLevelPM()
                              {
                                  Code = a.Code,
                                  Name = a.Name,
                                  SearchFields = a.SearchFields
                              }).FirstOrDefault();
                }
                return entity;
            }
            return null;
        }

        public ShipmentLevelPM GetSinglePM(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "ShipmentLevelPM" + code;
                ShipmentLevelPM entity;

                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.ShipmentLevels

                                              select new ShipmentLevelPM()
                                              {
                                                  Code = a.Code,
                                                  Name = a.Name,
                                                  SearchFields = a.SearchFields
                                              });
                        foreach (var s in entitystatuses)
                        {
                            string name = "ShipmentLevelPM" + s.Code;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, DateTime.UtcNow.AddHours(30), TimeSpan.Zero);
                            }
                        }
                        entity = (ShipmentLevelPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (ShipmentLevelPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.ShipmentLevels
                              where a.Code == code
                              select new ShipmentLevelPM()
                              {
                                  Code = a.Code,
                                  Name = a.Name,
                                  SearchFields = a.SearchFields
                              }).FirstOrDefault();
                }
                return entity;
            }
            return null;
        }

        public ShipmentLevelPM GetSinglePMByCode(string code,int tenant)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "ShipmentLevelPM" + code;
                ShipmentLevelPM entity;

                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.ShipmentLevels

                                              select new ShipmentLevelPM()
                                              {
                                                  Code = a.Code,
                                                  Name = a.Name,
                                                  SearchFields = a.SearchFields
                                              });
                        foreach (var s in entitystatuses)
                        {
                            string name = "ShipmentLevelPM" + s.Code;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, DateTime.UtcNow.AddHours(30), TimeSpan.Zero);
                            }
                        }
                        entity = (ShipmentLevelPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (ShipmentLevelPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.ShipmentLevels
                              where a.Code == code
                              select new ShipmentLevelPM()
                              {
                                  Code = a.Code,
                                  Name = a.Name,
                                  SearchFields = a.SearchFields
                              }).FirstOrDefault();
                }
                return entity;
            }
            return null;
        }
        

        public ShipmentLevelPM GetSinglePM(string code,int Tenant)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "ShipmentLevelPM" + code;
                ShipmentLevelPM entity;

                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.ShipmentLevels

                                              select new ShipmentLevelPM()
                                              {
                                                  Code = a.Code,
                                                  Name = a.Name,
                                                  SearchFields = a.SearchFields
                                              });
                        foreach (var s in entitystatuses)
                        {
                            string name = "ShipmentLevelPM" + s.Code;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, DateTime.UtcNow.AddHours(30), TimeSpan.Zero);
                            }
                        }
                        entity = (ShipmentLevelPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (ShipmentLevelPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.ShipmentLevels
                              where a.Code == code
                              select new ShipmentLevelPM()
                              {
                                  Code = a.Code,
                                  Name = a.Name,
                                  SearchFields = a.SearchFields
                              }).FirstOrDefault();
                }
                return entity;
            }
            return null;
        }

        public IQueryable<ShipmentLevelList> GetIQueryableEntityList(IQueryable<ShipmentLevel> iQueryable)
        {
            IQueryable<ShipmentLevelList> result = (from a in iQueryable
                                                    select new ShipmentLevelList()
                                                          {
                                                              Code = a.Code,
                                                              Name = a.Name,
                                                              SearchFields = a.SearchFields
                                                          });
            return result;
        }


    }
}