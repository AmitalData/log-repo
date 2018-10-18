using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class AWBStatusQuery
    {
        AWBStatusRepository repository;
         
        public AWBStatusQuery(int tenant)
        {
            repository = new AWBStatusRepository(tenant);
        }

        public AWBStatusQuery(AWBStatusRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<AWBStatusPM> GetStatusTypePMs()
        {
            return from a in repository.context.AWBStatus
                   select new AWBStatusPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, };
        }

        public AWBStatusPM GetSingleAWBStatusPM(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "AWBStatusPM" + code;
                AWBStatusPM entity;

                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.AWBStatus

                                              select new AWBStatusPM()
                                              {
                                                  Code = a.Code,
                                                  Name = a.Name,
                                                  SearchFields = a.SearchFields
                                              });
                        foreach (var s in entitystatuses)
                        {
                            string name = "AWBStatusPM" + s.Code;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, DateTime.UtcNow.AddHours(30), TimeSpan.Zero);
                            }
                        }
                        entity = (AWBStatusPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (AWBStatusPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.AWBStatus
                              where a.Code == code
                              select new AWBStatusPM()
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