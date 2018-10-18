using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class SpecialServicesTypeQuery
    {         
        SpecialServicesTypeRepository repository;
        public SpecialServicesTypeQuery()
        {
            repository = new SpecialServicesTypeRepository(); 
        }

        public SpecialServicesTypeQuery(SpecialServicesTypeRepository SpecialServicesTypeRepository)
        {
            repository = SpecialServicesTypeRepository;
        }

        public SpecialServicesTypeQuery(int tenant)
        {
            repository = new SpecialServicesTypeRepository(tenant);
        }

        public SpecialServicesTypePM GetSingleSpecialServicesTypePM(string id, int tenant)
        {
            SpecialServicesTypePM result = null;

            SpecialServicesType entityPoco = repository.GetSingleSpecialServicesType(id, tenant);

            if (entityPoco != null)
            {
                result = new SpecialServicesTypePM()
                {
                    Id = entityPoco.Id,
                    Tenant = entityPoco.Tenant,
                    LocalName = entityPoco.LocalName,
                    EnglishName = entityPoco.EnglishName,
                    Code = entityPoco.Code,
                    SearchFields = entityPoco.SearchFields,
                    InActive = entityPoco.InActive,
                };
            }

            SpecialServicesTypePM securedPm = new SpecialServicesTypePM();
            SecuredMapping.GetMappedPM(result, securedPm, "SpecialServicesType", tenant);

            return securedPm;
        }

        public SpecialServicesTypePM GetSinglePM(string id, int tenant)
        {
            SpecialServicesTypePM result = null;

            SpecialServicesType entityPoco = repository.GetSingleSpecialServicesType(id, tenant);

            if (entityPoco != null)
            {
                result = new SpecialServicesTypePM()
                {
                    Id = entityPoco.Id,
                    Tenant = entityPoco.Tenant,
                    LocalName = entityPoco.LocalName,
                    EnglishName = entityPoco.EnglishName,
                    Code = entityPoco.Code,
                    SearchFields = entityPoco.SearchFields,
                    InActive = entityPoco.InActive,
                };
            }

            SpecialServicesTypePM securedPm = new SpecialServicesTypePM();
            SecuredMapping.GetMappedPM(result, securedPm, "SpecialServicesType", tenant);

            return securedPm;
        }

        public IQueryable<SpecialServicesTypePM> GetSpecialServicesTypePMsByTenant(int tenant)
        {
            IQueryable<SpecialServicesType> pocos = repository.GetSpecialServicesTypes(tenant);

            IQueryable<SpecialServicesTypePM> entityPMs = (from a in pocos
                                                           select new SpecialServicesTypePM()
                                                                 {
                                                                     Id = a.Id,
                                                                     LocalName = a.LocalName,
                                                                     EnglishName = a.EnglishName,
                                                                     Code = a.Code,
                                                                     Tenant = a.Tenant,
                                                                     SearchFields = a.SearchFields,
                                                                     InActive = a.InActive,
                                                                 });
            return entityPMs;
        }

        public IQueryable<SpecialServicesTypeList> GetIQueryableEntityList(IQueryable<SpecialServicesType> iQueryable)
        {
            IQueryable<SpecialServicesTypeList> result = (from a in iQueryable
                                                    select new SpecialServicesTypeList()
                                                    {
                                                        Id = a.Id,
                                                        Tenant = a.Tenant,
                                                        Code = a.Code,
                                                        LocalName = a.LocalName,
                                                        EnglishName = a.EnglishName,
                                                        SearchFields = a.SearchFields,
                                                        InActive = a.InActive,
                                                    });
            return result;
        }
    }
}