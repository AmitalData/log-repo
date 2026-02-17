using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.Validating;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class CounterDefinitionService
    {         
        bool isNewEntity;
        private int tenant;
        public CounterDefinition Poco { get; set; }
        private CounterDefinitionPM entityPM;
        private IWebFreightContext objectContext;
        private CounterDefinitionRepository entityRepository;
        private CounterStatRepository counterStatRepository;
        ContactPM loggedContact = null;
        public CounterDefinitionService(IWebFreightContext objectContext, string loggedEmail,int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new CounterDefinitionRepository(objectContext);

            ContactQuery contactQuery = new ContactQuery(tenant);
            this.loggedContact = contactQuery.GetContactByNameAndTenant(loggedEmail, tenant, true);

        }

        public void Create(CounterDefinitionPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("CounterDefinition", tenant).ToString();
            this.Poco = new CounterDefinition();
            this.Poco.Id = this.entityPM.Id;

            CounterDefinitionValidating.Validate(theEntityPm);
            CounterDefinitionTracing.Trace(theEntityPm, Poco, isNewEntity);
            CounterDefinitionMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CounterDefinitionPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleCounterDefinition(theEntityPm.Id, theEntityPm.Tenant);

          
            CounterRepository counterRepository = new CounterRepository(objectContext);
            Counter counter = counterRepository.GetSingleCounter(theEntityPm.CounterId, theEntityPm.Tenant);
            counterStatRepository = new CounterStatRepository(objectContext);
            CounterDefinition counterDefinition = entityRepository.GetSingleCounterDefinition(theEntityPm.Id, theEntityPm.Tenant);

            if (theEntityPm.StartNumber < counterDefinition.StartNumber)
            {
                throw new Exception("The new start number must be greater than current start number!");
            }

            if (counterDefinition.UniquePerPrefix != theEntityPm.UniquePerPrefix)
            {
                List<CounterStat> counterStats = counterStatRepository.GetCounterCounterStats(theEntityPm.CounterId, theEntityPm.Tenant);
                if (counterStats.Count > 0)
                {
                    foreach (CounterStat stat in counterStats)
                    {
                        counterStatRepository.Remove(stat);
                    }
                    counterStatRepository.SubmitChanges();
                }
            }

            else
            {
                if (counterDefinition.StartNumber != theEntityPm.StartNumber)
                {
                    string prefix = counterDefinition.UniquePerPrefix ? counterDefinition.Prefix : null;
                    CounterStat counterStat = counterStatRepository.GetSingleCounterStat(counterDefinition.CounterId, prefix, counterDefinition.Tenant);
                    if (counterStat != null)
                    {
                        counterStat.LastValue = theEntityPm.StartNumber;
                        counterStatRepository.Update(counterStat);
                        counterStatRepository.SubmitChanges();
                    }
                }
            }

            CounterDefinitionValidating.Validate(theEntityPm);
            CounterDefinitionTracing.Trace(theEntityPm, Poco, isNewEntity);
            CounterDefinitionMapping.MapEntity(theEntityPm, Poco, isNewEntity);

            counter.ChangedDate = TenantServerConfigration.GetCurrentDateTime(theEntityPm.Tenant);
            counter.ChangedByUserId = this.loggedContact.Id;

            counterRepository.Update(counter);
            counterRepository.SubmitChanges();

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}