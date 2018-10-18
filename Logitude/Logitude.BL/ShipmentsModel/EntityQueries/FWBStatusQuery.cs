using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class FWBStatusQuery
    {
         FWBStatusRepository repository;
         
        public FWBStatusQuery(int tenant)
        {
            repository = new FWBStatusRepository(tenant);
        }

        public FWBStatusQuery(FWBStatusRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<FWBStatusPM> GetStatusTypePMs()
        {
            return from a in repository.context.FWBStatus
                   select new FWBStatusPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, };
        }

        public FWBStatusPM GetSingleFWBStatusPM(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "FWBStatusPM" + code;
                FWBStatusPM entity;

                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.FWBStatus

                                              select new FWBStatusPM()
                                              {
                                                  Code = a.Code,
                                                  Name = a.Name,
                                                  SearchFields = a.SearchFields
                                              });
                        foreach (var s in entitystatuses)
                        {
                            string name = "FWBStatusPM" + s.Code;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, DateTime.UtcNow.AddHours(30), TimeSpan.Zero);
                            }
                        }
                        entity = (FWBStatusPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (FWBStatusPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.FWBStatus
                              where a.Code == code
                              select new FWBStatusPM()
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