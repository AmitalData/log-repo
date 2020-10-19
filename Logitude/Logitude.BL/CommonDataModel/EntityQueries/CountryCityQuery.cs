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
    public class CountryCityQuery
    {
        CountryCityRepository repository;

        public CountryCityQuery()
        {
            repository = new CountryCityRepository(); 
        }

        public CountryCityQuery(int tenant)
        {
            repository = new CountryCityRepository(tenant);
        }

        public CountryCityQuery(CountryCityRepository repository)
        {
            this.repository = repository;
        }

        public CountryCityPM GetSinglePM(string id, int tenant)
        {
            CountryCityPM entityPM = (from a in repository.context.CountryCities.Include("Country").Include("State")
                                      where a.Tenant == tenant && a.Id == id
                                      select new CountryCityPM()
                                      {
                                          AddedManually = a.AddedManually,                                          
                                          EnglishName = a.EnglishName,
                                          Id = a.Id,
                                          Code = a.Code,
                                          InActive = a.InActive,
                                          LocalName = a.LocalName,
                                          Notes = a.Notes,
                                          Tenant = a.Tenant,
                                          SearchFields = a.SearchFields,
                                          ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                          CountryId = a.CountryId,
                                          CountryCode = a.Country == null ? null : a.Country.Code,
                                          CountryEnglishName = a.Country == null ? null : a.Country.EnglishName,
                                          StateId = a.StateId,
                                          StateCode = a.State == null ? null : a.State.Code,
                                          StateEnglishName = a.State == null ? null : a.State.EnglishName,
                                      }).FirstOrDefault();

            CountryCityPM securedEntityPM = new CountryCityPM();
            SecuredMapping.GetMappedPM(entityPM, securedEntityPM, "CountryCity", tenant);

            return securedEntityPM;
        }

        public CountryCityPM GetCountryCityPMByCountryIdAndNAme(string countryId, string cityName, int tenant)
        {
            CountryCityPM entityPM = (from a in repository.context.CountryCities.Include("Country").Include("State")
                                      where a.Tenant == tenant && a.CountryId == countryId && a.EnglishName == cityName
                                      select new CountryCityPM()
                                      {
                                          AddedManually = a.AddedManually,
                                          EnglishName = a.EnglishName,
                                          Id = a.Id,
                                          Code = a.Code,
                                          InActive = a.InActive,
                                          LocalName = a.LocalName,
                                          Notes = a.Notes,
                                          Tenant = a.Tenant,
                                          SearchFields = a.SearchFields,
                                          ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                          CountryId = a.CountryId,
                                          CountryCode = a.Country == null ? null : a.Country.Code,
                                          CountryEnglishName = a.Country == null ? null : a.Country.EnglishName,
                                          StateId = a.StateId,
                                          StateCode = a.State == null ? null : a.State.Code,
                                          StateEnglishName = a.State == null ? null : a.State.EnglishName,
                                      }).FirstOrDefault();

            CountryCityPM securedEntityPM = new CountryCityPM();
            SecuredMapping.GetMappedPM(entityPM, securedEntityPM, "CountryCity", tenant);

            return securedEntityPM;
        }

        public IQueryable<CountryCityPM> GetCountryCitiesPMsByTenant(int tenant)
        {
            IQueryable<CountryCityPM> iQueryable =
                from a in repository.context.CountryCities.Include("Country").Include("State")
                where a.Tenant == tenant
                select new CountryCityPM()
                {
                    AddedManually = a.AddedManually,
                    EnglishName = a.EnglishName,
                    Id = a.Id,
                    Code = a.Code,
                    InActive = a.InActive,
                    LocalName = a.LocalName,
                    Notes = a.Notes,
                    Tenant = a.Tenant,
                    SearchFields = a.SearchFields,
                    ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                    CountryId = a.CountryId,
                    CountryCode = a.Country == null ? null : a.Country.Code,
                    CountryEnglishName = a.Country == null ? null : a.Country.EnglishName,
                    StateId = a.StateId,
                    StateCode = a.State == null ? null : a.State.Code,
                    StateEnglishName = a.State == null ? null : a.State.EnglishName,
                };

            return iQueryable;
        }

        public IQueryable<CountryCityPM> GetCountryCitiesByName(string name, int tenant)
        {
            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.CountryCities.Include("Country").Include("State")
                         where a.Tenant == tenant
                         select new CountryCityPM()
                         {
                             AddedManually = a.AddedManually,
                             EnglishName = a.EnglishName,
                             Id = a.Id,
                             Code = a.Code,
                             InActive = a.InActive,
                             LocalName = a.LocalName,
                             Notes = a.Notes,
                             Tenant = a.Tenant,
                             SearchFields = a.SearchFields,
                             ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                             CountryId = a.CountryId,
                             CountryCode = a.Country == null ? null : a.Country.Code,
                             CountryEnglishName = a.Country == null ? null : a.Country.EnglishName,
                             StateId = a.StateId,
                             StateCode = a.State == null ? null : a.State.Code,
                             StateEnglishName = a.State == null ? null : a.State.EnglishName,
                         }).AsQueryable();

            IQueryable<CountryCityPM> query2 = null;

            if (!string.IsNullOrEmpty(name))
            {
                if (query2 != null)
                {
                    if (query2.Count() == 0)
                    {
                        query2 = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()));
                    }
                }

                else
                {
                    query2 = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()));
                }
            }

            if (query2 != null)
            {
                return query2;
            }

            else
                return query;
        }

        public IQueryable<CountryCityList> GetIQueryableEntityList(IQueryable<CountryCity> iQueryable)
        {
            IQueryable<CountryCityList> result = from a in iQueryable.Include("Country").Include("State")
                                                 select new CountryCityList()
                                                 {
                                                     EnglishName = a.EnglishName,
                                                     LocalName = a.LocalName,
                                                     InActive = a.InActive,
                                                     Notes = a.Notes,
                                                     Id = a.Id,
                                                     Code = a.Code,
                                                     Tenant = a.Tenant,
                                                     SearchFields = a.SearchFields,
                                                     AddedManually = a.AddedManually,
                                                     CountryId = a.CountryId,
                                                     CountryCode = a.Country == null ? null : a.Country.Code,
                                                     CountryEnglishName = a.Country == null ? null : a.Country.EnglishName,
                                                     StateId = a.StateId,
                                                     StateCode = a.State == null ? null : a.State.Code,
                                                     StateEnglishName = a.State == null ? null : a.State.EnglishName,
                                                 };
            return result;
        }

        public CountryCityPM GetSinglePMByCodeAndCountryId(string code, string countryId, int tenant)
        {
            CountryCityPM entityPM = (from a in repository.context.CountryCities.Include("Country").Include("State")
                                      where a.Tenant == tenant && a.Code == code && a.CountryId == countryId
                                      select new CountryCityPM()
                                      {
                                          AddedManually = a.AddedManually,
                                          EnglishName = a.EnglishName,
                                          Id = a.Id,
                                          Code = a.Code,
                                          InActive = a.InActive,
                                          LocalName = a.LocalName,
                                          Notes = a.Notes,
                                          Tenant = a.Tenant,
                                          SearchFields = a.SearchFields,
                                          ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                          CountryId = a.CountryId,
                                          CountryCode = a.Country == null ? null : a.Country.Code,
                                          CountryEnglishName = a.Country == null ? null : a.Country.EnglishName,
                                          StateId = a.StateId,
                                          StateCode = a.State == null ? null : a.State.Code,
                                          StateEnglishName = a.State == null ? null : a.State.EnglishName,
                                      }).FirstOrDefault();
            
            return entityPM;
        }

        public CountryCityPM GetSinglePMByCode(string code, int tenant)
        {
            CountryCityPM entityPM = (from a in repository.context.CountryCities.Include("Country").Include("State")
                                      where a.Tenant == tenant && a.Code == code
                                      select new CountryCityPM()
                                      {
                                          AddedManually = a.AddedManually,
                                          EnglishName = a.EnglishName,
                                          Id = a.Id,
                                          Code = a.Code,
                                          InActive = a.InActive,
                                          LocalName = a.LocalName,
                                          Notes = a.Notes,
                                          Tenant = a.Tenant,
                                          SearchFields = a.SearchFields,
                                          ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                          CountryId = a.CountryId,
                                          CountryCode = a.Country == null ? null : a.Country.Code,
                                          CountryEnglishName = a.Country == null ? null : a.Country.EnglishName,
                                          StateId = a.StateId,
                                          StateCode = a.State == null ? null : a.State.Code,
                                          StateEnglishName = a.State == null ? null : a.State.EnglishName,
                                      }).FirstOrDefault();

            CountryCityPM securedEntityPM = new CountryCityPM();
            SecuredMapping.GetMappedPM(entityPM, securedEntityPM, "CountryCity", tenant);

            return securedEntityPM;
        }
    }
}