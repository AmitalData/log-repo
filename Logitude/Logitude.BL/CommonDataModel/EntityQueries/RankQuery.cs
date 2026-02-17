using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class RankQuery
    {
        RankRepository repository;

        public RankQuery()
        {
            repository = new RankRepository(); 
        }

        public RankQuery(int tenant)
        {
            repository = new RankRepository(tenant);
        }

        public RankQuery(RankRepository repository)
        {
            this.repository = repository;
        }

        public RankPM GetSinglePM(string id, int tenant)
        {
            var rank = (from a in repository.context.Ranks
                        where a.Tenant == tenant && a.Id == id
                        select new RankPM()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Tenant = a.Tenant,
                            Code = a.Code,
                        }).FirstOrDefault();
            return rank;
        }
        
        public IQueryable<RankPM> GetRankPMsByTenant(int tenant)
        {
            var query = from a in repository.context.Ranks
                        where a.Tenant == tenant
                        select new RankPM()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Tenant = a.Tenant,
                            Code = a.Code,
                        };
            return query;
        }

        public IQueryable<RankList> GetIQueryableEntityList(IQueryable<Rank> iQueryable)
        {
            IQueryable<RankList> result = from rank in iQueryable
                                          select new RankList()
                                          {
                                              Id = rank.Id,
                                              Tenant = rank.Tenant,
                                              Code = rank.Code,
                                              Name = rank.Name,
                                              SearchFields = rank.SearchFields,
                                          };
            return result;
        }
    }
}
