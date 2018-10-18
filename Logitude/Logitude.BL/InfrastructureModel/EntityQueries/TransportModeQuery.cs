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
    public class TransportModeQuery
    {
        TransportModeRepository repository;
        public TransportModeQuery()
        {
            repository = new TransportModeRepository(); 
        }

        public TransportModeQuery(int tenant)
        {
            repository = new TransportModeRepository(tenant);
        }

        public TransportModeQuery(TransportModeRepository transportModeRepository)
        {
            repository = transportModeRepository;
        }

        public TransportModePM GetSinglePM(string id, int tenent = 0)
        {
            if (!string.IsNullOrEmpty(id))
            {

                string entityName = "TransportModePM" + id;
                TransportModePM entity;
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var transmodes = (from a in repository.context.TransportModes

                                          select new TransportModePM()
                                          {
                                              Id = a.Id,
                                              Name = a.Name,
                                              SearchFields = a.SearchFields,
                                          });
                        foreach (var s in transmodes)
                        {
                            string name = "TransportModePM" + s.Id;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, System.DateTime.UtcNow.AddHours(30), TimeSpan.Zero);
                            }
                        }
                        entity = (TransportModePM)CacheManager.CacheWrapper.Get(entityName);

                    }
                    else
                    {
                        entity = (TransportModePM)CacheManager.CacheWrapper.Get(entityName);

                    }
                }
                else
                {
                    entity = (from a in repository.context.TransportModes
                              where a.Id == id
                              select new TransportModePM()
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  SearchFields = a.SearchFields,
                              }).FirstOrDefault();
                }

                return entity;



            }
            return null;
        }

        public IQueryable<TransportModeList> GetIQueryableEntityList(IQueryable<TransportMode> iQueryable)
        {
            IQueryable<TransportModeList> result = from entity in iQueryable
                                                   select new TransportModeList()
                                                   {
                                                       Id = entity.Id,
                                                       Name = entity.Name,
                                                       SearchFields = entity.SearchFields,
                                                   };
            return result;
        }



    }
}