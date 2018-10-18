using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class FHLStatusQuery
    {
         FHLStatusRepository repository;
         
        public FHLStatusQuery(int tenant)
        {
            repository = new FHLStatusRepository(tenant);
        }

        public FHLStatusQuery(FHLStatusRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<FHLStatusPM> GetStatusTypePMs()
        {
            return from a in repository.context.FHLStatus
                   select new FHLStatusPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, };
        }

        public FHLStatusPM GetSingleFHLStatusPM(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "FHLStatusPM" + code;
                FHLStatusPM entity;

                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.FHLStatus

                                              select new FHLStatusPM()
                                              {
                                                  Code = a.Code,
                                                  Name = a.Name,
                                                  SearchFields = a.SearchFields
                                              });
                        foreach (var s in entitystatuses)
                        {
                            string name = "FHLStatusPM" + s.Code;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, DateTime.UtcNow.AddHours(30), TimeSpan.Zero);
                            }
                        }
                        entity = (FHLStatusPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (FHLStatusPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.FHLStatus
                              where a.Code == code
                              select new FHLStatusPM()
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
    }
}