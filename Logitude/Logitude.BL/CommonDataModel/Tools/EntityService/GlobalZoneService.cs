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
    public class GlobalZoneService
    {
        bool isNewEntity;
        private int tenant;
        public GlobalZone Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private GlobalZonePM entityPm;
        private ICommonDataContext objectContext;
        private GlobalZoneRepository entityRepository;
        public GlobalZoneService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new GlobalZoneRepository(objectContext);
        }

        public void Create(GlobalZonePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;

            bool exist = this.IsEntityExists();
                
            if (exist)
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", entityPm.Tenant);
                msg = msg.Replace("%Entity", "Global zone");
                throw new Exception(msg);
            }

            else
            {
                this.Poco = new GlobalZone();
                this.entityPm.Id = IdCounter.GetNumber("GlobalZone", tenant).ToString();

                this.Poco.Id = this.entityPm.Id;

                GlobalZoneValidating.Validate(entityPM);
                if (!entityPM.IsHybrid)
                {
                    GlobalZoneTracing.Trace(entityPM, Poco, isNewEntity);
                }

                GlobalZoneMapping.MapEntity(entityPM, Poco, isNewEntity);
                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
            }
        }

        public void Update(GlobalZonePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;


            this.Poco = entityRepository.GetSingleGlobalZone(entityPM.Id, entityPm.Tenant);

            GlobalZoneValidating.Validate(entityPM);

            string entityName = "GlobalZone" + entityPM.Id + entityPM.Tenant;
            string entityPmName = "GlobalZonePM" + entityPM.Id + entityPM.Tenant;

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
                GlobalZoneTracing.Trace(entityPM, Poco, isNewEntity);
            }

            GlobalZoneMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

        }

        private bool IsEntityExists()
        {
            bool myResult = false;

            if (isNewEntity)
            {
                myResult = (from a in entityRepository.GetGlobalZones(entityPm.Tenant)
                            where a.Code == entityPm.Code && a.Tenant == entityPm.Tenant
                            select a).Any();
            }

            else
            {
                myResult = (from a in entityRepository.GetGlobalZones(entityPm.Tenant)
                            where a.Code == entityPm.Code && a.Tenant == entityPm.Tenant && a.Id != entityPm.Id
                            select a).Any();
            }

            return myResult;
        }
    }
}
