using System;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class StateService
    {
        bool isNewEntity;
        private int tenant;
        public State Poco { get; set; }
        private string oldCountryId;
        private string newCountryId;
        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private StatePM entityPm;
        private ICommonDataContext objectContext;
        private StateRepository entityRepository;
        public StateService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new StateRepository(objectContext);
        }

        public void Create(StatePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;

            bool exist = this.IsEntityExists();

            if (exist)
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", tenant);
                msg = msg.Replace("%Entity", "State");
                throw new ApplicationException(msg);
            }

            else
            {
                this.Poco = new State();

                this.Poco.Id = IdCounter.GetNumber("State", entityPm.Tenant).ToString();
                entityPm.Id = this.Poco.Id;

                StateValidating.Validate(entityPM);

                if (!entityPM.IsHybrid)
                {
                    StateTracing.Trace(entityPM, Poco, isNewEntity);
                }

                oldCountryId = entityPM.CountryId;
                newCountryId = oldCountryId;

                StateMapping.MapEntity(entityPM, Poco, isNewEntity);

                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();

                this.UpdateCountries();
            }
        }

        public void Update(StatePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;

            bool exist = this.IsEntityExists();

            if (exist)
            {
                    string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", tenant);
                    msg = msg.Replace("%Entity", "State");
                    throw new ApplicationException(msg);
                }

            else
            {
                this.Poco = entityRepository.GetSingleState(entityPM.Id, entityPm.Tenant);

                StateValidating.Validate(entityPM);

                string entityName = "State" + entityPM.Id + entityPM.Tenant;
                string entityPmName = "StatePM" + entityPM.Id + entityPM.Tenant;

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
                StateTracing.Trace(entityPM, Poco, isNewEntity);
            }

            oldCountryId = Poco.CountryId;
            newCountryId = entityPM.CountryId;

            StateMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

            this.UpdateCountries();
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
                    if (!country.HasStates)
                    {
                        country.HasStates = true;
                        countryRepository.SubmitChanges();
                    }
                }
            }

            else
            {
                Country newCountry = countryRepository.GetSingleCountry(newCountryId, tenant);
                if (newCountry != null)
                {
                    if (!newCountry.HasStates)
                    {
                        newCountry.HasStates = true;
                        countryRepository.SubmitChanges();
                    }
                }

                if (oldCountryId != newCountryId)
                {
                    Country oldCountry = countryRepository.GetSingleCountry(oldCountryId, tenant);
                    if (oldCountry != null)
                    {
                        oldCountry.HasStates = entityRepository.IsCountryHasStates(oldCountryId, tenant);
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
                myResult = (from a in entityRepository.GetStates(tenant)
                            where a.Code == entityPm.Code && a.Tenant == entityPm.Tenant && a.CountryId == entityPm.CountryId
                            select a).Any();
            }

            else
            {
                myResult = (from a in entityRepository.GetStates(tenant)
                            where a.Code == entityPm.Code && a.Tenant == entityPm.Tenant && a.CountryId == entityPm.CountryId && a.Id != entityPm.Id
                            select a).Any();
        }
   
            return myResult;
        }
    }
}
