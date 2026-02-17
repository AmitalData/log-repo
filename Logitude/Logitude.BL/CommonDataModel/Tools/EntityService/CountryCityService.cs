using System;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CountryCityService
    {
        bool isNewEntity;
        private int tenant;
        private string oldCountryId;
        private string newCountryId;
        public CountryCity Poco { get; set; }
        private CountryCityPM entityPM;
        private ICommonDataContext objectContext;
        private CountryCityRepository entityRepository;
        public CountryCityService(ICommonDataContext objectContext, int tenant)
        {
            
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new CountryCityRepository(objectContext);
        }

        public void Create(CountryCityPM entityPM)
        {
            this.entityPM = entityPM;
            this.isNewEntity = true;

            this.Validate();

            bool exist = this.IsEntityExists();

            if (exist)
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", tenant);
                msg = msg.Replace("%Entity", "City");
                throw new ApplicationException(msg);
            }

            else
            {
                this.entityPM.Id = IdCounter.GetNumber("CountryCity", tenant).ToString();

                this.Poco = new CountryCity()
                {
                    Id = entityPM.Id,
                    Tenant = tenant,
                };

                if (!entityPM.IsHybrid)
                {
                    CountryCityTracing.Trace(entityPM, Poco, isNewEntity);
                }

                oldCountryId = entityPM.CountryId;
                newCountryId = oldCountryId;
                CountryCityMapping.MapEntity(entityPM, Poco, isNewEntity);

                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();

                this.UpdateCountries();
            }
        }
        public void Update(CountryCityPM entityPM)
        {
            this.entityPM = entityPM;
            this.isNewEntity = false;

            this.Validate();

            bool exist = this.IsEntityExists();

            if (exist)
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", tenant);
                msg = msg.Replace("%Entity", "City");
                throw new ApplicationException(msg);
            }

            else
            {
                this.Poco = entityRepository.GetSingleCountryCity(entityPM.Id, tenant);

                string entityName = "CountryCity" + entityPM.Id + entityPM.Tenant;
                string entityPmName = "CountryCityPM" + entityPM.Id + entityPM.Tenant;

                if (CacheManager.CacheWrapper.Get(entityName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityName);
                }

                if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityPmName);
                }

                if (!entityPM.IsHybrid)
                {
                    CountryCityTracing.Trace(entityPM, Poco, isNewEntity);
                }

                oldCountryId = Poco.CountryId;
                newCountryId = entityPM.CountryId;

                CountryCityMapping.MapEntity(entityPM, Poco, isNewEntity);

                entityRepository.Update(Poco);
                entityRepository.SubmitChanges();

                this.UpdateCountries();
            }
        }

        private void Validate()
        {
            if (this.entityPM.StateId == null)
            {
                if (this.entityPM.CountryId != null)
                {
                    CountryRepository countryRepository = new CountryRepository(tenant);
                    Country country = countryRepository.GetSingleCountry(this.entityPM.CountryId, tenant);
                    if (country != null)
                    {
                        if (country.IsStateRequired)
                        {
                            throw new ApplicationException("State field is required");
                        }
                    }
                }
            }
        }

        private void UpdateCountries()
        {
            CountryRepository countryRepository = new CountryRepository(tenant);

            if (isNewEntity)
            {
                Country country = countryRepository.GetSingleCountry(oldCountryId, tenant);
                if (country != null)
                {
                    if (!country.HasCitiesList)
                    {
                        country.HasCitiesList = true;
                        countryRepository.SubmitChanges();
                    }
                }
            }

            else
            {
                Country newCountry = countryRepository.GetSingleCountry(newCountryId, tenant);
                if (newCountry != null)
                {
                    if (!newCountry.HasCitiesList)
                    {
                        newCountry.HasCitiesList = true;
                        countryRepository.SubmitChanges();
                    }
                }

                if (oldCountryId != newCountryId)
                {
                    Country oldCountry = countryRepository.GetSingleCountry(oldCountryId, tenant);
                    if (oldCountry != null)
                    {
                        oldCountry.HasCitiesList = entityRepository.IsCountryHasCities(oldCountryId, tenant);
                        countryRepository.SubmitChanges();
                    }
                }
            }
        }

        private bool IsEntityExists()
        {
            bool myResult = false;

            if (isNewEntity)
            {
                myResult = (from a in entityRepository.GetCountryCities(tenant)
                            where a.Code == entityPM.Code && a.Tenant == entityPM.Tenant && a.CountryId == entityPM.CountryId
                            select a).Any();
            }

            else
            {
                myResult = (from a in entityRepository.GetCountryCities(tenant)
                            where a.Code == entityPM.Code && a.Tenant == entityPM.Tenant && a.CountryId == entityPM.CountryId && a.Id != entityPM.Id
                            select a).Any();
            }

            return myResult;
        }
    }
}
