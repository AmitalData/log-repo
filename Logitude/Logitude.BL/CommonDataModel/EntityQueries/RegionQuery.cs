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
    public class RegionQuery
    {
        RegionRepository repository;

        public RegionQuery()
        {
            repository = new RegionRepository(); 
        }

        public RegionQuery(RegionRepository RegionRepository)
        {
            repository = RegionRepository;
        }

        public RegionQuery(int tenant)
        {
            repository = new RegionRepository(tenant);
        }

        public IQueryable<RegionPM> GetRegionPMs(int tenant)
        {
            IQueryable<Region> pocos = repository.GetRegions(tenant);

            IQueryable<RegionPM> entityPMs = (from a in pocos
                                                           select new RegionPM()
                                                           {
                                                               Id = a.Id,
                                                               LocalName = a.LocalName,
                                                               Name = a.Name,                                                            
                                                               Tenant = a.Tenant,
                                                               SearchFields = a.SearchFields,
                                                               InActive = a.InActive,
                                                           });
            return entityPMs;
        }

        public RegionPM GetSinglePM(string id, int tenant)
        {
            RegionPM result = null;

            Region entityPoco = repository.GetSingleRegion(id, tenant);
            if (entityPoco != null)
            {
                result = new RegionPM()
                {
                    Id = entityPoco.Id,
                    Tenant = entityPoco.Tenant,
                    LocalName = entityPoco.LocalName,
                    Name = entityPoco.Name,
                    SearchFields = entityPoco.SearchFields,
                    InActive = entityPoco.InActive,
                };
            }

            RegionPM securedPm = new RegionPM();
            SecuredMapping.GetMappedPM(result, securedPm, "Region", tenant);

            return securedPm;
        }

        public RegionPM GetSingleRegionPM(string id, int tenant)
        {
            RegionPM result = null;

            Region entityPoco = repository.GetSingleRegion(id, tenant);
            if (entityPoco != null)
            {
                result = new RegionPM()
                {
                    Id = entityPoco.Id,
                    Tenant = entityPoco.Tenant,
                    LocalName = entityPoco.LocalName,
                    Name = entityPoco.Name,                
                    SearchFields = entityPoco.SearchFields,
                    InActive = entityPoco.InActive,
                };
            }

            RegionPM securedPm = new RegionPM();
            SecuredMapping.GetMappedPM(result, securedPm, "Region", tenant);

            return securedPm;
        }

        public IQueryable<RegionPM> GetRegionPMsByTenant(int tenant)
        {
            IQueryable<Region> pocos = repository.GetRegions(tenant);

            IQueryable<RegionPM> entityPMs = (from a in pocos
                                                           select new RegionPM()
                                                                 {
                                                                     Id = a.Id,
                                                                     LocalName = a.LocalName,
                                                                     Name = a.Name,                                                                  
                                                                     Tenant = a.Tenant,
                                                                     SearchFields = a.SearchFields,
                                                                     InActive = a.InActive,
                                                                 });
            return entityPMs;
        }

        public IQueryable<RegionList> GetIQueryableEntityList(IQueryable<Region> iQueryable)
        {
            IQueryable<RegionList> result = (from Region in iQueryable

                                                    select new RegionList()
                                                    {
                                                        Id = Region.Id,
                                                        Tenant = Region.Tenant,                                                  
                                                        LocalName = Region.LocalName,
                                                        Name = Region.Name,
                                                        SearchFields = Region.SearchFields,
                                                        InActive = Region.InActive,
                                                    });
            return result;
        }
    }
}
