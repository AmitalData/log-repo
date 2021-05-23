using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class HTSCodeQuery
    {
        HTSCodeRepository repository;

        public HTSCodeQuery()
        {
            this.repository = new HTSCodeRepository();
        }

        public HTSCodeQuery(int tenant)
        {
            this.repository = new HTSCodeRepository(tenant);
        }

        public HTSCodeQuery(HTSCodeRepository repository)
        {
            this.repository = repository;
        }

        public HTSCodePM GetSinglePM(string id, int tenant)
        {
            HTSCodePM result = null;
            HTSCode entityPoco = repository.GetSingleHTSCode(id, tenant);

            if (entityPoco != null)
            {
                CountryQuery countryQuery = new CountryQuery(tenant);
                CountryPM destinationCountry = countryQuery.GetSinglePM(entityPoco.DestinationCountryId,tenant);
                result = new HTSCodePM()
                {
                    Id = entityPoco.Id,
                    Tenant = entityPoco.Tenant,
                    Code = entityPoco.Code,
                    ItemId = entityPoco.ItemId,
                    DestinationCountryId = entityPoco.DestinationCountryId,
                    CountryEnglishName = destinationCountry != null?destinationCountry.EnglishName:"",
                    ApprovedByCustomer = entityPoco.ApprovedByCustomer,
                    InActive = entityPoco.InActive,
                };
            }

            return result;
        }

        public IQueryable<HTSCodeList> GetIQueryableEntityList(IQueryable<HTSCode> iQueryable)
        {
            IQueryable<HTSCodeList> result = from entity in iQueryable
                                               select new HTSCodeList()
                                               {
                                                   Id = entity.Id,
                                                   Tenant = entity.Tenant,
                                                   Code = entity.Code,
                                                   ItemId = entity.ItemId,
                                                   DestinationCountryId = entity.DestinationCountryId,
                                                   ApprovedByCustomer = entity.ApprovedByCustomer,
                                                   InActive = entity.InActive,
                                               };
            return result;
        }

        public List<HTSCodePM> GetHTSCodePMsByProductItemIds(string itemId, int tenant)
        {
            List<HTSCodePM> hTSCodes = (from entity in repository.context.HTSCodes.Include("Country")
                                        where entity.Tenant == tenant && entity.ItemId == itemId
                                        select new HTSCodePM()
                                        {
                                            Id = entity.Id,
                                            Tenant = entity.Tenant,
                                            Code = entity.Code,
                                            ItemId = entity.ItemId,
                                            DestinationCountryId = entity.DestinationCountryId,
                                            CountryEnglishName = entity.Country == null ? null : entity.Country.EnglishName,
                                            ApprovedByCustomer = entity.ApprovedByCustomer,
                                            InActive = entity.InActive,
                                        }).ToList();

            return hTSCodes;
        }       
    }
}
