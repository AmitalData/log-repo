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
    public class CommodityQuery
    {
        CommodityRepository repository;

        public CommodityQuery()
        {
            this.repository = new CommodityRepository(); 
        }

        public CommodityQuery(int tenant)
        {
            this.repository = new CommodityRepository(tenant);
        }

        public CommodityQuery(CommodityRepository repository)
        {
            this.repository = repository;
        }

        public CommodityPM GetSinglePM(string id, int tenant)
        {
            CommodityPM result = null;
            Commodity entityPoco = repository.GetSingleCommodity(id, tenant);

            if (entityPoco != null)
            {
                result = new CommodityPM()
                {
                    Id = entityPoco.Id,
                    Tenant = entityPoco.Tenant,
                    Code = entityPoco.Code,
                    Name = entityPoco.Name,
                    SearchFields = entityPoco.SearchFields,
                    InActive = entityPoco.InActive,
                    AirlineId = entityPoco.AirlineId,
                };
            }

            return result;
        }

        public IQueryable<CommodityList> GetIQueryableEntityList(IQueryable<Commodity> iQueryable)
        {
            IQueryable<CommodityList> result = from entity in iQueryable
                                               select new CommodityList()
                                             {
                                                 Id = entity.Id,
                                                 Tenant = entity.Tenant,
                                                 Code = entity.Code,
                                                 Name = entity.Name,
                                                 SearchFields = entity.SearchFields,
                                                 InActive = entity.InActive,
                                                 AirlineId = entity.AirlineId,
                                             };
            return result;
        }
    }
}