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
    public class CountryQuery
    {
        CountryRepository repository;



        public CountryQuery(int tenant)
        {
            repository = new CountryRepository(tenant);
        }

        public CountryQuery(CountryRepository cuntryRepository)
        {
            repository = cuntryRepository;
        }

        public CountryPM GetSinglePM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "CountryPM" + id + tenant;
                CountryPM entity;
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var countries = from a in repository.context.Countries
                                        where a.Tenant == tenant
                                        select new CountryPM()
                                        {
                                            AddedManually = a.AddedManually,
                                            Code = a.Code,
                                            EnglishName = a.EnglishName,
                                            GlobalZoneId = a.GlobalZoneId,
                                            Id = a.Id,
                                            InActive = a.InActive,
                                            LocalName = a.LocalName,
                                            Notes = a.Notes,
                                            Tenant = a.Tenant,
                                            EC = a.EC,
                                            HasStates = a.HasStates,
                                            IsStateRequired = a.IsStateRequired,
                                            SearchFields = a.SearchFields,
                                            ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                            HasCitiesList = a.HasCitiesList,
                                            IsNorthAmerica = a.IsNorthAmerica,
                                            IsGreaterChina = a.IsGreaterChina,
                                        };

                        if (tenant == 0)
                        {
                            countries = countries.Where(c => c.Id == id);
                        }
                        foreach (var c in countries)
                        {
                            string name = "CountryPM" + c.Id + tenant;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, c, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (CountryPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (CountryPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    CountryPM country = (from a in repository.context.Countries
                                         where a.Tenant == tenant && a.Id == id
                                         select new CountryPM()
                                         {
                                             AddedManually = a.AddedManually,
                                             Code = a.Code,
                                             EnglishName = a.EnglishName,
                                             GlobalZoneId = a.GlobalZoneId,
                                             Id = a.Id,
                                             InActive = a.InActive,
                                             LocalName = a.LocalName,
                                             Notes = a.Notes,
                                             Tenant = a.Tenant,
                                             EC = a.EC,
                                             HasStates = a.HasStates,
                                             IsStateRequired = a.IsStateRequired,
                                             SearchFields = a.SearchFields,
                                             ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                             HasCitiesList = a.HasCitiesList,
                                             IsNorthAmerica = a.IsNorthAmerica,
                                             IsGreaterChina = a.IsGreaterChina,
                                         }).FirstOrDefault();
                    entity = country;
                }
                CountryPM securedPm = new CountryPM();
                SecuredMapping.GetMappedPM(entity, securedPm, "Country", tenant);

                return securedPm;
            }
            return null;
        }

        public IQueryable<CountryPM> GetCountryPMsByTenant(int tenant)
        {
            IQueryable<CountryPM> query = from a in repository.context.Countries
                                          where a.Tenant == tenant
                                          select new CountryPM()
                                          {
                                              AddedManually = a.AddedManually,
                                              Code = a.Code,
                                              EnglishName = a.EnglishName,
                                              GlobalZoneId = a.GlobalZoneId,
                                              Id = a.Id,
                                              InActive = a.InActive,
                                              LocalName = a.LocalName,
                                              Notes = a.Notes,
                                              Tenant = a.Tenant,
                                              EC = a.EC,
                                              HasStates = a.HasStates,
                                              IsStateRequired = a.IsStateRequired,
                                              SearchFields = a.SearchFields,
                                              ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                              HasCitiesList = a.HasCitiesList,
                                              IsNorthAmerica = a.IsNorthAmerica,
                                              IsGreaterChina = a.IsGreaterChina,
                                          };
            return query;
        }

        public IQueryable<Country> GetAllCountries()
        {
            IQueryable<Country> query = (from a in repository.context.Countries select a);
                                           
            return query;
        }


        public IQueryable<CountryPM> GetCountriesByCodeOrName(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = from a in repository.context.Countries
                        where a.Tenant == tenant
                        select new CountryPM()
                        {
                            AddedManually = a.AddedManually,
                            Code = a.Code,
                            EnglishName = a.EnglishName,
                            GlobalZoneId = a.GlobalZoneId,
                            Id = a.Id,
                            InActive = a.InActive,
                            LocalName = a.LocalName,
                            Notes = a.Notes,
                            Tenant = a.Tenant,
                            EC = a.EC,
                            HasStates = a.HasStates,
                            IsStateRequired = a.IsStateRequired,
                            SearchFields = a.SearchFields,
                            ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                            HasCitiesList = a.HasCitiesList,
                            IsNorthAmerica = a.IsNorthAmerica,
                            IsGreaterChina = a.IsGreaterChina,
                        };

            IQueryable<CountryPM> query2 = null;
            if (!string.IsNullOrEmpty(code))
            {
                query2 = query.Where(d => d.Code.ToUpper().StartsWith(code.ToUpper()));
            }

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

        public IQueryable<CountryList> GetIQueryableEntityList(IQueryable<Country> iQueryable)
        {
            IQueryable<CountryList> result = from f in iQueryable.Include("GlobalZone")
                                             select new CountryList()
                                             {
                                                 Code = f.Code,
                                                 EnglishName = f.EnglishName,
                                                 LocalName = f.LocalName,
                                                 Id = f.Id,
                                                 Tenant = f.Tenant,
                                                 AddedManually = f.AddedManually,
                                                 InActive = f.InActive,
                                                 Notes = f.Notes,
                                                 GlobalZoneName = f.GlobalZone != null ? f.GlobalZone.EnglishName : null,
                                                 EC = f.EC,
                                                 HasStates = f.HasStates,
                                                 IsStateRequired = f.IsStateRequired,
                                                 SearchFields = f.SearchFields,
                                                 HasCitiesList = f.HasCitiesList,
                                                 IsNorthAmerica = f.IsNorthAmerica,
                                                 IsGreaterChina = f.IsGreaterChina,
                                             };
            return result;
        }

        public CountryPM GetSinglePMByCode(string code, int tenant)
        {
            CountryPM country = (from a in repository.context.Countries
                                 where a.Tenant == tenant && a.Code == code
                                 select new CountryPM()
                                 {
                                     AddedManually = a.AddedManually,
                                     Code = a.Code,
                                     EnglishName = a.EnglishName,
                                     GlobalZoneId = a.GlobalZoneId,
                                     Id = a.Id,
                                     InActive = a.InActive,
                                     LocalName = a.LocalName,
                                     Notes = a.Notes,
                                     Tenant = a.Tenant,
                                     EC = a.EC,
                                     HasStates = a.HasStates,
                                     IsStateRequired = a.IsStateRequired,
                                     SearchFields = a.SearchFields,
                                     ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                     HasCitiesList = a.HasCitiesList,
                                     IsNorthAmerica = a.IsNorthAmerica,
                                     IsGreaterChina = a.IsGreaterChina,
                                 }).FirstOrDefault();

            return country;
        }

        public IQueryable<CountryList> GetCountryListsByCounryCodeLists( List<string>counryCodes ,int tenant)
        {
            IQueryable<CountryList> query = from a in repository.context.Countries
                                          where a.Tenant == tenant && counryCodes.Contains(a.Code)
                                          select new CountryList()
                                          {
                                         
                                              Code = a.Code,
                                              EnglishName = a.EnglishName,
                                             
                                          };
            return query;
        }
    }
}