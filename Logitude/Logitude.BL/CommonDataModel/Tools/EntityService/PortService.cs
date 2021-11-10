using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class PortService
    {
        bool isNewEntity;
        private int tenant;
        public Port Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private PortPM entityPM;
        private ICommonDataContext objectContext;
        private PortRepository entityRepository;
        public PortService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new PortRepository(objectContext);
        }

        public void Create(PortPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("Port", tenant).ToString();

            bool exist = (from a in entityRepository.GetPorts(theEntityPm.Tenant)
                          where a.Code == theEntityPm.Code && a.Tenant == theEntityPm.Tenant && a.Country.Id == theEntityPm.CountryId
                          select a).Any();
            if (!exist)
            {
                this.Poco = new Port();
                this.Poco.Id = this.entityPM.Id;
                this.entityPM.AddedManually = true;

                if (!string.IsNullOrEmpty(entityPM.StateId))
                {
                    StateRepository stateRepository = new StateRepository(objectContext);
                    State state = stateRepository.GetSingleState(entityPM.StateId, entityPM.Tenant);
                    if (state != null)
                    {
                        entityPM.StateName = state.EnglishName;
                        entityPM.StateCode = state.Code;
                    }
                }

                if (!string.IsNullOrEmpty(entityPM.CountryId))
                {
                    CountryRepository countryRepository = new CountryRepository(objectContext);
                    Country country = countryRepository.GetSingleCountry(entityPM.CountryId, entityPM.Tenant);
                    if (country != null)
                    {
                        entityPM.CountryName = country.EnglishName;
                        entityPM.CountryCode = country.Code;
                    }
                }

                PortValidating.Validate(theEntityPm);

                if (!entityPM.IsHybrid)
                {
                    PortTracing.Trace(theEntityPm, Poco, isNewEntity);
                }

                PortMapping.MapEntity(theEntityPm, Poco, isNewEntity);
                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
                AddPortKafkaQueueMessage();
            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", theEntityPm.Tenant);
                msg = msg.Replace("%Entity", "Port");
                throw new Exception(msg);
            }
        }

        public void Update(PortPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSinglePort(theEntityPm.Tenant, theEntityPm.Id);

            string entityName = "Port" + theEntityPm.Id + theEntityPm.Tenant;
            string entityPmName = "PortPM" + theEntityPm.Id + theEntityPm.Tenant;

            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }

            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            if (!string.IsNullOrEmpty(entityPM.StateId))
            {
                if (entityPM.StateId != Poco.StateId)
                {
                    StateRepository stateRepository = new StateRepository(objectContext);
                    State state = stateRepository.GetSingleState(entityPM.StateId, entityPM.Tenant);
                    if (state != null)
                    {
                        entityPM.StateName = state.EnglishName;
                        entityPM.StateCode = state.Code;
                    }
                }
            }

            if (!string.IsNullOrEmpty(entityPM.CountryId))
            {
                if (entityPM.CountryId != Poco.CountryId)
                {
                    CountryRepository countryRepository = new CountryRepository(objectContext);
                    Country country = countryRepository.GetSingleCountry(entityPM.CountryId, entityPM.Tenant);
                    if (country != null)
                    {
                        entityPM.CountryName = country.EnglishName;
                        entityPM.CountryCode = country.Code;
                    }
                }
            }

            PortValidating.Validate(theEntityPm);
            if (!entityPM.IsHybrid)
            {
                PortTracing.Trace(theEntityPm, Poco, isNewEntity);
            }

            bool updateTimeZone = this.CheckUpdatingTenantsPortsTimeZones();
            PortMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            AddPortKafkaQueueMessage();
            
            if (updateTimeZone)
            {
                this.CreatePortTimeZoneQueueMessage();
            }
        }

        private void AddPortKafkaQueueMessage()
        {
            if (!FeatureToggleHelper.HasFeatureToggle("CTL", entityPM.Tenant))
            {
                return;
            }
            AddKafkaQueueMessage();
        }

        private void AddKafkaQueueMessage()
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("CToolLookups", 0);
            var queueMessage = new Dictionary<string, string>() {
                { "Entity", "Port" },
                { "EntityId", entityPM.Id },
                { "Tenant", tenant.ToString()}};
            queueservice.Send(queueMessage, tenant);
        }

        private bool CheckUpdatingTenantsPortsTimeZones()
        {
            if(entityPM.PortTimeZoneCode != Poco.PortTimeZoneCode && tenant == 0)
            {
                return true;
            }

            return false;
        }
        private void CreatePortTimeZoneQueueMessage()
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("UpdatePortTimeZone", 0);
            var queueMessage = new Dictionary<string, string>() {
                { "PortId", entityPM.Id },
                { "Tenant", tenant.ToString()}};
            queueservice.Send(queueMessage, tenant);
        }
    }
}
