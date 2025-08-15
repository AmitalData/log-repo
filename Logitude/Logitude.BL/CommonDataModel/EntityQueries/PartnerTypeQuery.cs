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
    public class PartnerTypeQuery
    {
        PartnerTypeRepository repository;



        public PartnerTypeQuery(int tenant)
        {
            repository = new PartnerTypeRepository(tenant);
        }

        public PartnerTypeQuery(PartnerTypeRepository repository)
        {
            this.repository = repository;
        }

        public PartnerTypePM GetSinglePM(string id)
        {
            string entityName = "PartnerTypePM" + id;
            PartnerTypePM entity;

            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    var entitystatuses = (from a in repository.context.PartnerTypes

                                          select new PartnerTypePM()
                                          {
                                              Id = a.Id,
                                              Name = a.Name,
                                              SearchFields = a.SearchFields,

                                          });
                    foreach (var s in entitystatuses)
                    {
                        string name = "PartnerTypePM" + s.Id;
                        if (CacheManager.CacheWrapper.Get(name) == null)
                        {
                            CacheManager.CacheWrapper.Insert(name, s, null, DateTime.UtcNow.AddHours(30), TimeSpan.Zero);
                        }
                    }
                    entity = (PartnerTypePM)CacheManager.CacheWrapper.Get(entityName);
                }
                else
                {
                    entity = (PartnerTypePM)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            else
            {
                entity = (from a in repository.context.PartnerTypes
                          where a.Id == id
                          select new PartnerTypePM()
                          {
                              Id = a.Id,
                              Name = a.Name,
                              SearchFields = a.SearchFields,

                          }).FirstOrDefault();
            }
            return entity;
        }

        public IQueryable<PartnerTypePM> GetPartnerTypePMs()
        {
            var query = from a in repository.context.PartnerTypes
                        select new PartnerTypePM()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            SearchFields = a.SearchFields,
                        };

            return query;
        }

        public IQueryable<PartnerTypeList> GetIQueryableEntityList(IQueryable<PartnerType> iQueryable)
        {
            IQueryable<PartnerTypeList> result = from entity in iQueryable
                                                 select new PartnerTypeList()
                                                 {
                                                     SearchFields = entity.SearchFields,
                                                     Name = entity.Name,
                                                     Id = entity.Id,
                                                 };
            return result;
        }
    }
}