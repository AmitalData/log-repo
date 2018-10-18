using System;
using System.Linq;
using System.Web;

using Simplog.Data.ShipmentsModel.Repositories;

using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class NextLegQuery
    {
        NextLegRepository repository;
         
        public NextLegQuery(int tenant)
        {
            repository = new NextLegRepository(tenant);
        }

        public NextLegQuery(NextLegRepository repository)
        {
            this.repository = repository;
        }

        public NextLegPM GetSingleNextLegPM(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "NextLegPM" + code;
                NextLegPM entity;

                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.NextLegs

                                              select new NextLegPM()
                                              {
                                                  Code = a.Code,
                                                  Name = a.Name,

                                              });
                        foreach (var s in entitystatuses)
                        {
                            string name = "NextLegPM" + s.Code;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, DateTime.UtcNow.AddHours(30), TimeSpan.Zero);
                            }
                        }
                        entity = (NextLegPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (NextLegPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.NextLegs
                              where a.Code == code
                              select new NextLegPM()
                              {
                                  Code = a.Code,
                                  Name = a.Name,

                              }).FirstOrDefault();
                }
                return entity;
            }
            return null;
        }
    }
}