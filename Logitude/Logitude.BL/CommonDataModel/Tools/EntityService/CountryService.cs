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
    public class CountryService
    {
        bool isNewEntity;
        private int tenant;
        public Country Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CountryPM entityPm;
        private ICommonDataContext objectContext;
        private CountryRepository entityRepository;

        public CountryService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CountryRepository(objectContext);
        }

        public void Create(CountryPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;

            bool exist = this.IsEntityExists();

            if (exist)
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", tenant);
                msg = msg.Replace("%Entity", "Country");
                throw new ApplicationException(msg);
            }

            else
            {
                this.Poco = new Country();

                this.Poco.Id = IdCounter.GetNumber("Country", entityPm.Tenant).ToString();
                entityPm.Id = this.Poco.Id;

                CountryValidating.Validate(entityPM);
                if (!entityPM.IsHybrid)
                {
                    CountryTracing.Trace(entityPM, Poco, isNewEntity);
                }

                CountryMapping.MapEntity(entityPM, Poco, isNewEntity);
                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
            }
        }

        public void Update(CountryPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;

            bool exist = this.IsEntityExists();

            if (exist)
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", tenant);
                msg = msg.Replace("%Entity", "Country");
                throw new ApplicationException(msg);
            }

            else
            {
                this.Poco = entityRepository.GetSingleCountry(entityPM.Id, entityPm.Tenant);

                CountryValidating.Validate(entityPM);

                string entityName = "Country" + entityPM.Id + entityPM.Tenant;
                string entityPmName = "CountryPM" + entityPM.Id + entityPM.Tenant;

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
                    CountryTracing.Trace(entityPM, Poco, isNewEntity);
                }

                CountryMapping.MapEntity(entityPM, Poco, isNewEntity);
                entityRepository.Update(Poco);
                entityRepository.SubmitChanges();
            }
        }

        private bool IsEntityExists()
        {
            bool myResult = false;

            if (isNewEntity)
            {
                myResult = (from a in entityRepository.GetCountries(entityPm.Tenant)
                            where a.Code == entityPm.Code && a.Tenant == entityPm.Tenant
                            select a).Any();
            }

            else
            {
                myResult = (from a in entityRepository.GetCountries(entityPm.Tenant)
                            where a.Code == entityPm.Code && a.Tenant == entityPm.Tenant && a.Id != entityPm.Id
                            select a).Any();
            }

            return myResult;
        }
    }
}
