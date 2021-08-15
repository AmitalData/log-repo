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
                CountryPM destinationCountry = countryQuery.GetSinglePM(entityPoco.DestinationCountryId, tenant);
                result = new HTSCodePM()
                {
                    Id = entityPoco.Id,
                    Tenant = entityPoco.Tenant,
                    Code = entityPoco.Code,
                    ItemId = entityPoco.ItemId,
                    DestinationCountryId = entityPoco.DestinationCountryId,
                    CountryEnglishName = destinationCountry != null ? destinationCountry.EnglishName : "",
                    ApprovedByCustomer = entityPoco.ApprovedByCustomer,
                    InActive = entityPoco.InActive,
                    LineNumber = entityPoco.LineNumber,
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
                                                 LineNumber = entity.LineNumber,
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
                                            LineNumber = entity.LineNumber,
                                        }).ToList();

            return hTSCodes;
        }

        public HTSCodePM GetSingleHTSCodeByProductItemAndCountry(string itemId, string countryId, int tenant)
        {
            return (from a in repository.context.HTSCodes.Include("Country")
                    where a.ItemId == itemId && a.DestinationCountryId == countryId && a.Tenant == tenant && !a.InActive
                    select new HTSCodePM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Code = a.Code,
                        ItemId = a.ItemId,
                        DestinationCountryId = a.DestinationCountryId,
                        CountryEnglishName = a.Country == null ? null : a.Country.EnglishName,
                        ApprovedByCustomer = a.ApprovedByCustomer,
                        InActive = a.InActive,
                        LineNumber = a.LineNumber,
                    }).FirstOrDefault();
        }

        public List<HTSCodePM> GetHTSCodeByProductItemIdsAndCountry(string itemsIds, string countryId, int tenant)
        {
            List<HTSCodePM> hTSCodes = new List<HTSCodePM>();
            List<string> itemsIdsList = itemsIds.Split(',').Select(p => p.Trim()).ToList().Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            if (itemsIdsList.Count() > 0)
            {
                hTSCodes = (from a in repository.context.HTSCodes.Include("Country")
                            where itemsIdsList.Contains(a.ItemId) && a.DestinationCountryId == countryId && a.Tenant == tenant && !a.InActive
                            select new HTSCodePM()
                            {
                                Id = a.Id,
                                Tenant = a.Tenant,
                                Code = a.Code,
                                ItemId = a.ItemId,
                                DestinationCountryId = a.DestinationCountryId,
                                CountryEnglishName = a.Country == null ? null : a.Country.EnglishName,
                                ApprovedByCustomer = a.ApprovedByCustomer,
                                InActive = a.InActive,
                                LineNumber = a.LineNumber,
                            }).ToList();
            }

            return hTSCodes;
        }
    }
}
